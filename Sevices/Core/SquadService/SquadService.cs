using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sevices.Core.SquadService
{
    public class SquadService : ISquadService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SquadService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //For factory role to see all squad in the factory.
        public ResultModel GetAllSquad(int pageIndex, int pageSize)
        {

            var result = new ResultModel();
            try
            {
                var data = _dbContext.Squad.Where(x => !x.isDeleted).OrderByDescending(i => i.name).ToList();
                var dataPaging = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<SquadModel>>(dataPaging),
                    Total = data.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllUserBySquadId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(x => x.squadId == id && !x.banStatus).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetAllUserNotInSquadId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(x => x.squadId != id && !x.banStatus).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        //Factory will create new squad 
        public ResultModel CreateSquad(CreateSquadModel model)
        {
            var result = new ResultModel();
            try
            {
                var nameExists = _dbContext.Squad.Any(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.ErrorMessage = "Tên tổ đã tồn tại!";
                }
                else
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    var listUserInGroup = listUser.Where(x => x.groupId != null).ToList();
                    if (listUserInGroup.Any())
                    {
                        var listName = listUserInGroup.Select(x => x.fullName).ToList();
                        result.ErrorMessage = $"Hãy xoá người dùng khỏi nhóm trước khi thêm vào tổ mới: {string.Join(", ", listName)}";
                    }
                    else
                    {
                        var newSquad = new Squad
                        {
                            name = model.name,
                            member = model.listUserId.Count,
                            isDeleted = false
                        };
                        _dbContext.Squad.Add(newSquad);

                        if (listUser.Any())
                        {
                            foreach (var user in listUser)
                            {
                                user.squadId = newSquad.id;
                            }
                            _dbContext.User.UpdateRange(listUser);
                        }

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = newSquad.id;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateSquad(UpdateSquadModel model)
        {
            var result = new ResultModel();
            try
            {
                bool nameExists = _dbContext.Squad.Any(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tổ tên này đã tồn tại.";
                }
                else
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    var listUserInGroup = listUser.Where(x => x.squadId != model.id && x.groupId != null).ToList();
                    if (listUserInGroup.Any())
                    {
                        var listName = listUserInGroup.Select(x => x.fullName).ToList();
                        result.ErrorMessage = $"Hãy xoá người dùng khỏi nhóm trước khi thay đổi tổ: {string.Join(", ", listName)}";
                    }
                    else
                    {
                        var data = _dbContext.Squad.FirstOrDefault(s => s.id == model.id);
                        if (data != null)
                        {
                            data.name = model.name;
                            data.member = model.listUserId.Count;
                            _dbContext.Squad.Update(data);

                            var listUserBySquad = _dbContext.User.Where(x => x.squadId == data.id).ToList();
                            if(listUser.Any())
                            {
                                listUserBySquad.AddRange(listUser);
                            }
                            foreach (var user in listUserBySquad)
                            {
                                if (model.listUserId.Contains(user.Id))
                                {
                                    user.squadId = data.id;
                                }
                                else
                                {
                                    user.squadId = null;
                                }
                            }
                            _dbContext.User.UpdateRange(listUser);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<Squad, SquadModel>(data);
                        }
                        else
                        {
                            result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                        }
                    }

                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel AddManagerToSquad(AddWorkerToSquadModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.squadId != model.squadId && user.groupId != null)
                {
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var squad = _dbContext.Squad.FirstOrDefault(g => g.id == model.squadId);
                    if (squad == null)
                    {
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        if (user.Role != null && user.Role.Name == "Manager")
                        {
                            //Update GroupId
                            user.squadId = model.squadId;
                            _dbContext.User.Update(user);

                            squad.member += 1;
                            _dbContext.Squad.Update(squad);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<SquadModel>(squad);
                        }
                        else
                        {
                            result.ErrorMessage = "Người dùng không phải quản lý!";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Fac and Manager can both use this function.
        public ResultModel AddWorkerToSquad(AddWorkerToSquadModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.squadId != model.squadId && user.groupId != null)
                {
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var squad = _dbContext.Squad.FirstOrDefault(g => g.id == model.squadId);
                    if (squad == null)
                    {
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        if (user.Role != null && user.Role.Name == "Worker")
                        {
                            //Update GroupId
                            user.squadId = model.squadId;
                            _dbContext.User.Update(user);

                            squad.member += 1;
                            _dbContext.Squad.Update(squad);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<SquadModel>(squad);
                        }
                        else
                        {
                            result.ErrorMessage = "Người dùng không phải công nhân!";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel RemoveUserFromSquad(RemoveWorkerFromSquadModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.squadId != model.squadId && user.groupId != null)
                {
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var squad = _dbContext.Squad.FirstOrDefault(g => g.id == model.squadId);
                    if (squad == null)
                    {
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        //Update GroupId
                        user.squadId = null;
                        _dbContext.User.Update(user);

                        squad.member -= 1;
                        _dbContext.Squad.Update(squad);

                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<SquadModel>(squad);
                        result.Succeed = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Not sure about this yet
        public ResultModel DeleteSquad(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var isExistedGroup = _dbContext.Group.Any(x => x.squadId == id);
                if (isExistedGroup)
                {
                    result.ErrorMessage = "Hãy xoá hết nhóm trước khi xoá tổ";
                }
                else
                {
                    var squad = _dbContext.Squad.Where(s => s.id == id).FirstOrDefault();
                    if (squad != null)
                    {
                        squad.isDeleted = true;
                        _dbContext.Squad.Update(squad);
                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<SquadModel>(squad);
                        result.Succeed = true;
                    }
                    else
                    {
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
    }
}

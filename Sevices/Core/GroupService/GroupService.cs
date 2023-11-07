using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sevices.Core.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public GroupService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //For Foreman role to see all group in the factory.
        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listGroup = _dbContext.Group.Where(x => x.isDeleted != true)
                   .OrderByDescending(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listGroup = listGroup.Where(x => x.name.Contains(search)).ToList();
                }

                var listGroupPaging = listGroup.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<GroupModel>();
                foreach (var item in listGroup)
                {

                    var tmp = new GroupModel
                    {
                        id = item.id,
                        name = item.name,
                        member = item.member
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listGroupPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllUserByGroupId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(x => x.groupId == id && !x.banStatus).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetAllUserNotInGroupId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(x => x.groupId != id && !x.banStatus).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        //Foreman will create new group 
        public ResultModel Create(CreateGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var nameExists = _dbContext.Group.Any(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Code = 16;
                    result.ErrorMessage = "Tên tổ đã tồn tại!";
                }
                else
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    var listUserInTeam = listUser.Where(x => x.teamId != null).ToList();
                    if (listUserInTeam.Any())
                    {
                        var listName = listUserInTeam.Select(x => x.fullName).ToList();
                        result.ErrorMessage = $"Hãy xoá người dùng khỏi nhóm trước khi thêm vào tổ mới: {string.Join(", ", listName)}";
                    }
                    else
                    {
                        var newGroup = new Group
                        {
                            name = model.name,
                            member = model.listUserId.Count,
                            isDeleted = false
                        };
                        _dbContext.Group.Add(newGroup);

                        if (listUser.Any())
                        {
                            foreach (var user in listUser)
                            {
                                user.groupId = newGroup.id;
                            }
                            _dbContext.User.UpdateRange(listUser);
                        }

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = newGroup.id;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                bool nameExists = _dbContext.Group.Any(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Code = 16;
                    result.Succeed = false;
                    result.ErrorMessage = "Tổ tên này đã tồn tại.";
                }
                else
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    var listUserInTeam = listUser.Where(x => x.groupId != model.id && x.teamId != null).ToList();
                    if (listUserInTeam.Any())
                    {
                        var listName = listUserInTeam.Select(x => x.fullName).ToList();
                        result.ErrorMessage = $"Hãy xoá người dùng khỏi nhóm trước khi thay đổi tổ: {string.Join(", ", listName)}";
                    }
                    else
                    {
                        var data = _dbContext.Group.FirstOrDefault(s => s.id == model.id);
                        if (data != null)
                        {
                            data.name = model.name;
                            data.member = model.listUserId.Count;
                            _dbContext.Group.Update(data);

                            var listUserByGroup = _dbContext.User.Where(x => x.groupId == data.id).ToList();
                            if(listUser.Any())
                            {
                                listUserByGroup.AddRange(listUser);
                            }
                            foreach (var user in listUserByGroup)
                            {
                                if (model.listUserId.Contains(user.Id))
                                {
                                    user.groupId = data.id;
                                }
                                else
                                {
                                    user.groupId = null;
                                }
                            }
                            _dbContext.User.UpdateRange(listUser);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<Group, GroupModel>(data);
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

        public ResultModel AddLeaderToGroup(AddWorkerToGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.groupId != model.groupId && user.teamId != null)
                {
                    result.Code = 19;
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                    if (group == null)
                    {
                        result.Code = 20;
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        if (user.Role != null && user.Role.Name == "Leader")
                        {
                            //Update TeamId
                            user.groupId = model.groupId;
                            _dbContext.User.Update(user);

                            group.member += 1;
                            _dbContext.Group.Update(group);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<TeamModel>(group);
                        }
                        else
                        {
                            result.Code = 21;
                            result.ErrorMessage = "Người dùng không phải trưởng nhóm!";
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

        //Foreman and Leader can both use this function.
        public ResultModel AddWorkerToGroup(AddWorkerToGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.groupId != model.groupId && user.teamId != null)
                {
                    result.Code = 19;
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                    if (group == null)
                    {
                        result.Code = 20;
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        if (user.Role != null && user.Role.Name == "Worker")
                        {
                            //Update TeamId
                            user.groupId = model.groupId;
                            _dbContext.User.Update(user);

                            group.member += 1;
                            _dbContext.Group.Update(group);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<TeamModel>(group);
                        }
                        else
                        {
                            result.Code = 22;
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

        public ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                }
                else if (user.groupId != model.groupId && user.teamId != null)
                {
                    result.Code = 19;
                    result.ErrorMessage = "Hãy xoá người dùng ra khỏi nhóm!";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                    if (group == null)
                    {
                        result.Code = 20;
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                    }
                    else
                    {
                        //Update TeamId
                        user.groupId = null;
                        _dbContext.User.Update(user);

                        group.member -= 1;
                        _dbContext.Group.Update(group);

                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<TeamModel>(group);
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
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var isExistedTeam = _dbContext.Team.Any(x => x.groupId == id);
                if (isExistedTeam)
                {
                    result.Code = 23;
                    result.ErrorMessage = "Hãy xoá hết nhóm trước khi xoá tổ!";
                }
                else
                {
                    var group = _dbContext.Group.Where(s => s.id == id).FirstOrDefault();
                    if (group != null)
                    {
                        group.isDeleted = true;
                        _dbContext.Group.Update(group);
                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<TeamModel>(group);
                        result.Succeed = true;
                    }
                    else
                    {
                        result.Code = 20;
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

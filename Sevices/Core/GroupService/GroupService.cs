using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;

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

        public ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var data = _dbContext.Group.Where(g => g.squadId == id && g.isDeleted != true).OrderByDescending(g => g.name).ToList();
                var dataPaging = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<SquadModel>>(dataPaging),
                    Total = data.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetAllUserByGroupId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(s => s.groupId == id && s.banStatus != true).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        //Factory and Manager can both use this 
        public ResultModel CreateGroup(CreateGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var newGroup = new Group
                {
                    name = model.name,
                    member = model.listUserId.Count,
                    squadId = model.squadId,
                    isDeleted = false
                };
                _dbContext.Group.Add(newGroup);

                if (model.listUserId.Any())
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    foreach (var user in listUser)
                    {
                        user.groupId = newGroup.id;
                    }
                    _dbContext.User.UpdateRange(listUser);
                }

                _dbContext.SaveChanges();
                result.Data = newGroup.id;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateGroup(UpdateGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var data = _dbContext.Group.FirstOrDefault(s => s.id == model.id);
                if (data != null)
                {
                    data.name = model.name;
                    data.squadId = model.squadId;
                    data.member = model.listUserId.Count;
                    _dbContext.Group.Update(data);

                    if (model.listUserId.Any())
                    {
                        var listUser = _dbContext.User.Where(x => x.groupId == data.id).ToList();
                        foreach (var user in listUser)
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
                    }

                    _dbContext.SaveChanges();
                    result.Data = _mapper.Map<GroupModel>(data);
                    result.Succeed = true;
                }
                else
                {
                    result.ErrorMessage = "Không tìm thấy nhóm";
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Need check again not complete. Fac and Manager can both use this function.
        public ResultModel AddWorkerToGroup(AddWorkerToGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                    if (group == null)
                    {
                        result.ErrorMessage = "Không tìm thấy nhóm";
                    }
                    else
                    {
                        //Update GroupId
                        user.groupId = model.groupId;
                        _dbContext.User.Update(user);

                        group.member += 1;
                        _dbContext.Group.Update(group);

                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<GroupModel>(group);
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

        public ResultModel RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                    if (group == null)
                    {
                        result.ErrorMessage = "Không tìm thấy nhóm";
                    }
                    else
                    {
                        //Update GroupId
                        user.groupId = null;
                        _dbContext.User.Update(user);

                        group.member -= 1;
                        _dbContext.Group.Update(group);

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<GroupModel>(group);
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
        public ResultModel DeleteGroup(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var data = _dbContext.Group.FirstOrDefault(s => s.id == id);
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.Group.Update(data);
                    _dbContext.SaveChanges();

                    result.Data = _mapper.Map<GroupModel>(data);
                    result.Succeed = true;
                }
                else
                {
                    result.ErrorMessage = "Không tìm thấy nhóm";
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

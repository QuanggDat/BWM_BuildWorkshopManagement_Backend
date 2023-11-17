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

        public ResultModel GetAllUserByGroupId(Guid id, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Include(x => x.Role).Where(x => x.groupId == id && !x.banStatus)
                    .OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

                var listUserPaging = listUser.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUser),
                    Total = listUser.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetAllUserNotInGroupId(Guid id, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Include(x => x.Role).Where(x => x.groupId != id && !x.banStatus).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

                var listUserPaging = listUser.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUserPaging),
                    Total = listUser.Count
                };
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
                    if(model.listUserId != null)
                    {
                        var newGroup = new Group
                        {
                            name = model.name,
                            member = 0,
                            isDeleted = false
                        };
                        _dbContext.Group.Add(newGroup);

                        foreach (var userId in model.listUserId)
                        {
                            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(x => x.Id == new Guid(userId!) && x.banStatus != true);
                            if (user == null)
                            {
                                result.Code = 18;
                                result.Succeed = false;
                                result.ErrorMessage = $"Không tìm thấy mã người dùng {userId} trong hệ thống !";
                                return result;
                            }
                            else
                            {
                                if (user.Role != null && user.Role.Name != "Worker")
                                {
                                    result.Code = 22;
                                    result.Succeed = false;
                                    result.ErrorMessage = $"Người dùng {userId} không phải công nhân!";
                                    return result;
                                }
                                else
                                {
                                    if (user.groupId != null)
                                    {
                                        result.Code = 95;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Công nhân đang ở tổ khác, hãy xoá ra khỏi trước khi thêm vào tổ mới!";
                                        return result;
                                    }
                                    else
                                    {
                                        user.groupId = newGroup.id;
                                        newGroup.member += 1;
                                    }
                                }
                            }                           
                        }
                        
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = newGroup.id;
                    }
                    else
                    {
                        var newGroup = new Group
                        {
                            name = model.name,
                            member = 0,
                            isDeleted = false
                        };
                        _dbContext.Group.Add(newGroup);
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

        public ResultModel AddWorkersToGroup(AddWorkersToGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId);
                if (group == null)
                {
                    result.Code = 20;
                    result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";
                }
                else
                {
                    var listUser = _dbContext.User.Include(r => r.Role).Where(x => model.listUserId.Contains(x.Id) && x.banStatus != true).ToList();
                    foreach (var userId in model.listUserId)
                    {
                        var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(x => x.Id == userId && x.banStatus != true);
                        if (user == null)
                        {
                            result.Code = 18;
                            result.Succeed = false;
                            result.ErrorMessage = $"Không tìm thấy mã người dùng {userId} trong hệ thống !";
                            return result;
                        }
                        else
                        {
                            if (user.Role != null && user.Role.Name != "Worker")
                            {
                                result.Code = 22;
                                result.Succeed = false;
                                result.ErrorMessage = $"Người dùng {userId} không phải công nhân!";
                                return result;
                            }
                            else
                            {
                                if (user.groupId != null)
                                {
                                    result.Code = 95;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Công nhân đang ở tổ khác, hãy xoá ra khỏi trước khi thêm vào tổ mới!";
                                    return result;
                                }
                                else
                                {
                                    user.groupId = model.groupId;
                                    group.member += 1;
                                    _dbContext.User.Update(user);
                                }
                            }
                        }
                    }

                    _dbContext.Group.Update(group);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = group.id;
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
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.userId && i.banStatus != true);
                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
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
                        //Update GroupId
                        user.groupId = null;
                        _dbContext.User.Update(user);

                        group.member -= 1;
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

        //Not sure about this yet
        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var isExistedUser = _dbContext.User.Any(x => x.groupId == id && x.banStatus != true);
                if (isExistedUser)
                {
                    result.Code = 94;
                    result.Succeed = false;
                    result.ErrorMessage = "Hãy xoá hết người dùng trước khi xoá tổ ! ";
                }
                else
                {
                    var group = _dbContext.Group.Where(s => s.id == id && s.isDeleted == false).FirstOrDefault();
                    if (group != null)
                    {
                        group.isDeleted = true;
                        _dbContext.Group.Update(group);
                        _dbContext.SaveChanges();
                        result.Data = group.id;
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

        public ResultModel Update(UpdateGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                bool nameExists = _dbContext.Group.Any(s => s.id != model.id && s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Code = 16;
                    result.Succeed = false;
                    result.ErrorMessage = "Tổ tên này đã tồn tại.";
                }
                else
                {
                    var newLeader = _dbContext.User.Include(r => r.Role).FirstOrDefault(x => x.Id == model.leaderId);
                    var data = _dbContext.Group.FirstOrDefault(s => s.id == model.id);
                    var oldLeader = _dbContext.User.Include(r => r.Role).FirstOrDefault(x => x.groupId == model.id && x.Role != null && x.Role.Name == "Leader");
                    if (data == null)
                    {
                        result.Code = 20;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy tổ trong hệ thống!";                                               
                    }
                    else
                    {
                        if (newLeader == null)
                        {
                            result.Code = 18;
                            result.Succeed = false;
                            result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
                        }
                        else
                        {
                            if (newLeader.Role != null && newLeader.Role.Name != "Leader")
                            {
                                result.Code = 21;
                                result.ErrorMessage = "Người dùng không phải tổ trưởng!";
                            }
                            else
                            {
                                // Delete group for old Leader
                                if(oldLeader != null)
                                {
                                    oldLeader.groupId = null;
                                    oldLeader.Group = null;
                                }
                                data.name = model.name;
                                newLeader.groupId = model.id;
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = _mapper.Map<Group, GroupModel>(data);
                            }
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

        public ResultModel AddLeaderToGroup(AddLeaderToGroupModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == model.userId && i.banStatus != true);
                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy người dùng trong hệ thống!";
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
                        var checkLeaderInGroup = _dbContext.User.Include(r => r.Role).Where(x => x.groupId == model.groupId && x.Role!.Name == "Leader" && x.banStatus != true).Any();
                        if (checkLeaderInGroup == true)
                        {
                            result.Code = 96;
                            result.ErrorMessage = "Tổ đã có tổ trưởng !";
                        }
                        else
                        {
                            if (user.Role != null && user.Role.Name == "Leader")
                            {
                                //Update GroupId
                                user.groupId = model.groupId;
                                _dbContext.User.Update(user);

                                _dbContext.Group.Update(group);
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = _mapper.Map<GroupModel>(group);
                            }
                            else
                            {
                                result.Code = 21;
                                result.ErrorMessage = "Người dùng không phải tổ trưởng!";
                            }
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
    }
}

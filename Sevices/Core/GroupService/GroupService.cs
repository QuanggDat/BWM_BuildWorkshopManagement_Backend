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

        public ResultModel Create(CreateGroupModel model)
        {
            var result = new ResultModel();

            try
            {
                var nameExists = _dbContext.Group.Any(s => s.name == model.name && !s.isDeleted);

                var newLeader = _dbContext.User.Include(r => r.Role).Include(x => x.Group)
                    .FirstOrDefault(x => x.Id == model.leaderId && x.banStatus == false);

                if (nameExists)
                {
                    result.Code = 16;
                    result.ErrorMessage = "Tên tổ đã tồn tại!";
                }
                else
                {
                    if (newLeader == null)
                    {
                        result.Code = 18;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin người dùng!";
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
                            if (newLeader.groupId != null || newLeader.Group != null)
                            {
                                result.Code = 102;
                                result.ErrorMessage = "Tổ trưởng đang ở tổ khác!";
                            }
                            else
                            {
                                var newGroup = new Group
                                {
                                    name = model.name,
                                    isDeleted = false
                                };

                                _dbContext.Group.Add(newGroup);
                                newLeader.groupId = newGroup.id;
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = newGroup.id;
                            }                           
                        }
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
                var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId && g.isDeleted == false);

                if (group == null)
                {
                    result.Code = 20;
                    result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                }
                else
                {
                    var listUser = _dbContext.User.Include(r => r.Role)
                        .Where(x => model.listUserId.Contains(x.Id) && x.banStatus != true).ToList();

                    foreach (var userId in model.listUserId)
                    {
                        var user = _dbContext.User.Include(r => r.Role)
                            .FirstOrDefault(x => x.Id == userId && x.banStatus != true);

                        if (user == null)
                        {
                            result.Code = 18;
                            result.Succeed = false;
                            result.ErrorMessage = $"Không tìm thấy thông tin người dùng {userId}!";
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
                                    result.ErrorMessage = $"Công nhân {userId} đang ở tổ khác, hãy xoá ra khỏi trước khi thêm vào tổ mới!";
                                    return result;
                                }
                                else
                                {
                                    user.groupId = model.groupId;
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

        public ResultModel Update(UpdateGroupModel model)
        {
            var result = new ResultModel();

            try
            {
                var data = _dbContext.Group.FirstOrDefault(s => s.id == model.id && s.isDeleted == false);

                if (data == null)
                {
                    result.Code = 20;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                }
                else
                {
                    bool nameExists = _dbContext.Group.Any(s => s.name == model.name && s.name != data.name && !s.isDeleted);

                    if (nameExists)
                    {
                        result.Code = 16;
                        result.Succeed = false;
                        result.ErrorMessage = "Tên tổ đã tồn tại!";
                    }
                    else
                    {
                        var currentLeader = _dbContext.User.Include(r => r.Role)
                            .FirstOrDefault(x => x.groupId == model.id && x.Role != null && x.Role.Name == "Leader" && x.banStatus == false);

                        if (currentLeader != null && currentLeader.Id == model.leaderId)
                        {
                            data.name = model.name;
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<Group, GroupModel>(data);
                        }
                        else
                        {
                            var newLeader = _dbContext.User.Include(r => r.Role).Include(x => x.Group)
                            .FirstOrDefault(x => x.Id == model.leaderId && x.banStatus == false);

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
                                    if (newLeader.groupId != null)
                                    {
                                        result.Code = 102;
                                        result.ErrorMessage = "Tổ trưởng đang ở tổ khác!";
                                    }
                                    else
                                    {
                                        if (currentLeader != null)
                                        {
                                            currentLeader.groupId = null;
                                            currentLeader.Group = null;
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
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel ChangeLeader(ChangeLeaderModel model)
        {
            var result = new ResultModel();

            try
            {
                var data = _dbContext.Group.FirstOrDefault(s => s.id == model.groupId && s.isDeleted == false);

                if (data == null)
                {
                    result.Code = 20;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                }
                else
                {
                    var currentLeader = _dbContext.User.Include(r => r.Role)
                            .FirstOrDefault(x => x.groupId == model.groupId && x.Role != null && x.Role.Name == "Leader" && x.banStatus == false);

                    var newLeader = _dbContext.User.Include(r => r.Role).Include(x => x.Group)
                   .FirstOrDefault(x => x.Id == model.leaderId && x.banStatus == false);

                    if (newLeader == null)
                    {
                        result.Code = 18;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin người dùng!";
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
                            if (newLeader.groupId != null)
                            {
                                result.Code = 102;
                                result.ErrorMessage = "Tổ trưởng đang ở tổ khác!";
                            }
                            else
                            {
                                if (currentLeader != null)
                                {
                                    currentLeader.groupId = null;
                                    currentLeader.Group = null;
                                }

                                newLeader.groupId = model.groupId;
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

        public ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = new ResultModel();

            try
            {
                var user = _dbContext.User.Include(r => r.Role)
                    .FirstOrDefault(i => i.Id == model.userId && i.banStatus != true);

                if (user == null)
                {
                    result.Code = 18;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng!";
                }
                else
                {
                    var group = _dbContext.Group.FirstOrDefault(g => g.id == model.groupId && g.isDeleted == false);

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
                        result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }       
        
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
                    var leader = _dbContext.User.Include(x => x.Role)
                        .FirstOrDefault(x => x.groupId == item.id && x.Role != null && x.Role.Name == "Leader");

                    var tmp = new GroupModel
                    {
                        id = item.id,
                        name = item.name,
                        //member = item,
                        leaderName = leader?.fullName ?? "",
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

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = _dbContext.Group.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 20;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                }
                else
                {
                    var leader = _dbContext.User.Include(x => x.Role)
                        .FirstOrDefault(x => x.groupId == id && x.Role != null && x.Role.Name == "Leader");

                    var group = new GroupModel
                    {
                        id = check.id,
                        name = check.name,
                        //member = check.member,
                        leaderName = leader?.fullName ?? "",
                    };

                    result.Data = group;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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

        public ResultModel GetWorkersByGroupId(Guid id, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();

            try
            {
                var listUser = _dbContext.User.Include(x => x.Role)
                    .Where(x => x.groupId == id && x.Role != null && x.Role.Name == "Worker" && !x.banStatus)
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
                var listUser = _dbContext.User.Include(x => x.Role)
                    .Where(x => x.groupId != id && !x.banStatus).OrderBy(s => s.fullName).ToList();

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
    }
}

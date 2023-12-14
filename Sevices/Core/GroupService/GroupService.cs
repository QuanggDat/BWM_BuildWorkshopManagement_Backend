using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
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

        public ResultModel Create(CreateGroupModel model, Guid userId)
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
                                _dbContext.User.Update(newLeader);

                                var log = new Data.Entities.Log()
                                {
                                    groupId = newGroup.id,
                                    userId = userId,
                                    modifiedTime = DateTime.UtcNow.AddHours(7),
                                    action = "Tạo nhóm mới " + newGroup.name+ " và Nhóm trưởng là "+newLeader.fullName,
                                };
                                _dbContext.Log.Add(log);

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

        public ResultModel AddWorkersToGroup(AddWorkersToGroupModel model, Guid userid)
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
                            result.ErrorMessage = $"Không tìm thấy thông tin mã người dùng {userId}!";
                            return result;
                        }
                        else
                        {
                            if (user.Role != null && user.Role.Name != "Worker")
                            {
                                result.Code = 22;
                                result.Succeed = false;
                                result.ErrorMessage = $"Người dùng {user.fullName} không phải công nhân!";
                                return result;
                            }
                            else
                            {
                                if (user.groupId != null)
                                {
                                    result.Code = 95;
                                    result.Succeed = false;
                                    result.ErrorMessage = $"Công nhân {user.fullName} đang ở tổ khác, hãy xoá ra khỏi trước khi thêm vào tổ mới!";
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

                    var log = new Data.Entities.Log()
                    {
                        groupId = group.id,
                        userId = userid,
                        modifiedTime = DateTime.UtcNow.AddHours(7),
                        action = "Thêm công nhân vào nhóm: " + group.name,
                    };
                    _dbContext.Log.Add(log);

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

        public ResultModel Update(UpdateGroupModel model, Guid userId)
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
                            _dbContext.Group.Update(data);

                            var log = new Data.Entities.Log()
                            {
                                groupId = data.id,
                                userId = userId,
                                modifiedTime = DateTime.UtcNow.AddHours(7),
                                action = "Cập nhật thông tin nhóm: " + data.name,
                            };
                            _dbContext.Log.Add(log);

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
                                        _dbContext.Group.Update(data);

                                        newLeader.groupId = model.id;
                                        _dbContext.User.Update(newLeader);

                                        var log = new Data.Entities.Log()
                                        {
                                            groupId = data.id,
                                            userId = userId,
                                            modifiedTime = DateTime.UtcNow.AddHours(7),
                                            action = "Cập nhật thông tin nhóm: " + data.name ,
                                        };
                                        _dbContext.Log.Add(log);

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

        public ResultModel ChangeLeader(ChangeLeaderModel model, Guid userId)
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
                                _dbContext.User.Update(newLeader);

                                var log = new Data.Entities.Log()
                                {
                                    groupId = data.id,
                                    userId = userId,
                                    modifiedTime = DateTime.UtcNow.AddHours(7),
                                    action = "Cập nhật thông tin trưởng nhóm: " + data.name,
                                };
                                _dbContext.Log.Add(log);

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

        public ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model, Guid userId)
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

                        var log = new Data.Entities.Log()
                        {
                            groupId = group.id,
                            userId = userId,
                            modifiedTime = DateTime.UtcNow.AddHours(7),
                            action = "Xóa thành viên khỏi nhóm: " + group.name,
                        };
                        _dbContext.Log.Add(log);

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

        public ResultModel Delete(Guid id, Guid userId)
        {
            var result = new ResultModel();

            try
            {
                var group = _dbContext.Group.Where(s => s.id == id && s.isDeleted == false).FirstOrDefault();

                if (group == null)
                {
                    result.Code = 20;
                    result.ErrorMessage = "Không tìm thấy thông tin tổ!";
                }
                else
                {
                    var currentUsersInGroup = _dbContext.User.Include(x => x.Group).Where(x => x.groupId == id).ToList();

                    if (currentUsersInGroup != null && currentUsersInGroup.Count > 0)
                    {                      
                        foreach(var user in currentUsersInGroup)
                        {
                            user.groupId = null;
                            user.Group = null;
                        }
                        _dbContext.User.UpdateRange(currentUsersInGroup);
                    }

                    group.isDeleted = true;
                    _dbContext.Group.Update(group);

                    var log = new Data.Entities.Log()
                    {
                        groupId = group.id,
                        userId = userId,
                        modifiedTime = DateTime.UtcNow.AddHours(7),
                        action = "Xóa nhóm: " + group.name,
                    };
                    _dbContext.Log.Add(log);

                    _dbContext.SaveChanges();
                    result.Data = group.id;
                    result.Succeed = true;
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

                    var amountWorker = _dbContext.User.Include(x => x.Role)
                        .Where(x => x.groupId == item.id && x.Role != null && x.Role.Name == "Worker").ToList().Count;

                    var tmp = new GroupModel
                    {
                        id = item.id,
                        name = item.name,
                        amountWorker = amountWorker,
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

                    var amountWorker = _dbContext.User.Include(x => x.Role)
                        .Where(x => x.groupId == id && x.Role != null && x.Role.Name == "Worker").ToList().Count;

                    var group = new GroupModel
                    {
                        id = check.id,
                        name = check.name,
                        amountWorker = amountWorker,
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

        public ResultModel GetAllUsersByGroupId(Guid id, string? search, int pageIndex, int pageSize)
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

        public ResultModel GetWorkersNotAtWorkByGroupId(Guid id, string? search)
        {
            var result = new ResultModel();

            try
            {
                var listWorkerIdInGroup = _dbContext.User.Include(x => x.Role)
                    .Where(x => x.groupId == id && x.Role != null && x.Role.Name == "Worker" && !x.banStatus).Select(x => x.Id).ToList();

                var checkWorkerIdInTask = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                        .Where(x => listWorkerIdInGroup.Contains(x.userId) && x.WorkerTask.isDeleted == false && x.WorkerTask.status == EWorkerTaskStatus.New
                                 || listWorkerIdInGroup.Contains(x.userId) && x.WorkerTask.isDeleted == false && x.WorkerTask.status == EWorkerTaskStatus.Pending
                                 || listWorkerIdInGroup.Contains(x.userId) && x.WorkerTask.isDeleted == false && x.WorkerTask.status == EWorkerTaskStatus.InProgress )
                                 .Select(x => x.userId).ToList();

                var listUserId = listWorkerIdInGroup.Except(checkWorkerIdInTask).ToList();

                var listUser = _dbContext.User.Include(x => x.Role)
                    .Where(x => listUserId.Contains(x.Id) && !x.banStatus)
                    .OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

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

        public ResultModel GetAllUsersNotInGroupId(Guid id, string? search)
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

        public ResultModel GetAllWorkerNotYetGroup(string? search)
        {
            var result = new ResultModel();

            try
            {
                var listUser = _dbContext.User.Include(x => x.Role).Include(x => x.Group)
                    .Where(x => x.Role != null && x.Role.Name == "Worker" && x.groupId == null && x.Group == null && x.banStatus == false).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

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

        public ResultModel GetAllLeaderHaveGroup(string? search)
        {
            var result = new ResultModel();

            try
            {
                var listUser = _dbContext.User.Include(x => x.Role).Include(x => x.Group)
                    .Where(x => x.Role != null && x.Role.Name == "Leader" && x.groupId != null && x.banStatus == false).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

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

        public ResultModel GetAllLogOnGroup(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listLog = _dbContext.Log.Include(x => x.Group).Include(x => x.User).Where(x => x.groupId != null).OrderByDescending(x => x.modifiedTime).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listLog = listLog.Where(x => x.action.Contains(search)).ToList();
                }

                var listLogPaging = listLog.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LogModel>();
                foreach (var item in listLogPaging)
                {
                    var tmp = new LogModel
                    {
                        id = item.id,
                        groupId = item.groupId,
                        groupName = item.Group?.name,
                        userId = item.userId,
                        userName = item.User?.fullName,
                        modifiedTime = item.modifiedTime,
                        action = item.action,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listLog.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllLeaderNoHaveGroup(string? search)
        {
            var result = new ResultModel();

            try
            {
                var listUser = _dbContext.User.Include(x => x.Role).Include(x => x.Group)
                    .Where(x => x.Role != null && x.Role.Name == "Leader" && x.groupId == null && x.banStatus == false).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

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
    }
}

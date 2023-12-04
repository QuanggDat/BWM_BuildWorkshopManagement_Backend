using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Sevices.Core.NotificationService;

namespace Sevices.Core.WorkerTaskService
{
    public class WorkerTaskService : IWorkerTaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public WorkerTaskService(AppDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public ResultModel Create(Guid userId, CreateWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var checkPriority = _dbContext.WorkerTask.FirstOrDefault(x => x.leaderTaskId == model.leaderTaskId && x.priority == model.priority && x.isDeleted == false);

            if (checkPriority != null)
            {
                result.Code = 92;
                result.Succeed = false;
                result.ErrorMessage = "Mức độ ưu tiên này đã tồn tại !";
            }           
            else
            {
                var workerTask = new WorkerTask
                {
                    createById = userId,
                    leaderTaskId = model.leaderTaskId,                   
                    priority = model.priority,
                    name = model.name,
                    startTime = model.startTime,
                    endTime = model.endTime,
                    status = EWorkerTaskStatus.New,
                    description = model.description,
                    isDeleted = false,
                };

                try
                {
                    _dbContext.WorkerTask.Add(workerTask);

                    foreach (var assignee in model.assignees)
                    {
                        bool checkWorkerDetail = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                            .Where(x => x.userId == assignee && x.WorkerTask.status != EWorkerTaskStatus.Completed 
                            && x.WorkerTask.endTime > model.startTime && x.WorkerTask.startTime < model.startTime && x.WorkerTask.isDeleted == false).Any();

                        if (checkWorkerDetail == true)
                        {
                            result.Code = 93;
                            result.Succeed = false;
                            result.ErrorMessage = "Công nhân hiện đang làm công việc khác, không thể thêm vào công việc hiện tại !";
                            return result;
                        }
                        else
                        {
                            _dbContext.WorkerTaskDetail.Add(new WorkerTaskDetail
                            {
                                workerTaskId = workerTask.id,
                                userId = assignee
                            });
                        }
                    }

                    _dbContext.SaveChanges();

                    _notificationService.CreateForManyUser(new Notification
                    {
                        workerTaskId = workerTask.id,
                        title = "Công việc",
                        content = "Bạn vừa nhận được 1 công việc mới!",
                        type = NotificationType.WorkerTask
                    },
                    model.assignees);

                    result.Succeed = true;
                    result.Data = workerTask.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            return result;
        }

        public ResultModel Update(UpdateWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = _dbContext.WorkerTask.Find(model.id);

                if (check == null)
                {
                    result.Code = 47;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
                    return result;
                }
                else
                {
                    var checkPriority = _dbContext.WorkerTask.FirstOrDefault(x => x.priority != check.priority 
                    && x.leaderTaskId == check.leaderTaskId && x.priority == model.priority && x.isDeleted == false);

                    if (checkPriority != null)
                    {
                        result.Code = 92;
                        result.Succeed = false;
                        result.ErrorMessage = "Mức độ ưu tiên này đã tồn tại !";
                    }
                    else
                    {
                        check.priority = model.priority;
                        check.name = model.name;
                        check.description = model.description;
                        check.startTime = model.startTime;
                        check.endTime = model.endTime;
                        check.status = model.status;                        

                        // Remove all old worker tasks detail
                        var currentWokerTaskDetails = _dbContext.WorkerTaskDetail
                            .Where(x => x.workerTaskId == model.id).ToList();

                        var currentListUserId = currentWokerTaskDetails.Select(x => x.userId);

                        List<Guid> workerGetNoti = model.assignees.Except(currentListUserId).ToList();

                        if (currentWokerTaskDetails != null && currentWokerTaskDetails.Count > 0)
                        {
                            _dbContext.WorkerTaskDetail.RemoveRange(currentWokerTaskDetails);
                        }

                        // Set new worker tasks detail
                        var workerTaskDetails = new List<WorkerTaskDetail>();
                        foreach (var assignee in model.assignees)
                        {
                            bool checkWorkerDetail = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                            .Where(x => x.userId == assignee && x.WorkerTask.id != model.id && x.WorkerTask.status != EWorkerTaskStatus.Completed 
                                     && x.WorkerTask.endTime > model.startTime && x.WorkerTask.startTime < model.startTime && x.WorkerTask.isDeleted == false).Any();

                            if (checkWorkerDetail == true)
                            {
                                result.Code = 93;
                                result.Succeed = false;
                                result.ErrorMessage = "Công nhân hiện đang làm công việc khác, không thể thêm vào công việc hiện tại !";
                                return result;
                            }
                            else
                            {
                                workerTaskDetails.Add(new WorkerTaskDetail
                                {
                                    workerTaskId = model.id,
                                    userId = assignee                                  
                                });
                            }
                        }

                        _dbContext.WorkerTaskDetail.AddRange(workerTaskDetails);
                        _dbContext.SaveChanges();

                        _notificationService.CreateForManyUser(new Notification
                        {
                            workerTaskId = model.id,
                            title = "Công việc",
                            content = "Bạn vừa nhận được 1 công việc mới!",
                            type = NotificationType.WorkerTask
                        },
                        workerGetNoti);

                        result.Succeed = true;
                        result.Data = model.id;
                    }
                }               
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.WorkerTask.Find(id);

            if (check == null)
            {
                result.Code = 47;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
            }
            else
            {
                try
                {
                    check.isDeleted = true;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }               
            }
            return result;
        }

        public ResultModel Assign(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.WorkerTaskDetail.FirstOrDefault(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);

            if (check != null)
            {
                result.Code = 48;
                result.Succeed = false;
                result.ErrorMessage = "Công nhân đã được thêm vào công việc!";
            }
            else
            {
                var workerTask = _dbContext.WorkerTask.Find(model.workerTaskId);

                bool checkWorkerDetail = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                                    .Where(x => x.userId == model.memberId && x.WorkerTask.status != EWorkerTaskStatus.Completed && x.WorkerTask.endTime > workerTask!.startTime && x.WorkerTask.startTime < workerTask!.startTime).Any();

                if (checkWorkerDetail == true)
                {
                    result.Code = 93;
                    result.Succeed = false;
                    result.ErrorMessage = "Công nhân hiện đang làm công việc khác, không thể thêm vào công việc hiện tại !";
                }
                else
                {
                    var workerTaskDetail = new WorkerTaskDetail
                    {
                        userId = model.memberId,
                        workerTaskId = model.workerTaskId,
                    };
                    try
                    {
                        _dbContext.WorkerTaskDetail.Add(workerTaskDetail);
                        _dbContext.SaveChanges();

                        _notificationService.Create(new Notification
                        {
                            userId = model.memberId,
                            workerTaskId = model.workerTaskId,
                            title = "Công việc",
                            content = "Bạn vừa nhận được 1 công việc mới!",
                            type = NotificationType.WorkerTask
                        });

                        result.Succeed = true;
                        result.Data = workerTaskDetail.id;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }                   
                }
            }
            return result;
        }

        public ResultModel UnAssign(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.WorkerTaskDetail.FirstOrDefault(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);
            if (check == null)
            {
                result.Code = 49;
                result.Succeed = false;
                result.ErrorMessage = "Công nhân chưa được thêm vào công việc!";
            }
            else
            {
                try
                {
                    _dbContext.WorkerTaskDetail.Remove(check);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = check.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }    
            }
            return result;
        }

        public ResultModel UpdateStatus(Guid id, EWorkerTaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = _dbContext.WorkerTask.Find(id);
            if (workerTask == null)
            {
                result.Code = 47;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
            }
            else
            {
                try
                {
                    if (status == EWorkerTaskStatus.Completed)
                    {
                        workerTask.completedTime = DateTime.Now;
                    }

                    workerTask.status = status;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }                
            }
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.WorkerTask.Include(x => x.CreateBy).Include(x => x.Resources)
                    .Include(x => x.LeaderTask).ThenInclude(x => x.Item)
                    .Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                    .Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 47;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
                }
                else
                {
                    var wokerTaskModel = new WorkerTaskModel
                    {
                        id = check.id,
                        createById = check.createById,
                        createByName = check.CreateBy.fullName,
                        leaderTaskId = check.leaderTaskId,
                        leaderTaskName = check.LeaderTask.name,
                        Item = check.LeaderTask.Item,
                        name = check.name,
                        priority = check.priority,
                        startTime = check.startTime,
                        endTime = check.endTime,
                        completeTime = check.completedTime,
                        description = check.description,
                        status = check.status,
                        Members = check.WorkerTaskDetails.Select(_ => new TaskMember
                        {
                            memberId = _.User.Id,
                            memberFullName = _.User.fullName,
                        }).ToList(),
                        feedbackTitle = check.feedbackTitle,
                        feedbackContent = check.feedbackContent,
                        resource = check.Resources.Select(_ => _.link).ToList(),
                    };
                    result.Data = wokerTaskModel;
                    result.Succeed = true;
                };
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetAll (string? search, int pageIndex, int pageSize)
        {

            var result = new ResultModel();
            result.Succeed = false;

            var listWorkerTask = _dbContext.WorkerTask.Include(x => x.CreateBy)
                .Include(x => x.LeaderTask).ThenInclude(x => x.Item)
                .Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.isDeleted == false).OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listWorkerTask = listWorkerTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listWorkerTaskPaging = listWorkerTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<WorkerTaskModel>();
                foreach (var item in listWorkerTaskPaging)
                {
                    var tmp = new WorkerTaskModel
                    {
                        id = item.id,
                        createById = item.createById,
                        leaderTaskId = item.leaderTaskId,
                        createByName = item.CreateBy.fullName,
                        leaderTaskName = item.LeaderTask.name,
                        Item = item.LeaderTask.Item,
                        name = item.name,
                        priority = item.priority,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completeTime = item.completedTime,
                        description = item.description,
                        status = item.status,
                        Members = item.WorkerTaskDetails.Select(_ => new TaskMember
                        {
                            memberId = _.User.Id,
                            memberFullName = _.User.fullName,
                        }).ToList(),
                        feedbackTitle = item.feedbackTitle,
                        feedbackContent = item.feedbackContent,
                        resource = item.Resources.Select(_ => _.link).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listWorkerTask.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetByLeaderTaskId (Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {

            var result = new ResultModel();
            result.Succeed = false;

            var listWorkerTask =  _dbContext.WorkerTask.Include(x => x.CreateBy)
                .Include(x => x.LeaderTask).ThenInclude(x => x.Item)
                .Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.leaderTaskId == leaderTaskId && x.isDeleted == false).OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listWorkerTask = listWorkerTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listWorkerTaskPaging = listWorkerTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<WorkerTaskModel>();
                foreach (var item in listWorkerTaskPaging)
                {
                    var tmp = new WorkerTaskModel
                    {
                        id = item.id,
                        createById = item.createById,
                        leaderTaskId = item.leaderTaskId,
                        createByName = item.CreateBy.fullName,
                        leaderTaskName = item.LeaderTask.name,
                        Item = item.LeaderTask.Item,
                        name = item.name,
                        priority = item.priority,                        
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completeTime =item.completedTime,
                        description = item.description,
                        status = item.status,                        
                        Members = item.WorkerTaskDetails.Select(_ => new TaskMember
                        {
                            memberId = _.User.Id,
                            memberFullName = _.User.fullName,
                        }).ToList(),
                        feedbackTitle = item.feedbackTitle,
                        feedbackContent = item.feedbackContent,
                        resource = item.Resources.Select(_ => _.link).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listWorkerTask.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;         
        }

        public ResultModel GetByUserId(Guid leaderTaskId, Guid userId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listWorkerTaskId = _dbContext.WorkerTaskDetail.Where(x => x.userId == userId).Select(x => x.workerTaskId).ToList();

            var listWorkerTask = _dbContext.WorkerTask.Include(x => x.CreateBy)
                .Include(x => x.LeaderTask).ThenInclude(x => x.Item)
                .Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.leaderTaskId == leaderTaskId && listWorkerTaskId.Contains(x.id) && x.isDeleted == false)
                .OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listWorkerTask = listWorkerTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listWorkerTaskPaging = listWorkerTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<WorkerTaskModel>();
                foreach (var item in listWorkerTaskPaging)
                {
                    var tmp = new WorkerTaskModel
                    {
                        id = item.id,
                        createById = item.createById,
                        leaderTaskId = item.leaderTaskId,
                        createByName = item.CreateBy.fullName,
                        leaderTaskName = item.LeaderTask.name,
                        Item = item.LeaderTask.Item,
                        name = item.name,
                        priority = item.priority,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completeTime = item.completedTime,
                        description = item.description,
                        status = item.status,
                        Members = item.WorkerTaskDetails.Select(_ => new TaskMember
                        {
                            memberId = _.User.Id,
                            memberFullName = _.User.fullName,
                        }).ToList(),
                        feedbackTitle = item.feedbackTitle,
                        feedbackContent = item.feedbackContent,
                        resource = item.Resources.Select(_ => _.link).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listWorkerTask.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel SendFeedback(SendFeedbackModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workTaskDetail = _dbContext.WorkerTask
                .Where(x => x.id == model.workerTaskId).FirstOrDefault();

            if (workTaskDetail == null )
            {
                result.Code = 107;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân chi tiết!";
            }
            else
            {
                if (workTaskDetail.status == EWorkerTaskStatus.Completed)
                {
                    result.Code = 108;
                    result.Succeed = false;
                    result.ErrorMessage = "Công việc này đã hoàn thành!";
                }
                else
                {                  
                    try
                    {
                        workTaskDetail.status = model.status;
                        workTaskDetail.feedbackTitle = model.feedbackTitle;
                        workTaskDetail.feedbackContent= model.feedbackContent;
                        
                        if (model.resource != null)
                        {
                            foreach (var resource in model.resource)
                            {
                                _dbContext.Resource.Add(new Resource
                                {
                                    workerTaskId = model.workerTaskId,
                                    link = resource
                                });
                            }
                        }
                        
                        _dbContext.SaveChanges();                        

                        result.Succeed = true;
                        result.Data = workTaskDetail.id;
                    }

                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }       
    }
}

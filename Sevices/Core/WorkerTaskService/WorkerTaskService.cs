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

            var stepTmp = _dbContext.Step.Find(model.stepId);
            if (stepTmp == null)
            {
                result.Code = 40;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin quy trình!";
            }
            else
            {
                var workerTask = new WorkerTask
                {
                    leaderTaskId = model.leaderTaskId,
                    name = stepTmp.name,
                    description = model.description,
                    startTime = model.startTime,
                    endTime = model.endTime,
                    status = ETaskStatus.New,
                    createById = userId,
                    isDeleted = false,
                };

                try
                {
                    _dbContext.WorkerTask.Add(workerTask);

                    foreach (var assignee in model.assignees)
                    {
                        _dbContext.WorkerTaskDetail.Add(new WorkerTaskDetail
                        {
                            workerTaskId = workerTask.id,
                            userId = assignee
                        });
                    }

                    _dbContext.SaveChanges();
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
                    check.name = model.name;
                    check.description = model.description;
                    check.startTime = model.startTime;
                    check.endTime = model.endTime;
                    check.status = model.status;

                    // Remove all old woker tasks detail
                    var currentWokerTaskDetails = _dbContext.WorkerTaskDetail
                        .Where(x => x.workerTaskId == model.id)
                        .ToList();
                    if (currentWokerTaskDetails != null && currentWokerTaskDetails.Count > 0)
                    {
                        _dbContext.WorkerTaskDetail.RemoveRange(currentWokerTaskDetails);
                    }

                    // Set new woker tasks detail
                    var workerTaskDetails = new List<WorkerTaskDetail>();
                    foreach (var assignee in model.assignees)
                    {
                        workerTaskDetails.Add(new WorkerTaskDetail
                        {
                            workerTaskId = model.id,
                            userId = assignee
                        });
                    }

                    _dbContext.WorkerTaskDetail.AddRange(workerTaskDetails);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = model.id;
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
                return result;
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
                return result;
            }            
        }

        public ResultModel Assign(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.WorkerTaskDetail.SingleOrDefault(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);
            if (check != null)
            {
                result.Code = 48;
                result.Succeed = false;
                result.ErrorMessage = "Công nhân đã được thêm vào công việc!";
                return result;
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
                    result.Succeed = true;
                    result.Data = workerTaskDetail.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }         
        }

        public ResultModel UnAssign(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.WorkerTaskDetail.SingleOrDefault(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);
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
        public ResultModel UpdateStatus(Guid id, ETaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = _dbContext.WorkerTask.Find(id);
            if (workerTask == null)
            {
                result.Code = 47;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
                return result;
            }
            else
            {
                try
                {
                    if (status == ETaskStatus.Completed)
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
                return result;
            }           
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.WorkerTask.Where(x => x.id == id && x.isDeleted != true)
                    .FirstOrDefault();

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
                        leaderTaskId = check.leaderTaskId,
                        stepId = check.stepId,
                        userId = check.createById,
                        name = check.name,
                        description = check.description,
                        startTime = check.startTime,
                        endTime = check.endTime,
                        status = check.status,
                        Members = check.WorkerTaskDetails.Select(_ => new TaskMember
                        {
                            memberId = _.User.Id,
                            memberFullName = _.User.fullName,
                        }).ToList(),
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

        public ResultModel GetAll (Guid leaderTaskId)
        {

            var result = new ResultModel();
            result.Succeed = false;

            var check =  _dbContext.WorkerTask.Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.leaderTaskId == leaderTaskId && x.isDeleted == false).ToList();

            if (check == null)
            {
                result.Code = 81;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm";
            }
            else
            {
                try
                {
                    var list = new List<WorkerTaskModel>();
                    foreach (var item in check)
                    {
                        var tmp = new WorkerTaskModel
                        {
                            workerTaskId = item.id,
                            leaderTaskId = item.leaderTaskId,
                            stepId = item.stepId,
                            userId = item.createById,                           
                            name = item.name,
                            description = item.description,
                            startTime = item.startTime,
                            endTime = item.endTime,
                            status = item.status,
                            Members = item.WorkerTaskDetails.Select(_ => new TaskMember
                            {
                                memberId = _.User.Id,
                                memberFullName = _.User.fullName,
                            }).ToList(),
                        };
                        list.Add(tmp);
                    }
                    result.Data = new PagingModel()
                    {
                        Data = list,
                        Total = check.Count
                    };
                    result.Succeed = true;

                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                }
            }
            return result;         
        }
    }
}

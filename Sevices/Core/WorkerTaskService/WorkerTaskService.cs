using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using static Data.Models.WorkerTaskModel;

namespace Sevices.Core.WorkerTaskService
{
    public class WorkerTaskService : IWorkerTaskService
    {
        private readonly AppDbContext _dbContext;
        public WorkerTaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreateWorkerTask(Guid userId, CreateWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = new WorkerTask
            {
                managerTaskId = model.managerTaskId,
                name = model.name,
                description = model?.description ?? "",
                startTime = model!.startTime,
                endTime = model.endTime,
                status = (TaskStatus)Data.Enums.TaskStatus.New,
                createById = userId,
                isDeleted = false,
            };

            try
            {
                await _dbContext.WorkerTask.AddAsync(workerTask);

                foreach (var assignee in model.assignees)
                {
                    await _dbContext.WorkerTaskDetail.AddAsync(new WorkerTaskDetail
                    {
                        workerTaskId = workerTask.id,
                        userId = assignee
                    });
                }

                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = workerTask.id;

            }

            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public async Task<ResultModel> UpdateWorkerTask(UpdateWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = await _dbContext.WorkerTask.FindAsync(model.workerTaskId);
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin WorkerTask!";
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
                    var currentWokerTaskDetails = await _dbContext.WorkerTaskDetail
                        .Where(x => x.workerTaskId == model.workerTaskId)
                        .ToListAsync();
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
                            workerTaskId = model.workerTaskId,
                            userId = assignee
                        });
                    }

                    await _dbContext.WorkerTaskDetail.AddRangeAsync(workerTaskDetails);

                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = model.workerTaskId;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public async Task<ResultModel> DeleteWorkerTask(Guid workerTaskId)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WorkerTask.FindAsync(workerTaskId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin WokerTask!";
                return result;
            }
            else
            {
                try
                {
                    check.isDeleted = true;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = workerTaskId;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }            
        }

        public async Task<ResultModel> AssignWorkerTask(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WorkerTaskDetail.SingleOrDefaultAsync(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);
            if (check != null)
            {
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
                    await _dbContext.WorkerTaskDetail.AddAsync(workerTaskDetail);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<ResultModel> UnAssignWorkerTask(AssignWorkerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WorkerTaskDetail.SingleOrDefaultAsync(x => x.userId == model.memberId && x.workerTaskId == model.workerTaskId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Công nhân chưa được thêm vào công việc!";
                return result;
            }
            else
            {
                try
                {
                    _dbContext.WorkerTaskDetail.Remove(check);
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = check.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }
            
        }
        public async Task<ResultModel> UpdateWorkerTaskStatus(Guid workerTaskId, TaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = await _dbContext.WorkerTask.FindAsync(workerTaskId);
            if (workerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin WokerTask!";
                return result;
            }
            else
            {
                try
                {
                    workerTask.status = status;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = workerTaskId;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }           
        }

        public async Task<List<WorkerTaskModel>> GetAllWorkerTask(Guid managerTaskId)
        {          

            var check = await _dbContext.WorkerTask.Include(x => x.WorkerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.managerTaskId == managerTaskId && x.isDeleted == false).ToListAsync();
            var list = new List<WorkerTaskModel>();
            foreach (var item in check)
            {
                var user = await _dbContext.Users.FindAsync(item.createById);
                var tmp = new WorkerTaskModel
                {
                    managerTaskId = item.managerTaskId,
                    userId = item.createById,
                    workerTaskId = item.id,
                    name = item.name,
                    description = item.description,
                    startTime = item.startTime,
                    endTime = item.endTime,
                    status = item.status,
                    userFullName = user!.fullName,
                    Members = item.WorkerTaskDetails.Select(_ => new TaskMember
                    {
                        memberId = _.User.Id,
                        memberFullName = _.User.fullName,
                    }).ToList(),
                };
                list.Add(tmp);
            }
            return list;
        }
       
    }
}

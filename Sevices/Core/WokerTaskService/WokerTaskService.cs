using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.WokerTaskModel;

namespace Sevices.Core.WokerTaskService
{
    public class WokerTaskService : IWokerTaskService
    {
        private readonly AppDbContext _dbContext;
        public WokerTaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreateWokerTask(Guid userId, CreateWokerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var wokerTask = new WokerTask
            {
                managerTaskId = model.managerTaskId,
                name = model.name,
                description = model?.description ?? "",
                timeStart = model.timeStart,
                timeEnd = model.timeEnd,
                status = (TaskStatus)Data.Enums.TaskStatus.New,
                createById = userId,
                isDeleted = false,
            };

            try
            {
                await _dbContext.WokerTask.AddAsync(wokerTask);

                foreach (var assignee in model.assignees)
                {
                    await _dbContext.WokerTaskDetail.AddAsync(new WokerTaskDetail
                    {
                        wokerTaskId = wokerTask.id,
                        userId = new Guid(assignee)
                    });
                }

                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = wokerTask.id;

            }

            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public async Task<ResultModel> UpdateWokerTask(UpdateWokerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = await _dbContext.WokerTask.FindAsync(model.wokerTaskId);
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy WokerTask";
                    return result;
                }
                else
                {
                    check.name = model.name;
                    check.description = model.description;
                    check.timeStart = model.timeStart;
                    check.timeEnd = model.timeEnd;
                    check.status = model.status;

                    // Remove all old woker tasks detail
                    var currentWokerTaskDetails = await _dbContext.WokerTaskDetail
                        .Where(x => x.wokerTaskId == model.wokerTaskId)
                        .ToListAsync();
                    if (currentWokerTaskDetails != null && currentWokerTaskDetails.Count > 0)
                    {
                        _dbContext.WokerTaskDetail.RemoveRange(currentWokerTaskDetails);
                    }

                    // Set new woker tasks detail
                    var wokerTaskDetails = new List<WokerTaskDetail>();
                    foreach (var assignee in model.assignees)
                    {
                        wokerTaskDetails.Add(new WokerTaskDetail
                        {
                            wokerTaskId = model.wokerTaskId,
                            userId = assignee
                        });
                    }

                    await _dbContext.WokerTaskDetail.AddRangeAsync(wokerTaskDetails);

                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = model.wokerTaskId;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public async Task<ResultModel> DeleteWokerTask(Guid wokerTaskId)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WokerTask.FindAsync(wokerTaskId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy WokerTask";
                return result;
            }
            else
            {
                try
                {
                    check.isDeleted = true;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = wokerTaskId;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }            
        }

        public async Task<ResultModel> AssignWokerTask(AssignWokerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WokerTaskDetail.SingleOrDefaultAsync(x => x.userId == model.memberId && x.wokerTaskId == model.wokerTaskId);
            if (check != null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Công nhân đã được thêm vào công việc";
                return result;
            }
            else
            {
                var wokerTaskDetail = new WokerTaskDetail
                {
                    userId = model.memberId,
                    wokerTaskId = model.wokerTaskId,
                };
                try
                {
                    await _dbContext.WokerTaskDetail.AddAsync(wokerTaskDetail);
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = wokerTaskDetail.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }

                return result;
            }         
        }

        public async Task<ResultModel> UnAssignWokerTask(AssignWokerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.WokerTaskDetail.SingleOrDefaultAsync(x => x.userId == model.memberId && x.wokerTaskId == model.wokerTaskId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Công nhân chưa được thêm vào công việc";
                return result;
            }
            else
            {
                try
                {
                    _dbContext.WokerTaskDetail.Remove(check);
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
        public async Task<ResultModel> UpdateWokerTaskStatus(Guid wokerTaskId, TaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var wokerTask = await _dbContext.WokerTask.FindAsync(wokerTaskId);
            if (wokerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy WokerTask";
                return result;
            }
            else
            {
                try
                {
                    wokerTask.status = status;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = wokerTaskId;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }           
        }

        public async Task<List<WokerTaskResponseModel>> GetAllWokerTask(Guid managerTaskId)
        {          

            var check = await _dbContext.WokerTask.Include(x => x.WokerTaskDetails).ThenInclude(x => x.User)
                .Where(x => x.managerTaskId == managerTaskId && x.isDeleted == false).ToListAsync();
            var list = new List<WokerTaskResponseModel>();
            foreach (var item in check)
            {
                var user = await _dbContext.Users.FindAsync(item.createById);
                var tmp = new WokerTaskResponseModel
                {
                    managerTaskId = item.managerTaskId,
                    userId = item.createById,
                    wokerTaskId = item.id,
                    name = item.name,
                    description = item.description,
                    timeStart = item.timeStart,
                    timeEnd = item.timeEnd,
                    status = item.status,
                    userFullName = user!.fullName,
                    Members = item.WokerTaskDetails.Select(_ => new TaskMemberResponse
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

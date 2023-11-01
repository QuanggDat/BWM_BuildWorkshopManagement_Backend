using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.LeaderTaskService
{
    public class LeaderTaskService : ILeaderTaskService
    {
        private readonly AppDbContext _dbContext;

        public LeaderTaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreatedLeaderTask(Guid createById, CreateLeaderTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;            

            var check = await _dbContext.User.FindAsync(model.leaderId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin Leader!";
                return result;
            }
            else
            {
                var orderTmp = await _dbContext.Order.FindAsync(model.orderId);
                if (orderTmp == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                    return result;
                }
                else
                {
                    var procedureTmp = await _dbContext.Order.FindAsync(model.procedureId);
                    if (procedureTmp == null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin quy trình!";
                        return result;
                    }
                    else
                    {
                        var check1 = await _dbContext.LeaderTask.SingleOrDefaultAsync(a => a.procedureId == model.procedureId && a.orderId == model.orderId && a.isDeleted == false);

                        if (check1 != null)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Công việc đã được tạo!";
                            return result;
                        }
                        else
                        {
                            if (orderTmp.status != Data.Enums.OrderStatus.InProgress)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Đơn hàng đang không tiến hành!";
                                return result;
                            }

                            else
                            {
                                if (model.startTime >= model.endTime)
                                {
                                    result.Succeed = false;
                                    result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn hoặc bằng ngày kết thúc!";
                                    return result;
                                }
                                else
                                {

                                    var leaderTask = new LeaderTask
                                    {
                                        leaderId = model.leaderId,
                                        createById = createById,
                                        orderId = model.orderId,
                                        procedureId = model.procedureId,
                                        startTime = model.startTime,
                                        endTime = model.endTime,
                                        description = model.description,
                                        status = 0,
                                        isDeleted = false
                                    };

                                    try
                                    {
                                        await _dbContext.LeaderTask.AddAsync(leaderTask);
                                        await _dbContext.SaveChangesAsync();
                                        result.Succeed = true;
                                        result.Data = leaderTask.id;
                                    }
                                    catch (Exception ex)
                                    {
                                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                    }
                                    return result;
                                }
                            }
                        }
                    }
                }
                
            }           
        }

        public async Task<ResultModel> UpdateLeaderTask(UpdateLeaderTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var leaderTask = await _dbContext.LeaderTask.FindAsync(model.id);
            if (leaderTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin leaderTask!";
                return result;
            }
            else
            {                                               
                if (model.startTime > model.endTime)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!";
                    return result;
                }
                else
                {
                    leaderTask.startTime = model.startTime;
                    leaderTask.endTime = model.endTime;
                    leaderTask.status = model.status;
                    leaderTask.description = model.description;

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        result.Succeed = true;
                        result.Data = leaderTask.id;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                    return result;
                }             
            }      
        }

        public async Task<List<LeaderTaskModel>> GetLeaderTaskByOrderId(Guid orderId)
        {
            var result = new List<LeaderTaskModel>();
            var leaderTask = await _dbContext.LeaderTask.Include(x => x.Procedure)
                .Where(a => a.orderId == orderId && a.isDeleted == false).ToListAsync();
            if (leaderTask == null) { 
                return null; 
            }
            else
            {
                foreach (var item in leaderTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var leaderTmp = await _dbContext.User.FindAsync(item.leaderId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new LeaderTaskModel
                    {
                        createByName = createByTmp!.fullName,
                        leaderName = leaderTmp!.fullName,
                        orderName = orderTmp!.name,
                        createdById = item.createById,
                        name = item.Procedure.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        completedTime = item.completedTime,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
            
        }

        public async Task<List<LeaderTaskModel>> GetLeaderTaskByLeaderId(Guid leaderId)
        {
            var result = new List<LeaderTaskModel>();
            var leaderTask = await _dbContext.LeaderTask.Include(x => x.Procedure)
                .Where(a => a.leaderId == leaderId && a.isDeleted == false).ToListAsync();

            if (leaderTask == null)
            {
                return null;
            }
            else
            {
                foreach (var item in leaderTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var leaderTmp = await _dbContext.User.FindAsync(item.leaderId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new LeaderTaskModel
                    {
                        createByName = createByTmp!.fullName,
                        leaderName = leaderTmp!.fullName,
                        orderName = orderTmp!.name,
                        createdById = item.createById,
                        name = item.Procedure.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
        }

        public async Task<List<LeaderTaskModel>> GetLeaderTaskByForemanId(Guid foremanId)
        {
            var result = new List<LeaderTaskModel>();
            var leaderTask = await _dbContext.LeaderTask.Include(x => x.Procedure)
                .Where(a => a.createById == foremanId && a.isDeleted == false).ToListAsync();
            if (leaderTask == null)
            {
                return null;
            }
            else
            {
                foreach (var item in leaderTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var leaderTmp = await _dbContext.User.FindAsync(item.leaderId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new LeaderTaskModel
                    {
                        createByName = createByTmp!.fullName,
                        leaderName = leaderTmp!.fullName,
                        orderName = orderTmp!.name,
                        createdById = item.createById,
                        name = item.Procedure.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
        }

        public async Task<ResultModel> UpdateLeaderTaskStatus(Guid leaderTaskId, TaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var task = await _dbContext.LeaderTask.FindAsync(leaderTaskId);
            if (task == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc!";
                return result;
            }
            else
            {
                try
                {
                    task.status = status;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = task.id;

                }

                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }
            
        }

        public async Task<ResultModel> DeleteLeaderTask(Guid leaderTaskId)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false; 

            var check = await _dbContext.LeaderTask.FindAsync(leaderTaskId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc!";
                return result;
            }
            else
            {
                try
                {
                    check.isDeleted = true;
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

        public async Task<ResultModel> AssignLeaderTask(Guid leaderTaskId, Guid teamId)
        {

            ResultModel result = new ResultModel();

            result.Succeed = false;
            var task = await _dbContext.LeaderTask.FindAsync(leaderTaskId);
            if (task == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc!";
                return result;
            }
            else
            {
                var check = await _dbContext.Team.SingleOrDefaultAsync(x => x.id == teamId);
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nhóm không hợp lệ!";
                    return result;
                }
                else
                {
                    if (check.isDeleted == true)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Nhóm đã xoá!";
                        return result;
                    }
                    else
                    {
                        try
                        {
                            task.teamId = teamId;
                            await _dbContext.SaveChangesAsync();
                            result.Succeed = true;
                            result.Data = task.id;
                        }
                        catch (Exception ex)
                        {
                            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        }
                        return result;
                    }
                }
            }
        }  

        /*
        public async Task<ResultModel> AssignManagerTask(AssignManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.ManagerTaskGroup.SingleOrDefaultAsync(x => x.groupId == model.groupId && x.managerTaskId == model.taskManagerId);
            
            if (check != null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Tổ đã được gắn vào task!";
                return result;
            }

            var managerTaskGroup = new ManagerTaskGroup
            {
                groupId = model.groupId,
                managerTaskId = model.taskManagerId,
            };
            try
            {
                await _dbContext.ManagerTaskGroup.AddAsync(managerTaskGroup);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = managerTaskGroup.id; 
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
        
        public async Task<ResultModel> UnAssignManagerTask(AssignManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var check = await _dbContext.ManagerTaskGroup.SingleOrDefaultAsync(x => x.groupId == model.groupId && x.managerTaskId == model.taskManagerId);

            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Tổ chưa được gắn vào task!";
                return result;
            }

            try
            {
                _dbContext.ManagerTaskGroup.Remove(check);
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
        */

    }
}

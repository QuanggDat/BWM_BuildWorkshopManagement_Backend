using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sevices.Core.NotificationService;
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
        private readonly INotificationService _notificationService;

        public LeaderTaskService(AppDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public ResultModel Created(Guid createById, CreateLeaderTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.User.Find(model.leaderId);
            if (check == null)
            {
                result.Code = 38;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin trưởng nhóm!";
            }
            else
            {
                var orderTmp = _dbContext.Order.Find(model.orderId);
                if (orderTmp == null)
                {
                    result.Code = 39;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var itemTmp = _dbContext.Item.Find(model.itemId);
                    if (itemTmp == null)
                    {
                        result.Code = 80;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin mặt hàng!";
                    }
                    else
                    {
                        var procedureTmp = _dbContext.Procedure.Find(model.procedureId);
                        if (procedureTmp == null)
                        {
                            result.Code = 40;
                            result.Succeed = false;
                            result.ErrorMessage = "Không tìm thấy thông tin quy trình!";
                        }
                        else
                        {
                            var check1 = _dbContext.LeaderTask.SingleOrDefault(a => a.orderId == model.orderId && a.itemId == model.itemId && a.procedureId == model.procedureId && a.isDeleted == false);

                            if (check1 != null)
                            {
                                result.Code = 41;
                                result.Succeed = false;
                                result.ErrorMessage = "Công việc đã được tạo!";
                            }
                            else
                            {
                                if (orderTmp.status != OrderStatus.InProgress)
                                {
                                    result.Code = 42;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Đơn hàng đang không tiến hành!";
                                }

                                else
                                {
                                    if (model.startTime >= model.endTime)
                                    {
                                        result.Code = 43;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn hoặc bằng ngày kết thúc!";
                                    }
                                    else
                                    {
                                        var item = _dbContext.Item.Find(model.itemId);
                                        var procedure = _dbContext.Procedure.Find(model.procedureId);
                                        var leaderTask = new LeaderTask
                                        {
                                            createById = createById,
                                            leaderId = model.leaderId,
                                            orderId = model.orderId,
                                            itemId = model.itemId,
                                            itemName = item!.name,
                                            procedureId = model.procedureId,
                                            name = procedure!.name,
                                            startTime = model.startTime,
                                            endTime = model.endTime,
                                            description = model.description,
                                            status = 0,
                                            isDeleted = false
                                        };

                                        try
                                        {
                                            _dbContext.LeaderTask.Add(leaderTask);
                                            _dbContext.SaveChanges();
                                            result.Succeed = true;
                                            result.Data = leaderTask.id;
                                        }
                                        catch (Exception ex)
                                        {
                                            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public ResultModel Update(UpdateLeaderTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var leaderTask = _dbContext.LeaderTask.Find(model.id);
            if (leaderTask == null)
            {
                result.Code = 38;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
            }
            else
            {
                if (model.startTime > model.endTime)
                {
                    result.Code = 43;
                    result.Succeed = false;
                    result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!";
                }
                else
                {
                    leaderTask.name = model.name;
                    leaderTask.startTime = model.startTime;
                    leaderTask.endTime = model.endTime;
                    leaderTask.status = model.status;
                    leaderTask.description = model.description;

                    try
                    {
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = leaderTask.id;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.LeaderTask.Find(id);
            if (check == null)
            {
                result.Code = 44;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
            }
            else
            {
                try
                {
                    check.isDeleted = true;
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

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.LeaderTask.Where(x => x.id == id && x.isDeleted != true)
                    .FirstOrDefault();

                if (check == null)
                {
                    result.Code = 38;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                }
                else
                {
                    var leaderTaskModel = new LeaderTaskModel
                    {
                        createdById = check.createById,
                        leaderId = check.leaderId,
                        orderId = check.orderId,
                        itemName = check.itemName,
                        name = check.name,
                        startTime = check.startTime,
                        endTime = check.endTime,
                        status = check.status,
                        completedTime = check.completedTime,
                        description = check.description,
                        isDeleted = check.isDeleted,
                    };
                    result.Data = leaderTaskModel;
                    result.Succeed = true;
                };
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByOrderId(Guid orderId)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.LeaderTask
                .Where(a => a.orderId == orderId && a.isDeleted == false)
                .OrderByDescending(x => x.itemName).ToList();

            if (check == null)
            {
                result.Code = 33;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin đơn hàng !";
            }
            else
            {
                try
                {
                    var list = new List<LeaderTaskModel>();
                    foreach (var item in check)
                    {
                        var tmp = new LeaderTaskModel
                        {
                            id = item.id,
                            createdById = item.createById,
                            leaderId = item.leaderId,
                            itemName = item.itemName,
                            name = item.name,
                            startTime = item.startTime,
                            endTime = item.endTime,
                            status = item.status,
                            completedTime = item.completedTime,
                            description = item.description,
                            isDeleted = item.isDeleted,
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

        public ResultModel GetByOrderIdAndLeaderId(Guid orderId, Guid leaderId)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var checkOrder = _dbContext.LeaderTask
                .Where(a => a.orderId == orderId && a.isDeleted == false).SingleOrDefault();
            if (checkOrder == null)
            {
                result.Code = 39;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
            }
            else
            {
                var checkLeader = _dbContext.LeaderTask
                .Where(a => a.leaderId == leaderId && a.isDeleted == false).SingleOrDefault();
                if (checkLeader == null)
                {
                    result.Code = 38;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin trưởng nhóm!";
                }
                else
                {
                    var listLeaderTask = _dbContext.LeaderTask
                    .Where(a => a.orderId == orderId && a.leaderId == leaderId && a.isDeleted == false)
                    .OrderByDescending(x => x.itemName).ToList();
                    try
                    {
                        var list = new List<LeaderTaskModel>();
                        foreach (var item in listLeaderTask)
                        {
                            var tmp = new LeaderTaskModel
                            {
                                id = item.id,
                                createdById = item.createById,
                                itemName = item.itemName,
                                name = item.name,
                                startTime = item.startTime,
                                endTime = item.endTime,
                                status = item.status,
                                completedTime = item.completedTime,
                                description = item.description,
                                isDeleted = item.isDeleted,
                            };
                            list.Add(tmp);
                        }
                        result.Data = new PagingModel()
                        {
                            Data = list,
                            Total = listLeaderTask.Count
                        };
                        result.Succeed = true;

                    }
                    catch (Exception e)
                    {
                        result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                    }
                }
            }
            return result;
        }

        public ResultModel GetByLeaderId(Guid leaderId)
        {
            var result = new ResultModel();
            result.Succeed = false;
            var checkLeader = _dbContext.LeaderTask
                    .Where(a => a.leaderId == leaderId && a.isDeleted == false)
                    .OrderByDescending(x => x.itemName).ToList();
            if (checkLeader == null)
            {
                result.Code = 38;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin trưởng nhóm!";
            }
            else 
            {
                try
                {
                    var list = new List<LeaderTaskModel>();
                    foreach (var item in checkLeader)
                    {
                        var tmp = new LeaderTaskModel
                        {
                            id = item.id,
                            createdById = item.createById,
                            itemName = item.itemName,
                            name = item.name,
                            startTime = item.startTime,
                            endTime = item.endTime,
                            status = item.status,
                            completedTime = item.completedTime,
                            description = item.description,
                            isDeleted = item.isDeleted,
                        };
                        list.Add(tmp);
                    }
                    result.Data = new PagingModel()
                    {
                        Data = list,
                        Total = checkLeader.Count
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

        public ResultModel UpdateStatus(Guid id, ETaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var task = _dbContext.LeaderTask.Find(id);
            if (task == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                return result;
            }
            else
            {
                try
                {
                    task.status = status;
                    _dbContext.SaveChanges();
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

       #region Comment
        /*
         public async Task<List<LeaderTaskModel>> GetByLeaderId(Guid leaderId)
        {
            var result = new List<LeaderTaskModel>();
            var leaderTask = await _dbContext.LeaderTask.Include(x => x.Procedure)
                .Where(a => a.leaderId == leaderId && a.isDeleted == false).ToListAsync();

            if (leaderTask == null)
            {
                return null!;
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

        public async Task<List<LeaderTaskModel>> GetByForemanId(Guid foremanId)
        {
            var result = new List<LeaderTaskModel>();
            var leaderTask = await _dbContext.LeaderTask.Include(x => x.Procedure)
                .Where(a => a.createById == foremanId && a.isDeleted == false).ToListAsync();
            if (leaderTask == null)
            {
                return null!;
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
         
        public async Task<ResultModel> AssignLeaderTask(Guid leaderTaskId, Guid teamId)
        {

            ResultModel result = new ResultModel();

            result.Succeed = false;
            var task = await _dbContext.LeaderTask.FindAsync(leaderTaskId);
            if (task == null)
            {
                result.Code = 44;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                return result;
            }
            else
            {
                var check = await _dbContext.Team.SingleOrDefaultAsync(x => x.id == teamId);
                if (check == null)
                {
                    result.Code = 45;
                    result.Succeed = false;
                    result.ErrorMessage = "Nhóm không hợp lệ!";
                    return result;
                }
                else
                {
                    if (check.isDeleted == true)
                    {
                        result.Code = 46;
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
        #endregion
    }
}

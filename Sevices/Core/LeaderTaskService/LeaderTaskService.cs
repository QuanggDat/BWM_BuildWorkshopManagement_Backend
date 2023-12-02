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

        public ResultModel Create(Guid createById, CreateLeaderTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var leaderTmp = _dbContext.User.Include(x => x.Role)
                .FirstOrDefault(x => x.Id == model.leaderId && x.Role != null && x.Role.Name == "Leader" && x.banStatus == false);

            if (leaderTmp == null)
            {
                result.Code = 38;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin tổ trưởng!";
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
                        result.Code = 14;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin sản phẩm!";
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(itemTmp.drawingsTechnical)  || string.IsNullOrWhiteSpace(itemTmp.drawings2D) || string.IsNullOrWhiteSpace(itemTmp.drawings3D))
                        {
                            result.Code = 103;
                            result.Succeed = false;
                            result.ErrorMessage = "Sản phẩm chưa đủ 3 bản vẽ, hãy cập nhập đủ bản vẽ trong sản phẩm trước khi tạo công việc!";
                        }
                        else
                        {
                            var check = _dbContext.LeaderTask.FirstOrDefault(a => a.orderId == model.orderId && a.itemId == model.itemId && a.name == model.name && a.isDeleted == false);

                            if (check != null)
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
                                        var checkPriority = _dbContext.LeaderTask.FirstOrDefault(x => x.orderId == model.orderId && x.itemId == model.itemId && x.priority == model.priority && x.isDeleted == false);

                                        if (checkPriority != null)
                                        {
                                            result.Code = 91;
                                            result.Succeed = false;
                                            result.ErrorMessage = "Mức độ ưu tiên đã tồn tại !";
                                        }
                                        else
                                        {
                                            var leaderTask = new LeaderTask
                                            {
                                                createById = createById,
                                                leaderId = model.leaderId,
                                                orderId = model.orderId,
                                                itemId = model.itemId,
                                                priority = model.priority,
                                                itemQuantity = model.itemQuantity,
                                                name = model.name,
                                                startTime = model.startTime,
                                                endTime = model.endTime,
                                                description = model.description,
                                                status = ETaskStatus.New,
                                                isDeleted = false
                                            };

                                            try
                                            {
                                                _dbContext.LeaderTask.Add(leaderTask);
                                                _dbContext.SaveChanges();

                                                _notificationService.Create(new Notification
                                                {
                                                    userId = model.leaderId,
                                                    leaderTaskId = leaderTask.id,
                                                    title = "Công việc",
                                                    content = "Bạn vừa nhận được 1 công việc mới!",
                                                    type = NotificationType.TaskReport
                                                });

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
            }
            return result;
        }

        public ResultModel CreateAcceptanceTask(Guid createById, CreateAcceptanceTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var leaderTmp = _dbContext.User.Find(model.leaderId);

            if (leaderTmp == null)
            {
                result.Code = 38;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin tổ trưởng!";
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
                            var check = _dbContext.LeaderTask.FirstOrDefault(a => a.orderId == model.orderId && a.name == "Công việc nghiệm thu" && a.isDeleted == false);

                            if (check != null)
                            {
                                result.Code = 87;
                                result.Succeed = false;
                                result.ErrorMessage = "Công việc nghiệm thu cho đơn hàng này đã được tạo!";
                            }
                            else
                            {
                                var leaderTask = new LeaderTask
                                {
                                    createById = createById,
                                    leaderId = model.leaderId,
                                    orderId = model.orderId,
                                    name = "Công việc nghiệm thu",
                                    startTime = model.startTime,
                                    endTime = model.endTime,
                                    description = model.description,
                                    status = ETaskStatus.Acceptance,
                                    isDeleted = false
                                };

                                try
                                {
                                    _dbContext.LeaderTask.Add(leaderTask);
                                    _dbContext.SaveChanges();

                                    _notificationService.Create(new Notification
                                    {
                                        userId = model.leaderId,
                                        leaderTaskId = leaderTask.id,
                                        title = "Công việc",
                                        content = "Bạn vừa nhận được 1 công việc nghiệm thu mới!",
                                        type = NotificationType.TaskReport
                                    });

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
                result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
            }
            else
            {            
                if (model.startTime >= model.endTime)
                {
                    result.Code = 43;
                    result.Succeed = false;
                    result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!";
                }               
                else
                {
                    var checkPriority = _dbContext.LeaderTask.FirstOrDefault(x => x.priority != leaderTask.priority && x.orderId == leaderTask!.orderId && x.itemId == leaderTask.itemId && x.priority == model.priority && x.isDeleted == false); 
                    
                    if (checkPriority != null)
                    {
                        result.Code = 91;
                        result.Succeed = false;
                        result.ErrorMessage = "Mức độ ưu tiên đã tồn tại !";
                    }
                    else
                    {
                        leaderTask.name = model.name;
                        leaderTask.leaderId = model.leaderId;
                        leaderTask.priority = model.priority;
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
                result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
            }
            else
            {
                try
                {
                    var currentWorkerTasks = _dbContext.WorkerTask.Where(x => x.leaderTaskId == id).ToList();

                    if (currentWorkerTasks != null && currentWorkerTasks.Count > 0)
                    {
                        foreach (var workerTask in currentWorkerTasks)
                        {
                            workerTask.isDeleted = true;
                        }
                        _dbContext.WorkerTask.UpdateRange(currentWorkerTasks);
                    }

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
                var check = _dbContext.LeaderTask.Include(x => x.Leader).Include(x => x.Item)
                    .Include(x => x.Reports).ThenInclude(x => x.Resources)
                    .FirstOrDefault(x => x.id == id && x.isDeleted != true);

                if (check == null)
                {
                    result.Code = 44;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
                }
                else
                {
                    var order = _dbContext.Order.Find(check.orderId);

                    var createBy = _dbContext.User.Find(check.createById);

                    var leaderTaskModel = new LeaderTaskModel
                    {
                        id = check.id,
                        createdById = check.createById,
                        createdByName = createBy?.fullName ?? "",
                        leaderId = check.leaderId,
                        leaderName = check.Leader?.fullName ?? "",
                        orderId = check.orderId,
                        orderName = order!.name,
                        itemId = check.itemId,
                        Item = check.Item,
                        name = check.name,
                        priority = check.priority,                        
                        itemQuantity = check.itemQuantity,
                        itemCompleted = check.itemCompleted,
                        itemFailed = check.itemFailed,
                        startTime = check.startTime,
                        endTime = check.endTime,                        
                        completedTime = check.completedTime,
                        status = check.status,
                        description = check.description,

                        listReportInTasks = check.Reports.Select(x => new TaskReportModel
                        {
                            id = x.id,
                            reportType = x.reportType,
                            title = x.title,
                            content = x.content,
                            createdDate = x.createdDate,
                            resource = x.Resources.Select(x => x.link).ToList()
                        }).ToList(),

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

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listLeaderTask = _dbContext.LeaderTask.Include(x => x.Leader).Include(x => x.Item)
                .Include(x => x.Reports).ThenInclude(x => x.Resources)
                .Where(a => a.isDeleted == false).OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listLeaderTask = listLeaderTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listLeaderTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LeaderTaskModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var order = _dbContext.Order.Find(item.orderId);

                    var createBy = _dbContext.User.Find(item.createById);

                    var tmp = new LeaderTaskModel
                    {
                        id = item.id,
                        createdById = item.createById,
                        createdByName = createBy?.fullName?? "",
                        leaderId = item.leaderId,
                        leaderName = item.Leader?.fullName ?? "",
                        orderId = item.orderId,
                        orderName = order!.name,
                        itemId = item.itemId,
                        Item = item.Item,
                        itemQuantity = item.itemQuantity,
                        itemCompleted = item.itemCompleted,
                        itemFailed = item.itemFailed,
                        name = item.name,
                        priority = item.priority,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completedTime = item.completedTime,
                        status = item.status,
                        description = item.description,

                        listReportInTasks = item.Reports.Select(x => new TaskReportModel
                        {
                            id = x.id,
                            reportType = x.reportType,
                            title = x.title,
                            content = x.content,
                            createdDate = x.createdDate,
                            resource = x.Resources.Select(x => x.link).ToList()
                        }).ToList(),

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
            return result;
        }

        public ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listLeaderTask = _dbContext.LeaderTask.Include(x => x.Leader).Include(x => x.Item)
                .Include(x => x.Reports).ThenInclude(x => x.Resources)
                .Where(a => a.orderId == orderId && a.isDeleted == false)
                .OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listLeaderTask = listLeaderTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listLeaderTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LeaderTaskModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var order = _dbContext.Order.Find(item.orderId);

                    var createBy = _dbContext.User.Find(item.createById);

                    var tmp = new LeaderTaskModel
                    {
                        id = item.id,
                        createdById = item.createById,
                        createdByName = createBy?.fullName ?? "",
                        leaderId = item.leaderId,
                        leaderName = item.Leader?.fullName ?? "",
                        orderId = item.orderId,
                        orderName = order!.name,
                        itemId = item.itemId,
                        Item = item.Item,
                        itemQuantity = item.itemQuantity,
                        itemCompleted = item.itemCompleted,
                        itemFailed = item.itemFailed,
                        name = item.name,
                        priority = item.priority,                       
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completedTime = item.completedTime,
                        status = item.status,                      
                        description = item.description,

                        listReportInTasks = item.Reports.Select(x => new TaskReportModel
                        {
                            id = x.id,
                            reportType = x.reportType,
                            title = x.title,
                            content = x.content,
                            createdDate = x.createdDate,
                            resource = x.Resources.Select(x => x.link).ToList()
                        }).ToList(),

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
            return result;
        }

        public ResultModel GetByLeaderId(Guid leaderId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listLeaderTask = _dbContext.LeaderTask.Include(x => x.Reports).ThenInclude(x => x.Resources)
                .Include(x => x.Leader).Include(x => x.Order).Include(x => x.Item)
                    .Where(a => a.leaderId == leaderId && a.Order.status == OrderStatus.InProgress && a.isDeleted == false)
                    .OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listLeaderTask = listLeaderTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listLeaderTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LeaderTaskModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var order = _dbContext.Order.Find(item.orderId);

                    var createBy = _dbContext.User.Find(item.createById);

                    var tmp = new LeaderTaskModel
                    {
                        id = item.id,
                        createdById = item.createById,
                        createdByName = createBy?.fullName ?? "",
                        leaderId = item.leaderId,
                        leaderName = item.Leader?.fullName ?? "",
                        orderId = item.orderId,
                        orderName = order!.name,
                        itemId = item.itemId,
                        Item = item.Item,
                        itemQuantity = item.itemQuantity,
                        itemCompleted = item.itemCompleted,
                        itemFailed = item.itemFailed,
                        name = item.name,
                        priority = item.priority,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completedTime = item.completedTime,
                        status = item.status,
                        description = item.description,

                        listReportInTasks = item.Reports.Select(x => new TaskReportModel
                        {
                            id = x.id,
                            reportType = x.reportType,
                            title = x.title,
                            content = x.content,
                            createdDate = x.createdDate,
                            resource = x.Resources.Select(x => x.link).ToList()
                        }).ToList(),

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
            return result;
        }

        public ResultModel GetByOrderIdAndItemId(Guid orderId, Guid itemId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listLeaderTask = _dbContext.LeaderTask.Include(x => x.Reports).ThenInclude(x => x.Resources)
                .Include(x => x.Leader).Include(x => x.Order).Include(x => x.Item).Include(x => x.WorkerTasks)
                    .Where(a => a.orderId == orderId && a.itemId == itemId && a.isDeleted == false)
                    .OrderByDescending(x => x.startTime).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listLeaderTask = listLeaderTask.Where(x => x.name.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listLeaderTask.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LeaderTaskModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var order = _dbContext.Order.Find(item.orderId);

                    var createBy = _dbContext.User.Find(item.createById);

                    var tmp = new LeaderTaskModel
                    {
                        id = item.id,
                        createdById = item.createById,
                        createdByName = createBy?.fullName ?? "",
                        leaderId = item.leaderId,
                        leaderName = item.Leader?.fullName ?? "",
                        orderId = item.orderId,
                        orderName = order!.name,
                        itemId = item.itemId,
                        Item = item.Item,
                        itemQuantity = item.itemQuantity,
                        itemCompleted = item.itemCompleted,
                        itemFailed = item.itemFailed,
                        name = item.name,
                        priority = item.priority,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        completedTime = item.completedTime,
                        status = item.status,
                        description = item.description,

                        listReportInTasks = item.Reports.Select(x => new TaskReportModel
                        {
                            id = x.id,
                            reportType = x.reportType,
                            title = x.title,
                            content = x.content,
                            createdDate = x.createdDate,
                            resource = x.Resources.Select(x => x.link).ToList()
                        }).ToList(),

                        listWorkerTasks = item.WorkerTasks.Select(x => new WorkerTaskModel
                        {
                            id = x.id,
                            name = x.name
                        }).ToList(),

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
            return result;
        }

        public ResultModel UpdateStatus(Guid id, ETaskStatus status)
        {
            ResultModel result = new ResultModel();

            result.Succeed = false;

            var task = _dbContext.LeaderTask.Find(id);

            if (task == null)
            {
                result.Code = 44;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
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
            }
            return result;
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

using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Sevices.Core.NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.TaskReportModel;

namespace Sevices.Core.ReportService
{
    public class TaskReportService : ITaskReportService
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public TaskReportService(AppDbContext dbContext, INotificationService notificationService, IMapper mapper)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public ResultModel CreateAcceptanceReport(Guid reporterId, CreateAcceptanceReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);

            if (user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải tổ trưởng!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask.FirstOrDefault(x => x.id == model.acceptanceTaskId && x.name == "Công việc nghiệm thu");

                if (leaderTask == null)
                {
                    result.Code = 23;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc nghiệm thu!";
                }
                else
                {
                    var checkReport = _dbContext.Report.Any(x => x.leaderTaskId == model.acceptanceTaskId && x.reportType == ReportType.AcceptanceReport);

                    if (checkReport == true)
                    {
                        result.Code = 52;
                        result.Succeed = false;
                        result.ErrorMessage = "Báo cáo nghiệm thu cho công việc này đã được thực hiện!";
                    }
                    else
                    {
                        var report = new Report
                        {
                            leaderTaskId = model.acceptanceTaskId,
                            reporterId = reporterId,
                            title = model.title,
                            content = model.content,
                            reportType = ReportType.AcceptanceReport,
                            createdDate = DateTime.UtcNow.AddHours(7),
                        };

                        var order = _dbContext.Order.Where(x => x.id == leaderTask.orderId).FirstOrDefault();

                        if (order != null)
                        {
                            order.acceptanceTime = DateTime.Now;
                        }

                        try
                        {
                            _dbContext.Report.Add(report);

                            if (model.resource != null)
                            {
                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        reportId = report.id,
                                        link = resource
                                    });
                                }

                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        orderId = order!.id,
                                        link = resource
                                    });
                                }
                            }
                            _dbContext.SaveChanges();

                            if (leaderTask.createById != null)
                            {
                                _notificationService.Create(new Notification
                                {
                                    userId = leaderTask.createById.Value,
                                    reportId = report.id,
                                    title = "Báo cáo nghiệm thu",
                                    content = "Bạn vừa nhận được 1 báo cáo nghiệm thu mới!",
                                    type = NotificationType.TaskReport
                                });
                            }

                            result.Succeed = true;
                            result.Data = report.id;
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
        public ResultModel CreateProgressReport(Guid reporterId, CreateProgressReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);

            if (user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải tổ trưởng!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask.FirstOrDefault(x => x.id == model.leaderTaskId);

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
                }
                else
                {
                    if (model.itemFailed > leaderTask.itemQuantity)
                    {
                        result.Code = 98;
                        result.Succeed = false;
                        result.ErrorMessage = "Số lượng mặt hàng thất bại không lớn hơn số lượng mặt hàng!";
                    }
                    else
                    {
                        var report = new Report
                        {
                            reporterId = reporterId,
                            leaderTaskId = model.leaderTaskId,
                            title = model.title,
                            content = model.content,
                            itemFailed = model.itemFailed,
                            reportType = ReportType.ProgressReport,
                            status = ReportStatus.Pending,  
                            createdDate = DateTime.UtcNow.AddHours(7),
                        };

                        try
                        {
                            _dbContext.Report.Add(report);

                            if (model.resource != null)
                            {
                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        reportId = report.id,
                                        link = resource
                                    });
                                }
                            }

                            _dbContext.Report.Add(report);
                            _dbContext.SaveChanges();

                            if (leaderTask.createById != null)
                            {
                                _notificationService.Create(new Notification
                                {
                                    userId = leaderTask.createById.Value,
                                    reportId = report.id,
                                    title = "Báo cáo tiến độ",
                                    content = "Bạn vừa nhận được 1 báo cáo tiến độ mới!",
                                    type = NotificationType.TaskReport
                                });
                            }

                            result.Succeed = true;
                            result.Data = report.id;
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

        public ResultModel CreateProblemReport(Guid reporterId, CreateProblemReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);

            if (user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải tổ trưởng !!!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask.FirstOrDefault(x => x.id == model.leaderTaskId);

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc tổ trưởng!";
                }
                else
                {
                    var canSendReport = CanSendProblemTaskReport(leaderTask);

                    if (canSendReport == false)
                    {
                        result.Code = 53;
                        result.Succeed = false;
                        result.ErrorMessage = "Đã qua ngày kết thúc công việc, không thể gửi báo cáo vào lúc này!";
                    }
                    else
                    {
                        var report = new Report
                        {
                            leaderTaskId = model.leaderTaskId,
                            reporterId = reporterId,
                            title = model.title,
                            content = model.content,
                            reportType = ReportType.ProblemReport,
                            status = ReportStatus.Pending,
                            createdDate = DateTime.UtcNow.AddHours(7),
                        };

                        try
                        {
                            _dbContext.Report.Add(report);

                            if (model.resource != null)
                            {
                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        reportId = report.id,
                                        link = resource
                                    });
                                }
                            }

                            if (model.listSupply.Any())
                            {
                                var listMaterialId = model.listSupply.Select(x => x.materialId).Distinct().ToList();

                                var listMaterial = _dbContext.Material.Where(x => listMaterialId.Contains(x.id) && !x.isDeleted).ToList();

                                var listSupply = new List<Supply>();

                                foreach (var supply in model.listSupply)
                                {
                                    var mate = listMaterial.FirstOrDefault(x => x.id == supply.materialId);
                                    var matePrice = mate != null ? mate.price : 0;
                                    var newSupply = new Supply()
                                    {
                                        reportId = report.id,
                                        materialId = supply.materialId,
                                        materialName = mate.name,
                                        materialSupplier = mate.supplier,
                                        materialThickness = mate.thickness,
                                        materialSku = mate.sku,
                                        materialColor = mate.color,
                                        materialUnit = mate.unit,
                                        amount = supply.amount,
                                        price = matePrice,
                                        totalPrice = matePrice * supply.amount,
                                        status = model.supplyStatus,
                                    };
                                    listSupply.Add(newSupply);
                                }
                                _dbContext.Supply.AddRange(listSupply);
                            }

                            _dbContext.SaveChanges();

                            if (leaderTask.createById != null)
                            {
                                _notificationService.Create(new Notification
                                {
                                    userId = leaderTask.createById.Value,
                                    reportId = report.id,
                                    title = "Báo cáo vấn đề",
                                    content = "Bạn vừa nhận được 1 báo cáo vấn đề mới!",
                                    type = NotificationType.TaskReport
                                });
                            }

                            result.Succeed = true;
                            result.Data = report.id;
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

        public ResultModel Update(UpdateTaskReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.Report.Find(model.id);

            if (check == null)
            {
                result.Code = 60;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
            }
            else
            {
                if (DateTime.Now >= check.createdDate.AddDays(1))
                {
                    result.Code = 100;
                    result.Succeed = false;
                    result.ErrorMessage = "Đã quá hạn sửa báo cáo!";
                }
                else
                {
                    if (check.reportType == ReportType.ProgressReport && check.status != ReportStatus.Pending)
                    {
                        result.Code = 111;
                        result.Succeed = false;
                        result.ErrorMessage = "Báo cáo đã nhận phản hồi, không thể chỉnh sửa!";
                    }
                    else
                    {
                        check.title = model.title;
                        check.content = model.content;

                        // Remove all old resource
                        var currentResources = _dbContext.Resource.Where(x => x.reportId == check.id).ToList();

                        if (currentResources != null && currentResources.Count > 0)
                        {
                            _dbContext.Resource.RemoveRange(currentResources);
                        }

                        if (model.resource != null)
                        {
                            foreach (var resource in model.resource)
                            {
                                _dbContext.Resource.Add(new Resource
                                {
                                    reportId = check.id,
                                    link = resource
                                });
                            }
                        }
                        try
                        {
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = check.id;
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

        public ResultModel UpdateStatusReport(Guid id, ReportStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.Report.Find(id);

            if (check == null)
            {
                result.Code = 60;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
            }
            else
            {
                check.status = status;
                try
                {
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

        public ResultModel UpdateProblemTaskReport(UpdateProblemTaskReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = _dbContext.Report.Find(model.id);

            if (check == null)
            {
                result.Code = 60;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
            }
            else
            {
                if (DateTime.Now >= check.createdDate.AddDays(1))
                {
                    result.Code = 100;
                    result.Succeed = false;
                    result.ErrorMessage = "Đã quá hạn sửa báo cáo!";
                }
                else
                {
                    if (check.status != ReportStatus.Pending)
                    {
                        result.Code = 111;
                        result.Succeed = false;
                        result.ErrorMessage = "Báo cáo đã nhận phản hồi, không thể chỉnh sửa!";
                    }
                    else
                    {
                        check.title = model.title;
                        check.content = model.content;

                        // Remove all old resource
                        var currentResources = _dbContext.Resource.Where(x => x.reportId == check.id).ToList();

                        if (currentResources != null && currentResources.Count > 0)
                        {
                            _dbContext.Resource.RemoveRange(currentResources);
                        }

                        if (model.resource != null)
                        {
                            foreach (var resource in model.resource)
                            {
                                _dbContext.Resource.Add(new Resource
                                {
                                    reportId = check.id,
                                    link = resource
                                });
                            }
                        }

                        // Remove all old supply
                        var currentSupply = _dbContext.Supply.Where(x => x.reportId == check.id).ToList();

                        if (currentSupply != null && currentSupply.Count > 0)
                        {
                            _dbContext.Supply.RemoveRange(currentSupply);
                        }

                        if (model.listSupply.Any())
                        {
                            var listMaterialId = model.listSupply.Select(x => x.materialId).Distinct().ToList();

                            var listMaterial = _dbContext.Material.Where(x => listMaterialId.Contains(x.id) && !x.isDeleted).ToList();

                            var listSupply = new List<Supply>();
                            foreach (var supply in model.listSupply)
                            {
                                var mate = listMaterial.FirstOrDefault(x => x.id == supply.materialId);
                                var matePrice = mate != null ? mate.price : 0;
                                var newSupply = new Supply()
                                {
                                    reportId = check.id,
                                    materialId = supply.materialId,
                                    materialName = mate.name,
                                    materialSupplier = mate.supplier,
                                    materialThickness = mate.thickness,
                                    materialSku = mate.sku,
                                    materialColor = mate.color,
                                    materialUnit = mate.unit,
                                    amount = supply.amount,
                                    price = matePrice,
                                    totalPrice = matePrice * supply.amount,
                                    status = model.supplyStatus,
                                };
                                listSupply.Add(newSupply);
                            }
                            _dbContext.Supply.AddRange(listSupply);
                        }
                        try
                        {
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = check.id;
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

        public ResultModel SendProgressReportFeedback(SendProgressReportFeedbackModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var report = _dbContext.Report.Include(x => x.LeaderTask)
                .Where(x => x.id == model.reportId).FirstOrDefault();

            if (report == null && report!.reportType != ReportType.ProgressReport)
            {
                result.Code = 54;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo tiến độ!";
            }
            else
            {
                if (report.status == ReportStatus.Complete)
                {
                    result.Code = 55;
                    result.Succeed = false;
                    result.ErrorMessage = "Báo cáo này đã hoàn thành!";
                }
                else
                {
                    var leaderTask = _dbContext.LeaderTask.Find(report.leaderTaskId);

                    try
                    {
                        if (leaderTask != null && model.status == ReportStatus.Complete)
                        {
                            leaderTask.itemFailed = report.itemFailed;
                            leaderTask.itemCompleted = leaderTask.itemQuantity - leaderTask.itemFailed;
                            leaderTask.completedTime = DateTime.Now;
                            leaderTask.status = ETaskStatus.Completed;
                        }
                        else if (leaderTask != null && model.status == ReportStatus.NotAchieved)
                        {
                            leaderTask.status = ETaskStatus.NotAchieved;
                        }

                        report.responseContent = model.responseContent;
                        _dbContext.SaveChanges();

                        _notificationService.Create(new Notification
                        {
                            userId = report.reporterId,
                            reportId = report.id,
                            title = "Phản hồi báo cáo tiến độ",
                            content = "Bạn vừa nhận được 1 phản hồi báo cáo tiến độ mới!",
                            type = NotificationType.TaskReport
                        });

                        result.Succeed = true;
                        result.Data = report.id;
                    }

                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }

        public ResultModel SendProblemReportFeedback(SendProblemReportFeedbackModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var report = _dbContext.Report.Include(x => x.LeaderTask)
                .FirstOrDefault(x => x.id == model.reportId);

            if (report == null && report!.reportType != ReportType.ProblemReport)
            {
                result.Code = 13;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo vấn đề!";
            }
            else
            {
                try
                {
                    report.responseContent = model.responseContent;
                    report.status = model.status;
                    _dbContext.SaveChanges();

                    _notificationService.Create(new Notification
                    {
                        userId = report.reporterId,
                        reportId = report.id,
                        title = "Phản hồi báo cáo vấn đề",
                        content = "Bạn vừa nhận được 1 phản hồi báo cáo vấn đề mới!",
                        type = NotificationType.TaskReport
                    });

                    result.Succeed = true;
                    result.Data = report.id;
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
            result.Succeed = false;
            try
            {
                var report = _dbContext.Report.Include(x => x.Reporter).Include(x => x.Resources).Include(x => x.Supplies)
                    .Include(x => x.LeaderTask).ThenInclude(x => x.CreateBy).FirstOrDefault(x => x.id == id);

                if (report == null)
                {
                    result.Code = 54;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
                }
                else
                {
                    if (report.reportType == ReportType.ProgressReport)
                    {
                        var taskReport = new TaskReportModel
                        {
                            id = report.id,
                            leaderTaskId = report.LeaderTask.id,
                            leaderTaskName = report.LeaderTask.name,
                            reporterId = report.reporterId,
                            reporterName = report.Reporter.fullName,
                            responderId = report.LeaderTask.createById,
                            responderName = report.LeaderTask.CreateBy?.fullName ?? "",
                            reportType = report.reportType,
                            title = report.title,
                            content = report.content,
                            createdDate = report.createdDate,
                            status = report.status,
                            responseContent = report.responseContent,
                            itemQuantity = report.LeaderTask.itemQuantity,
                            itemFailed = report.itemFailed,                                                    
                            resource = report.Resources.Select(x => x.link).ToList()
                        };
                        result.Data = taskReport;
                        result.Succeed = true;
                    }
                    else if(report.reportType == ReportType.ProblemReport)
                    {
                        var taskReport = new TaskReportModel
                        {
                            id = report.id,

                            leaderTaskId = report.LeaderTask.id,
                            leaderTaskName = report.LeaderTask.name,
                            reporterId = report.reporterId,
                            reporterName = report.Reporter.fullName,
                            responderId = report.LeaderTask.createById,
                            responderName = report.LeaderTask.CreateBy?.fullName ?? "",
                            reportType = report.reportType,
                            status = report.status,
                            title = report.title,
                            content = report.content,
                            createdDate = report.createdDate,
                            responseContent = report.responseContent,
                            resource = report.Resources.Select(x => x.link).ToList(),
                            listSupply = _mapper.Map<List<SupplyModel>>(report.Supplies)
                        };
                        result.Data = taskReport;
                        result.Succeed = true;
                    }
                    else
                    {
                        var taskReport = new TaskReportModel
                        {
                            id = report.id,
                            leaderTaskId = report.LeaderTask.id,
                            leaderTaskName = report.LeaderTask.name,
                            reporterId = report.reporterId,
                            reporterName = report.Reporter.fullName,
                            responderId = report.LeaderTask.createById,
                            responderName = report.LeaderTask.CreateBy?.fullName ?? "",
                            title = report.title,
                            content = report.content,
                            createdDate = report.createdDate,
                            resource = report.Resources.Select(x => x.link).ToList(),
                        };
                        result.Data = taskReport;
                        result.Succeed = true;
                    }    
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetProblemReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listTaskReport = _dbContext.Report.Include(x => x.Reporter).Include(x => x.LeaderTask).Include(x => x.Resources).Include(x => x.Supplies)
                .Where(x => x.leaderTaskId == leaderTaskId && x.reportType == ReportType.ProblemReport).OrderByDescending(x => x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listTaskReport = listTaskReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listTaskReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var tmp = new TaskReportModel
                    {
                        id = item.id,
                        leaderTaskId = item.LeaderTask.id,
                        leaderTaskName = item.LeaderTask.name,
                        reporterId = item.reporterId,
                        reporterName = item.Reporter.fullName,
                        responderId = item.LeaderTask.createById,
                        responderName = item.LeaderTask.CreateBy?.fullName??"",
                        reportType = item.reportType,
                        status = item.status,
                        title = item.title,
                        content = item.content,
                        createdDate = item.createdDate,
                        responseContent = item.responseContent,
                        resource = item.Resources.Select(x => x.link).ToList(),
                        listSupply = _mapper.Map<List<SupplyModel>>(item.Supplies)
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTaskReport.Count
                };
                result.Succeed = true;
            }

            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }

            return result;
        }

        public ResultModel GetProgressReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listTaskReport = _dbContext.Report.Include(x => x.Reporter).Include(x => x.LeaderTask).Include(x => x.Resources)
                .Where(x => x.leaderTaskId == leaderTaskId && x.reportType == ReportType.ProgressReport)
                .OrderByDescending(x => x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listTaskReport = listTaskReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listTaskReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var tmp = new TaskReportModel
                    {
                        id = item.id,
                        leaderTaskId = item.LeaderTask.id,
                        leaderTaskName = item.LeaderTask.name,
                        reporterId = item.reporterId,
                        reporterName = item.Reporter.fullName,
                        responderId = item.LeaderTask.createById,
                        responderName = item.LeaderTask.CreateBy?.fullName ?? "",
                        reportType = item.reportType,
                        title = item.title,
                        content = item.content,
                        itemFailed = item.itemFailed,
                        createdDate = item.createdDate,
                        status = item.status,
                        responseContent = item.responseContent,
                        resource = item.Resources.Select(x => x.link).ToList()
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTaskReport.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }

            return result;
        }

        public ResultModel GetReportByLeaderId(Guid leaderId, Guid leaderTaskId, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listReport = _dbContext.Report.Include(x=>x.Reporter).Include(x=>x.Supplies).Include(x=>x.LeaderTask).ThenInclude(x=>x.CreateBy)
                    .Where(a => a.reporterId == leaderId && a.leaderTaskId == leaderTaskId).OrderByDescending(x => x.createdDate).ToList();
            try
            {
                var listReportPaging = listReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportViewModel>();
                foreach (var item in listReportPaging)
                {
                    var tmp = new TaskReportViewModel
                    {
                        id = item.id,
                        leaderTaskId = item.LeaderTask.id,
                        leaderTaskName = item.LeaderTask.name,
                        reporterId = item.Reporter.Id,
                        reporterName = item.Reporter.fullName,
                        responderId = item.LeaderTask.CreateBy?.Id,
                        responderName = item.LeaderTask.CreateBy?.fullName,
                        reportType = item.reportType,
                        title = item.title,
                        content = item.content,
                        itemFailed = item.itemFailed,
                        createdDate = item.createdDate,
                        status = item.status,
                        responseContent = item.responseContent,
                        resource = item.Resources.Select(x => x.link).ToList(),
                        supply = item.Supplies.Select(y => new SupplyModel
                        {
                            id = y.id,
                            materialId = y.materialId,
                            materialName = y.materialName,
                            materialColor = y.materialColor,
                            materialSku = y.materialSku,
                            materialSupplier = y.materialSupplier,
                            materialThickness = y.materialThickness,
                            materialUnit = y.materialUnit,
                        }).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listReport.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetReportByForemanId(Guid foremanId, Guid leaderTaskId, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listReport = _dbContext.Report.Include(x => x.Reporter).Include(x => x.Supplies).Include(x => x.LeaderTask).ThenInclude(x => x.CreateBy).Include(x=>x.Order)
                    .Where(a => a.LeaderTask.createById==foremanId && a.leaderTaskId == leaderTaskId).OrderByDescending(x => x.createdDate).ToList();
            try
            {
                var listReportPaging = listReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportViewModel>();
                foreach (var item in listReportPaging)
                {
                    var tmp = new TaskReportViewModel
                    {
                        id = item.id,
                        leaderTaskId = item.LeaderTask.id,
                        leaderTaskName = item.LeaderTask.name,
                        reporterId = item.Reporter.Id,
                        reporterName = item.Reporter.fullName,
                        responderId = item.LeaderTask.CreateBy?.Id,
                        responderName = item.LeaderTask.CreateBy?.fullName,
                        reportType = item.reportType,
                        title = item.title,
                        content = item.content,
                        itemFailed = item.itemFailed,
                        createdDate = item.createdDate,
                        status = item.status,
                        responseContent = item.responseContent,
                        resource = item.Resources.Select(x => x.link).ToList(),
                        supply = item.Supplies.Select(y => new SupplyModel
                        {
                            id = y.id,
                            materialId = y.materialId,
                            materialName = y.materialName,
                            materialColor = y.materialColor,
                            materialSku = y.materialSku,
                            materialSupplier = y.materialSupplier,
                            materialThickness = y.materialThickness,
                            materialUnit = y.materialUnit,
                        }).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listReport.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listTaskReport = _dbContext.Report.Include(x => x.Reporter).Include(x => x.LeaderTask).Include(x => x.Resources).Include(x => x.Supplies)
                .Where(x => x.leaderTaskId == leaderTaskId).OrderByDescending(x => x.reportType).ToList();


            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listTaskReport = listTaskReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listTaskReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var tmp = new TaskReportModel
                    {
                        id = item.id,
                        leaderTaskId = item.LeaderTask.id,
                        leaderTaskName = item.LeaderTask.name,
                        reporterId = item.reporterId,
                        reporterName = item.Reporter.fullName,
                        responderId = item.LeaderTask.createById,
                        responderName = item.LeaderTask.CreateBy?.fullName ?? "",
                        reportType = item.reportType,
                        title = item.title,
                        content = item.content,
                        itemFailed = item.itemFailed,
                        createdDate = item.createdDate,
                        status = item.status,
                        responseContent = item.responseContent,
                        resource = item.Resources.Select(x => x.link).ToList(),
                        listSupply = _mapper.Map<List<SupplyModel>>(item.Supplies)
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTaskReport.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }

            return result;
        }
        #region Validate
        private bool CanSendProblemTaskReport(LeaderTask leaderTask)
        {
            var now = DateTime.UtcNow.AddHours(7);
            if (leaderTask.endTime>now && leaderTask.startTime <= now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        #endregion
    }
}

using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Sevices.Core.NotificationService;

namespace Sevices.Core.OrderReportService
{
    public class OrderReportService : IOrderReportService
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public OrderReportService(AppDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public ResultModel Create (Guid reporterId, CreateOrderReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if (user!.Role != null && user.Role.Name != "Foreman")
            {
                result.Code = 56;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải quản đốc !";
            }
            else
            {
                var order = _dbContext.Order
                .Where(x => x.id == model.orderId)
                .SingleOrDefault();
                if (order == null)
                {
                    result.Code = 57;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    if (order.status != OrderStatus.InProgress)
                    {
                        result.Code = 58;
                        result.Succeed = false;
                        result.ErrorMessage = "Đơn hàng đang không tiến hành!";                        
                    }
                    else
                    {
                        var report = new Report
                        {
                            orderId = model.orderId,
                            title = model.title,
                            content = model.content,
                            reporterId = reporterId,
                            reportStatus = model.reportStatus,
                            createdDate = DateTime.Now,
                            reportType = ReportType.OrderReport
                        };

                        try
                        {
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
        /*
        public ResultModel SendReviews (ReviewsOrderReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var report = _dbContext.Report
                .Where(x => x.id == model.reportId).SingleOrDefault();

            if (report == null)
            {
                result.Code = 60;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo !";
            }
            else
            {
                if (report.reportStatus == ReportStatus.Complete)
                {
                    result.Code = 61;
                    result.Succeed = false;
                    result.ErrorMessage = "Báo cáo này đã hoàn thành !";
                }
                else
                {
                    var order = _dbContext.Order
                    .Find(report.orderId);

                    try
                    {
                        report.reportStatus = model.reportStatus;
                        report.responseContent = model.contentReviews;

                        if (order != null && model.reportStatus == ReportStatus.Complete)
                        {
                            order.acceptanceTime = DateTime.Now;
                            order.status = OrderStatus.Completed;
                        }

                        _dbContext.SaveChanges();
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
        */
        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Report.Where(x => x.id == id).SingleOrDefault();

                if (check == null)
                {
                    result.Code = 60;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
                }
                else
                {
                    var report = new OrderReportModel
                    {
                        orderId = check.orderId,
                        title = check.title,
                        content = check.content,
                        createdDate = check.createdDate,
                        reportStatus = check.reportStatus,
                        reporterId = check.reporterId,
                        resource = check.Resources.Select(x => x.link).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByForemanId(Guid foremanId)
        {
            ResultModel result = new ResultModel();
            try
            {
                var checkReport = _dbContext.Report
                .Where(x => x.reporterId == foremanId).ToList();

                if (checkReport == null)
                {
                    result.Code = 65;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin quản đốc";
                }
                else
                {
                    var list = checkReport.Select(report => new OrderReportModel
                    {
                        orderId = report.orderId,
                        title = report.title,
                        content = report.content,
                        createdDate = report.createdDate,
                        reportStatus = report.reportStatus,
                        reporterId = report.reporterId,
                        resource = report.Resources.Select(x => x.link).ToList()
                    }).ToList();

                    var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                    result.Data = new PagingModel()
                    {
                        Data = sortedList,
                        Total = checkReport.Count
                    };
                    result.Succeed = true;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetByOrderId (Guid orderId)
        {
            ResultModel result = new ResultModel();
            try
            {
                var checkReport = _dbContext.Report
                .Where(x => x.orderId == orderId).ToList();

                if (checkReport == null)
                {
                    result.Code = 57;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng";
                }
                else
                {
                    var list = checkReport.Select(report => new OrderReportModel
                    {
                        
                        title = report.title,
                        content = report.content,
                        createdDate = report.createdDate,
                        reportStatus = report.reportStatus,
                        reporterId = report.reporterId,
                        resource = report.Resources.Select(x => x.link).ToList()
                    }).ToList();

                    var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                    result.Data = new PagingModel()
                    {
                        Data = sortedList,
                        Total = checkReport.Count
                    };
                    result.Succeed = true;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAll ()
        {
            ResultModel result = new ResultModel();
            try
            {
                var checkReport = _dbContext.Report
                .Where(x => x.reportType == ReportType.OrderReport).ToList();

                var list = checkReport.Select(report => new OrderReportModel
                {
                    orderId = report.orderId,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    reporterId = report.reporterId,
                    resource = report.Resources.Select(x => x.link).ToList()
                }).ToList();

                var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                result.Data = new PagingModel()
                {
                    Data = sortedList,
                    Total = checkReport.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}

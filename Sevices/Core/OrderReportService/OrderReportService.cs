using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderReportService(AppDbContext dbContext, INotificationService notificationService, IMapper mapper)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public ResultModel Create(Guid reporterId, CreateOrderReportModel model)
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
                var order = _dbContext.Order.Where(x => x.id == model.orderId).FirstOrDefault();

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
                            status = model.status,
                            createdDate = DateTime.UtcNow.AddHours(7),
                            reportType = ReportType.OrderReport
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

                            _dbContext.SaveChanges();
                            result.Succeed = true;

                            _notificationService.Create(new Notification
                            {
                                userId = order.createdById,
                                reportId = report.id,
                                title = "Báo cáo đơn hàng",
                                content = "Bạn vừa nhận được 1 báo cáo đơn hàng mới!",
                                type = NotificationType.TaskReport
                            });

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

        public ResultModel Update(UpdateOrderReportModel model)
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
                    check.title = model.title;
                    check.content = model.content;
                    check.status = model.status;

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
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Report.Where(x => x.id == id).Include(x => x.Resources)
                    .Include(x => x.Order).Include(x => x.Reporter).FirstOrDefault();

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
                        id = check.id,
                        orderId = check.orderId,
                        order = check.Order != null ? _mapper.Map<OrderModel>(check.Order) : null,
                        reporterId = check.reporterId,                       
                        reporter = check.Reporter != null ? _mapper.Map<UserModel>(check.Reporter) : null,
                        title = check.title,
                        content = check.content,
                        createdDate = check.createdDate,
                        status = check.status,                      
                        resource = check.Resources.Select(x => x.link).ToList()
                    };

                    result.Data = report;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByForemanId(Guid foremanId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            var listOrderReport = _dbContext.Report.Where(x => x.reporterId == foremanId)
                .Include(x => x.Resources).Include(x => x.Order).Include(x => x.Reporter).OrderByDescending(x=>x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).OrderByDescending(x => x.createdDate).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReportPaging.Select(report => new OrderReportModel
                {
                    id = report.id,
                    orderId = report.orderId,
                    order = report.Order != null ? _mapper.Map<OrderModel>(report.Order) : null,
                    reporterId = report.reporterId,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    status = report.status,                   
                    resource = report.Resources.Select(x => x.link).ToList()
                }).ToList();

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listOrderReport.Count
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
            ResultModel result = new ResultModel();

            var listOrderReport = _dbContext.Report.Where(x => x.orderId == orderId)
                .Include(x => x.Reporter).Include(x => x.Resources).OrderByDescending(x => x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).OrderByDescending(x => x.createdDate).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReport.Select(report => new OrderReportModel
                {
                    id = report.id,
                    orderId = report.orderId,
                    reporterId = report.reporterId,
                    reporter = report.Reporter != null ? _mapper.Map<UserModel>(report.Reporter) : null,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    status = report.status,
                    resource = report.Resources.Select(x => x.link).ToList()
                }).ToList();

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listOrderReport.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            var listOrderReport = _dbContext.Report.Where(x => x.reportType == ReportType.OrderReport)
                .Include(x => x.Resources).Include(x => x.Order).Include(x => x.Reporter).OrderByDescending(x => x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).OrderByDescending(x => x.createdDate).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReportPaging.Select(report => new OrderReportModel
                {
                    id = report.id,
                    orderId = report.orderId,
                    order = report.Order != null ? _mapper.Map<OrderModel>(report.Order) : null,
                    reporterId = report.reporterId,
                    reporter = report.Reporter != null ? _mapper.Map<UserModel>(report.Reporter) : null,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    status = report.status,                   
                    resource = report.Resources.Select(x => x.link).ToList()
                }).ToList();

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listOrderReport.Count
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

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
                var order = _dbContext.Order.Where(x => x.id == model.orderId).SingleOrDefault();
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
                            status = model.reportStatus,
                            createdDate = DateTime.Now,
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
                            /*
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
                                        amount = supply.amount,
                                        price = matePrice,
                                        totalPrice = matePrice * supply.amount,
                                        status = model.supplyStatus,
                                    };
                                    listSupply.Add(newSupply);
                                }
                                _dbContext.Supply.AddRange(listSupply);
                            }
                            */
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

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Report.Where(x => x.id == id)
                    .Include(x => x.Resources).SingleOrDefault();

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
                        reportStatus = check.status,
                        reporterId = check.reporterId,
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

            var listOrderReport = _dbContext.Report.Include(x => x.Resources)
                .Where(x => x.reporterId == foremanId).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReportPaging.Select(report => new OrderReportModel
                {
                    id = report.id,
                    orderId = report.orderId,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.status,
                    reporterId = report.reporterId,
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

            var listOrderReport = _dbContext.Report.Include(x => x.Resources)
                .Where(x => x.orderId == orderId).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReport.Select(report => new OrderReportModel
                {
                    id = report.id,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.status,
                    reporterId = report.reporterId,
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
            var listOrderReport = _dbContext.Report.Include(x => x.Resources)
                .Where(x => x.reportType == ReportType.OrderReport).ToList();
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listOrderReport = listOrderReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listOrderReportPaging = listOrderReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = listOrderReport.Select(report => new OrderReportModel
                {
                    id = report.id,
                    orderId = report.orderId,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.status,
                    reporterId = report.reporterId,
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

        #region comment
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
        #endregion
    }
}

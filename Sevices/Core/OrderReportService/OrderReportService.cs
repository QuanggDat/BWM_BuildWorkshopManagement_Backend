using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.OrderReportModel;

namespace Sevices.Core.OrderReportService
{
    public class OrderReportService : IOrderReportService
    {
        private readonly AppDbContext _dbContext;

        public OrderReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreateOrderReport(Guid reporterId, CreateOrderReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if (user!.Role != null && user.Role.Name != "Foreman")
            {
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải quản đốc!";
                return result;
            }
            else
            {
                var order = await _dbContext.Order
                .Where(x => x.id == model.orderId)
                .SingleOrDefaultAsync();
                if (order == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                    return result;
                }
                else
                {
                    if (order.status != Data.Enums.OrderStatus.InProgress)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Đơn hàng đang không tiến hành!";
                        return result;
                    }
                    else
                    {
                        var checkReport = await _dbContext.Report.AnyAsync(x => x.orderId == model.orderId);

                        if (checkReport == true)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Báo cáo tiến độ cho đơn hàng này đã được thực hiện!";
                            return result;
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
                            };

                            try
                            {
                                await _dbContext.Report.AddAsync(report);
                                await _dbContext.SaveChangesAsync();
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
                }          
            }       
        }

        public async Task<ResultModel> ReviewsOrderReport(ReviewsOrderReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var report = await _dbContext.Report
                .Where(x => x.id == model.reportId).SingleOrDefaultAsync();

            if (report == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo !";
                return result;
            }
            else
            {
                if (report.reportStatus == Data.Enums.ReportStatus.Complete)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Báo cáo này đã hoàn thành !";
                    return result;
                }
                else
                {
                    var order = await _dbContext.Order
                    .FindAsync(report.orderId);

                    try
                    {
                        report.reportStatus = model.reportStatus;
                        report.responseContent = model.contentReviews;

                        if (order != null && model.reportStatus == Data.Enums.ReportStatus.Complete)
                        {
                            order.acceptanceDate = DateTime.Now;
                            order.status = Data.Enums.OrderStatus.Completed;
                        }

                        await _dbContext.SaveChangesAsync();
                        result.Succeed = true;
                        result.Data = report.id;
                    }

                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                    return result;
                }          
            }          
        }

        public async Task<OrderReportModel?> GetOrderReportById(Guid reportId)
        {
            var report = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Order)
                .Where(x => x.id == reportId)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return null;
            }
            else
            {
                var reporter = report.Reporter;

                return new OrderReportModel
                {
                    orderName = report.Order.name,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    responseContent = report.responseContent,

                    reporter = new ReporterOrderReport
                    {
                        id = reporter.Id,
                        fullName = reporter.fullName,
                        phoneNumber = reporter.UserName,
                        email = reporter.Email,
                    },
                };
            }
        }

        public async Task<List<OrderReportModel>> GetOrderReportsByForemanId(Guid foremanId)
        {
            var checkReport = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Order)
                .Where(x => x.reporterId == foremanId && x.reportType == Data.Enums.ReportType.ProgressReport)
                .ToListAsync();

            if (checkReport == null)
            {
                return new List<OrderReportModel>();
            }
            else
            {
                var list = checkReport.Select(report => new OrderReportModel
                {
                    orderName = report.Order.name,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    responseContent = report.responseContent,

                    reporter = new ReporterOrderReport
                    {
                        id = report.Reporter.Id,
                        fullName = report.Reporter.fullName,
                        phoneNumber = report.Reporter.UserName,
                        email = report.Reporter.Email,
                    },

                }).ToList();

                var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                return sortedList;
            }         
        }
    }
}

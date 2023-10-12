using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.ReportModel;

namespace Sevices.Core.ReportService
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _dbContext;

        public ReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreateReport(Guid reporterId, CreateReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var managerTask = await _dbContext.ManagerTask.Include(x => x.Order)
                .Include(x => x.Manager)
                .Where(x => x.id == model.managerTaskId)
                .SingleOrDefaultAsync();

            if (managerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy Manager Task";
                return result;
            }

            if (model.reportType == Data.Enums.ReportType.ProgressReport)
            {
                
                var canSendReport = CanSendProgressReport(managerTask);

                if (!canSendReport)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Chưa thề gửi báo cáo vào lúc này";
                    return result;
                }

                var checkReport = await _dbContext.Report.Include(x => x.ManagerTask)
                .Where(x => x.managerTaskId == model.managerTaskId)
                .SingleOrDefaultAsync();

                if (checkReport != null || checkReport.reportType == Data.Enums.ReportType.ProgressReport)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Báo cáo tiến độ cho công việc này đã được thực hiện!";
                    return result;
                }

                var report = new Report

                {
                    managerTaskId = model.managerTaskId,
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

            if (model.reportType == Data.Enums.ReportType.ProblemReport)
            {
                var canSendReport = CanSendProblemReport(managerTask);

                if (!canSendReport)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Chưa thể gửi báo cáo vào lúc này";
                    return result;
                }

                var report = new Report
                {
                    managerTaskId = model.managerTaskId,
                    title = model.title,
                    content = model.content,
                    reporterId = reporterId,
                    createdDate = DateTime.Now,
                };

                try
                {
                    await _dbContext.Report.AddAsync(report);
                    if(model.resource != null) 
                    {
                        foreach (var resource in model.resource)
                        {
                            await _dbContext.Resource.AddAsync(new Resource
                            {
                                reportId = report.id,
                                link = resource
                            });
                        }
                    }
                    
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

        private bool CanSendProgressReport(ManagerTask managerTask)
        {
            var now = DateTime.Now;
            return now >= managerTask.timeEnd;
        }

        private bool CanSendProblemReport(ManagerTask managerTask)
        {
            var now = DateTime.Now;
            return now >= managerTask.timeStart && now <= managerTask.timeEnd;
        }
    }
}

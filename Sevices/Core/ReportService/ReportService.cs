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
                    result.ErrorMessage = "Chưa thể gửi báo cáo vào lúc này";
                    return result;
                }

                var checkReport = await _dbContext.Report
               .AnyAsync(x => x.managerTaskId == model.managerTaskId || x.reportType == Data.Enums.ReportType.ProgressReport);
                
                if (checkReport == true)
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
                    reportType = model.reportType,
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
                    reportType = model.reportType,
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
        
        public async Task<ResponseReportModel?> GetReportByReportId(Guid reportId)
        {
            var report = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Resources)                
                .Include(x => x.ManagerTask)                
                    .ThenInclude(x => x.CreateBy)
       
                .Where(x => x.id == reportId)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return null;
            }
            ResponseReportModel result = null;

            if (report.reportType == Data.Enums.ReportType.ProgressReport)
            {
                var reviewer = report.ManagerTask.CreateBy;
                var reporter = report.Reporter;

                result = new ResponseReportModel
                {
                    id = report.id,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    contentReviews = report.contentReviews,

                    reporter = new Reporter
                    {
                        id = reporter.Id,
                        fullName = reporter.fullName,
                        phoneNumber = reporter.UserName,
                        email = reporter.Email,
                    },

                    reviewer = new Reviewer
                    {
                        id = reviewer.Id,
                        fullName = reviewer.fullName,
                    },
                };
            }

            if (report.reportType == Data.Enums.ReportType.ProblemReport)
            {
                var reviewer = report.ManagerTask.CreateBy;
                var reporter = report.Reporter;

                result = new ResponseReportModel
                {
                    id = report.id,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    contentReviews = report.contentReviews,

                    reporter = new Reporter
                    {
                        id = reporter.Id,
                        fullName = reporter.fullName,
                        phoneNumber = reporter.UserName,
                        email = reporter.Email,
                    },

                    reviewer = new Reviewer
                    {
                        id = reviewer.Id,
                        fullName = reviewer.fullName,
                    },

                    resource = report.Resources.Select(x => x.link).ToList()
                    
                };               
            }

            return result;
        }
            
        public async Task<ResultModel> ReviewsReport(ReviewsReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var report = await _dbContext.Report.FindAsync(model.reportId) ;
            if (report == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy reportId!";
                return result;
            }
            try
            {
                report.reportStatus = model.reportStatus;
                report.contentReviews = model.contentReviews;
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

        public async Task<List<ResponseReportModel>> GetTeamReportsByManagerTaskId(Guid managerTaskId)
        {
            var report = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Resources)                
                .Include(x => x.ManagerTask)                
                    .ThenInclude(x => x.CreateBy)
       
                .Where(x => x.managerTaskId == managerTaskId)
                .ToListAsync();

            if (report == null)
            {
                return new List<ResponseReportModel>();
            }
            return null;
        }
    }
}

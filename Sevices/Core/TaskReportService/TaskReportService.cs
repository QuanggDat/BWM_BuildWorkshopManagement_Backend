using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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

        public TaskReportService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreateTaskReport(Guid reporterId, CreateTaskReportModel model)
        {            
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if(user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải trưởng nhóm !";
                return result;
            }
            else
            {
                var leaderTask = await _dbContext.LeaderTask.Include(x => x.Order)
                .Include(x => x.Leader)
                .Where(x => x.id == model.leaderTaskId)
                .SingleOrDefaultAsync();

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                    return result;
                }
                else
                {
                    if (model.reportType == Data.Enums.ReportType.ProgressReport)
                    {
                        var canSendReport = CanSendProgressTaskReport(leaderTask);
                        var checkReport = await _dbContext.Report.AnyAsync(x => x.leaderTaskId == model.leaderTaskId || x.reportType == Data.Enums.ReportType.ProgressReport);

                        if (checkReport == true)
                        {
                            result.Code = 52;
                            result.Succeed = false;
                            result.ErrorMessage = "Báo cáo tiến độ cho công việc này đã được thực hiện!";
                            return result;
                        }

                        else
                        {
                            if (!canSendReport)
                            {
                                result.Code = 53;
                                result.Succeed = false;
                                result.ErrorMessage = "Chưa thể gửi báo cáo vào lúc này!";
                                return result;
                            }
                            else
                            {
                                var report = new Report
                                {
                                    leaderTaskId = model.leaderTaskId,
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
                        }
                    }

                    else
                    {
                        var canSendReport = CanSendProblemTaskReport(leaderTask);

                        if (!canSendReport)
                        {
                            result.Code = 53;
                            result.Succeed = false;
                            result.ErrorMessage = "Chưa thể gửi báo cáo vào lúc này!";
                            return result;
                        }
                        else
                        {
                            var report = new Report
                            {
                                leaderTaskId = model.leaderTaskId,
                                title = model.title,
                                content = model.content,
                                reporterId = reporterId,
                                reportType = model.reportType,
                                createdDate = DateTime.Now,
                            };

                            try
                            {
                                await _dbContext.Report.AddAsync(report);
                                if (model.resource != null)
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
                    }
                }
                return result;
            }      
        }        
        
        public async Task<TaskReportModel?> GetTaskReportById(Guid reportId)
        {
            var report = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Resources)                
                .Include(x => x.LeaderTask)                
                    .ThenInclude(x => x.CreateBy)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.Order)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.Procedure)
                .Where(x => x.id == reportId)
                .SingleOrDefaultAsync();

            if (report == null)
            {
                return null;
            }

            TaskReportModel result = null!;
            if (report.reportType == Data.Enums.ReportType.ProgressReport)
            {
                var reviewer = report.LeaderTask.CreateBy;
                var reporter = report.Reporter;

                result = new TaskReportModel
                {
                    orderName = report.LeaderTask.Order.name,
                    leaderTaskName = report.LeaderTask.Procedure.name,                  
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    responseContent = report.responseContent,

                    reporter = new ReporterTaskReport
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

            else 
            {
                var reviewer = report.LeaderTask.CreateBy;
                var reporter = report.Reporter;

                result = new TaskReportModel
                {
                    orderName = report.LeaderTask.Order.name,
                    leaderTaskName = report.LeaderTask.Procedure.name,                    
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    responseContent = report.responseContent,

                    reporter = new ReporterTaskReport
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
            
        public async Task<ResultModel> TaskReportResponse(ReviewsReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var report = await _dbContext.Report.Include(x => x.LeaderTask)
                .Where(x => x.id == model.reportId).SingleOrDefaultAsync() ;

            if (report == null)
            {
                result.Code = 54;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
                return result;
            }
            else
            {
                if (report.reportType == Data.Enums.ReportType.ProgressReport)
                {
                    if (report.reportStatus == Data.Enums.ReportStatus.Complete)
                    {
                        result.Code = 55;
                        result.Succeed = false;
                        result.ErrorMessage = "Báo cáo này đã hoàn thành!";
                        return result;
                    }
                    else
                    {
                        var leaderTask = await _dbContext.LeaderTask
                        .FindAsync(report.leaderTaskId);

                        try
                        {
                            report.reportStatus = model.reportStatus;
                            report.responseContent = model.responseContent;

                            if (leaderTask != null && model.reportStatus == Data.Enums.ReportStatus.Complete)
                            {
                                leaderTask.completedTime = DateTime.Now;
                                leaderTask.status = ETaskStatus.Completed;
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
                }

                else
                {
                    try
                    {
                        report.responseContent = model.responseContent;
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

        public async Task<List<TaskReportModel>> GetProgressTaskReportsByLeaderId(Guid leaderId)
        {
            var checkReport = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Resources)                
                .Include(x => x.LeaderTask)                
                    .ThenInclude(x => x.CreateBy)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.Order)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.Procedure)
                .Where(x => x.reporterId == leaderId && x.reportType == Data.Enums.ReportType.ProgressReport)
                .ToListAsync();
            
            if (checkReport == null)
            {
                return new List<TaskReportModel>();
            }
            else
            {
                var list = checkReport.Select(report => new TaskReportModel
                {
                    orderName = report.LeaderTask.Order.name,
                    leaderTaskName = report.LeaderTask.Procedure.name,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    reportStatus = report.reportStatus,
                    responseContent = report.responseContent,

                    reporter = new ReporterTaskReport
                    {
                        id = report.Reporter.Id,
                        fullName = report.Reporter.fullName,
                        phoneNumber = report.Reporter.UserName,
                        email = report.Reporter.Email,
                    },

                    reviewer = new Reviewer
                    {
                        id = report.LeaderTask.CreateBy.Id,
                        fullName = report.LeaderTask.CreateBy.fullName,
                    },
                }).ToList();

                var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                return sortedList;
            }
            
        }

        public async Task<List<TaskReportModel>> GetProblemTaskReportsByLeaderId(Guid leaderId)
        {
            var checkReport = await _dbContext.Report
                .Include(x => x.Reporter)
                .Include(x => x.Resources)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.CreateBy)
                .Include(x => x.LeaderTask)
                    .ThenInclude(x => x.Order)
                .Where(x => x.reporterId == leaderId && x.reportType == Data.Enums.ReportType.ProblemReport)
                .ToListAsync();

            if (checkReport == null)
            {
                return new List<TaskReportModel>();
            }
            else
            {
                var list = checkReport.Select(report => new TaskReportModel
                {
                    orderName = report.LeaderTask.Order.name,
                    leaderTaskName = report.LeaderTask.Procedure.name,
                    title = report.title,
                    content = report.content,
                    createdDate = report.createdDate,
                    responseContent = report.responseContent,

                    reporter = new ReporterTaskReport
                    {
                        id = report.Reporter.Id,
                        fullName = report.Reporter.fullName,
                        phoneNumber = report.Reporter.UserName,
                        email = report.Reporter.Email,
                    },

                    reviewer = new Reviewer
                    {
                        id = report.LeaderTask.CreateBy.Id,
                        fullName = report.LeaderTask.CreateBy.fullName,
                    },

                    resource = report.Resources.Select(x => x.link).ToList()

                }).ToList();

                var sortedList = list.OrderByDescending(x => x.createdDate).ToList();
                return sortedList;
            }        
        }

        #region Validate
        private bool CanSendProgressTaskReport(LeaderTask leaderTask)
        {
            var now = DateTime.Now;
            return now >= leaderTask.endTime;
        }

        private bool CanSendProblemTaskReport(LeaderTask leaderTask)
        {
            var now = DateTime.Now;
            return now >= leaderTask.startTime && now <= leaderTask.endTime;
        }
        #endregion
    }
}

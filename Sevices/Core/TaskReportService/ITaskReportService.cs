using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.TaskReportModel;

namespace Sevices.Core.ReportService
{
    public interface ITaskReportService
    {
        ResultModel CreateProblemReport(Guid reporterId, CreateProblemReportModel model);
        ResultModel CreateProgressReport(Guid reporterId, CreateProgressReportModel model);
        ResultModel CreateAcceptanceReport(Guid reporterId, CreateAcceptanceReportModel model);
        ResultModel Update(UpdateReportModel model);
        ResultModel SendProblemReportFeedback(SendProblemReportFeedbackModel model);
        ResultModel SendProgressReportFeedback(SendProgressReportFeedbackModel model);
        ResultModel GetById(Guid reportId);
        ResultModel GetProblemReportByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetProgressReportByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
    }
}

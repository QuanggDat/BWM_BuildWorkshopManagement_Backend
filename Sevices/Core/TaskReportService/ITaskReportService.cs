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
        ResultModel Update(UpdateTaskReportModel model);
        ResultModel UpdateProblemTaskReport(UpdateProblemTaskReportModel model);
        ResultModel SendProblemReportFeedback(SendProblemReportFeedbackModel model);
        ResultModel SendProgressReportFeedback(SendProgressReportFeedbackModel model);
        ResultModel GetById(Guid id);
        ResultModel GetProblemReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetProgressReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetReportByLeaderId(Guid leaderId, Guid leaderTaskId, int pageIndex, int pageSize);
        ResultModel GetReportByForemanId(Guid foremanId, string? search, int pageIndex, int pageSize);
        ResultModel UpdateStatusReport (Guid id, ReportStatus status);
    }
}

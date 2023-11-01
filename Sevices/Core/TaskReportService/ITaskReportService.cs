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
        Task<ResultModel> CreateTaskReport(Guid reporterId, CreateTaskReportModel model);
        Task<TaskReportModel?> GetTaskReportById(Guid reportId);
        Task<ResultModel> TaskReportResponse(ReviewsReportModel model);
        Task<List<TaskReportModel>> GetProgressTaskReportsByLeaderId(Guid leaderId);
        Task<List<TaskReportModel>> GetProblemTaskReportsByLeaderId(Guid leaderId);
    }
}

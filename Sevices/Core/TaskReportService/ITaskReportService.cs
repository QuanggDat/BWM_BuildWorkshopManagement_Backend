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
        ResultModel Create(Guid reporterId, CreateTaskReportModel model);     
        ResultModel SendResponse(SendResponseModel model);
        ResultModel GetById(Guid reportId);
        ResultModel GetProblemTaskReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetProgressTaskReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
    }
}

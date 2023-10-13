using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.ReportModel;

namespace Sevices.Core.ReportService
{
    public interface IReportService
    {
        Task<ResultModel> CreateReport(Guid reporterId, CreateReportModel model);
        Task<ResponseReportModel?> GetReportByReportId(Guid reportId);
        Task<ResultModel> ReviewsReport(ReviewsReportModel model);
        Task<List<ResponseReportModel>> GetTeamReportsByManagerTaskId(Guid managerTaskId);
    }
}

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
        public Task<ResultModel> CreateTeamReport(Guid reporterId, CreateReportModel model);
    }
}

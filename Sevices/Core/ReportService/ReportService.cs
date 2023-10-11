using Data.DataAccess;
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

        public async Task<ResultModel> CreateTeamReport(Guid reporterId, CreateReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var managerTask = await _dbContext.ManagerTask.Include(x => x.Manager)
                .Where(x => x.id == model.managerTaskId)
                .SingleOrDefaultAsync();

            if (managerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy ManagerTask";
                return result;
            }
            return result;
        }
    }
}

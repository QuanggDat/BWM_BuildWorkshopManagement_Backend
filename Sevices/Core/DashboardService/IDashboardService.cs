using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.DashboardService
{
    public interface IDashboardService
    {
        ResultModel UserDashboard();
        ResultModel OrderDashboard();
        ResultModel OrderByMonthDashboard(int year);
        ResultModel LeaderTaskDashboard();
        ResultModel WorkerTaskDashboard(Guid leaderId);
    }
}

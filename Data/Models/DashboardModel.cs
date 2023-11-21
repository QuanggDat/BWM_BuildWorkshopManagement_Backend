using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class DashboardModel
    {
        public class UseDashboardModel
        {
            public Guid roleId { get; set; }
            public string roleName { get; set; } = null!;
            public int totalUser { get; set; } 
        }
        public class OrderDashboardModel
        {
            public OrderStatus orderStatus { get; set; } 
            public int total { get; set; }
        }
        public class OrderByMonthDashboardModel
        {
            public int month { get; set; }
            public int total { get; set; }
        }
        public class TaskDashboardModel
        {
            public ETaskStatus taskStatus { get; set; }
            public int total { get; set; }
        }
        public class WorkerTaskDashboardModel
        {
            public EWorkerTaskStatus taskStatus { get; set; }
            public int total { get; set; }
        }

    }
}

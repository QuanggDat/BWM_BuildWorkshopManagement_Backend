using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateWorkerTaskModel
    {
        public Guid leaderTaskId { get; set; }
        public string name { get; set; } = null!;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public ETaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public List<Guid> assignees { get; set; } = null!;
    }

    public class UpdateWorkerTaskModel
    {
        public Guid workerTaskId { get; set; }
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public ETaskStatus status { get; set; }
        public List<Guid> assignees { get; set; } = null!;
    }

    public class UpdateWorkerTaskStatusModel
    {
        public ETaskStatus status { get; set; }
    }

    public class TaskMember
    {
        public Guid memberId { get; set; }
        public string memberFullName { get; set; } = null!;
    }

    public class WorkerTaskModel
    {
        public Guid workerTaskId { get; set; }
        public Guid? userId { get; set; }
        public string userFullName { get; set; } = null!;
        public Guid leaderTaskId { get; set; }
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public ETaskStatus status { get; set; }
        public List<TaskMember> Members { get; set; } = null!;
    }

    public class AssignWorkerTaskModel
    {
        public Guid memberId { get; set; }
        public Guid workerTaskId { get; set; }
    }

}

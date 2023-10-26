using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class WorkerTaskModel
    {
        public class CreateWorkerTaskModel
        {
            public Guid managerTaskId { get; set; }
            public string name { get; set; } = null!;
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public TaskStatus status { get; set; }
            public string description { get; set; } = null!;
            public List<string> assignees { get; set; } = null!;
        }

        public class UpdateWorkerTaskModel
        {
            public Guid workerTaskId { get; set; }
            public string name { get; set; } = null!;
            public string description { get; set; } = null!;
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public TaskStatus status { get; set; }
            public List<Guid> assignees { get; set; } = null!;
        }

        public class UpdateWorkerTaskStatusModel
        {
            public TaskStatus status { get; set; }
        }

        public class TaskMemberResponse
        {
            public Guid memberId { get; set; }
            public string memberFullName { get; set; } = null!;
        }

        public class WorkerTaskResponseModel
        {
            public Guid workerTaskId { get; set; }
            public Guid? userId { get; set; }
            public string userFullName { get; set; } = null!;
            public Guid managerTaskId { get; set; }
            public string name { get; set; } = null!;
            public string description { get; set; } = null!;
            public DateTime? startTime { get; set; }
            public DateTime? endTime { get; set; }
            public TaskStatus status { get; set; }
            public List<TaskMemberResponse> Members { get; set; } = null!;
        }

        public class AssignWorkerTaskModel
        {
            public Guid memberId { get; set; }
            public Guid workerTaskId { get; set; }
        }
    }
}

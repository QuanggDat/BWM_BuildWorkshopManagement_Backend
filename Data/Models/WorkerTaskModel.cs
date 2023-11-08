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
        public Guid stepId { get; set; }
        public int priority { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public ETaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public List<Guid> assignees { get; set; } = null!;
    }

    public class UpdateWorkerTaskModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public int priority { get; set; }    
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public ETaskStatus status { get; set; }
        public string description { get; set; } = null!;
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
        public Guid id { get; set; }

        public Guid? createById { get; set; }
        public Guid leaderTaskId { get; set; }
        public Guid stepId { get; set; }     

        public string name { get; set; } = null!;
        
        public int priority { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completeTime { get; set; }

        public string? description { get; set; }
        public ETaskStatus status { get; set; }
        public bool isDeleted { get; set; }
        public List<TaskMember> Members { get; set; } = null!;
    }

    public class AssignWorkerTaskModel
    {
        public Guid memberId { get; set; }
        public Guid workerTaskId { get; set; }
    }

}

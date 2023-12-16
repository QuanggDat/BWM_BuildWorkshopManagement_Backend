using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class GroupModel
    {
        public Guid id {  get; set; }
        public string name { get; set; } = null!;
        public string leaderName { get; set; } = null!;
        public int amountWorker { get; set; }
    }

    public class WorkerAndTaskOfWorkerModel
    {
        public Guid userId { get; set; }
        public string fullName { get; set; } = null!;
        public Guid? workerTaskId { get; set; }
        public string? workerTaskName { get; set; }
        public EWorkerTaskStatus? statusTask { get; set; }
        public DateTime? startTimeTask { get; set; }
        public DateTime? endTimeTask { get; set; }
    }

    public class CreateGroupModel
    {
        public string name { get; set; } = null!;
        public Guid leaderId { get; set; } 
    }
    
    public class UpdateGroupModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public Guid leaderId { get; set; }
    }

    public class DeleteGroupModel
    {
        public Guid id { get; set; }
    }
}

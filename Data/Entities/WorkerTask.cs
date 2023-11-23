using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class WorkerTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("createById")]
        public Guid? createById { get; set; }
        public virtual User CreateBy { get; set; } = null!;

        [ForeignKey("leaderTaskId")]
        public Guid leaderTaskId { get; set; }
        public LeaderTask LeaderTask { get; set; } = null!;

        public string name { get; set; } = null!;
        public int priority { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }
        public EWorkerTaskStatus status { get; set; }
        public string? description { get; set; } = null!; public 

        string? feedbackTitle { get; set; }
        public string? feedbackContent { get; set; }     
        public bool isDeleted { get; set; }

        public virtual List<Resource> Resources { get; set; } = new();
        public virtual List<WorkerTaskDetail> WorkerTaskDetails { get; set; } = new();
    }
}

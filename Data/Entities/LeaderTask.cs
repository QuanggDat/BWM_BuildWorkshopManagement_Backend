﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LeaderTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("leaderId")]
        public Guid leaderId { get; set; }
        public virtual User Leader { get; set; } = null!;

        [ForeignKey("createById")]
        public Guid? createById { get; set; }
        public virtual User CreateBy { get; set; } = null!;

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("teamId")]
        public Guid? teamId { get; set; }
        public virtual Team? Team { get; set; } = null!;

        [ForeignKey("procedureId")]
        public Guid procedureId { get; set; }
        public virtual Procedure Procedure { get; set; } = null!;
        public DateTime estimatedStartTime { get; set; }
        public DateTime estimatedEndTime { get; set; }
        public DateTime startTime { get; set; } 
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }
        public TaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; }

        public virtual List<WorkerTask> WorkerTasks { get; set; } = new();
    }
}
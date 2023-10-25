using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class WokerTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("createById")]
        public Guid? createById { get; set; }
        public virtual User CreateBy { get; set; } = null!;

        [ForeignKey("managerTaskId")]
        public Guid managerTaskId { get; set; }
        public ManagerTask ManagerTask { get; set; } = null!;

        public string name { get; set; } = null!;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }      
        public TaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; }
   
        public virtual List<WokerTaskDetail> WokerTaskDetails { get; set; } = new();
    }
}

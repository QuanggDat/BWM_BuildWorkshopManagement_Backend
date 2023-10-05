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
        [ForeignKey("userId")]
        public Guid userId { get; set; }
        [ForeignKey("managerTaskId")]
        public Guid managerTaskId { get; set; } 
        public virtual ManagerTask ManagerTask { get; set; } = null!;
        public string name { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public DateTime completedTime { get; set; }
        public int productCompleted { get; set; }
        public int productFailed { get; set; }
        public string description { get; set; }
        public bool isDeleted { get; set; }
        public virtual ICollection<WokerTaskDetail> WokerTaskDetail { get; set; } = new List<WokerTaskDetail>();

    }
}

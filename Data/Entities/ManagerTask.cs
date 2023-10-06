using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ManagerTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("managerId")]
        public Guid managerId { get; set; }
        public virtual User manager { get; set; } = null!;

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        public string name { get; set; } = null!;
        public DateTime timeStart { get; set; } 
        public DateTime timeEnd { get; set; }
        public DateTime? completedTime { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; } 
        public virtual List<Group> Groups { get; set; } = new();
        public virtual List<WokerTask> WokerTasks { get; set; } = new();
    }
}

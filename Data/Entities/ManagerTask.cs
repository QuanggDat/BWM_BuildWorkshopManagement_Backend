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
        [ForeignKey("orderId")]
        public Guid orderId { get; set; }
        public Order Order { get; set; }
        public string name { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public DateTime completedTime { get; set; }
        public string description { get; set; }
        public bool isDeleted { get; set; } 
        public virtual ICollection<Group> Group { get; set; } = new List<Group>();
        public virtual ICollection<WokerTask> WokerTask { get; set; } = new List<WokerTask>();
    }
}

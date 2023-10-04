using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Task
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string name { get; set; }
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public DateTime completedTime { get; set; }
        public int productCompleted { get; set; }
        public int productFailed { get; set; }
        public string description { get; set; }
        public bool isDeleted { get; set; }
        [ForeignKey("orderId")]
        public Guid orderId { get; set;}
        public Order Order { get; set; }
        public virtual ICollection<GroupMember> GroupMember { get; set; } = new List<GroupMember>();
    }
}

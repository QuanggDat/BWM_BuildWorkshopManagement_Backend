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
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("assignToId")]
        public Guid assignToId { get; set; }
        public virtual User AssignTo { get; set; } = null!;

        [ForeignKey("createdById")]
        public Guid createdById { get; set; }
        public virtual User CreatedBy { get; set; } = null!;

        [Column(TypeName = "nvarchar(500)")]
        public string name { get; set; } = null!;
        public string customerName { get; set; } = null!;    
        
        public DateTime createTime { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public DateTime? inProgressTime { get; set; }
        public DateTime? quoteTime { get; set; }
        public DateTime? acceptanceTime { get; set; }

        public string fileContract { get; set; } = null!;
        public string fileQuote { get; set; } = null!;

        public string description { get; set; } = null!;
        public OrderStatus status { get; set; }       
        public double totalPrice { get; set; }
        
        public virtual List<OrderDetail> OrderDetails { get; set; } = new();
        public virtual List<LeaderTask> LeaderTasks { get; set; } = new();
        public virtual List<Resource> Resources { get; set; } = new();
    }
}

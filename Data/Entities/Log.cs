using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public virtual User? User { get; set; } = null!;

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public Order? Order { get; set; }

        [ForeignKey("orderDetailId")]
        public Guid? orderDetailId { get; set; }
        public OrderDetail? OrderDetail { get; set; }

        [ForeignKey("itemId")]
        public Guid? itemId { get; set; }
        public virtual Item? Item { get; set; }

        [ForeignKey("materialId")]
        public Guid? materialId { get; set; }
        public virtual Material? Material { get; set; }

        [ForeignKey("groupId")]
        public Guid? groupId { get; set; }
        public virtual Group? Group { get; set; }

        public DateTime modifiedTime { get; set; }
        public string action { get; set; } = null!;

    }
}

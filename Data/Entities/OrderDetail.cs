using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public Guid id { get; set; }

        [ForeignKey("orderId")]
        public Guid orderId { get; set; }
        public Order Order { get; set; } = null!;

        [ForeignKey("itemId")]
        public Guid? itemId { get; set; }
        public Item? Item { get; set; }

        [ForeignKey("areaId")]
        public Guid areaId { get; set; }
        public Area area { get; set; } = null!;

        public int quantity { get; set; }
        public double price { get; set; } = 0;
        public double totalPrice { get; set; } = 0;
        public string? description { get; set; }
        public bool isDeleted { get; set; }       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }

        [ForeignKey("itemId")]
        public Guid itemId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("orderId")]
        public Guid orderId { get; set; }
        public Order Order { get; set; }
    }
}

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
        public Guid itemId { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
        public Guid id { get; set; }
        [ForeignKey("orderId")]
        public Order order { get; set; }
        public Guid orderId { get; set; }
        [ForeignKey("itemId")]
        public Item item { get; set; }
    }
}

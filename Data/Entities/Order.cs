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
        [Column(TypeName = "nvarchar(500)")]
        public string name { get; set; }
        public string customerName { get; set; }
        public DateTime orderDate { get; set; }
        public string description { get; set; }
        public OrderStatus status { get; set; }
        public string fileContract { get; set; }
        public string fileQuote { get; set; }
        public DateTime quoteDate { get; set;}
        public double totalPrice { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}

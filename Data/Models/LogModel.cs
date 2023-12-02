using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class LogModel
    {
        public Guid id { get; set; }
        public Guid? orderId { get; set; }
        public Order Order { get; set; }
        public Guid? orderDetailId { get; set; }
        public OrderDetail orderDetail { get; set; }
        public Guid? itemId { get; set; }
        public Item? Item { get; set; }
        public Guid userId { get; set; }
        public User? User { get; set; }
        public DateTime modifiedTime { get; set; }
        public string action { get; set; } = null!;
    }
}

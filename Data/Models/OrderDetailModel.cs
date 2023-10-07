using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class OrderDetailModel
    {
        public Guid id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
        public string description { get; set; }
        public Guid orderId { get; set; }
        public Guid itemId { get; set; }
        public ItemModel? item { get; set; }
    }

    public class UpdateOrderDetailModel
    {
        public Guid id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string description { get; set; }
    }
}

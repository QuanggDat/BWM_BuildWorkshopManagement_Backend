using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class OrderModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string customerName { get; set; } = null!;
        public Guid assignToId { get; set; }
        public UserModel assign { get; set; } = null!;
        public Guid createdById { get; set; }
        public UserModel createdBy { get; set; } = null!;
        public DateTime orderDate { get; set; }
        public string? description { get; set; } = null!;
        public OrderStatus status { get; set; }
        public string fileContract { get; set; } = null!;
        public string fileQuote { get; set; } = null!;
        public DateTime quoteDate { get; set; }
        public double totalPrice { get; set; }
        public DateTime? acceptanceDate { get; set; }
    }

    public class CreateOrderModel
    {
        public string name { get; set; }
        public string customerName { get; set; }
        public string fileQuote { get; set; }
        public string fileContract { get; set; }
        public Guid assignToId { get; set; }
        public string description { get; set; } = "";
    }

    public class QuoteMaterialOrderModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string customerName { get; set; } = null!;
        public Guid assignToId { get; set; }
        public UserModel assign { get; set; } = null!;
        public Guid createdById { get; set; }
        public UserModel createdBy { get; set; } = null!;
        public DateTime orderDate { get; set; }
        public string? description { get; set; } = null!;
        public OrderStatus status { get; set; }
        public string fileContract { get; set; } = null!;
        public string fileQuote { get; set; } = null!;
        public DateTime quoteDate { get; set; }
        public double totalPrice { get; set; }
        public DateTime? acceptanceDate { get; set; }
        public List<QuoteMaterialModel> listQuoteMaterial { get; set; } = new();
    }

    public class QuoteMaterialModel
    {
        public Guid materialId { get; set; }
        public string? name { get; set; }
        public string? sku { get; set; }
        public int quantity { get; set; } = 0;
        public double price { get; set; } = 0;
        public double totalPrice { get; set; } = 0;
    }
}

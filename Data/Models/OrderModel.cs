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
        public DateTime orderDate { get; set; }
        public string description { get; set; } = null!;
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
        public string fileQuote { get; set; } = "https://firebasestorage.googleapis.com/v0/b/tamnt-sj.appspot.com/o/B%C3%81O%20GI%C3%81%20VP%20TH%E1%BB%A6Y%20S%C3%8DNH.xlsx?alt=media&token=0ecbce7a-e971-45ac-bb91-f5c46f8dd98e&_gl=1*xxbb9e*_ga*MTEwMDcxMTY0Mi4xNjk2MjUxNDAx*_ga_CW55HF8NVT*MTY5NjI2MjgzNi4zLjEuMTY5NjI2MjkwNy42MC4wLjA.";
        public string fileContract { get; set; }
        public Guid assignToId { get; set; }
    }
}

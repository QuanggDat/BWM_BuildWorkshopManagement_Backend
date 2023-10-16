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
        //public string fileQuote { get; set; } = "https://firebasestorage.googleapis.com/v0/b/tamnt-sj.appspot.com/o/TestExcel.xlsx?alt=media&token=63fd7bf2-bfc5-4a7e-adae-e844b4c9c29e&_gl=1*wtrzqx*_ga*MTEwMDcxMTY0Mi4xNjk2MjUxNDAx*_ga_CW55HF8NVT*MTY5NzA0Njg2Ny4xMy4xLjE2OTcwNDY4OTkuMjguMC4w";
        //public string fileQuote { get; set; } = "https://firebasestorage.googleapis.com/v0/b/tamnt-sj.appspot.com/o/TestExcel1.xlsx?alt=media&token=2090e439-92dd-4d16-aade-216b8d14fea4&_gl=1*3slhzp*_ga*MTEwMDcxMTY0Mi4xNjk2MjUxNDAx*_ga_CW55HF8NVT*MTY5Njc4MzkxNS45LjEuMTY5Njc4NDAwOS4yOC4wLjA.";
        //public string fileQuote { get; set; } = "https://firebasestorage.googleapis.com/v0/b/tamnt-sj.appspot.com/o/TestExcel2.xlsx?alt=media&token=beaa9410-7c47-45ba-956c-8646f23c613e&_gl=1*6an7tm*_ga*MTEwMDcxMTY0Mi4xNjk2MjUxNDAx*_ga_CW55HF8NVT*MTY5Njk1MTM4OS4xMS4xLjE2OTY5NTE0MDUuNDQuMC4w";
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

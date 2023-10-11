using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ConvertOrderExcelModel
    {
        public string? Error { get; set; }
        public List<string> ListCodeItem { get; set; } = new();
        public List<OrderExcelModel> ListConverted { get; set; } = new();
    }

    public class OrderExcelModel
    {
        public string name { get; set; }
        public double price { get; set; }

        public List<OrderChildrenExcelModel> children { get; set; } = new();
    }

    public class OrderChildrenExcelModel 
    {
        public string name { get; set; }
        public double price { get; set; }

        public List<OrderItemExcelModel> listOrderItem { get; set; } = new();
    }

    public class OrderItemExcelModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public double length { get; set; } = 0;
        public double depth { get; set; } = 0;
        public double height { get; set; } = 0;
        public string unit { get; set; }
        public double mass { get; set; } = 0;
        public int quantity { get; set; } = 0;
        public string description { get; set; }
    }
}

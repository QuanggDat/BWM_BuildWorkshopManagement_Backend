using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ResponeMaterialModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string? image { get; set; } = null!;
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; }
        public string unit { get; set; } = null!;
        public string sku { get; set; } = null!;
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } = null!;
        public int amount { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
        public Guid materialCategoryId { get; set; }
        public string materialCategoryName { get; set; } = null!;
        public Guid? createById { get; set; }
        public string createByName { get; set; } = null!;
    }

    public class CreateMaterialModel
    {
        public Guid materialCategoryId { get; set; }
        public string name { get; set; } = null!;
        public string? image { get; set; } 
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; } 
        public string unit { get; set; } = null!;
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } = null!;
        public int amount { get; set; }
        public double price { get; set; }
    }

    public class UpdateMaterialModel
    {
        public Guid id { get; set; }
        public Guid materialCategoryId { get; set; }
        public string name { get; set; } = null!;
        public string image { get; set; } = null!;
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; }
        public string unit { get; set; } = null!;
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } = null!;
        public int amount { get; set; }
        public double price { get; set; }
    }

    public class UpdateMaterialAmountModel
    {
        public Guid id { get; set; }
        public int amount { get; set; }   
    }
   
}

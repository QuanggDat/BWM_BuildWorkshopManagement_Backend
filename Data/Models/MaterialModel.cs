using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class MaterialModel
    {
        public Guid id { get; set; }
        public Guid materialCategoryId { get; set; }
        public string materialCategoryName { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? image { get; set; } = null!;
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; }
        public string unit { get; set; } = null!;
        public string sku { get; set; } = null!;
        public string importPlace { get; set; } = null!;
        public double price { get; set; }       
    }

    public class ViewMaterialInLeaderTask
    {
        public Guid id { get; set; }
        public Guid materialCategoryId { get; set; }
        public string materialCategoryName { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? image { get; set; } = null!;
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; }
        public string unit { get; set; } = null!;
        public string sku { get; set; } = null!;
        public string importPlace { get; set; } = null!;
        public double price { get; set; }
        public int quantity { get; set; }
        public double totalPrice { get; set; }
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
        public string importPlace { get; set; } = null!;
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
        public string importPlace { get; set; } = null!;
        public double price { get; set; }
    }

}

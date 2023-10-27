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
        public Guid userId { get; set; }
        public string name { get; set; } = null!;
        public string image { get; set; } = null!;
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
        public Guid? createById { get; set; }
    }

    public class CreateMaterialModel
    {
        public Guid materialCategoryId { get; set; }
        public string name { get; set; } = null!;
        public string image { get; set; } = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR2_4Ps9OjcZ7Wz84elhQhUtOoXFsko0p5XSsAZuD5So8thFrFU4IW5edaI";
        public string color { get; set; } = null!;
        public string supplier { get; set; } = null!;
        public double thickness { get; set; } 
        public string unit { get; set; } = null!;
        public string importPlace { get; set; } = null!;
        public int amount { get; set; }
        public double price { get; set; }
    }

    public class UpdateMaterialModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string image { get; set; } = null!;
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
        public Guid? createById { get; set; }
    }

    public class UpdateMaterialAmountModel
    {
        public Guid id { get; set; }
        [Required] public int amount { get; set; }
        public Guid? CreateById { get; set; }
    }

    public class DeleteMaterialModel
    {
        public Guid id { get; set; }
        public bool isDeleted { get; set; } = true;
    }

    public class ItemMaterialModel
    {
        public Guid id { get; set; }
        public Guid itemId { get; set; }
        public Guid materialId { get; set; }
        public Guid createById { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
    }

    public class AddMaterialToItemModel
    {
        [Required] public Guid itemId { get; set; }
        [Required] public Guid materialId { get; set; }
        public Guid createById { get; set; }
        [Required] public int quantity { get; set; }
        [Required] public double price { get; set; }
        public double totalPrice { get; set; }
    }

    public class UpdateMaterialToItemModel
    {
        [Required] public Guid id { get; set; }
        public Guid createById { get; set; }
        [Required] public int quantity { get; set; }
        [Required] public double price { get; set; }
        public double totalPrice { get; set; }
    }
}

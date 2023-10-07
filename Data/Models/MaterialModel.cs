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
        public string name { get; set; }
        public string image { get; set; }
        public string color { get; set; }
        public string supplier { get; set; }
        public double thickness { get; set; }
        public string unit { get; set; }
        public string sku { get; set; }
        public DateTime importDate { get; set; }
        public string importPlace { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
        public bool isDeleted { get; set; }
        public Guid categoryId { get; set; }
    }

    public class CreateMaterialModel
    {
        [Required] public string name { get; set; }
        public string image { get; set; }
        [Required] public string color { get; set; }
        [Required] public string supplier { get; set; }
        [Required] public double thickness { get; set; }
        [Required] public string unit { get; set; }
        public string sku { get; set; }
        [Required] public DateTime importDate { get; set; }
        [Required] public string importPlace { get; set; }
        [Required] public int amount { get; set; }
        [Required] public double price { get; set; }
        [Required] public double totalPrice { get; set; }
        [Required] public bool isDeleted { get; set; } = false;
        [Required] public Guid categoryId { get; set; }
    }

    public class UpdateMaterialModel
    {
        public Guid id { get; set; }
        [Required] public string name { get; set; }
        public string image { get; set; }
        [Required] public string color { get; set; }
        [Required] public string supplier { get; set; }
        [Required] public double thickness { get; set; }
        [Required] public string unit { get; set; }
        public string sku { get; set; }
        [Required] public DateTime importDate { get; set; }
        [Required] public string importPlace { get; set; }
        [Required] public int amount { get; set; }
        [Required] public double price { get; set; }
        [Required] public double totalPrice { get; set; }
        [Required] public Guid categoryId { get; set; }
    }

    public class UpdateMaterialAmountModel
    {
        public Guid id { get; set; }
        [Required] public int amount { get; set; }
    }

    public class DeleteMaterialModel
    {
        public Guid id { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}

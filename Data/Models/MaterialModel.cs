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
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; } 
        public string sku { get; set; } 
        public int quantity { get; set; }
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } 
        public double thickness { get; set; }
        public string supplier { get; set; } 
        public int amount { get; set; }
        public double price { get; set; }
        public int categoryId { get; set; }
        public int itemId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CreateMaterialModel
    {
        public string image { get; set; }
        [Required] public string name { get; set; }
        [Required] public double price { get;set; }
        [Required] public double thickness { get; set; }
        [Required] public string supplier { get;set; }
        [Required] public int amount { get;}
        [Required] public string importPlace { get; set; }
        [Required] public DateTime importDate { get; set; }= DateTime.Now;
        public bool IsDelete { get; set; }= false;
    }

    public class UpdateMaterialModel
    {
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public DateTime importDate { get; set; }=DateTime.Now;
        public string importPlace { get; set; }
        public double thickness { get; set; }
        public string supplier { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public int categoryId { get; set; }
    }

    public class DeleteMaterialModel
    {
        public int id { get; set; }
        public bool IsDeleted { get; set; } = true;
    }
}

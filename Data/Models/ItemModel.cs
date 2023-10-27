using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItemModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; }
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } 
        public string drawings2D { get; set; }
        public string drawings3D { get; set; } 
        public string description { get; set; }
        public double price { get; set; }
        public bool isDeleted { get; set; }
        public Guid createById { get; set; }
        
    }

    public class CreateItemModel
    {
        public string name { get; set; }
        public string image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; }
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } 
        public string drawings2D { get; set; } 
        public string drawings3D { get; set; } 
        public string description { get; set; }
        public double price { get; set; }
        public bool isDeleted { get; set; } = false;
        public Guid areaId { get; set; }
        public Guid? createById { get; set; }
      
    }

    public class UpdateItemModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; }
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } 
        public string drawings2D { get; set; } 
        public string drawings3D { get; set; } 
        public string description { get; set; }
        public double price { get; set; }
        public Guid areaId { get; set; }
        public Guid? createById { get; set;}
        //[Required] public Guid categoryId { get; set; }
    }

    public class DeleteItemModel
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

    


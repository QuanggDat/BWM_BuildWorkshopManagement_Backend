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
        public string name { get; set; } = null!;
        public string? image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; } = null!;
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public double price { get; set; }
        public Guid? createById { get; set; }
        public string createByName { get; set; } = null!;
        public List<_Procedure> Procedures { get; set; } = null!;
        public List<_Material> Materials { get; set; } = null!;
    }
    public class _Procedure
    {
        public Guid procedureId { get; set; }
        public string procedureName { get; set; } = null!;
    }

    public class _Material
    {
        public Guid materialId { get; set; }
        public string materialName { get; set; } = null!;
    }

    public class CreateItemModel
    {
        public string name { get; set; } = null!;
        public string? image { get; set; } 
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; } = null!;
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public double price { get; set; }
        public List<Guid> procedures { get; set; } = null!;
        public List<Guid> materials { get; set; } = null!;
    }

    public class UpdateItemModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string? image { get; set; }
        public double length { get; set; } 
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; } = null!;
        public double mass { get; set; }
        public string drawingsTechnical { get; set; } = null!;
        public string drawings2D { get; set; } = null!;
        public string drawings3D { get; set; } = null!;
        public string description { get; set; } = null!;
        public double price { get; set; }
        public List<Guid> procedures { get; set; } = null!;
        public List<Guid> materials { get; set; } = null!;
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

    


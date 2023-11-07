using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("itemCategoryId")]
        public Guid? itemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; } = null!;

        public string name { get; set; } = null!;
        public string code { get; set; } = null!;
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
        public bool isDeleted { get; set; }

        public virtual List<ProcedureItem> ProcedureItems { get; set; } = new();
        public virtual List<OrderDetail> OrderDetails { get; set; } = new();
        public virtual List<ItemMaterial> ItemMaterials { get; set; } = new();
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Material
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        [Column(TypeName = "nvarchar(1000)")]
        public string image { get; set; } = null!;
        [Column(TypeName = "nvarchar(1000)")]
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
        public bool isDeleted { get; set; }

        [ForeignKey("categoryId")]
        public Guid categoryId { get; set; }
        public MaterialCategory Category { get; set; }
        public virtual List<ItemMaterial> ItemMaterials { get; set; } = new();
    }
}

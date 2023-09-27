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
        [Key] public int materialId { get; set; }
        public string image { get; set; }
        [Column(TypeName = "nvarchar(500)")] public string name { get; set; } 
        [Column(TypeName = "nvarchar(500)")] public string sku { get; set; } 
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } 
        public double thickness { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string supplier { get; set; } 
        public int amount { get; set; }
        public double price { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public int itemId { get; set; }
        [ForeignKey("itemId")]
        public bool isDeleted { get; set; }
    }
}

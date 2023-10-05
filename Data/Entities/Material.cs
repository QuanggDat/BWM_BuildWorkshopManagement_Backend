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
        public string name { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string image { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
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
        [ForeignKey("materialId")]
        public Guid categoryId { get; set; }
        public ICollection<ItemMaterial> ItemMaterial { get; set; }
    }
}

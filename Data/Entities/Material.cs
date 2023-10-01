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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        public string image { get; set; }
        [Column(TypeName = "nvarchar(1000)")] public string name { get; set; }
        [Column(TypeName = "nvarchar(1000)")] public string sku { get; set; }
        public string color { get; set; }
        public double thickness { get; set; }
        public string unit { get; set; }
        public int quantity { get; set; }
        [Column(TypeName = "nvarchar(1000)")] public string supplier { get; set; }
        public DateTime importDate { get; set; }
        public string importPlace { get; set; }
        public int amount { get; set; }
        public int price { get; set; }
        public int totalPrice { get; set; }
        [ForeignKey("categoryId")]
        public Guid categoryId { get; set; }
        [ForeignKey("itemId")]
        public Guid itemId { get; set; }
        public bool isDeleted { get; set; }

        public ICollection<Connect1> Connect1 { get; set; }
    }
}

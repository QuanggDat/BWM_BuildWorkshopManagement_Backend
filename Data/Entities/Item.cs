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
        [Column(TypeName = "nvarchar(500)")]
        public string name { get; set; }
        public string image { get; set; }
        public double mass { get; set; }
        public string unit { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string technical { get; set; }
        public string twoD { get; set; }
        public string threeD { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        [ForeignKey("areaId")]
        public Guid areaId { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
        public ICollection<ItemMaterial> ItemMaterial { get; set; }
    }
}

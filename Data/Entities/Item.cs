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
        [Key] public int id { get; set; }
        [Column(TypeName = "nvarchar(500)")] public string name { get; set; } = string.Empty;
        public int quantity { get; set; }
        public double mass { get; set; }
        public string unit { get; set; } 
        public double length { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string description { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public bool isDeleted { get; set; }
        public bool status { get; set; }

        public ICollection<Material> Materials { get; set; }
    }
}

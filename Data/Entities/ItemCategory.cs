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
    public class ItemCategory
    {
        [Key] public Guid id { get; set; }
        [Column(TypeName = "nvarchar(500)")] public string name { get; set; }
        public bool isDeleted { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}

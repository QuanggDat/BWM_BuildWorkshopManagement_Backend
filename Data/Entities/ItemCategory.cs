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
    public class ItemCategory : BaseEntity
    {
        [Column(TypeName = "nvarchar(500)")] public string name { get; set; }
        public bool isDeleted { get; set; }

        // 1 category có nhiều item
        public virtual List<Item> items { get; set; } = new();
    }
}

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
    public class MaterialCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid categoryId { get; set; }
        [Column(TypeName = "nvarchar(1000)")] public string name { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Material> Materials { get; set; }
    }
}

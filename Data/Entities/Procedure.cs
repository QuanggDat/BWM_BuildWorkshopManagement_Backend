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
    public class Procedure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Guid id { get; set; }
        [Column(TypeName = "nvarchar(1000)")] 
        public string name { get; set; } = null!;
        public bool isDeleted { get; set; }
        public ICollection<ProcedureItem> ProcedureItem { get; set; }
    }
}

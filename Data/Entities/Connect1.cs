using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Connect1
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        [ForeignKey("itemId")]
        public Guid itemId { get; set; }
        [ForeignKey("materialId")]
        public Guid materialId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProcedureStep
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("procedureId")]
        public Guid procedureId { get; set; }
        public Procedure Procedure { get; set; } = null!;

        [ForeignKey("stepId")]
        public Guid stepId { get; set; }
        public Step Step { get; set; } = null!;

        public int priority { get; set; } 
    }
}

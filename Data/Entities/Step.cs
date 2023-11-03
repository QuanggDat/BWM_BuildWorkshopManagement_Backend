using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Step
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("procedureId")]
        public Guid procedureId { get; set; }
        public Procedure Procedure { get; set; } = null!;

        public int priority { get; set; }
        public int estimatedCompletedTime { get; set; }
        public string name { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
}

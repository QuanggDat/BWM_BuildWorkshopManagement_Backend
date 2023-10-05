using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class WokerTaskDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("wokerTaskId")]
        public Guid wokerTaskId { get; set; }
        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public virtual WokerTask WokerTask { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}

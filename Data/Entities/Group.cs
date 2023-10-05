using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid nestId { get; set; }
        public Guid userId { get; set; }
        public virtual Squad Nest { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        [ForeignKey("managerId")]
        public Guid managerId { get; set; }
        public virtual ManagerTask ManagerTask { get; set; } = null!;
    }
}

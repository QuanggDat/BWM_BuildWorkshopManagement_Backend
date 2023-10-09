using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ManagerTaskGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("managerTaskId")]
        public Guid? managerTaskId { get; set; }
        public virtual ManagerTask ManagerTask { get; set; } = null!;

        [ForeignKey("groupId")]
        public Guid? groupId { get; set; }
        public virtual Group Group { get; set; } = null!;
    }
}

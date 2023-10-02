using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class UserTask
    {
        [ForeignKey("userId")]
        public Guid userId { get; set; }
        [ForeignKey("taskId")]
        public Guid taskId { get; set; }
        public virtual Task Task { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}

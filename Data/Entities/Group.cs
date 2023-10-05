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
        [ForeignKey("squadId")]
        public Guid squadId { get; set; }
        public virtual Squad Squad { get; set; } = null!;
        [ForeignKey("managerId")]
        public Guid managerId { get; set; }
        public virtual ManagerTask ManagerTask { get; set; } = null!;
        public virtual ICollection<User> User { get; set; } = new List<User>();
    }
}

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
        public string name { get; set; }
        public int member { get; set; }
        public bool isDeleted { get; set; }
        [ForeignKey("squadId")]
        public Guid squadId { get; set; }
        public virtual Squad Squad { get; set; } = null!;

        public virtual List<User> Users { get; set; } = new();
        public virtual List<ManagerTaskGroup> ManagerTaskGroups { get; set; } = new();
    }
}

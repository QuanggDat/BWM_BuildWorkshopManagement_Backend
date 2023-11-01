using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Team
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("groupId")]
        public Guid groupId { get; set; }
        public virtual Group Group { get; set; } = null!;

        public string name { get; set; } = null!;
        public int member { get; set; }
        public bool isDeleted { get; set; }
              
        public virtual List<User> Users { get; set; } = new();
        public virtual List<LeaderTask> LeaderTasks { get; set; } = new();
    }
}

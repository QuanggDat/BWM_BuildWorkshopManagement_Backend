using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Squad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public int member { get; set; }
        public bool isDeleted { get; set; }

        [ForeignKey("managerId")]
        public Guid managerId { get; set; }
        public User manager { get; set; }

        public virtual List<Group> Groups { get; set; } = new();
    }
}


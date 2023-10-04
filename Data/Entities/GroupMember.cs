using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class GroupMember
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("groupId")]
        public Guid groupId { get; set; }
        public virtual Group Group { get; set; } = null!;

        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

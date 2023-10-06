using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public User User { get; set; } = null!;
        public bool seen { get; set; }
        public string action { get; set; } = null!;
        public string? description { get; set; }
        public string subject { get; set; } = null!;
        public string content { get; set; } = null!;
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime dateUpdated { get; set; } = DateTime.Now;
        public bool isDeleted { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Notification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public User User { get; set; }
        public bool seen { get; set; }
        public string action { get; set; }
        public string description { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
        public bool isDeleted { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public DateTime dateUpdated { get; set; } = DateTime.Now;

        //public ICollection<User> Users { get; set; }
    }
}

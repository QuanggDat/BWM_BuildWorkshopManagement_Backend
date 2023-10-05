using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Guid id { get; set; }
        public string title { get; set; } = null!;
        public string overviewReport { get; set; } = null!;
        public string? doneReport { get; set; } = null!;
        public string? doingReport { get; set; } = null!;
        public string? todoReport { get; set; } = null!;
        public DateTime createdDate { get; set; }
        public bool status { get; set; }

        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public User User { get; set; } = null!;
        [ForeignKey("taskId")]
        public Guid taskId { get; set; }
        public Task Task { get; set; }

        public ICollection<Resource> Resource { get; set; }
        //public ICollection<Notification> Notifications { get; set; }
    }
}

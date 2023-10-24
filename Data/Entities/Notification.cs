using Data.Enums;
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
        public string? title { get; set; }
        public string? content { get; set; }
        public bool seen { get; set; } = false;
        public NotificationType? type { get; set; }
        public bool isDeleted { get; set; } = false;
        public DateTime dateCreated { get; set; } = DateTime.Now;

        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public Order? Order { get; set; }

        [ForeignKey("reportId")]
        public Guid? reportId { get; set; }
        public Report Report { get; set; } = null!;

        [ForeignKey("managerTaskId")]
        public Guid? managerTaskId { get; set; }
        public ManagerTask ManagerTask { get; set; } = null!;

        [ForeignKey("wokerTaskId")]
        public Guid? wokerTaskId { get; set; }
        public WokerTask WokerTask { get; set; } = null!;

    }
}

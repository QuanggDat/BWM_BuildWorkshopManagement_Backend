using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;

namespace Data.Models
{
    public class NotificationModel
    {
        public Guid id { get; set; }
        public string? title { get; set; } 
        public string? content { get; set; }
        public bool seen { get; set; } 
        public NotificationType? type { get; set; }
        public bool isDeleted { get; set; }
        public DateTime createdDate { get; set; }

        public Guid userId { get; set; }
        public UserModel User { get; set; } = null!;

        public Guid? orderId { get; set; }
        public OrderModel? Order { get; set; }

        public Guid? reportId { get; set; }
        public ReportModel? Report { get; set; }
    }

    public class NotificationCreateModel
    {
        public string? title { get; set; }
        public string? content { get; set; }
        public Guid userId { get; set; }
        public Guid? orderId { get; set; }
    }

    public class NewNotificationModel
    {
        public NotificationModel Notification { get; set; }
        public int CountUnseen { get; set; }
    }

}

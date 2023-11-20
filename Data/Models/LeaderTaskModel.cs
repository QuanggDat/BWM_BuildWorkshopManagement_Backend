using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateLeaderTaskModel
    {
        public Guid leaderId { get; set; }
        public Guid orderId { get; set; }
        public Guid? itemId { get; set; }
        public int priority { get; set; }
        public int itemQuantity { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; } 
    }
    public class CreateAcceptanceTaskModel
    {
        public Guid leaderId { get; set; }
        public Guid orderId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string? description { get; set; } 
    }

    public class LeaderTaskModel
    {
        public Guid id { get; set; }
        public Guid? leaderId { get; set; }
        public string? leaderName { get; set; } 

        public Guid? createdById { get; set; } = null!;
        public string createdByName { get; set; } = null!;

        public Guid? orderId { get; set; }
        public string orderName { get; set; } = null!;

        public Guid? itemId { get; set; }
        public string? itemName { get; set; } 
        public string? drawingsTechnical { get; set; }
        public string name { get; set; } = null!;
        public int priority { get; set; }

        public int itemQuantity { get; set; }
        public int? itemCompleted { get; set; }
        public int? itemFailed { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }

        public ETaskStatus status { get; set; }
        public string? description { get; set; } 
        public bool isDeleted { get; set; }
    }

    public class UpdateLeaderTaskModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public Guid? leaderId { get; set; }
        public int priority { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public ETaskStatus status { get; set; }
        public string? description { get; set; }   
    }

}

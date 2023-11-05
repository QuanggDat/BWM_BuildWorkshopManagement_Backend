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
        public Guid procedureId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string description { get; set; } = null!;        
    }
    public class LeaderTaskModel
    {
        public Guid leaderId { get; set; }
        public string leaderName { get; set; } = null!;
        public Guid orderId { get; set; }
        public string orderName { get; set; } = null!;    
        public Guid? createdById { get; set; } = null!;
        public string createByName { get; set; } = null!;
        public string name { get; set; } = null!;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }
        public ETaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
    public class UpdateLeaderTaskModel
    {
        public Guid id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public ETaskStatus status { get; set; }
        public string description { get; set; } = null!;
        
    }
    public class AssignLeaderTaskModel
    {
        public Guid teamId { get; set; }
        public Guid leaderTaskId { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateManagerTaskModel
    {
        public Guid managerId { get; set; }
        public Guid orderId { get; set; }
        public Guid procedureId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string description { get; set; } = null!;        
    }
    public class ResponseManagerTaskModel
    {
        public Guid managerId { get; set; }
        public string managerName { get; set; } = null!;
        public Guid orderId { get; set; }
        public string orderName { get; set; } = null!;    
        public Guid? createdById { get; set; } = null!;
        public string createByName { get; set; } = null!;
        public string name { get; set; } = null!;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime? completedTime { get; set; }
        public TaskStatus status { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
    public class UpdateManagerTaskModel
    {
        public Guid id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public TaskStatus status { get; set; }
        public string description { get; set; } = null!;
        
    }
    public class AssignManagerTaskModel
    {
        public Guid groupId { get; set; }
        public Guid taskManagerId { get; set; }
    }

}

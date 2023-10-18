using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class WokerTaskModel
    {
        public class CreateWokerTaskModel
        {
            public Guid managerTaskId { get; set; }
            public string name { get; set; } = null!;
            public DateTime timeStart { get; set; }
            public DateTime timeEnd { get; set; }
            public TaskStatus status { get; set; }
            public string description { get; set; } = null!;
            public List<string> assignees { get; set; } = null!;
        }

        public class UpdateWokerTaskModel
        {
            public Guid wokerTaskId { get; set; }
            public string name { get; set; } = null!;
            public string description { get; set; } = null!;
            public DateTime timeStart { get; set; }
            public DateTime timeEnd { get; set; }
            public TaskStatus status { get; set; }
            public List<Guid> assignees { get; set; } = null!;
        }

        public class UpdateWokerTaskStatusModel
        {
            public TaskStatus status { get; set; }
        }
        public class TaskMemberResponse
        {
            public Guid memberId { get; set; }
            public string memberFullName { get; set; } = null!;
        }
        public class WokerTaskResponseModel
        {
            public Guid wokerTaskId { get; set; }
            public Guid? userId { get; set; }
            public string userFullName { get; set; } = null!;
            public Guid managerTaskId { get; set; }
            public string name { get; set; } = null!;
            public string description { get; set; } = null!;
            public DateTime? timeStart { get; set; }
            public DateTime? timeEnd { get; set; }
            public TaskStatus status { get; set; }
            public List<TaskMemberResponse> Members { get; set; } = null!;
        }

        public class AssignWokerTaskModel
        {
            public Guid memberId { get; set; }
            public Guid wokerTaskId { get; set; }
        }
    }
}

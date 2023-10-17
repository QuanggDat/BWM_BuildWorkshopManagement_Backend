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
            public DateTime? completedTime { get; set; }
            public TaskStatus status { get; set; }
            public string description { get; set; } = null!;
            public bool isDeleted { get; set; }
            public List<string> Assignees { get; set; } = null!;
        }
    }
}

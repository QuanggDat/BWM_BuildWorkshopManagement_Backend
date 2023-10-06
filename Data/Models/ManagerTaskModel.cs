using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateManagerTaskModel
    {
        public Guid managerId { get; set; }
        public Guid orderId { get; set; }
        public string name { get; set; } = null!;
        public DateTime timeStart { get; set; }
        public DateTime timeEnd { get; set; }
        public DateTime completedTime { get; set; }
        public string description { get; set; } = null!;
        public bool isDeleted { get; set; }
    }
}

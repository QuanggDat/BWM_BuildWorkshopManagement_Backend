using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class GroupModel
    {
        public Guid id {  get; set; }
        public string name { get; set; }
        public int member { get; set; }
        public bool isDeleted { get; set; }
        public Guid squadId { get; set; }
    }

    public class CreateGroupModel
    {
        public string name { get; set; }
        public int member { get; set; } = 0;
        public Guid squadId { get; set; }
        public bool isDeleted { get; set; } 
    }

    public class UpdateGroupModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid squadId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TeamModel
    {
        public Guid id {  get; set; }
        public string name { get; set; }
        public int member { get; set; }
        public bool isDeleted { get; set; }
        public Guid groupId { get; set; }
    }

    public class CreateTeamModel
    {
        public string name { get; set; }
        public Guid groupId { get; set; }
        public List<Guid> listUserId { get; set; } = new();
    }

    public class UpdateTeamModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid groupId { get; set; }
        public List<Guid> listUserId { get; set; } = new();
    }
}

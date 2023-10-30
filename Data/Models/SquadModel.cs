using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class SquadModel
    {
        public Guid id {  get; set; }
        public string name { get; set; }
        public int member { get; set; }
        public bool isDeleted { get; set; }
    }

    public class CreateSquadModel
    {
        public string name { get; set; }
        public List<Guid> listUserId { get; set; } = new();
    }

    public class UpdateSquadModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<Guid> listUserId { get; set; } = new();
    }

    public class DeleteSquadModel
    {
        public Guid id { get; set; }
    }
}

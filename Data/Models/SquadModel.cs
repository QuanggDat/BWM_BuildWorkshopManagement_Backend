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
        public Guid userId { get; set; }
        public bool isDeleted { get; set; }
    }

    public class CreateSquadModel
    {
        public string name { get; set; }
        public bool isDeleted { get; set; } = false;
    }

    public class UpdateSquadModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class DeleteSquadModel
    {
        public Guid id { get; set; }
    }
}

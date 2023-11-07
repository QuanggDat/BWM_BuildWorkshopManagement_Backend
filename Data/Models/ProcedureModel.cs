using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ProcedureModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
    public class CreateProcedureModel
    {
        public string name { get; set; } = null!;
    }
    public class UpdateProcedureModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
}

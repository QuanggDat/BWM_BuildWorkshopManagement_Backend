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
        public List<ProcedureStepModel> listStep { get; set; } = null!;
    }

    public class ProcedureViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
    public class CreateProcedureModel
    {
        public string name { get; set; } = null!;
        public List<AddProcedureStep> listStep { get; set; } = null!;
    }
    public class ProcedureStepModel
    {
        public Guid stepId { get; set; }
        public string stepName { get; set; } = null!;
        public int priority { get; set; }
    }
    public class AddProcedureStep
    {
        public Guid stepId { get; set; }
        public int priority { get; set; }
    }
    public class UpdateProcedureModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public List<AddProcedureStep> listStep { get; set; } = null!;
    }
}

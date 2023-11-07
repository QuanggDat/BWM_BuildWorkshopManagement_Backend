using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class StepModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
    public class CreateStepModel
    {
        public string name { get; set; } = null!;
    }
    public class UpdateStepModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
}

using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ReportModel
    {
        public class CreateReportModel
        {
            public Guid? managerTaskId { get; set; }
            public ReportType reportType { get; set; }
            public string title { get; set; } = null!;
            public string? content { get; set; } = null!;
            public ReportStatus? reportStatus { get; set; }
            public List<string>? resource { get; set; } 
            public DateTime createdDate { get; set; }

        }
    }
}

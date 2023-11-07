using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateTaskReportModel
    {
        public Guid? leaderTaskId { get; set; }
        public ReportType reportType { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? reportStatus { get; set; }
        public List<string>? resource { get; set; }
        public DateTime createdDate { get; set; }
    }

    public class TaskReportModel
    {
        public Guid id { get; set; }
        public Guid? leaderTaskId { get; set; }       
        public ReportType reportType { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? reportStatus { get; set; }
        public List<string>? resource { get; set; }
        public DateTime createdDate { get; set; }
        public Guid reporterId { get; set; }
        public Guid? responderId { get; set; } 
        public string? responseContent { get; set; }
    }

    public class SendResponseModel
    {
        public Guid reportId { get; set; }
        public ReportStatus? reportStatus { get; set; }
        public string responseContent { get; set; } = null!;
    }

}

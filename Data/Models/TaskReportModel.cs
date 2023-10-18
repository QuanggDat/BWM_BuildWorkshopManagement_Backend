using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class TaskReportModel
    {
        public class CreateTaskReportModel
        {
            public Guid? managerTaskId { get; set; }
            public ReportType reportType { get; set; }
            public string title { get; set; } = null!;
            public string? content { get; set; } = null!;
            public ReportStatus? reportStatus { get; set; }
            public List<string>? resource { get; set; } 
            public DateTime createdDate { get; set; }

        }

        public class UpdateTaskReportModel
        {
            public Guid? managerTaskId { get; set; }
            public ReportType reportType { get; set; }
            public string title { get; set; } = null!;
            public string? content { get; set; } = null!;
            public ReportStatus? reportStatus { get; set; }
            public List<string>? resource { get; set; }
        }

        public class ResponseTaskReportModel
        {
            public Guid id { get; set; }
            public Guid? managerTaskId { get; set; }
            public string orderName { get; set; } = null!;
            public string managerTaskName { get; set; } = null!;
            public ReportType reportType { get; set; }
            public string title { get; set; } = null!;
            public string? content { get; set; } = null!;
            public ReportStatus? reportStatus { get; set; }
            public List<string>? resource { get; set; }           
            public DateTime createdDate { get; set; }
            public Reporter reporter { get; set; } = null!;
            public Reviewer reviewer { get; set; } = null!; 
            public string? responseContent { get; set; }            

        }

        public class Reviewer
        {
            public Guid id { get; set; }
            public string fullName { get; set; } = null!;
        }

        public class Reporter
        {
            public Guid id { get; set; }
            public string fullName { get; set; } = null!;
            public string phoneNumber { get; set; } = null!;
            public string email { get; set; } = null!;
        }

        public class ReviewsReportModel
        {
            public Guid reportId { get; set; }
            public ReportStatus? reportStatus { get; set;}
            public string responseContent { get; set; } = null!;
        }
    }
}

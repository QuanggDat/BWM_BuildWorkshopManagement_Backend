using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateOrderReportModel
    {
        public Guid? orderId { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? reportStatus { get; set; }
        public DateTime createdDate { get; set; }

    }

    public class OrderReportModel
    {
        public Guid id { get; set; }
        public string orderName { get; set; } = null!;
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? reportStatus { get; set; }
        public DateTime createdDate { get; set; }
        public ReporterOrderReport reporter { get; set; } = null!;
        public string? responseContent { get; set; }
    }

    public class ReporterOrderReport
    {
        public Guid id { get; set; }
        public string fullName { get; set; } = null!;
        public string phoneNumber { get; set; } = null!;
        public string email { get; set; } = null!;
    }

    public class ReviewsOrderReportModel
    {
        public Guid reportId { get; set; }
        public ReportStatus reportStatus { get; set; }
        public string contentReviews { get; set; } = null!;
    }

}

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
        public ReportStatus? status { get; set; }
        public List<string>? resource { get; set; }
    }

    public class OrderReportModel
    {
        public Guid id { get; set; }
        public Guid? orderId { get; set; }
        public string orderName { get; set; } = null!;
        public Guid reporterId { get; set; }
        public string reporterName { get; set; } = null!;
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? status { get; set; }
        public DateTime createdDate { get; set; }     
        public List<string>? resource { get; set; }
    }
}

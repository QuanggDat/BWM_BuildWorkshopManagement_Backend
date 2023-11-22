using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Report
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public virtual Order Order { get; set; } = null!;

        [ForeignKey("leaderTaskId")]
        public Guid? leaderTaskId { get; set; }
        public virtual LeaderTask LeaderTask { get; set; } = null!;

        [ForeignKey("reporterId")]
        public Guid reporterId { get; set; }
        public virtual User Reporter { get; set; } = null!;

        public ReportType reportType { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public int? itemFailed { get; set; }
        public DateTime createdDate { get; set; }       

        public ReportStatus? status { get; set; }
        public string? responseContent { get; set; } = null!;

        public virtual List<Resource> Resources { get; set; } = new();
        public virtual List<Supply> Supplies { get; set; } = new();
    }
}

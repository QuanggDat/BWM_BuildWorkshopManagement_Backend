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

        [ForeignKey("managerTaskId")]
        public Guid? managerTaskId { get; set; }
        public virtual ManagerTask ManagerTask { get; set; } = null!;

        [ForeignKey("reporterId")]
        public Guid reporterId { get; set; }
        public virtual User Reporter { get; set; } = null!;

        public ReportType reportType { get; set; }
        
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;

        public ReportStatus? reportStatus { get; set; }

        public DateTime createdDate { get; set; }                   
        public virtual List<Resource> Resources { get; set; } = new();

    }
}

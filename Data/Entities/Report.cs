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
        public Guid reporterId { get; set; }
        public virtual User reporter { get; set; } = null!;
        public string title { get; set; } = null!;
        public string overviewReport { get; set; } = null!;
        public string? doneReport { get; set; } = null!;
        public string? doingReport { get; set; } = null!;
        public string? todoReport { get; set; } = null!;
        public DateTime createdDate { get; set; }
        public int period { get; set; }

        
       
    }
}

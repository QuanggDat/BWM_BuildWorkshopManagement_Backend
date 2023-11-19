using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Resource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("reportId")]
        public Guid? reportId { get; set; }
        public Report? Report { get; set; } 

        [ForeignKey("orderId")]
        public Guid? orderId { get; set; }
        public Order? Order { get; set; }

        public string link { get; set; } = null!;        
    }
}

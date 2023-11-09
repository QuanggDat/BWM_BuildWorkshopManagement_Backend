using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Supply
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey("reportId")]
        public Guid reportId { get; set; }
        public Report Report { get; set; } = null!;

        public string materialName { get; set; } = null!;
        public int amount { get; set; }
        public double totalPrice { get; set; }
    }
}

using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ResourceModel
    {
        public Guid id { get; set; }
        public Guid? reportId { get; set; }
        public Guid? orderId { get; set; }
        public string link { get; set; } = null!;
    }
}

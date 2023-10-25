using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class FloorModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public double price { get; set; }
        public bool isDeleted { get; set; }
    }
}

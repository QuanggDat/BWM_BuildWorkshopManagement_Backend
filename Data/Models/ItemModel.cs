using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItemModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double mass { get; set; }
        public string unit { get; set; }
        public double lenghth { get; set; }
        public double width { get; set; }
        public double heighth { get; set; }
        public string description { get; set; }
        public int categoryId { get; set; }
        public bool IsDeleted { get; set; }
        public bool status { get; set; }
    }
}

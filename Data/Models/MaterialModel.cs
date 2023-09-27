using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class MaterialModel
    {
        public int id { get; set; }
        public string name { get; set; } 
        public string sku { get; set; } 
        public int quantity { get; set; }
        public DateTime importDate { get; set; }
        public string importPlace { get; set; } 
        public double thickness { get; set; }
        public string supplier { get; set; } 
        public int amount { get; set; }
        public int categoryId { get; set; }
        public int itemId { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class AreaModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public double price { get; set; }
        public bool isDeleted { get; set; }
        public Guid? parentId { get; set; }
        public AreaModel? parent { get; set; }
        public List<ItemModel> items { get; set; } = new();
    }
}

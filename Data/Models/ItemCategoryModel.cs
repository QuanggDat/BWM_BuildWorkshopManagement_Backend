using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItemCategoryModel
    {
        public Guid id { get; set; }      
        public string name { get; set; } = null!;
        public int quantityItem { get; set; }
    }

    public class CreateItemCategoryModel
    {
        public string name { get; set; } = null!;
    }

    public class UpdateItemCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
}

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
        public string name { get; set; }
        public bool isDeleted { get; set; } 
    }

    public class CreateItemCategoryModel
    {
        [Required] public string name { get; set; }
        public bool isDeleted { get; set; } = false;
    }

    public class UpdateItemCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class DeleteItemCategoryModel
    {
        public Guid id { get; set;}
        public bool isDeleted { get; set; } = true;
    }
}

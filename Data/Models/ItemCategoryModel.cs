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
        public int categoryId { get; set; }
        public string name { get; set; }
        public bool IsDelete { get; set; }
    }

    public class CreateItemCategoryModel
    {
        [Required] public string name { get; set; }
        public bool IsDelete { get; set; } = false;
    }

    public class UpdateItemCategoryModel
    {
        public int categoryId { get; set; }
        public string name { get; set; }
    }

    public class DeleteItemCategoryModel
    {
        public int categoryId { get; set;}
        public bool IsDelete { get; set; } = true;
    }
}

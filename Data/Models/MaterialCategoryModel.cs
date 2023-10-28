using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class MaterialCategoryModel
    {
        public Guid id { get; set; }
        public Guid? createById { get; set; }
        public string createByName { get; set; } = null!;
        public string name { get; set; } = null!;        
    }

    public class CreateMaterialCategoryModel
    {      
        public string name { get; set; } = null!;
    }

    public class UpdateMaterialCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }
}

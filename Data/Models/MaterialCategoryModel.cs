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
        public string name { get; set; }
        public bool isDeleted { get; set; }
    }

    public class CreateMaterialCategoryModel
    {
        [Required] public string name { get; set; }
        public bool isDeleted { get; set; } = false;
    }
    public class UpdateMaterialCategoryModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class DeleteMaterialCategoryModel
    {
        public Guid id { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}

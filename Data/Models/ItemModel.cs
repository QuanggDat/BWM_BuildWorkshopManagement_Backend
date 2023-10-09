using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ItemModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double length { get; set; }
        public double depth { get; set; }
        public double height { get; set; }
        public string unit { get; set; }
        public double mass { get; set; }
        public string technical { get; set; }
        public string twoD { get; set; }
        public string threeD { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public bool isDeleted { get; set; }
        public Guid areaId { get; set; }
        public Guid categoryId { get; set; }
    }

    public class CreateItemModel
    {
        [Required] public string name { get; set; }
        public string image { get; set; }
        [Required] public double length { get; set; }
        [Required] public double depth { get; set; }
        [Required] public double height { get; set; }
        [Required] public string unit { get; set; }
        [Required] public double mass { get; set; }
        public string technical { get; set; }
        public string twoD { get; set; }
        public string threeD { get; set; }
        public string description { get; set; }
        [Required] public double price { get; set; }
        [Required] public bool isDeleted { get; set; } = false;
        [Required] public Guid areaId { get; set; }
        [Required] public Guid categoryId { get; set; }
    }

    public class UpdateItemModel
    {
        public Guid id { get; set; }
        [Required] public string name { get; set; }
        public string image { get; set; }
        [Required] public double length { get; set; }
        [Required] public double depth { get; set; }
        [Required] public double height { get; set; }
        [Required] public string unit { get; set; }
        [Required] public double mass { get; set; }
        public string technical { get; set; }
        public string twoD { get; set; }
        public string threeD { get; set; }
        public string description { get; set; }
        [Required] public double price { get; set; }
        [Required] public Guid areaId { get; set; }
        [Required] public Guid categoryId { get; set; }
    }

    public class DeleteItemModel
    {
        public Guid id { get; set; }
        public bool isDeleted { get; set; } = true;
    }
}

    


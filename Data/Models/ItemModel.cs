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
        public string image { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double mass { get; set; }
        public string unit { get; set; }
        public double lenghth { get; set; }
        public double width { get; set; }
        public double heighth { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public int categoryId { get; set; }
        public bool IsDeleted { get; set; }
        public bool status { get; set; }
    }

    public class CreateItemModel
    {
        public string image { get; set; }
        [Required] public string name { get; set; }
        [Required] public double price { get; set; }
        [Required] public double mass { get; set; }
        [Required] public string unit { get; set; }
        [Required] public double lenghth { get; set; }
        [Required] public double width { get; set; }
        [Required] public double heighth { get; set; }
        [Required] public int categoryId { get; set; }
        public bool status { get; set; }
        public bool IsDelete { get; set; }= false;
    }

    public class UpdateItemModel
    {
        public Guid id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double mass { get; set; }
        public string unit { get; set; }
        public double lenghth { get; set; }
        public double width { get; set; }
        public double heighth { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public Guid itemCategoryId { get; set; }
    }

    public class DeleteItemModel
    {
        public Guid id { get; set; }
        public bool IsDeleted { get; set; } = true;
    }
}

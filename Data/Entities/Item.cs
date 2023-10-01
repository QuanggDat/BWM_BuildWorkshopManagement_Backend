using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Item : BaseEntity
    {
        // Những field nào không requie phải có data thì thêm dấu ? để khi gen db nó biết là có NOT NULL hay không (ví dụ field description)
        // viết thường chữ đầu thì viết thường hết cho đồng bộ
        public string image { get; set; }
        [Column(TypeName = "nvarchar(500)")] public string name { get; set; } = string.Empty;
        public int quantity { get; set; }
        public double mass { get; set; }
        public string unit { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string? description { get; set; }
        public double price { get; set; }
        public bool isDeleted { get; set; }
        public bool status { get; set; }


        //public int categoryId { get; set; }
        //[ForeignKey("categoryId")]

        // cách tổ chức khoá ngoại 
        // khoá ngoại nên là tên bảng + đuôi Id cho rõ ràng
        public Guid itemCategoryId { get; set; }
        [ForeignKey("itemCategoryId")]
        public virtual ItemCategory? itemCategory { get; set; }

        //public ICollection<Material> materials { get; set; }
    }
}

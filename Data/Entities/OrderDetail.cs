using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public Guid id { get; set; }

        [ForeignKey("orderId")]
        public Guid orderId { get; set; }
        public Order Order { get; set; } = null!;

        [ForeignKey("itemId")]
        public Guid? itemId { get; set; }
        public Item? Item { get; set; }

        public string? itemCategoryName { get; set; } 
        public string? itemName { get; set; }
        public string? itemCode { get; set; }
        public string? itemImage { get; set; }
        public double itemLength { get; set; }
        public double itemDepth { get; set; }
        public double itemHeight { get; set; }
        public string? itemUnit { get; set; } 
        public double itemMass { get; set; }
        public string? itemDrawingsTechnical { get; set; }
        public string? itemDrawings2D { get; set; } 
        public string? itemDrawings3D { get; set; } 

        public string? description { get; set; }
        public int quantity { get; set; }
        public double price { get; set; } = 0;
        public double totalPrice { get; set; } = 0;

        public virtual List<OrderDetailMaterial> OrderDetailMaterials { get; set; } = new();

    }
}

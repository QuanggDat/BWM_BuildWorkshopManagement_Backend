using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class OrderDetailMaterialModel
    {
        public Guid id { get; set; }
        public Guid orderDetailId { get; set; }
        public Guid materialId { get; set; }

        public string materialName { get; set; } = null!;
        public string materiaSupplier { get; set; } = null!;
        public double materiaThickness { get; set; }
        public string materiaSku { get; set; } = null!;
        public int quantity { get; set; }
        public double price { get; set; }
        public double totalPrice { get; set; }
    }

    public class UpdateOrderDetailMaterialModel
    {
        public Guid id { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
    }
}

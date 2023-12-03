using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class OrderModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string customerName { get; set; } = null!;
        public Guid assignToId { get; set; }
        public UserModel AssignTo { get; set; } = null!;
        public Guid createdById { get; set; }
        public UserModel CreatedBy { get; set; } = null!;
        public DateTime createTime { get; set; }
        //public DateTime updateTime { get; set; }
        public string? description { get; set; } = null!;
        public OrderStatus status { get; set; }
        public string fileContract { get; set; } = null!;
        public string fileQuote { get; set; } = null!;
        public DateTime? quoteTime { get; set; }
        public double totalPrice { get; set; }
        public DateTime? acceptanceTime { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public DateTime? inProgressTime { get; set; }
        public List<string> resources { get; set; } = new();
    }

    public class ViewOrderModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public string customerName { get; set; } = null!;
        public Guid assignToId { get; set; }
        public UserModel? AssignTo { get; set; } = null!;
        public Guid createdById { get; set; }
        public UserModel? CreatedBy { get; set; } = null!;
        public DateTime createTime { get; set; }
        public string? description { get; set; } = null!;
        public OrderStatus status { get; set; }
        public string fileContract { get; set; } = null!;
        public string fileQuote { get; set; } = null!;
        public DateTime? quoteTime { get; set; }
        public double totalPrice { get; set; }
        public DateTime? acceptanceTime { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public DateTime? inProgressTime { get; set; }
        public List<LeaderTaskViewModel>? leaderTask { get; set; }
        public List<OrderDetailView>? orderDetail { get; set; }
    }

    public class LeaderTaskViewModel
    {
        public Guid leaderTaskId { get; set; }
        public List<Guid>? workerTaskId { get; set; }
    }

    public class OrderDetailView
    {
        public Guid orderDetailId { get; set; }
        public List<Guid>? orderDetailMaterialId { get; set; }
    }

    public class CreateOrderModel
    {
        public string name { get; set; }
        public string customerName { get; set; }
        public string fileQuote { get; set; }
        public string fileContract { get; set; }
        public Guid assignToId { get; set; }
        public string description { get; set; } = "";
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }

    public class UpdateOrderModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string customerName { get; set; }
        public string fileContract { get; set; }
        public Guid assignToId { get; set; }
        public string description { get; set; } = "";
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }

    public class QuoteMaterialOrderModel
    {
        public Guid orderId { get; set; }

        public double totalPriceOrder { get; set; } = 0;
        public List<QuoteMaterialDetailModel> listFromOrder { get; set; } = new();
        public double totalPriceSupplyDamage { get; set; } = 0;
        public List<QuoteMaterialDetailModel> listFromSupplyDamage { get; set; } = new();

        public double percentDamage { get; set; } = 0;
    }

    public class QuoteMaterialDetailModel
    {
        public Guid materialId { get; set; }
        public string? name { get; set; }
        public string? sku { get; set; }
        public string? supplier { get; set; }
        public double thickness { get; set; } = 0;
        public string? color { get; set; }
        public string? unit { get; set; }
        public int quantity { get; set; } = 0;
        public double price { get; set; } = 0;
        public double totalPrice { get; set; } = 0;
    }
}

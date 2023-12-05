using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;

namespace Data.Models
{
    public class OrderDetailModel
    {
        public Guid id { get; set; }
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
        public ItemModel? item { get; set; }
    }

    public class OrderDetailViewlModel
    {
        public Guid id { get; set; }
        public Guid? itemId { get; set; }
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

        public List<ViewLeaderTask>? leaderTasks { get; set; }
    }

    public class ViewLeaderTask
    {
        public Guid id { get; set; }
        public Guid? leaderId { get; set; }
        public string? leaderName { get; set; }

        public Guid? createdById { get; set; } = null!;
        public string? createdByName { get; set; }

        public string name { get; set; } = null!;
        public int priority { get; set; }

        public Guid? itemId { get; set; }
        public string? itemName { get; set; }

        public int itemQuantity { get; set; }
        public int? itemCompleted { get; set; }
        public int? itemFailed { get; set; }

        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
        public DateTime? completedTime { get; set; }

        public ETaskStatus status { get; set; }
        public string? description { get; set; }
        public bool isDeleted { get; set; }

        public List<WorkerTaskViewModel>? workerTask { get; set; }
    }

    public class CreateOrderDetailModel
    {
        public Guid? itemId { get; set; }
        public Guid? orderId { get; set; }

        /*public string? itemDrawingsTechnical { get; set; }
        public string? itemDrawings2D { get; set; }
        public string? itemDrawings3D { get; set; }*/
        public string? description { get; set; }

        public int quantity { get; set; }
    }

    public class UpdateOrderDetailModel
    {
        public Guid id { get; set; }
        public string? itemDrawingsTechnical { get; set; }
        public string? itemDrawings2D { get; set; }
        public string? itemDrawings3D { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string description { get; set; }
    }
}

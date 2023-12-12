using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class LogModel
    {
        public Guid id { get; set; }
        public Guid? orderId { get; set; }
        public string? orderName { get; set; }
        public Guid? orderDetailId { get; set; }
        public string? orderDetailName { get; set; }
        public Guid? itemId { get; set; }
        public string? itemName { get; set; }
        public Guid? userId { get; set; }
        public string? userName { get; set; }
        public Guid? groupId { get; set; }
        public string? groupName { get; set; }
        public Guid? materialId { get; set; }
        public string? materialName { get; set; }
        public DateTime modifiedTime { get; set; }
        public string action { get; set; } = null!;
    }
}

using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.OrderService
{
    public interface IOrderService
    {
        ResultModel GetAllWithPaging(int pageIndex, int pageSize);
        ResultModel GetQuotesByUserWithPaging(Guid userId, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        Task<ResultModel> Create(CreateOrderModel model);
        ResultModel UpdateStatus(Guid id, OrderStatus status);
    }
}

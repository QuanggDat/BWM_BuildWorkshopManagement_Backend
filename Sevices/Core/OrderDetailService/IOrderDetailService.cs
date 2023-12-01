using Data.Models;

namespace Sevices.Core.OrderDetailService
{
    public interface IOrderDetailService
    {
        ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize, string? search = null);
        ResultModel CreateOrderDetail (CreateOrderDetailModel model);
        ResultModel Update(UpdateOrderDetailModel model);
    }
}

using Data.Models;

namespace Sevices.Core.OrderDetailService
{
    public interface IOrderDetailService
    {
        ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize, string? search = null);
        ResultModel CreateOrderDetail (CreateOrderDetailModel model, Guid userId);
        ResultModel Update(UpdateOrderDetailModel model, Guid userId);
        ResultModel Delete(Guid id, Guid userId);
        ResultModel GetAllByOrderDetailId(Guid id);
        ResultModel GetLogOnOrderDetailByOrderId(Guid orderId, string? search, int pageIndex, int pageSize);
    }
}

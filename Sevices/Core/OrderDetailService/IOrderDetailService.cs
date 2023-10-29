using Data.Models;

namespace Sevices.Core.OrderDetailService
{
    public interface IOrderDetailService
    {
        ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize);
        ResultModel Update(UpdateOrderDetailModel model);
    }
}

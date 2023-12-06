using Data.Models;

namespace Sevices.Core.OrderDetailMaterialService
{
    public interface IOrderDetailMaterialService
    {
        ResultModel GetByOrderDetailIdWidthPaging(Guid orderDetailId, string? search, int pageIndex, int pageSize);       
    }
}

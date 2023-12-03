using Data.Enums;
using Data.Models;

namespace Sevices.Core.OrderService
{
    public interface IOrderService
    {
        ResultModel GetAllWithPaging(int pageIndex, int pageSize, string? search = null);
        ResultModel GetAllWithSearchAndPaging(int pageIndex, int pageSize, string? search = null);
        ResultModel GetByForemanId(Guid foremanId, int pageIndex, int pageSize, string? search = null);
        ResultModel GetQuotesByUserWithPaging(Guid userId, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetQuoteMaterialById(Guid id);
        Task<ResultModel> Create(CreateOrderModel model, Guid createdById);
        ResultModel GetAllLogOnOrder(string? search, int pageIndex, int pageSize);
        ResultModel Update(UpdateOrderModel model, Guid userId);
        ResultModel UpdateStatus(Guid id, OrderStatus status, Guid userId);
        Task<FileResultModel> ExportQuoteToPDF(Guid id);
        ResultModel ReCalculatePrice(Guid id);
        ResultModel SyncItem(Guid id, Guid userId);
        ResultModel SyncItemV1(Guid id);
    }
}

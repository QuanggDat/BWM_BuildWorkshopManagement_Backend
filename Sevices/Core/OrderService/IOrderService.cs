using Data.Enums;
using Data.Models;

namespace Sevices.Core.OrderService
{
    public interface IOrderService
    {
        ResultModel GetAllWithPaging(int pageIndex, int pageSize);
        ResultModel GetQuotesByUserWithPaging(Guid userId, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetQuoteMaterialById(Guid id);
        Task<ResultModel> Create(CreateOrderModel model, Guid createdById);
        ResultModel UpdateStatus(Guid id, OrderStatus status, Guid userId);
        Task<FileResultModel> ExportQuoteToPDF(Guid id);
        ResultModel ReCalculatePrice(Guid id);
    }
}

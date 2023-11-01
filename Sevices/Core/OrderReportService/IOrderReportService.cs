using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.OrderReportModel;

namespace Sevices.Core.OrderReportService
{
    public interface IOrderReportService
    {
        Task<ResultModel> CreateOrderReport(Guid reporterId, CreateOrderReportModel model);
        Task<OrderReportModel?> GetOrderReportById(Guid reportId);
        Task<ResultModel> ReviewsOrderReport(ReviewsOrderReportModel model);
        Task<List<OrderReportModel>> GetOrderReportsByForemanId(Guid foremanId);    
    }
}

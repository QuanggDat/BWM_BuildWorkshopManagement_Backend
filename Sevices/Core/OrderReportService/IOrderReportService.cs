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
        ResultModel Create(Guid id, CreateOrderReportModel model);   
        ResultModel GetById(Guid id);
        ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize);
        ResultModel GetByForemanId (Guid foremanId, string? search, int pageIndex, int pageSize);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);

    }
}

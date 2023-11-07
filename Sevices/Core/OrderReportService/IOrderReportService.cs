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
        ResultModel GetByOrderId(Guid orderId);
        ResultModel GetByForemanId (Guid foremanId);
        ResultModel GetAll();
        //ResultModel SendReviews(ReviewsOrderReportModel model);
    }
}

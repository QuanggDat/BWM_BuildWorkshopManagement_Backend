using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.OrderDetailService
{
    public interface IOrderDetailService
    {
        ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize);
        ResultModel Update(UpdateOrderDetailModel model);
    }
}

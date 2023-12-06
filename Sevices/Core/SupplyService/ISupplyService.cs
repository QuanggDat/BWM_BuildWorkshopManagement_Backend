using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.SupplyService
{
    public interface ISupplyService
    {
        ResultModel GetByOrderId(Guid orderId);
    }
}

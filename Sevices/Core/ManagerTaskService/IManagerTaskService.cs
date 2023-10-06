using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ManagerTaskService
{
    public interface IManagerTaskService
    {
        Task<ResultModel> CreatedManagerTask(CreateManagerTaskModel model);
        Task<List<ResponseManagerTaskModel>> GetManagerTaskByOrderId(Guid orderId);
    }
}

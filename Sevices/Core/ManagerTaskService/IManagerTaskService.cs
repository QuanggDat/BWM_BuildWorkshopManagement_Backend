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
        Task<ResultModel> CreatedManagerTask(Guid createById, CreateManagerTaskModel model);
        Task<List<ResponseManagerTaskModel>> GetManagerTaskByOrderId(Guid orderId);
        Task<List<ResponseManagerTaskModel>> GetManagerTaskByManagerId(Guid managerId);
        Task<bool> UpdateManagerTaskStatus(Guid taskManagerId, TaskStatus status);
        Task<ResultModel> UpdateManagerTask(UpdateManagerTaskModel model);
        Task<int> DeleteManagerTask(Guid taskManagerId);
    }
}

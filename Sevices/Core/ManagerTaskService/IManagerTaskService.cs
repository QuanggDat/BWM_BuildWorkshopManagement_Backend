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
        Task<List<ManagerTaskModel>> GetManagerTaskByOrderId(Guid orderId);
        Task<List<ManagerTaskModel>> GetManagerTaskByManagerId(Guid managerId);
        Task<List<ManagerTaskModel>> GetManagerTaskByFactory(Guid managerId);
        Task<ResultModel> UpdateManagerTaskStatus(Guid taskManagerId, TaskStatus status);
        Task<ResultModel> UpdateManagerTask(UpdateManagerTaskModel model);
        Task<ResultModel> DeleteManagerTask(Guid taskManagerId);
        Task<ResultModel> AssignManagerTask(Guid taskManagerId, Guid groupId);

    }
}

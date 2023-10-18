using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.WokerTaskModel;

namespace Sevices.Core.WokerTaskService
{
    public interface IWokerTaskService
    {
        Task<ResultModel> CreateWokerTask(Guid userId, CreateWokerTaskModel model);
        Task<ResultModel> UpdateWokerTask(UpdateWokerTaskModel model);
        Task<ResultModel> DeleteWokerTask(Guid wokerTaskId);
        Task<ResultModel> AssignWokerTask(AssignWokerTaskModel model);
        Task<ResultModel> UnAssignWokerTask(AssignWokerTaskModel model);
        Task <List<WokerTaskResponseModel>> GetAllWokerTask(Guid managerTaskId);
        Task<ResultModel> UpdateWokerTaskStatus(Guid wokerTaskId, TaskStatus status);
    }
}

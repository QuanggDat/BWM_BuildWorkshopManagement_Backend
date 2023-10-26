using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.WorkerTaskModel;

namespace Sevices.Core.WorkerTaskService
{
    public interface IWorkerTaskService
    {
        Task<ResultModel> CreateWorkerTask(Guid userId, CreateWorkerTaskModel model);
        Task<ResultModel> UpdateWorkerTask(UpdateWorkerTaskModel model);
        Task<ResultModel> DeleteWorkerTask(Guid workerTaskId);
        Task<ResultModel> AssignWorkerTask(AssignWorkerTaskModel model);
        Task<ResultModel> UnAssignWorkerTask(AssignWorkerTaskModel model);
        Task <List<WorkerTaskResponseModel>> GetAllWorkerTask(Guid managerTaskId);
        Task<ResultModel> UpdateWorkerTaskStatus(Guid workerTaskId, TaskStatus status);
    }
}

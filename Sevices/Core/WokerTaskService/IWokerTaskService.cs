using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.WokerTaskService
{
    public interface IWokerTaskService
    {
        Task<ResultModel> CreateTask(Guid userId, CreateTaskRequest request);
        Task<ResultModel> UpdateTask(UpdateTaskRequest request);
        Task<ResultModel> DeleteTask(Guid taskId);
        Task<ResultModel> AssignTask(AssignTaskRequest request);
        Task<ResultModel> UnAssignTask(AssignTaskRequest request);
        Task<ResultModel<TaskResponse>> GetAllTask(Guid projectId);
        Task<bool> UpdateTaskStatus(Guid taskId, ProjectTaskStatus status);
    }
}

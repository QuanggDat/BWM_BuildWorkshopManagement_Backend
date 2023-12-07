using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.WorkerTaskService
{
    public interface IWorkerTaskService
    {
        ResultModel Create(Guid userId, CreateWorkerTaskModel model);
        ResultModel Update(UpdateWorkerTaskModel model);
        ResultModel Delete(Guid workerTaskId);
        ResultModel Assign(AssignWorkerTaskModel model);
        ResultModel UnAssign(AssignWorkerTaskModel model);
        ResultModel GetByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize);
        ResultModel GetByLeaderTaskIdAndUserId(Guid leaderTaskId, Guid userId, string? search, int pageIndex, int pageSize);
        ResultModel GetByUserId(Guid userId, string? search, int pageIndex, int pageSize);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);    
        ResultModel UpdateStatus(Guid workerTaskId, EWorkerTaskStatus status);
        ResultModel SendFeedback(SendFeedbackModel model);
    }
}

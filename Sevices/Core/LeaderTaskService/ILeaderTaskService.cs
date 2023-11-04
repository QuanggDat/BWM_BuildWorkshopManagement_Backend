using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.LeaderTaskService
{
    public interface ILeaderTaskService
    {
        Task<ResultModel> CreatedLeaderTask(Guid createById, CreateLeaderTaskModel model);
        Task<List<LeaderTaskModel>> GetLeaderTaskByOrderId(Guid orderId);
        Task<List<LeaderTaskModel>> GetLeaderTaskByLeaderId(Guid leaderId);
        Task<List<LeaderTaskModel>> GetLeaderTaskByForemanId(Guid foremanId);
        Task<ResultModel> UpdateLeaderTaskStatus(Guid leaderTaskId, TaskStatus status);
        Task<ResultModel> UpdateLeaderTask(UpdateLeaderTaskModel model);
        Task<ResultModel> DeleteLeaderTask(Guid leaderTaskId);
        //Task<ResultModel> AssignLeaderTask(Guid leaderTaskId, Guid teamId);
    }
}

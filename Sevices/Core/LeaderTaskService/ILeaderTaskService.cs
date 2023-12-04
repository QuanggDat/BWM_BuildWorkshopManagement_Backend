using Data.Enums;
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
        ResultModel Create(Guid createById, CreateLeaderTaskModel model);
        ResultModel CreateAcceptanceTask(Guid createById, CreateAcceptanceTaskModel model);
        ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize);
        ResultModel GetByLeaderId(Guid leaderId, string? search, int pageIndex, int pageSize);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetMaterialByLeaderTaskId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel GetByOrderIdAndItemId(Guid orderId, Guid itemId, string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel UpdateStatus(Guid id, ETaskStatus status);
        ResultModel Update(UpdateLeaderTaskModel model);
        ResultModel Delete(Guid leaderTaskId);              
    }
}

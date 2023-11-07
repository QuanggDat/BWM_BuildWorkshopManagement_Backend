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
        ResultModel Created(Guid createById, CreateLeaderTaskModel model);
        ResultModel GetByOrderId(Guid orderId);
        ResultModel GetByLeaderId(Guid leadeId);
        ResultModel GetByOrderIdAndLeaderId(Guid orderId, Guid leaderId);
        ResultModel GetById(Guid id);
        ResultModel UpdateStatus(Guid id, ETaskStatus status);
        ResultModel Update(UpdateLeaderTaskModel model);
        ResultModel Delete(Guid leaderTaskId);              
    }
}

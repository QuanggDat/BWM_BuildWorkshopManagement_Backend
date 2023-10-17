using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.HumanResourceService
{
    public interface IGroupService
    {
        Task<ResultModel> CreateGroup(CreateGroupModel model);
        ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize);
        ResultModel GetAllUserByGroupId(Guid id);
        ResultModel AddWorkerToGroup(WorkerToGroup model);
        ResultModel RemoveWorkerFromGroup(WorkerToGroup model);
        ResultModel UpdateGroup(UpdateGroupModel model);
        ResultModel DeleteGroup(Guid id);
    }
}

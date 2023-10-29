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
        ResultModel CreateGroup(CreateGroupModel model);
        ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize);
        ResultModel GetAllUserByGroupId(Guid id);
        ResultModel AddWorkerToGroup(AddWorkerToGroupModel model);
        ResultModel RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model);
        ResultModel UpdateGroup(UpdateGroupModel model);
        ResultModel DeleteGroup(Guid id);
    }
}

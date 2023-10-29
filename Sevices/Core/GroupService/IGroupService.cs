using Data.Models;

namespace Sevices.Core.GroupService
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

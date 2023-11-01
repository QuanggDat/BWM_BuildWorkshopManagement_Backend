using Data.Models;

namespace Sevices.Core.GroupService
{
    public interface IGroupService
    {
        ResultModel CreateGroup(CreateGroupModel model);
        ResultModel GetAllGroup(string? search, int pageIndex, int pageSize);
        ResultModel GetAllUserByGroupId(Guid id);
        ResultModel GetAllUserNotInGroupId(Guid id);
        ResultModel AddLeaderToGroup(AddWorkerToGroupModel model);
        ResultModel AddWorkerToGroup(AddWorkerToGroupModel model);
        ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model);
        ResultModel UpdateGroup(UpdateGroupModel model);
        ResultModel DeleteGroup(Guid id);
    }
}

using Data.Models;

namespace Sevices.Core.GroupService
{
    public interface IGroupService
    {
        ResultModel Create(CreateGroupModel model);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetAllUserByGroupId(Guid id);
        ResultModel GetAllUserNotInGroupId(Guid id);
        ResultModel AddLeaderToGroup(AddWorkerToGroupModel model);
        ResultModel AddWorkerToGroup(AddWorkerToGroupModel model);
        ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model);
        ResultModel Update(UpdateGroupModel model);
        ResultModel Delete(Guid id);
    }
}

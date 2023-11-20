using Data.Models;

namespace Sevices.Core.GroupService
{
    public interface IGroupService
    {
        ResultModel Create(CreateGroupModel model);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetAllUserByGroupId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel GetAllUserNotInGroupId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model);
        ResultModel ChangeLeader(ChangeLeaderModel model);
        ResultModel AddWorkersToGroup(AddWorkersToGroupModel model);
        ResultModel Update(UpdateGroupModel model);
        ResultModel Delete(Guid id);
    }
}

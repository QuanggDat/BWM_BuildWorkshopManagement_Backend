using Data.Models;

namespace Sevices.Core.GroupService
{
    public interface IGroupService
    {
        ResultModel Create(CreateGroupModel model, Guid userId);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetAllLogOnGroup(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetAllUsersByGroupId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel GetWorkersByGroupId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel GetWorkersNotAtWorkByGroupId (Guid id, string? search);
        ResultModel GetWorkerAndTaskOfWorkerByGroupId(Guid id, string? search, int pageIndex, int pageSize);
        ResultModel GetAllUsersNotInGroupId(Guid id, string? search);
        ResultModel GetAllWorkerNotYetGroup(string? search);
        ResultModel GetAllLeaderHaveGroup(string? search);
        ResultModel GetAllLeaderNoHaveGroup(string? search);
        ResultModel RemoveUserFromGroup(RemoveWorkerFromGroupModel model, Guid userId);
        ResultModel ChangeLeader(ChangeLeaderModel model, Guid userId);
        ResultModel AddWorkersToGroup(AddWorkersToGroupModel model, Guid userId);
        ResultModel Update(UpdateGroupModel model, Guid userId);
        ResultModel Delete(Guid id, Guid userId);
    }
}

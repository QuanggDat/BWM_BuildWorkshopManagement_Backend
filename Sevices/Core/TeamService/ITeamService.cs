using Data.Models;

namespace Sevices.Core.TeamService
{
    public interface ITeamService
    {
        ResultModel CreateTeam(CreateTeamModel model);
        ResultModel GetTeamByGroupId(Guid groupId, string? search, int pageIndex, int pageSize);
        ResultModel GetAllUserByTeamId(Guid id);
        ResultModel AddWorkerToTeam(AddWorkerToTeamModel model);
        ResultModel RemoveWorkerFromTeam(RemoveWorkerFromTeamModel model);
        ResultModel UpdateTeam(UpdateTeamModel model);
        ResultModel DeleteTeam(Guid id);
    }
}

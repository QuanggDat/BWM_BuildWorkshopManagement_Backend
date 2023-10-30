using Data.Models;

namespace Sevices.Core.SquadService
{
    public interface ISquadService
    {
        ResultModel CreateSquad(CreateSquadModel model);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetAllUserBySquadId(Guid id);
        ResultModel GetAllUserNotInSquadId(Guid id);
        ResultModel AddManagerToSquad(AddWorkerToSquadModel model);
        ResultModel AddWorkerToSquad(AddWorkerToSquadModel model);
        ResultModel RemoveUserFromSquad(RemoveWorkerFromSquadModel model);
        ResultModel UpdateSquad(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

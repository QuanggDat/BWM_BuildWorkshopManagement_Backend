using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.HumanResourceService
{
    public interface ISquadService
    {
        Task<ResultModel> CreateSquad(CreateSquadModel model);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetAllUserBySquadId(Guid id);
        ResultModel AddManagerToSquad(WorkerToSquad model);
        ResultModel AddWorkerToSquad(WorkerToSquad model);
        ResultModel RemoveWorkerFromSquad(WorkerToSquad model);
        Task<ResultModel> UpdateSquad(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

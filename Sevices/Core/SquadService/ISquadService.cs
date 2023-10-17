using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.SquadService
{
    public interface ISquadService
    {
        Task<ResultModel> CreateSquad(CreateSquadModel model);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetAllUserBySquadId(Guid id);
        ResultModel AddManagerToSquad(AddWorkerToSquadModel model);
        ResultModel AddWorkerToSquad(AddWorkerToSquadModel model);
        ResultModel RemoveWorkerFromSquad(RemoveWorkerFromSquadModel model);
        Task<ResultModel> UpdateSquad(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

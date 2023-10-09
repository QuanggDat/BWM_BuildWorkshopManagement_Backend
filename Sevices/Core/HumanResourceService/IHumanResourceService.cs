using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.HumanResourceService
{
    public interface IHumanResourceService
    {
        Task<ResultModel> CreateSquad(CreateSquadModel model);
        Task<ResultModel> AddGroup(AddGroupModel model);
        Task<ResultModel> AddWorkerToGroup(Guid id);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetSquadById(Guid id);
        ResultModel GetGroupById(Guid id);
        Task<ResultModel> UpdateSquadAsync(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

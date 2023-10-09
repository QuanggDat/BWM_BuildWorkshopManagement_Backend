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
        ResultModel AddWorkerToGroup(AddWorkerToGroup model);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetSquadById(Guid id);
        ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize);
        ResultModel GetUserByGroupId(Guid id, int pageIndex, int pageSize);
        Task<ResultModel> UpdateSquad(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

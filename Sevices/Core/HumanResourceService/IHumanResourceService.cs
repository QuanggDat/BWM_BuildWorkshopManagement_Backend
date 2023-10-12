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
        Task<ResultModel> CreateGroup(CreateGroupModel model);
        ResultModel AddWorkerToSquad(AddWorkerToSquad model);
        ResultModel AddWorkerToGroup(AddWorkerToGroup model);
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        //ResultModel GetAllUserByGroupId(Guid id);
        //ResultModel GetAllUserBySquadId(Guid id);
        ResultModel GetSquadById(Guid id);
        Task<List<HumanResources>> GetAllUser();
        ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize);
        ResultModel GetUserByGroupId(Guid id, int pageIndex, int pageSize);
        Task<ResultModel> UpdateSquad(UpdateSquadModel model);
        Task<ResultModel> UpdateGroup(UpdateGroupModel model);
    }
}

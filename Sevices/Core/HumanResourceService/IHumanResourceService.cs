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
        ResultModel GetAllSquad(int pageIndex, int pageSize);
        ResultModel GetSquadById(Guid id);
        ResultModel AddWorkerToSquad(AddWorkerToSquad model);
        Task<ResultModel> UpdateSquad(UpdateSquadModel model);

        Task<List<HumanResources>> GetAllUser();

        Task<ResultModel> CreateGroup(CreateGroupModel model);
        ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize);
        ResultModel AddWorkerToGroup(AddWorkerToGroup model);
        Task<ResultModel> UpdateGroup(UpdateGroupModel model);
        ResultModel GetUserByGroupId(Guid id, int pageIndex, int pageSize);

        //ResultModel GetAllUserByGroupId(Guid id);
        //ResultModel GetAllUserBySquadId(Guid id);
        
    }
}

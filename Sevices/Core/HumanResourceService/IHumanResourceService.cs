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
        ResultModel GetAllSquad();
        ResultModel GetSquadById(Guid id);
        Task<ResultModel> UpdateSquadAsync(UpdateSquadModel model);
        ResultModel DeleteSquad(Guid id);
    }
}

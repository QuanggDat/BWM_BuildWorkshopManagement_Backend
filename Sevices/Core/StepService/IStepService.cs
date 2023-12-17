using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.StepService
{
    public interface IStepService
    {
        ResultModel Create(CreateStepModel model);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetAllWithoutPaging();
        ResultModel GetById(Guid id);
        ResultModel Update(UpdateStepModel model);
        ResultModel Delete(Guid id);
    }
}

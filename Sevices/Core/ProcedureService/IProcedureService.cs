using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ProcedureService
{
    public interface IProcedureService
    {
        ResultModel Create(CreateProcedureModel model);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetAllWithoutPaging();
        ResultModel GetAllWithoutPagingAndStep();
        ResultModel GetById(Guid id);
        ResultModel Update(UpdateProcedureModel model);
        ResultModel Delete(Guid id);
    }
}

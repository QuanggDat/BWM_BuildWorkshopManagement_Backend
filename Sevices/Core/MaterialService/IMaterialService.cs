using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.MaterialService
{
    public interface IMaterialService
    {
        ResultModel Create(CreateMaterialModel model);
        ResultModel Update(UpdateMaterialModel model);
        ResultModel Delete(Guid id);
        ResultModel GetById(Guid id);
        ResultModel GetAll(string? search,int pageIndex, int pageSize);
        ResultModel GetByMaterialCategoryId(Guid materialCategoryId, string? search, int pageIndex, int pageSize);
        ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize);
    }
}

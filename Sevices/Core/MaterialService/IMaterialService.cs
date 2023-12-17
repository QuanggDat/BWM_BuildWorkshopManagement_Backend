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
        ResultModel Create(CreateMaterialModel model, Guid userId);
        ResultModel Update(UpdateMaterialModel model, Guid userId);
        ResultModel Delete(Guid id, Guid userId);
        ResultModel GetById(Guid id);
        ResultModel GetAll(string? search,int pageIndex, int pageSize);
        ResultModel GetAllWithoutPaging();
        ResultModel GetByMaterialCategoryId(Guid materialCategoryId, string? search, int pageIndex, int pageSize);
        ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize);
        ResultModel GetAllLogOnMaterial(string? search, int pageIndex, int pageSize);
    }
}

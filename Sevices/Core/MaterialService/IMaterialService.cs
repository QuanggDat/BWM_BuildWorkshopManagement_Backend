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
        ResultModel CreateMaterial(Guid createdById,CreateMaterialModel model);
        ResultModel UpdateMaterial(UpdateMaterialModel model);
        ResultModel DeleteMaterial(Guid id);
        ResultModel GetMaterialById(Guid id);
        ResultModel UpdateMaterialAmount(UpdateMaterialAmountModel model);
        ResultModel GetAllMaterial(string? search,int pageIndex, int pageSize);
        ResultModel GetMaterialByMaterialCategoryId(Guid id, int pageIndex, int pageSize);
        
    }
}

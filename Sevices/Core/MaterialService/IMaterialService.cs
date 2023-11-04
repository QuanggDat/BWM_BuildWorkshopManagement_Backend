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
        ResultModel CreateMaterial(CreateMaterialModel model);
        ResultModel UpdateMaterial(UpdateMaterialModel model);
        ResultModel DeleteMaterial(Guid id);
        ResultModel GetMaterialById(Guid id);
        ResultModel GetAllMaterial(string? search,int pageIndex, int pageSize);
        ResultModel GetMaterialByMaterialCategoryId(Guid materialCategoryId, int pageIndex, int pageSize);
        
    }
}

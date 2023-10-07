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
        Task<ResultModel> CreateCategory(CreateMaterialCategoryModel model);
        Task<ResultModel> CreateMaterial(CreateMaterialModel model);
        ResultModel GetAllCategory();
        ResultModel GetAllMaterial();
        ResultModel GetMaterialById(Guid id);
        ResultModel GetCategoryById(Guid id);
        ResultModel UpdateMaterialCategory(UpdateMaterialCategoryModel model);
        ResultModel UpdateMaterial(UpdateMaterialModel model);
        ResultModel UpdateMaterialAmount(UpdateMaterialAmountModel model);
        ResultModel DeleteMaterial(Guid id);
        ResultModel DeleteCategory(Guid id);
    }
}

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
        Task<ResultModel> CreateMaterial(Guid id, CreateMaterialModel model);
        ResultModel Search(string search, int pageIndex, int pageSize);
        ResultModel GetAllMaterial(int pageIndex, int pageSize);
        ResultModel SortMaterialbyThickness(int pageIndex, int pageSize);
        ResultModel SortMaterialbyPrice(int pageIndex, int pageSize);
        ResultModel GetAllMaterialByCategoryId(Guid id, int pageIndex, int pageSize);
        ResultModel GetMaterialById(Guid id);
        ResultModel UpdateMaterial(Guid id, UpdateMaterialModel model);
        ResultModel UpdateMaterialAmount(Guid id, UpdateMaterialAmountModel model);
        ResultModel DeleteMaterial(Guid id);
    }
}

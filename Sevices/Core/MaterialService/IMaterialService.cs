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
        Task<ResultModel> CreateMaterial(CreateMaterialModel model);
        ResultModel Search(string search, int pageIndex, int pageSize);
        ResultModel GetAllMaterial(int pageIndex, int pageSize);
        ResultModel SortMaterialbyThickness(int pageIndex, int pageSize);
        ResultModel SortMaterialbyPrice(int pageIndex, int pageSize);
        ResultModel GetMaterialById(Guid id);
        ResultModel UpdateMaterial(UpdateMaterialModel model);
        ResultModel UpdateMaterialAmount(UpdateMaterialAmountModel model);
        ResultModel DeleteMaterial(Guid id);
    }
}

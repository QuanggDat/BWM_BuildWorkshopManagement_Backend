using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.CategoryService
{
    public interface ICategoryService
    {
        //Task<ResultModel> CreateItemCategory(CreateItemCategoryModel model);
        //ResultModel GetAllItemCategory(int pageIndex, int pageSize);
        //ResultModel GetItemCategoryById(Guid id);
        //ResultModel UpdateItemCategory(UpdateItemCategoryModel model);
        //ResultModel DeleteItemCategory(Guid id);
        Task<ResultModel> CreateMaterialCategory(CreateMaterialCategoryModel model);
        ResultModel GetAllMaterialCategory(int pageIndex, int pageSize);
        ResultModel GetMaterialCategoryById(Guid id);
        ResultModel UpdateMaterialCategory(UpdateMaterialCategoryModel model);
        ResultModel DeleteMaterialCategory(Guid id);
    }
}

using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ItemCategoryService
{
    public interface IItemCategoryService
    {
        ResultModel CreateItemCategory(CreateItemCategoryModel model);
        ResultModel GetAllItemCategory(string? search, int pageIndex, int pageSize);
        ResultModel GetItemCategoryById(Guid id);
        ResultModel UpdateItemCategory(UpdateItemCategoryModel model);
        ResultModel DeleteItemCategory(Guid id);
    }
}

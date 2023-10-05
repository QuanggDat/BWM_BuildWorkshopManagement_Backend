using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ItemService
{
    public interface IItemService
    {
        Task<ResultModel> CreateCategory(CreateItemCategoryModel model);
        Task<ResultModel> CreateItem(CreateItemModel model);
        ResultModel GetAllCategory();
        ResultModel GetAllItem();
        ResultModel GetItemById(Guid id);
        ResultModel GetCategoryById(Guid id);
        ResultModel UpdateItemCategory(UpdateItemCategoryModel model);
        ResultModel UpdateItem(UpdateItemModel model);
        ResultModel DeleteItem(Guid id);
        ResultModel DeleteCategory(Guid id);
    }
}

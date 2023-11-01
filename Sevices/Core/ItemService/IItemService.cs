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
        Task<ResultModel> CreateItem(CreateItemModel model);
        Task<ResultModel> UpdateItem(UpdateItemModel model);
        Task<ResultModel> DeleteItem(Guid id);
        Task<ResultModel> GetAllItem(string? search, int pageIndex, int pageSize);
        Task<ResultModel> GetItemById(Guid id);
        Task<ResultModel> GetItemByItemCategoryId(Guid itemCategoryId, int pageIndex, int pageSize);
    }
}

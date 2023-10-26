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
        Task<ResultModel> CreateItem(Guid id, CreateItemModel model);
        Task<ResultModel> AddMaterialToItem(Guid id, Guid itemId, AddMaterialToItemModel model);
        ResultModel UpdateMaterialToItem(Guid id, Guid userId, UpdateMaterialToItemModel model);
        ResultModel Search(string search, int pageIndex, int pageSize);
        ResultModel GetAllItem(int pageIndex, int pageSize);
        ResultModel SortItembyPrice(int pageIndex, int pageSize);
        ResultModel GetItemById(Guid id);
        ResultModel UpdateItem(Guid id, Guid userId, UpdateItemModel model);
        ResultModel DeleteItem(Guid id);
    }
}

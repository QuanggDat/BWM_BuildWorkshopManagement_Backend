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
        ResultModel Search(string search, int pageIndex, int pageSize);
        ResultModel GetAllItem(int pageIndex, int pageSize);
        ResultModel SortItembyPrice(int pageIndex, int pageSize);
        ResultModel GetItemById(Guid id);
        ResultModel UpdateItem(UpdateItemModel model);
        ResultModel DeleteItem(Guid id);
    }
}

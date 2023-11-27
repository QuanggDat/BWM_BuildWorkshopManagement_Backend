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
        ResultModel Create(CreateItemModel model);
        ResultModel Update(UpdateItemModel model);
        ResultModel DuplicateItem(Guid id, int num);
        ResultModel Delete(Guid id);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetByItemCategoryId (Guid itemCategoryId, string? search, int pageIndex, int pageSize);
    }
}

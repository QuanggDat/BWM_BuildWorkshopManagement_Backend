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
        ResultModel Create(CreateItemModel model, Guid userId);
        ResultModel Update(UpdateItemModel model, Guid userId);
        ResultModel DuplicateItem(Guid id, int number);
        ResultModel Delete(Guid id, Guid userId);
        ResultModel GetAllWithSearchAndPaging(string? search, int pageIndex, int pageSize);
        ResultModel GetAll();
        ResultModel GetAllLogOnItem(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel GetByItemCategoryId (Guid itemCategoryId, string? search, int pageIndex, int pageSize);
    }
}

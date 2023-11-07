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
        ResultModel Create(CreateItemCategoryModel model);
        ResultModel GetAll(string? search, int pageIndex, int pageSize);
        ResultModel GetById(Guid id);
        ResultModel Update(UpdateItemCategoryModel model);
        ResultModel Delete(Guid id);
    }
}

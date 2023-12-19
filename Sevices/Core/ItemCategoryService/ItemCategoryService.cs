using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ItemCategoryService
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ItemCategoryService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public ResultModel Create(CreateItemCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;

            try
            {
                var checkExists = _dbContext.ItemCategory.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);

                if (checkExists != null)
                {
                    result.Code = 30;
                    result.Succeed = false;
                    result.ErrorMessage = "Tên loại sản phẩm đã tồn tại !";
                }
                else
                {
                    var newCategory = new ItemCategory
                    {
                        name = model.name,
                        isDeleted = false
                    };

                    _dbContext.ItemCategory.Add(newCategory);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = newCategory.id;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateItemCategoryModel model)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.ItemCategory.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 31;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm !";
                }
                else
                {
                    var checkExists = _dbContext.ItemCategory.FirstOrDefault(x => x.name == model.name && x.name != check.name && !x.isDeleted);

                    if (checkExists != null)
                    {
                        result.Code = 30;
                        result.Succeed = false;
                        result.ErrorMessage = "Tên loại sản phẩm đã tồn tại !";
                    }
                    else
                    {
                        check.name = model.name;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = check.id;
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listItemCategory = _dbContext.ItemCategory.Include(x => x.Items)
                    .Where(x => x.isDeleted != true).OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    search = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listItemCategory = listItemCategory.Where(x => FnUtil.Remove_VN_Accents(x.name).ToUpper().Contains(search)).ToList();
                }

                var listItemCategoryPaging = listItemCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ItemCategoryModel>();
                foreach (var item in listItemCategoryPaging)
                {

                    var tmp = new ItemCategoryModel
                    {
                        id = item.id,
                        name = item.name,
                        quantityItem = item.Items.Count,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItemCategory.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = _dbContext.ItemCategory.Include(x => x.Items)
                    .Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 31;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm!";
                }
                else
                {

                    var itemCategoryModel = new ItemCategoryModel
                    {
                        id = check.id,
                        name = check.name,
                        quantityItem = check.Items.Count,
                    };

                    result.Data = itemCategoryModel;
                    result.Succeed = true;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();

            try
            {
                var isExistedItem = _dbContext.Item.Any(x => x.itemCategoryId == id && x.isDeleted != true);
                if (isExistedItem)
                {
                    result.Code = 32;
                    result.ErrorMessage = "Hãy xoá hết sản phẩm trước khi xoá loại sản phẩm !";
                }
                else
                {
                    var check = _dbContext.ItemCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                    if (check == null)
                    {
                        result.Code = 31;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm!";
                    }
                    else
                    {
                        check.isDeleted = true;
                        _dbContext.SaveChanges();
                        result.Data = check.id;
                        result.Succeed = true;
                    }
                }               
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
    }
}

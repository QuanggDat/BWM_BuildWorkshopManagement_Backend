using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
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

        public ResultModel CreateItemCategory(CreateItemCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrWhiteSpace(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống !";
                }
                else
                {
                    var checkExists = _dbContext.ItemCategory.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                    if (checkExists != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên ItemCategory này đã tồn tại !";
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
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateItemCategory(UpdateItemCategoryModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.ItemCategory.Where(x => x.id == model.id && x.isDeleted != true).SingleOrDefault();
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin ItemCategory !";
                }
                else
                {
                    //Validation
                    if (string.IsNullOrEmpty(model.name))
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên ItemCategory không được để trống !";
                    }
                    else
                    {
                        if (model.name != check.name)
                        {
                            var checkExists = _dbContext.ItemCategory.FirstOrDefault(x => x.name == model.name && !x.isDeleted);
                            if (checkExists != null)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Tên này đã tồn tại !";
                            }
                            else
                            {
                                check.name = model.name;
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = "Cập nhập thành công " + check.id;
                            }
                        }
                        else
                        {
                            check.name = model.name;
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = "Cập nhập thành công " + check.id;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllItemCategory(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listItemCategory = _dbContext.ItemCategory.Where(x => x.isDeleted != true)
                   .OrderByDescending(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listItemCategory = listItemCategory.Where(x => x.name.Contains(search)).ToList();
                }

                var listItemCategoryPaging = listItemCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ItemCategoryModel>();
                foreach (var item in listItemCategoryPaging)
                {

                    var tmp = new ItemCategoryModel
                    {
                        id = item.id,
                        name = item.name,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItemCategoryPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetItemCategoryById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var check = _dbContext.ItemCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin ItemCategory!";
                }
                else
                {

                    var itemCategoryModel = new ItemCategoryModel
                    {
                        id = check.id,
                        name = check.name,
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

        public ResultModel DeleteItemCategory(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var isExistedItem = _dbContext.Item.Any(x => x.itemCategoryId == id && x.isDeleted != true);
                if (isExistedItem)
                {
                    result.ErrorMessage = "Hãy xoá hết mặt hàng trước khi xoá loại mặt hàng ! ";
                }
                var check = _dbContext.ItemCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                }
                else
                {
                    check.isDeleted = true;
                    _dbContext.SaveChanges();
                    result.Data = "Xoá thành công " + check.id;
                    result.Succeed = true;
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

using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Sevices.Core.ItemService
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ItemService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ResultModel> CreateCategory(CreateItemCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var newCategory = new ItemCategory
                {
                    name = model.name,
                    isDeleted = false
                };
                _dbContext.ItemCategory.Add(newCategory);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = newCategory.categoryId;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> CreateItem(CreateItemModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var newItem = new Item
                {
                    name = model.name,
                    image=model.image,
                    mass= model.mass,
                    length=model.length,
                    width=model.width,
                    height=model.height,
                    technical=model.technical,
                    twoD=model.twoD,
                    threeD=model.threeD,
                    description=model.description,
                    price=model.price,
                    areaId=model.areaId,
                    categoryId=model.categoryId,
                    isDeleted = false
                };
                _dbContext.Item.Add(newItem);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = newItem.id;
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
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == model.id).FirstOrDefault();
                if (data != null)
                {
                    data.name = model.name;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<ItemCategory, ItemCategoryModel>(data);
                }
                else
                {
                    result.ErrorMessage = "ItemCategory" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel UpdateItem(UpdateItemModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.id == model.id).FirstOrDefault();
                if (data != null)
                {
                    data.name = model.name;
                    data.image = model.image;
                    data.mass = model.mass;
                    data.length = model.length;
                    data.width = model.width;
                    data.height = model.height;
                    data.technical = model.technical;
                    data.twoD = model.twoD;
                    data.threeD = model.threeD;
                    data.description = model.description;
                    data.price = model.price;
                    data.areaId = model.areaId;
                    data.categoryId = model.categoryId;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Item, ItemModel>(data);
                }
                else
                {
                    result.ErrorMessage = "Item" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllCategory()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.ItemCategory.Where(i=> i.isDeleted != true);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<ItemCategoryModel>(data);
                    result.Data = view!;
                    result.Succeed = true;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllItem()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.isDeleted != true);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<ItemModel>(data);
                    result.Data = view!;
                    result.Succeed = true;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetItemById(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.id == id && i.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<ItemModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Item" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel GetCategoryById(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == id && i.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<ItemCategoryModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "ItemCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel DeleteItem(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<Item, ItemModel>(data);
                    resultModel.Data = view;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Item" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel DeleteCategory(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<ItemCategory, ItemCategoryModel>(data);
                    resultModel.Data = view;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "ItemCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }
    }
}

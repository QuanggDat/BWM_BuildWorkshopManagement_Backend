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
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == model.categoryId).FirstOrDefault();
                if (data != null)
                {
                    data.name = model.name;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.ItemCategory, ItemCategoryModel>(data);
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
                    data.image = model.image;
                    data.name = model.name;
                    data.mass = model.mass;
                    data.length = model.lenghth;
                    data.width = model.width;
                    data.height = model.heighth;
                    data.unit = model.unit;
                    data.description = model.description;
                    data.quantity = model.quantity;
                    data.price = model.price;
                    data.categoryId = model.categoryId;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.Item, ItemModel>(data);
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
                var data = _dbContext.ItemCategory;
                var view = _mapper.ProjectTo<ItemCategoryModel>(data);
                result.Data = view;
                result.Succeed = true;

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
                var data = _dbContext.Item;
                var view = _mapper.ProjectTo<ItemModel>(data);
                result.Data = view;
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetItemById(int id)
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

        public ResultModel GetCategoryById(int id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == id && i.IsDeleted != true);
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

        public ResultModel DeleteItem(int id)
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

        public ResultModel DeleteCategory(int id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.ItemCategory.Where(i => i.categoryId == id).FirstOrDefault();
                if (data != null)
                {
                    data.IsDeleted = true;
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

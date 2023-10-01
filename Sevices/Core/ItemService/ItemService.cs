using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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
                var data = _dbContext.ItemCategory.Where(i => i.id == model.id).FirstOrDefault();
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
                    data.itemCategoryId = model.itemCategoryId;
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
                var data = _dbContext.ItemCategory.Where(i => i.id == id && i.isDeleted != true);
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
                var data = _dbContext.ItemCategory.Where(i => i.id == id).FirstOrDefault();
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

        // DEMO
        public ResultModel GetItemsPaging(int pageIndex, int pageSize)
        {
            var resultModel = new ResultModel();
            try
            {
                var listData = _dbContext.Item.Where(x => !x.isDeleted).ToList(); // Luon luon .toList() neu muon lay ra 1 cai list, chu yeu de no convert ve dang List<object> de tuong tac du lieu de hon
                                                                                  // .OrderBy(x => x.height) // sap xep theo asc
                                                                                  // .OrderByDescending(x => x.length) // sap xep theo desc
                                                                                  // trong linq con rat nhieu ham ho tro de lam nhieu viec khac, chiu kho xem them.
                var listDataPaging = listData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                //.Skip((pageIndex - 1) * pageSize).Take(pageSize) la paging (pageIndex la vi tri trang (trang 1, trnag 2,...), pageSize la so luong row trong 1 trang
                
                resultModel.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<ItemModel>>(listDataPaging),
                    Total = listData.Count
                };
                resultModel.Succeed = true;

            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }
    }
}

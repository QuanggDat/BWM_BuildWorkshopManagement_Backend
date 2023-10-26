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
using System.Drawing.Printing;

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

        public ResultModel Search(string search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.isDeleted != true && i.name.Contains(search)).OrderByDescending(i => i.name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<ItemModel>>(data),
                    Total = data.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public async Task<ResultModel> CreateItem(Guid id, CreateItemModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrEmpty(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống.";
                    return result;
                }
                else
                {
                    if (model.mass < 0)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Diện tích không được âm.";
                        return result;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.unit))
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Đơn vị này không được để trống.";
                            return result;
                        }
                        else
                        {
                            if (model.length < 0)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Chiều dài không được âm.";
                                return result;
                            }
                            else
                            {
                                if (model.height < 0)
                                {
                                    result.Succeed = false;
                                    result.ErrorMessage = "Chiều cao không được âm.";
                                    return result;
                                }
                                else
                                {
                                    if (model.depth < 0)
                                    {
                                        result.Succeed = false;
                                        result.ErrorMessage = "Chiều rộng không được âm.";
                                        return result;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(model.drawingsTechnical))
                                        {
                                            result.Succeed = false;
                                            result.ErrorMessage = "Bản vẽ này không được để trống.";
                                            return result;
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(model.drawingsTechnical))
                                            {
                                                result.Succeed = false;
                                                result.ErrorMessage = "Bản vẽ này không được để trống.";
                                                return result;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(model.drawings2D))
                                                {
                                                    result.Succeed = false;
                                                    result.ErrorMessage = "Bản vẽ này không được để trống.";
                                                    return result;
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(model.drawings3D))
                                                    {
                                                        result.Succeed = false;
                                                        result.ErrorMessage = "Bản vẽ này không được để trống.";
                                                        return result;
                                                    }
                                                    else
                                                    {
                                                        if (model.price < 0)
                                                        {
                                                            result.Succeed = false;
                                                            result.ErrorMessage = "Giá tiền không được âm.";
                                                            return result;
                                                        }
                                                        else
                                                        {
                                                            if (model.areaId == Guid.Empty)
                                                            {
                                                                result.Succeed = false;
                                                                result.ErrorMessage = "Không nhận được area";
                                                                return result;
                                                            }
                                                            else
                                                            {
                                                                //Create Item
                                                                var newItem = new Item
                                                                {
                                                                    name = model.name,
                                                                    image = model.image,
                                                                    mass = model.mass,
                                                                    unit = model.unit,
                                                                    length = model.length,
                                                                    depth = model.depth,
                                                                    height = model.height,
                                                                    drawingsTechnical = model.drawingsTechnical,
                                                                    drawings2D = model.drawings2D,
                                                                    drawings3D = model.drawings3D,
                                                                    description = model.description,
                                                                    price = model.price,
                                                                    //areaId=model.areaId,
                                                                    //categoryId=model.categoryId,
                                                                    isDeleted = false,
                                                                    createById=id
                                                                };
                                                                _dbContext.Item.Add(newItem);
                                                                await _dbContext.SaveChangesAsync();
                                                                result.Succeed = true;
                                                                result.Data = newItem.id;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> AddMaterialToItem(Guid id, Guid itemId, AddMaterialToItemModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data =_dbContext.Material.Where(i => i.id == model.materialId).FirstOrDefault();
                if(data != null)
                {
                    var newMaterialItem = new ItemMaterial
                    {
                        createdById = id,
                        itemId = itemId,
                        materialId = model.materialId,
                        quantity = model.quantity,
                        price = model.price,
                        totalPrice = model.totalPrice,
                    };
                    newMaterialItem.totalPrice = model.quantity * model.price;
                    _dbContext.ItemMaterial.Add(newMaterialItem);
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = newMaterialItem.id;
                }
                else
                {
                    result.ErrorMessage = "ItemMaterial" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }

            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public ResultModel UpdateItem(Guid id, Guid userId, UpdateMaterialToItemModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                //Update Item
                var data = _dbContext.ItemMaterial.Where(i => i.id == model.id).FirstOrDefault();
                if (data != null)
                {
                    data.price = model.price;
                    data.quantity = model.quantity;
                    data.totalPrice = model.price * model.quantity;
                    data.createdById = userId;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<ItemMaterial, ItemMaterialModel>(data);
                }
                else
                {
                    result.ErrorMessage = "ItemMaterial" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel UpdateItem(Guid id, Guid userId, UpdateItemModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                //Validation
                if (string.IsNullOrEmpty(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống.";
                    return result;
                }
                else
                {
                    if (model.mass < 0)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Diện tích không được âm.";
                        return result;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.unit))
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Đơn vị này không được để trống.";
                            return result;
                        }
                        else
                        {
                            if (model.length < 0)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Chiều dài không được âm.";
                                return result;
                            }
                            else
                            {
                                if (model.height < 0)
                                {
                                    result.Succeed = false;
                                    result.ErrorMessage = "Chiều cao không được âm.";
                                    return result;
                                }
                                else
                                {
                                    if (model.depth < 0)
                                    {
                                        result.Succeed = false;
                                        result.ErrorMessage = "Chiều rộng không được âm.";
                                        return result;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(model.drawingsTechnical))
                                        {
                                            result.Succeed = false;
                                            result.ErrorMessage = "Bản vẽ này không được để trống.";
                                            return result;
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(model.drawings2D))
                                            {
                                                result.Succeed = false;
                                                result.ErrorMessage = "Bản vẽ này không được để trống.";
                                                return result;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(model.drawings3D))
                                                {
                                                    result.Succeed = false;
                                                    result.ErrorMessage = "Bản vẽ này không được để trống.";
                                                    return result;
                                                }
                                                else
                                                {
                                                    if (model.price < 0)
                                                    {
                                                        result.Succeed = false;
                                                        result.ErrorMessage = "Giá tiền không được âm.";
                                                        return result;
                                                    }
                                                    else
                                                    {
                                                        if (model.areaId == Guid.Empty)
                                                        {
                                                            result.Succeed = false;
                                                            result.ErrorMessage = "Không nhận được area";
                                                            return result;
                                                        }
                                                        else
                                                        {
                                                            //Update Item
                                                            var data = _dbContext.Item.Where(i => i.id == model.id).FirstOrDefault();
                                                            if (data != null)
                                                            {
                                                                data.name = model.name;
                                                                data.image = model.image;
                                                                data.mass = model.mass;
                                                                data.unit = model.unit;
                                                                data.length = model.length;
                                                                data.depth = model.depth;
                                                                data.height = model.height;
                                                                data.drawingsTechnical = model.drawingsTechnical;
                                                                data.drawings2D = model.drawings2D;
                                                                data.drawings3D = model.drawings3D;
                                                                data.description = model.description;
                                                                data.price = model.price;
                                                                //data.areaId = model.areaId;
                                                                //data.categoryId = model.categoryId;
                                                                data.createById = userId;
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
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
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

        public ResultModel GetAllItem(int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.isDeleted != true).OrderByDescending(i=>i.name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<ItemModel>>(data),
                    Total = data.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel SortItembyPrice(int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Item.Where(i => i.isDeleted != true).OrderByDescending(i => i.price).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(); ;
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<ItemModel>>(data),
                    Total = data.Count
                };
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
    }
}

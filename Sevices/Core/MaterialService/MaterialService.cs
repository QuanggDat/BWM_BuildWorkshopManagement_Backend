using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace Sevices.Core.MaterialService
{
    public class MaterialService : IMaterialService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public MaterialService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ResultModel> CreateCategory(CreateMaterialCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrEmpty(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống";
                    return result;
                }
                bool nameExists = await _dbContext.Squad.AnyAsync(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này đã tồn tại.";
                }

                //Create Category
                else {
                    var newCategory = new MaterialCategory
                    {
                        name = model.name,
                        isDeleted = false
                    };
                    _dbContext.MaterialCategory.Add(newCategory);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<ResultModel> CreateMaterial(CreateMaterialModel model)
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
                if (string.IsNullOrEmpty(model.color))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Màu này không được để trống.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.supplier))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nguồn cung này không được để trống.";
                    return result;
                }
                if (model.amount < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Độ dày không được âm.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.unit))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Đơn vị này không được để trống.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.importPlace))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nơi nhập này không được để trống.";
                    return result;
                }
                if (model.amount < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Số lượng không được âm.";
                    return result;
                }
                if (model.price < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Giá tiền không được âm.";
                    return result;
                }
                if (model.categoryId == Guid.Empty)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không nhận được category";
                    return result;
                }

                //Create Material 
                var newMaterial = new Material
                {
                    name = model.name,
                    image = model.image,
                    color = model.color,
                    supplier = model.supplier,
                    thickness = model.thickness,
                    unit = model.unit,
                    sku = model.sku,
                    importDate = model.importDate,
                    importPlace = model.importPlace,
                    amount = model.amount,
                    price = model.price,
                    totalPrice = model.totalPrice,
                    categoryId = model.categoryId,
                    isDeleted = false
                };
                newMaterial.totalPrice = model.price * model.amount;
                newMaterial.sku = $"{model.name[0]}-{newMaterial.supplier}-{model.thickness}";

                _dbContext.Material.Add(newMaterial);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = newMaterial.id;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.MaterialCategory.Where(c => c.id == model.id).FirstOrDefault();
                if (data != null)
                {
                    //Validation
                    if (string.IsNullOrEmpty(model.name))
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên này không được để trống.";
                        return result;
                    }

                    //Update Material Category
                    else {
                        data.name = model.name;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Data.Entities.MaterialCategory, MaterialCategoryModel>(data);
                    }
                }
                else
                {
                    result.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel UpdateMaterial(UpdateMaterialModel model)
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
                if (string.IsNullOrEmpty(model.color))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Màu này không được để trống.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.supplier))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nguồn cung này không được để trống.";
                    return result;
                }
                if (model.amount < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Độ dày không được âm.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.unit))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Đơn vị này không được để trống.";
                    return result;
                }
                if (string.IsNullOrEmpty(model.importPlace))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nơi nhập này không được để trống.";
                    return result;
                }
                if (model.amount < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Số lượng không được âm.";
                    return result;
                }
                if (model.price < 0)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Giá tiền không được âm.";
                    return result;
                }
                if (model.categoryId == Guid.Empty)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không nhận được category";
                    return result;
                }

                //Update Material
                var data = _dbContext.Material.Where(m => m.id == model.id).FirstOrDefault();
                if (data != null)
                {
                    data.name = model.name;
                    data.image = model.image;
                    data.color = model.color;
                    data.supplier = model.supplier;
                    data.thickness = model.thickness;
                    data.unit = model.unit;
                    data.sku = model.sku;
                    data.importDate = model.importDate;
                    data.importPlace = model.importPlace;
                    data.amount = model.amount;
                    data.price = model.price;
                    data.totalPrice = model.totalPrice = model.price * model.amount;
                    data.categoryId = model.categoryId;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Material, MaterialModel>(data);
                }
                else
                {
                    result.ErrorMessage = "Material" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel UpdateMaterialAmount(UpdateMaterialAmountModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(m => m.id == model.id).FirstOrDefault();
                if (data != null)
                { 
                    //Validation
                    if (model.amount < 0)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Số lượng không được âm.";
                        return result;
                    }

                    //Update Amount
                    else {
                        data.amount = model.amount;
                        data.totalPrice = data.price * model.amount;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Material, MaterialModel>(data);
                    }
                }
                else
                {
                    result.ErrorMessage = "Material" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllCategory(int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.MaterialCategory.Where(i => i.isDeleted != true).OrderByDescending(i=>i.name);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<MaterialCategoryModel>(data);
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

        public ResultModel GetAllMaterial(int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(i => i.isDeleted != true).OrderByDescending(i => i.importDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<MaterialModel>>(data),
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

        public ResultModel GetMaterialById(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(m => m.id == id && m.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<MaterialModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Material" + ErrorMessage.ID_NOT_EXISTED;
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
                var data = _dbContext.MaterialCategory.Where(c => c.id == id && c.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<MaterialCategoryModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel DeleteMaterial(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<Material, MaterialModel>(data);
                    resultModel.Data = view;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Material" + ErrorMessage.ID_NOT_EXISTED;
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
                var data = _dbContext.MaterialCategory.Where(c => c.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<MaterialCategory, MaterialCategoryModel>(data);
                    resultModel.Data = view;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel SortMaterialbyPrice()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(i => i.isDeleted != true).OrderByDescending(i=>i.price);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<MaterialModel>(data);
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

        public ResultModel SortMaterialbyThickness()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(i => i.isDeleted != true).OrderByDescending(i => i.thickness);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<MaterialModel>(data);
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
    }
}

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

            //Validation
            if (string.IsNullOrEmpty(model.name) || string.IsNullOrEmpty(model.supplier) || string.IsNullOrEmpty(model.sku) || model.importDate == null || string.IsNullOrEmpty(model.importPlace) || model.amount <= 0 || model.price <= 0)
            {
                result.ErrorMessage = "Thông tin nhập vào không hợp lệ.";
                result.Succeed = false;
                return result;
            }

            

            try
            {
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
                    data.name = model.name;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.MaterialCategory, MaterialCategoryModel>(data);
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
                if (string.IsNullOrEmpty(model.name) || string.IsNullOrEmpty(model.supplier) || string.IsNullOrEmpty(model.sku) || model.importDate == null || string.IsNullOrEmpty(model.importPlace) || model.amount <= 0 || model.price <= 0)
                {
                    result.ErrorMessage = "Thông tin nhập vào không hợp lệ.";
                    result.Succeed = false;
                    return result;
                }

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
                    //result.Data = _mapper.Map<MaterialCategory, MaterialCategoryModel>(data);
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

        public ResultModel GetAllCategory()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.MaterialCategory.Where(i => i.isDeleted != true);
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

        public ResultModel GetAllMaterial()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Material.Where(i => i.isDeleted != true);
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
    }
}

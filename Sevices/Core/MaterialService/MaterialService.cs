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
        
        public ResultModel CreateMaterial(Guid createdById, CreateMaterialModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkMaterial = _dbContext.Material.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                if (checkMaterial != null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên vật liệu này đã tồn tại !";
                }
                else 
                {
                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId).SingleOrDefault();
                    if (checkCategory == null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory !";
                    }
                    else
                    {
                        //Create Material 
                        var material = new Material
                        {
                            createById = createdById,
                            materialCategoryId = model.materialCategoryId,
                            name = model.name,
                            image = model.image,
                            color = model.color,
                            supplier = model.supplier,
                            thickness = model.thickness,
                            unit = model.unit,
                            sku = $"{model.name[0]}-{model.supplier}-{model.thickness}",
                            importDate = model.importDate,
                            importPlace = model.importPlace,
                            amount = model.amount,
                            price = model.price,
                            totalPrice = model.price * model.amount,
                            isDeleted = false
                        };

                        _dbContext.Material.Add(material);
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = material.id;
                    }                 
                }                                  
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateMaterial(UpdateMaterialModel model)
        {
            ResultModel result = new ResultModel();
            try
            {              
                var check = _dbContext.Material.Where(m => m.id == model.id).FirstOrDefault();
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material!";
                    return result;
                }
                else
                {
                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId).SingleOrDefault();
                    if (checkCategory == null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory !";
                    }
                    else
                    {
                        if (model.name != check.name)
                        {
                            var checkExists = _dbContext.Material.FirstOrDefault(x => x.name == model.name && !x.isDeleted);
                            if (checkExists != null)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Tên này đã tồn tại !";
                                return result;
                            }
                            else
                            {
                                check.name = model.name;
                                check.image = model.image;
                                check.color = model.color;
                                check.supplier = model.supplier;
                                check.thickness = model.thickness;
                                check.unit = model.unit;                                
                                check.importDate = model.importDate;
                                check.importPlace = model.importPlace;
                                check.amount = model.amount;
                                check.price = model.price;
                                check.totalPrice = model.price * model.amount;
                                check.sku = $"{model.name[0]}-{model.supplier}-{model.thickness}";
                                check.materialCategoryId = model.materialCategoryId;

                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = "Cập nhập thành công " + check.id;
                            }
                        }
                        else
                        {
                            check.name = model.name;
                            check.image = model.image;
                            check.color = model.color;
                            check.supplier = model.supplier;
                            check.thickness = model.thickness;
                            check.unit = model.unit;
                            check.importDate = model.importDate;
                            check.importPlace = model.importPlace;
                            check.amount = model.amount;
                            check.price = model.price;
                            check.totalPrice = model.price * model.amount;
                            check.sku = $"{model.name[0]}-{model.supplier}-{model.thickness}";
                            check.materialCategoryId = model.materialCategoryId;

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

        public ResultModel DeleteMaterial(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Material.Where(m => m.id == id).FirstOrDefault();
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material!";
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

        public ResultModel GetMaterialById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var check = _dbContext.Material.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material!";
                    return result;
                }
                else
                {
                    var createBy = _dbContext.Users.Find(check.createById);
                    var materialCategory = _dbContext.MaterialCategory.Find(check.materialCategoryId);

                    var ResponMaterialModel = new ResponeMaterialModel
                    {
                        id = check.id,
                        createById = check.createById,
                        createByName = createBy!.fullName,
                        materialCategoryId = check.materialCategoryId,
                        materialCategoryName = materialCategory!.name,
                        name = check.name,
                        image = check.image,
                        color = check.color,
                        supplier = check.supplier,
                        thickness = check.thickness,
                        unit = check.unit,
                        sku = check.sku,
                        importDate = check.importDate,
                        importPlace = check.importPlace,
                        amount = check.amount,
                        price = check.price,
                        totalPrice = check.totalPrice,
                    };

                    result.Data = ResponMaterialModel;
                    result.Succeed = true;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateMaterialAmount(UpdateMaterialAmountModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Material.Where(m => m.id == model.id).FirstOrDefault();
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory !";
                    return result;
                }
                else
                {
                    check.amount = model.amount;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = "Cập nhập thành công " + check.id;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllMaterial(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listMaterialCategory = _dbContext.Material.Where(x => x.isDeleted == false)
                   .OrderByDescending(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listMaterialCategory = listMaterialCategory.Where(x => x.name.Contains(search)).ToList();
                }

                var listMaterialCategoryPaging = listMaterialCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ResponeMaterialModel>();
                foreach (var item in listMaterialCategoryPaging)
                {
                    var createBy = _dbContext.Users.Find(item.createById);
                    var materialCategory = _dbContext.MaterialCategory.Find(item.materialCategoryId);

                    var tmp = new ResponeMaterialModel
                    {
                        id = item.id,
                        createById = item.createById,
                        createByName = createBy!.fullName,
                        materialCategoryId = item.materialCategoryId,
                        materialCategoryName = materialCategory!.name,
                        name = item.name,
                        image = item.image,
                        color = item.supplier,
                        thickness = item.thickness,
                        unit = item.unit,
                        sku = item.sku,
                        importDate = item.importDate,
                        importPlace = item.importPlace,
                        amount = item.amount,
                        price = item.price,
                        totalPrice = item.totalPrice,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listMaterialCategoryPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetMaterialByMaterialCategoryId(Guid materialCategoryId, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.MaterialCategory.Where(x => x.id == materialCategoryId && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material Category!";
                    return result;
                }

                
                
                var listMaterialCategory = _dbContext.Material.Where(x => x.materialCategoryId == materialCategoryId && x.isDeleted == false)
                   .OrderByDescending(x => x.name).ToList();


                var listMaterialCategoryPaging = listMaterialCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ResponeMaterialModel>();
                foreach (var item in listMaterialCategoryPaging)
                {
                    var createBy = _dbContext.Users.Find(item.createById);
                    var materialCategory = _dbContext.MaterialCategory.Find(item.materialCategoryId);

                    var tmp = new ResponeMaterialModel
                    {
                        id = item.id,
                        createById = item.createById,
                        createByName = createBy!.fullName,
                        materialCategoryId = item.materialCategoryId,
                        materialCategoryName = materialCategory!.name,
                        name = item.name,
                        image = item.image,
                        color = item.supplier,
                        thickness = item.thickness,
                        unit = item.unit,
                        sku = item.sku,
                        importDate = item.importDate,
                        importPlace = item.importPlace,
                        amount = item.amount,
                        price = item.price,
                        totalPrice = item.totalPrice,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listMaterialCategoryPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        
    }
}

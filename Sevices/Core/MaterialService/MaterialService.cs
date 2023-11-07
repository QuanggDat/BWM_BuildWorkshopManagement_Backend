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
        
        public ResultModel Create(CreateMaterialModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkMaterial = _dbContext.Material.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                if (checkMaterial != null)
                {
                    result.Code = 27;
                    result.Succeed = false;
                    result.ErrorMessage = "Tên vật liệu này đã tồn tại !";
                }
                else 
                {
                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId && x.isDeleted != true).SingleOrDefault();
                    if (checkCategory == null)
                    {
                        result.Code = 28;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu !";
                        
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(model.image))
                        {
                            model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                        }
                        //Create Material 
                        var material = new Material
                        {
                            materialCategoryId = model.materialCategoryId,
                            name = model.name,                            
                            image = model.image,
                            color = model.color,
                            supplier = model.supplier,
                            thickness = model.thickness,
                            unit = model.unit,
                            sku = $"{model.name[0]}-{model.supplier}-{model.thickness}",
                            importPlace = model.importPlace,
                            price = model.price,
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

        public ResultModel Update(UpdateMaterialModel model)
        {
            ResultModel result = new ResultModel();
            try
            {              
                var check = _dbContext.Material.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();
                if (check == null)
                {
                    result.Code = 29;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin vật liệu!";
                }
                else
                {
                    if (string.IsNullOrEmpty(model.image))
                    {
                        model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                    }
                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId && x.isDeleted != true).SingleOrDefault();
                    if (checkCategory == null)
                    {
                        result.Code = 28;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu !";
                    }
                    else
                    {
                        if (model.name != check.name)
                        {
                            var checkExists = _dbContext.Material.FirstOrDefault(x => x.name == model.name && !x.isDeleted);
                            if (checkExists != null)
                            {
                                result.Code = 27;
                                result.Succeed = false;
                                result.ErrorMessage = "Tên vật liệu đã tồn tại !";
                            }
                            else
                            {
                                check.name = model.name;
                                check.image = model.image;
                                check.color = model.color;
                                check.supplier = model.supplier;
                                check.thickness = model.thickness;
                                check.unit = model.unit;                                
                                check.importPlace = model.importPlace;
                                check.price = model.price;
                                check.sku = $"{model.name[0]}-{model.supplier}-{model.thickness}";
                                check.materialCategoryId = model.materialCategoryId;

                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = "Cập nhập thành công " + check.name;
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
                            check.importPlace = model.importPlace;
                            check.price = model.price;
                            check.sku = $"{model.name[0]}-{model.supplier}-{model.thickness}";
                            check.materialCategoryId = model.materialCategoryId;

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = "Cập nhập thành công " + check.name;
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

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {

                var check = _dbContext.Material.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();
                if (check == null)
                {
                    result.Code = 29;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin vật liệu!";
                }
                else
                {
                    check.isDeleted = true;
                    _dbContext.SaveChanges();
                    result.Data = "Xoá thành công " + check.name;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var check = _dbContext.Material.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 29;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin vật liệu!";
                }
                else
                {
                    var materialModel = new MaterialModel
                    {
                        id = check.id,
                        materialCategoryId = check.materialCategoryId,
                        name = check.name,
                        image = check.image,
                        color = check.color,
                        supplier = check.supplier,
                        thickness = check.thickness,
                        unit = check.unit,
                        sku = check.sku,
                        importPlace = check.importPlace,
                        price = check.price,
                    };

                    result.Data = materialModel;
                    result.Succeed = true;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
       

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listMaterialCategory = _dbContext.Material.Where(x => x.isDeleted == false)
                   .OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listMaterialCategory = listMaterialCategory.Where(x => x.name.Contains(search)).ToList();
                }

                var listMaterialCategoryPaging = listMaterialCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<MaterialModel>();
                foreach (var item in listMaterialCategoryPaging)
                {               
                    var tmp = new MaterialModel
                    {
                        id = item.id,
                        materialCategoryId = item.materialCategoryId,                        
                        name = item.name,
                        image = item.image,
                        color = item.color,
                        supplier = item.supplier,
                        thickness = item.thickness,
                        unit = item.unit,
                        sku = item.sku,
                        importPlace = item.importPlace,
                        price = item.price,
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

        public ResultModel GetByMaterialCategoryId(Guid materialCategoryId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.MaterialCategory.Where(x => x.id == materialCategoryId && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 28;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                }
                else
                {
                    var listMaterialCategory = _dbContext.Material.Where(x => x.materialCategoryId == materialCategoryId && x.isDeleted == false)
                   .OrderBy(x => x.name).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        listMaterialCategory = listMaterialCategory.Where(x => x.name.Contains(search)).ToList();
                    }

                    var listMaterialCategoryPaging = listMaterialCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<MaterialModel>();
                    foreach (var item in listMaterialCategoryPaging)
                    {
                                            
                        var tmp = new MaterialModel
                        {
                            id = item.id,
                            materialCategoryId = item.materialCategoryId,
                            name = item.name,
                            image = item.image,
                            color = item.color,
                            supplier = item.supplier,
                            thickness = item.thickness,
                            unit = item.unit,
                            sku = item.sku,
                            importPlace = item.importPlace,
                            price = item.price,
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
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}

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
        
        public ResultModel Create(CreateMaterialModel model, Guid userId)
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
                    result.ErrorMessage = "Tên vật liệu đã tồn tại !";
                }
                else 
                {
                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId && x.isDeleted != true).FirstOrDefault();

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

                        var log = new Data.Entities.Log()
                        {
                            materialId = material.id,
                            userId = userId,
                            modifiedTime = DateTime.UtcNow.AddHours(7),
                            action = "Thêm vật liệu :" + material.name,
                        };
                        _dbContext.Log.Add(log);

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

        public ResultModel Update(UpdateMaterialModel model, Guid userId)
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

                    var checkCategory = _dbContext.MaterialCategory.Where(x => x.id == model.materialCategoryId && x.isDeleted != true).FirstOrDefault();

                    if (checkCategory == null)
                    {
                        result.Code = 28;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu !";
                    }
                    else
                    {
                        var checkExists = _dbContext.Material.FirstOrDefault(x => x.name == model.name && x.name != check.name && !x.isDeleted);

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
                            _dbContext.Material.Update(check);

                            var log = new Data.Entities.Log()
                            {
                                materialId = check.id,
                                userId = userId,
                                modifiedTime = DateTime.UtcNow.AddHours(7),
                                action = "Cập nhật vật liệu :" + check.name,
                            };
                            _dbContext.Log.Add(log);

                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = check.id;
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

        public ResultModel Delete(Guid id, Guid userId)
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
                    var checkMaterialExistItem = _dbContext.ItemMaterial.FirstOrDefault(x => x.materialId == check.id);

                    if (checkMaterialExistItem != null)
                    {
                        result.Code = 82;
                        result.Succeed = false;
                        result.ErrorMessage = "Vật liệu này đang tồn tại trong sản phẩm, hãy xoá ra khỏi sản phẩm trước khi xoá vật liệu!";
                    }
                    else
                    {
                        check.isDeleted = true;
                        _dbContext.Material.Update(check);

                        var log = new Data.Entities.Log()
                        {
                            materialId = check.id,
                            userId = userId,
                            modifiedTime = DateTime.UtcNow.AddHours(7),
                            action = "Xóa vật liệu :" + check.name,
                        };
                        _dbContext.Log.Add(log);

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

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var check = _dbContext.Material.Include(x => x.MaterialCategory)
                    .Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

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
                        materialCategoryName = check.MaterialCategory.name,
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
                var listMaterial = _dbContext.Material.Include(x => x.MaterialCategory)
                    .Where(x => x.isDeleted == false).OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listMaterial = listMaterial.Where(x => x.name.Contains(search)).ToList();
                }

                var listMaterialPaging = listMaterial.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<MaterialModel>();
                foreach (var item in listMaterialPaging)
                {               
                    var tmp = new MaterialModel
                    {
                        id = item.id,
                        materialCategoryId = item.materialCategoryId,        
                        materialCategoryName =item.MaterialCategory.name,
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
                    Total = listMaterial.Count
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
                var check = _dbContext.MaterialCategory
                    .Where(x => x.id == materialCategoryId && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 28;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                }
                else
                {
                    var listMaterial = _dbContext.Material.Where(x => x.materialCategoryId == materialCategoryId && x.isDeleted == false)
                            .OrderBy(x => x.name).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        listMaterial = listMaterial.Where(x => x.name.Contains(search)).ToList();
                    }

                    var listMaterialPaging = listMaterial.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<MaterialModel>();
                    foreach (var item in listMaterialPaging)
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
                        Total = listMaterial.Count
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
        
        public ResultModel GetByOrderId(Guid orderId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).Where(x => x.id == orderId).FirstOrDefault();

                if (order == null)
                {
                    result.Code = 97;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var listOrderDetailId = order.OrderDetails.Select(x => x.id).ToList();

                    var listMaterialId = _dbContext.OrderDetailMaterial.Include(x => x.Material)
                        .Where(x => listOrderDetailId.Contains(x.orderDetailId)).Select(x => x.materialId).Distinct().ToList();

                    var listMaterial = _dbContext.Material.Include(x => x.MaterialCategory)
                        .Where(x => listMaterialId.Contains(x.id) && x.isDeleted == false)
                            .OrderBy(x => x.name).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        listMaterial = listMaterial.Where(x => x.name.Contains(search)).ToList();
                    }

                    var listMaterialPaging = listMaterial.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<MaterialModel>();
                    foreach (var item in listMaterialPaging)
                    {

                        var tmp = new MaterialModel
                        {
                            id = item.id,
                            materialCategoryId = item.materialCategoryId,
                            materialCategoryName = item.MaterialCategory.name,
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
                        Total = listMaterial.Count
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

        public ResultModel GetAllLogOnMaterial(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listLog = _dbContext.Log.Include(x=>x.Material).Where(x => x.materialId!=null).OrderBy(x => x.modifiedTime).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listLog = listLog.Where(x => x.action.Contains(search)).ToList();
                }

                var listLogPaging = listLog.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LogModel>();
                foreach (var item in listLogPaging)
                {
                    var tmp = new LogModel
                    {
                        id = item.id,
                        materialId=item.materialId,
                        Material=item.Material,
                        userId=item.userId,
                        modifiedTime=item.modifiedTime,
                        action = item.action,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listLog.Count
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

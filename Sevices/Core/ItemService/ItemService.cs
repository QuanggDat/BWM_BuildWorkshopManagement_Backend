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
using Sevices.Core.UtilsService;
using Microsoft.EntityFrameworkCore;
using Data.Enums;
using Aspose.Cells;
using Data.Utils;

namespace Sevices.Core.ItemService
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUtilsService _utilsService;
        private readonly IConfiguration _configuration;

        public ItemService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IUtilsService utilsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _utilsService = utilsService;
            _configuration = configuration;
        }

        public ResultModel Create(CreateItemModel model, Guid userId)
        {
            var result = new ResultModel();
            result.Succeed = false;

            try
            {
                var checkCategory = _dbContext.ItemCategory.FirstOrDefault(x => x.id == model.itemCategoryId && x.isDeleted != true);

                if (checkCategory == null)
                {
                    result.Code = 33;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm !";
                }
                else
                {
                    var listItem = _dbContext.Item.Where(x => !x.isDeleted).ToList();

                    var listItemCodeDB = listItem.Select(x => x.code).Distinct().ToList();

                    var randomCode = _utilsService.GenerateItemCode(listItemCodeDB, listItemCodeDB);

                    if (string.IsNullOrEmpty(model.image))
                    {
                        model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                    }

                    //Create Item
                    var item = new Item
                    {
                        itemCategoryId = model.itemCategoryId,
                        name = model.name,
                        code = randomCode,
                        image = model.image,
                        length = model.length,
                        depth = model.depth,
                        height = model.height,
                        unit = model.unit,
                        mass = model.mass,
                        drawingsTechnical = model.drawingsTechnical,
                        drawings2D = model.drawings2D,
                        drawings3D = model.drawings3D,
                        description = model.description,
                        isDeleted = false
                    };

                    _dbContext.Item.Add(item);

                    foreach (var procedure in model.listProcedure)
                    {
                        _dbContext.ProcedureItem.Add(new ProcedureItem
                        {
                            itemId = item.id,
                            procedureId = procedure.procedureId,
                            priority = procedure.priority
                        });
                    }

                    foreach (var material in model.listMaterial)
                    {
                        var _material = _dbContext.Material.Find(material.materialId);

                        if (_material == null)
                        {
                            result.Code = 62;
                            result.Succeed = false;
                            result.ErrorMessage = $"Không tìm thấy thông tin mã vật liệu {material.materialId} !";
                            return result;
                        }
                        else
                        {
                            _dbContext.ItemMaterial.Add(new ItemMaterial
                            {
                                itemId = item.id,
                                materialId = material.materialId,
                                quantity = material.quantity,
                                price = _material.price,
                                totalPrice = material.quantity * _material.price
                            });
                            item.price += material.quantity * _material.price;
                        }
                    }

                    var log = new Data.Entities.Log()
                    {
                        itemId = item.id,
                        userId = userId,
                        modifiedTime = DateTime.UtcNow.AddHours(7),
                        action = "Tạo sản phẩm mới :" + item.name,
                    };
                    _dbContext.Log.Add(log);

                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = item.id;
                }             
            }
            
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel DuplicateItem(Guid id, int number)
        {
            var result = new ResultModel();
            try
            {
                var listNewDupItemId = new List<Guid>();

                var item = _dbContext.Item.Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                    .Include(x => x.ItemCategory).Include(x => x.ItemMaterials).FirstOrDefault(x => x.id == id);

                if (item == null)
                {
                    result.Code = 34;
                    result.Succeed= false;
                    result.ErrorMessage = "Không tìm thấy thông tin sản phẩm!";
                }
                else
                {
                    var listItem = _dbContext.Item.Where(x => !x.isDeleted).ToList();

                    var listItemCodeDB = listItem.Select(x => x.code).Distinct().ToList();

                    var listItemCodeCreated = new List<string>();

                    for (int i = 1; i <= number; i++)
                    {
                        var randomCode = _utilsService.GenerateItemCode(listItemCodeDB, listItemCodeCreated);
                        listItemCodeCreated.Add(randomCode);

                        var newItem = new Item
                        {
                            itemCategoryId = item.itemCategoryId,
                            code = randomCode,
                            name = item.name,                            
                            image = item.image,
                            length = item.length,
                            depth = item.depth,
                            height = item.height,
                            unit = item.unit,
                            mass = item.mass,
                            drawingsTechnical = item.drawingsTechnical,
                            drawings2D = item.drawings2D,
                            drawings3D = item.drawings3D,                            
                            description = item.description,
                            price = item.price,
                            isDeleted = false,
                        };

                        _dbContext.Item.Add(newItem);
                        listNewDupItemId.Add(newItem.id);

                        foreach (var procedure in item.ProcedureItems)
                        {
                            _dbContext.ProcedureItem.Add(new ProcedureItem
                            {
                                itemId = newItem.id,
                                procedureId = procedure.procedureId,
                                priority = procedure.priority
                            });
                        }

                        foreach (var material in item.ItemMaterials)
                        {
                            _dbContext.ItemMaterial.Add(new ItemMaterial
                            {
                                itemId = newItem.id,
                                materialId = material.materialId,
                                quantity = material.quantity,
                                totalPrice = material.totalPrice
                            });
                        }
                    }
                  
                    _dbContext.SaveChanges();
                    result.Data = listNewDupItemId;
                    result.Succeed = true;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateItemModel model, Guid userId)
        {
            ResultModel result = new ResultModel();

            try
            {   
                var check =  _dbContext.Item.FirstOrDefault(x => x.id == model.id && x.isDeleted != true);
                
                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin sản phẩm!";
                }
                else
                {
                    var checkCategory = _dbContext.ItemCategory.FirstOrDefault(x => x.id == model.itemCategoryId && x.isDeleted != true);

                    if (checkCategory == null)
                    {
                        result.Code = 33;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm !";
                    }
                    else
                    {

                        var checkItemExist = _dbContext.OrderDetail.Include(x => x.Order).FirstOrDefault(x => x.itemId == check.id
                                && x.Order.status == OrderStatus.InProgress || x.Order.status == OrderStatus.Cancel || x.Order.status == OrderStatus.Completed);
                        /*
                        if (checkItemExist != null)
                        {
                            result.Code = 105;
                            result.Succeed = false;
                            result.ErrorMessage = "Sản phẩm đã đi vào sản xuất, không thể chỉnh sửa sản phẩm!";
                        }

                        else
                        {

                        }
                        */
                        if (string.IsNullOrEmpty(model.image))
                        {
                            model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                        }

                        check.itemCategoryId = model.itemCategoryId;
                        check.name = model.name;
                        check.image = model.image;
                        check.length = model.length;
                        check.depth = model.depth;
                        check.height = model.height;
                        check.unit = model.unit;
                        check.mass = model.mass;
                        check.drawingsTechnical = model.drawingsTechnical;
                        check.drawings2D = model.drawings2D;
                        check.drawings3D = model.drawings3D;
                        check.description = model.description;

                        // Remove all old Procedure Item
                        var currentProcedureItems = _dbContext.ProcedureItem
                            .Where(x => x.itemId == model.id).ToList();

                        if (currentProcedureItems != null && currentProcedureItems.Count > 0)
                        {
                            _dbContext.ProcedureItem.RemoveRange(currentProcedureItems);
                        }

                        // Set new Procedure Item
                        var procedureItems = new List<ProcedureItem>();

                        foreach (var procedure in model.listProcedure)
                        {
                            procedureItems.Add(new ProcedureItem
                            {
                                itemId = model.id,
                                procedureId = procedure.procedureId,
                                priority = procedure.priority
                            });
                        }

                        // Remove all old Material Item
                        var currentMaterialItems = _dbContext.ItemMaterial
                            .Where(x => x.itemId == model.id).ToList();

                        if (currentMaterialItems != null && currentMaterialItems.Count > 0)
                        {
                            _dbContext.ItemMaterial.RemoveRange(currentMaterialItems);
                        }

                        // return price = 0
                        check.price = 0;

                        // Set new Material Item
                        var materialItems = new List<ItemMaterial>();

                        foreach (var material in model.listMaterial)
                        {
                            var _material = _dbContext.Material.Find(material.materialId);

                            if (_material == null)
                            {
                                result.Code = 62;
                                result.Succeed = false;
                                result.ErrorMessage = $"Không tìm thấy thông tin mã vật liệu {material.materialId} !";
                                return result;
                            }
                            else
                            {
                                materialItems.Add(new ItemMaterial
                                {
                                    itemId = model.id,
                                    materialId = material.materialId,
                                    quantity = material.quantity,
                                    price = _material.price,
                                    totalPrice = material.quantity * _material!.price
                                });
                                check.price += material.quantity * _material.price;
                            }
                        }

                        var log = new Data.Entities.Log()
                        {
                            itemId = check.id,
                            userId = userId,
                            modifiedTime = DateTime.UtcNow.AddHours(7),
                            action = "Cập nhật sản phẩm :" + check.name,
                        };
                        _dbContext.Log.Add(log);

                        _dbContext.ProcedureItem.AddRange(procedureItems);
                        _dbContext.ItemMaterial.AddRange(materialItems);

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = check.id;
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
                var check = _dbContext.Item.FirstOrDefault(x => x.id == id && x.isDeleted != true);

                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin sản phẩm!";
                }
                else
                {
                    var checkItemExist = _dbContext.OrderDetail.Include(x => x.Order)
                        .FirstOrDefault(x => x.itemId == check.id); 

                    if (checkItemExist != null)
                    {
                        result.Code = 104;
                        result.Succeed = false;
                        result.ErrorMessage = $"Không thể xoá sản phẩm, sản phẩm đang trong đơn hàng !";
                    }
                    else
                    {
                        check.isDeleted = true;
                        _dbContext.Item.Update(check);

                        var log = new Data.Entities.Log()
                        {
                            itemId = check.id,
                            userId = userId,
                            modifiedTime = DateTime.UtcNow.AddHours(7),
                            action = "Xóa sản phẩm :" + check.name,
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

        public ResultModel GetAllWithSearchAndPaging(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listItem =  _dbContext.Item.Where(x => x.isDeleted != true)                   
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure).ThenInclude(x => x.ProcedureSteps)
                    .Include(x => x.ItemCategory).Include(x => x.ItemMaterials).OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    search = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listItem = listItem.Where(x => FnUtil.Remove_VN_Accents(x.name).ToUpper().Contains(search) || FnUtil.Remove_VN_Accents(x.code).ToUpper().Contains(search)).ToList();
                }

                var listItemPaging = listItem.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ItemModel>();
                foreach (var item in listItemPaging)
                {                    
                    var tmp = new ItemModel
                    {
                        id = item.id,
                        itemCategoryId = item.itemCategoryId,
                        itemCategoryName = item.ItemCategory?.name?? "",
                        code = item.code,
                        name = item.name,
                        image = item.image,
                        length = item.length,
                        depth = item.depth,
                        height = item.height,
                        unit = item.unit,
                        mass = item.mass,
                        drawingsTechnical = item.drawingsTechnical,
                        drawings2D = item.drawings2D,
                        drawings3D = item.drawings3D,
                        description = item.description,
                        price = item.price,
                        listMaterial = item.ItemMaterials.Select(x => new ItemMaterialModel
                        {
                            materialId = x.materialId,
                            quantity = x.quantity,
                        }).ToList(),

                        listProcedure = item.ProcedureItems.Select(x => new ProcedureItemModel
                        {
                            procedureId = x.procedureId,
                            priority = x.priority,
                            listStep = x.Procedure.ProcedureSteps.Select(x => new StepProcedureModel
                            {
                                stepId = x.stepId,
                                priority = x.priority,
                            }).ToList(),
                        }).ToList(),
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItem.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetItemNotExistsInOrder(Guid orderId)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listItemIdInOrder = _dbContext.OrderDetail.Where(x => x.orderId == orderId).Select(x => x.itemId).ToList();

                var listItem = _dbContext.Item.Where(x => x.isDeleted != true && !listItemIdInOrder.Contains(x.id)).OrderBy(x => x.name).ToList();

                var list = new List<ItemModel>();
                foreach (var item in listItem)
                {
                    var tmp = new ItemModel
                    {
                        id = item.id,
                        code = item.code,
                        name = item.name,                      
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItem.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAll()
        {
            ResultModel result = new ResultModel();

            try
            {
                var listItem = _dbContext.Item.Where(x => x.isDeleted != true)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure).ThenInclude(x => x.ProcedureSteps)
                    .Include(x => x.ItemCategory).Include(x => x.ItemMaterials).OrderBy(x => x.name).ToList();

                var list = new List<ItemModel>();
                foreach (var item in listItem)
                {
                    var tmp = new ItemModel
                    {
                        id = item.id,
                        itemCategoryId = item.itemCategoryId,
                        itemCategoryName = item.ItemCategory?.name ?? "",
                        code = item.code,
                        name = item.name,
                        image = item.image,
                        length = item.length,
                        depth = item.depth,
                        height = item.height,
                        unit = item.unit,
                        mass = item.mass,
                        drawingsTechnical = item.drawingsTechnical,
                        drawings2D = item.drawings2D,
                        drawings3D = item.drawings3D,
                        description = item.description,
                        price = item.price,
                        listMaterial = item.ItemMaterials.Select(x => new ItemMaterialModel
                        {
                            materialId = x.materialId,
                            quantity = x.quantity,
                        }).ToList(),

                        listProcedure = item.ProcedureItems.Select(x => new ProcedureItemModel
                        {
                            procedureId = x.procedureId,
                            priority = x.priority,
                            listStep = x.Procedure.ProcedureSteps.Select(x => new StepProcedureModel
                            {
                                stepId = x.stepId,
                                priority=x.priority,
                            }).ToList(),
                        }).ToList(),
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItem.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.Item.Where(x => x.id == id && x.isDeleted != true)
                    .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure).ThenInclude(x => x.ProcedureSteps)     
                    .Include(x => x.ItemCategory).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin sản phẩm!";
                }
                else
                {
                    var item = new ItemModel
                    {
                        id = check.id,
                        itemCategoryId = check.itemCategoryId,
                        itemCategoryName = check.ItemCategory?.name ?? "",
                        code = check.code,
                        name = check.name,
                        image = check.image,
                        length = check.length,
                        depth = check.depth,
                        height = check.height,
                        unit = check.unit,
                        mass = check.mass,
                        drawingsTechnical = check.drawingsTechnical,
                        drawings2D = check.drawings2D,
                        drawings3D = check.drawings3D,
                        description = check.description,
                        price = check.price,

                        listMaterial = check.ItemMaterials.Select(x => new ItemMaterialModel
                        {
                            materialId = x.materialId,
                            quantity = x.quantity,
                        }).ToList(),

                        listProcedure = check.ProcedureItems.Select(x => new ProcedureItemModel
                        {
                            procedureId = x.procedureId,
                            priority = x.priority,
                            listStep = x.Procedure.ProcedureSteps.Select(x => new StepProcedureModel
                            {
                                stepId = x.stepId,
                                priority = x.priority,
                            }).ToList(),
                        }).ToList(),
                    };
                    result.Data = item;
                    result.Succeed = true;
                };                                 
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByItemCategoryId(Guid itemCategoryId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            var check = _dbContext.ItemCategory.Where(x => x.id == itemCategoryId && x.isDeleted != true).FirstOrDefault();

            if (check == null)
            {
                result.Code = 33;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin loại sản phẩm!";
            }
            else
            {
                try
                {
                    var listItem = _dbContext.Item.Where(x => x.itemCategoryId == itemCategoryId && x.isDeleted != true)
                        .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure).ThenInclude(x => x.ProcedureSteps)
                        .Include(x => x.ItemMaterials).ThenInclude(x => x.Material).OrderBy(x => x.name).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        search = FnUtil.Remove_VN_Accents(search).ToUpper();
                        listItem = listItem.Where(x => FnUtil.Remove_VN_Accents(x.name).ToUpper().Contains(search) || FnUtil.Remove_VN_Accents(x.code).ToUpper().Contains(search)).ToList();
                    }
                    var listItemPaging = listItem.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<ItemModel>();
                    foreach (var item in listItemPaging)
                    {
                        var tmp = new ItemModel
                        {
                            itemCategoryId = item.itemCategoryId,
                            id = item.id,
                            code = item.code,
                            name = item.name,
                            image = item.image,
                            length = item.length,
                            depth = item.depth,
                            height = item.height,
                            unit = item.unit,
                            mass = item.mass,
                            drawingsTechnical = item.drawingsTechnical,
                            drawings2D = item.drawings2D,
                            drawings3D = item.drawings3D,
                            description = item.description,
                            price = item.price,
                            listMaterial = item.ItemMaterials.Select(x => new ItemMaterialModel
                            {
                                materialId = x.materialId,
                                quantity = x.quantity,
                            }).ToList(),

                            listProcedure = item.ProcedureItems.Select(x => new ProcedureItemModel
                            {
                                procedureId = x.procedureId,
                                priority = x.priority,
                                listStep = x.Procedure.ProcedureSteps.Select(x => new StepProcedureModel
                                {
                                    stepId = x.stepId,
                                    priority = x.priority,
                                }).ToList(),
                            }).ToList(),
                        };
                        list.Add(tmp);
                    }
                    result.Data = new PagingModel()
                    {
                        Data = list,
                        Total = listItem.Count
                    };
                    result.Succeed = true;

                }
                catch (Exception e)
                {
                    result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                }
            }   
            return result;
        }

        public ResultModel GetAllLogOnItem(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listLog = _dbContext.Log.Include(x => x.Item).Include(x=>x.User).Where(x => x.itemId != null).OrderByDescending(x => x.modifiedTime).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    search = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listLog = listLog.Where(x => FnUtil.Remove_VN_Accents(x.action).ToUpper().Contains(search)).ToList();
                }

                var listLogPaging = listLog.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<LogModel>();
                foreach (var item in listLogPaging)
                {
                    var tmp = new LogModel
                    {
                        id = item.id,
                        itemId = item.materialId,
                        itemName = item.Item?.name,
                        userId = item.userId,
                        userName = item.User?.fullName,
                        modifiedTime = item.modifiedTime,
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

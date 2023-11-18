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

        public ResultModel Create(CreateItemModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkCategory = _dbContext.ItemCategory.Where(x => x.id == model.itemCategoryId && x.isDeleted != true).SingleOrDefault();
                if (checkCategory == null)
                {
                    result.Code = 33;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại mặt hàng !";

                }
                else
                {
                    bool hasDuplicates = model.listProcedure.GroupBy(x => x.priority).Any(g => g.Count() > 1);
                    if (hasDuplicates)
                    {
                        result.Code = 15;
                        result.Succeed = false;
                        result.ErrorMessage = "Mức độ ưu tiên của các quy trình không được trùng nhau !";
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
                                    totalPrice = material.quantity * _material.price
                                });
                                item.price += material.quantity * _material.price;
                            }
                        }

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = item.id;
                    }
                }             
            }
            
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateItemModel model)
        {
            ResultModel result = new ResultModel();
            try
            {   
                var check =  _dbContext.Item.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();
                
                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin mặt hàng!";
                }
                else
                {
                    bool hasDuplicates = model.listProcedure.GroupBy(x => x.priority).Any(g => g.Count() > 1);
                    if (hasDuplicates)
                    {
                        result.Code = 15;
                        result.Succeed = false;
                        result.ErrorMessage = "Mức độ ưu tiên của các quy trình không được trùng nhau !";
                    }
                    else
                    {
                        var checkCategory = _dbContext.ItemCategory.Where(x => x.id == model.itemCategoryId && x.isDeleted != true).SingleOrDefault();
                        if (checkCategory == null)
                        {
                            result.Code = 33;
                            result.Succeed = false;
                            result.ErrorMessage = "Không tìm thấy thông tin loại mặt hàng !";

                        }
                        else
                        {
                            if (string.IsNullOrEmpty(model.image))
                            {
                                model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                            }

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
                                .Where(x => x.itemId == model.id)
                                .ToList();
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
                                .Where(x => x.itemId == model.id)
                                .ToList();
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
                                        totalPrice = material.quantity * _material!.price
                                    });
                                    check.price += material.quantity * _material.price;
                                }
                            }

                            _dbContext.ProcedureItem.AddRange(procedureItems);
                            _dbContext.ItemMaterial.AddRange(materialItems);

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

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Item.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();
                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin mặt hàng!";
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

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var listItem =  _dbContext.Item.Where(x => x.isDeleted != true)
                    .Include(x => x.ItemCategory)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                    .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                   .OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listItem = listItem.Where(x => x.name.Contains(search)).ToList();
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
                        listMaterialId = item.ItemMaterials.Select(x => x.id).ToList(),
                        listProcedureId = item.ProcedureItems.Select(x => x.id).ToList()
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
                    .Include(x => x.ItemCategory)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                    .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                    .FirstOrDefault();

                if (check == null)
                {
                    result.Code = 34;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin mặt hàng!";
                }
                else
                {
                    var item = new ItemModel
                    {
                        id = check.id,
                        itemCategoryId = check.itemCategoryId,
                        itemCategoryName = check.ItemCategory?.name ?? "",
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
                        listMaterialId = check.ItemMaterials.Select(x => x.id).ToList(),
                        listProcedureId = check.ProcedureItems.Select(x => x.id).ToList(),
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
                result.ErrorMessage = "Không tìm thấy thông tin loại mặt hàng!";
            }
            else
            {
                try
                {
                    var listItem = _dbContext.Item.Where(x => x.itemCategoryId == itemCategoryId && x.isDeleted != true)
                        .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                        .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                       .OrderBy(x => x.name).ToList();
                    if (!string.IsNullOrEmpty(search))
                    {
                        listItem = listItem.Where(x => x.name.Contains(search)).ToList();
                    }
                    var listItemPaging = listItem.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<ItemModel>();
                    foreach (var item in listItemPaging)
                    {
                        var tmp = new ItemModel
                        {
                            itemCategoryId = item.itemCategoryId,
                            id = item.id,
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
                            listMaterialId = item.ItemMaterials.Select(x => x.id).ToList(),
                            listProcedureId = item.ProcedureItems.Select(x => x.id).ToList(),
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
    }
}

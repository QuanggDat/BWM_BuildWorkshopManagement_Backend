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

        public async Task<ResultModel> CreateItem(CreateItemModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkCategory = _dbContext.ItemCategory.Where(x => x.id == model.itemCategoryId && x.isDeleted != true).SingleOrDefault();
                if (checkCategory == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại mặt hàng !";

                }
                else
                {
                    if (string.IsNullOrEmpty(model.image))
                    {
                        model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fno_photo.jpg?alt=media&token=3dee5e48-234a-44a1-affa-92c8cc4de565&_gl=1*bxxcv*_ga*NzMzMjUwODQ2LjE2OTY2NTU2NjA.*_ga_CW55HF8NVT*MTY5ODIyMjgyNC40LjEuMTY5ODIyMzIzNy41Ny4wLjA&fbclid=IwAR0aZK4I3ay2MwA-5AyI-cqz5cGAMFcbwoAiMBHYe8TEim-UTtlbREbrCS0";
                    }

                    var listItem = _dbContext.Item.Where(x => !x.isDeleted).ToList();
                    var listItemCodeDB = listItem.Select(x => x.code).Distinct().ToList();
                    var randomCode = _utilsService.GenerateItemCode(listItemCodeDB, listItemCodeDB);

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
                        price = model.price,
                        isDeleted = false
                    };


                    _dbContext.Item.Add(item);

                    foreach (var procedure in model.procedures)
                    {
                        await _dbContext.ProcedureItem.AddAsync(new ProcedureItem
                        {
                            itemId = item.id,
                            procedureId = procedure
                        });
                    }

                    foreach (var material in model.materials)
                    {
                        await _dbContext.ItemMaterial.AddAsync(new ItemMaterial
                        {
                            itemId = item.id,
                            materialId = material
                        });
                    }

                    await _dbContext.SaveChangesAsync();
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

        public async Task<ResultModel> UpdateItem(UpdateItemModel model)
        {
            ResultModel result = new ResultModel();
            try
            {   
                var check = await _dbContext.Item.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefaultAsync();
                
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material!";
                }
                else
                {
                    var checkCategory = _dbContext.ItemCategory.Where(x => x.id == model.itemCategoryId && x.isDeleted != true).SingleOrDefault();
                    if (checkCategory == null)
                    {
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
                        check.price = model.price;

                        // Remove all old Procedure Item
                        var currentProcedureItems = await _dbContext.ProcedureItem
                            .Where(x => x.itemId == model.id)
                            .ToListAsync();
                        if (currentProcedureItems != null && currentProcedureItems.Count > 0)
                        {
                            _dbContext.ProcedureItem.RemoveRange(currentProcedureItems);
                        }

                        // Set new Procedure Item
                        var procedureItems = new List<ProcedureItem>();
                        foreach (var procedure in model.procedures)
                        {
                            procedureItems.Add(new ProcedureItem
                            {
                                itemId = model.id,
                                procedureId = procedure
                            });
                        }

                        // Remove all old Material Item
                        var currentMaterialItems = await _dbContext.ItemMaterial
                            .Where(x => x.itemId == model.id)
                            .ToListAsync();
                        if (currentMaterialItems != null && currentMaterialItems.Count > 0)
                        {
                            _dbContext.ItemMaterial.RemoveRange(currentMaterialItems);
                        }

                        // Set new Material Item
                        var materialItems = new List<ItemMaterial>();
                        foreach (var material in model.materials)
                        {
                            materialItems.Add(new ItemMaterial
                            {
                                itemId = model.id,
                                materialId = material
                            });
                        }

                        await _dbContext.ProcedureItem.AddRangeAsync(procedureItems);
                        await _dbContext.ItemMaterial.AddRangeAsync(materialItems);

                        await _dbContext.SaveChangesAsync();
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

        public async Task<ResultModel> DeleteItem(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = await _dbContext.Item.Where(x => x.id == id && x.isDeleted != true).FirstOrDefaultAsync();
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Item!";
                }
                else
                {
                    check.isDeleted = true;
                    await _dbContext.SaveChangesAsync();

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

        public async Task<ResultModel> GetAllItem(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            try
            {
                var listItem = await _dbContext.Item.Where(x => x.isDeleted != true)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                    .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                   .OrderByDescending(x => x.name).ToListAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    listItem = listItem.Where(x => x.name.Contains(search)).ToList();
                }

                var listItemPaging = listItem.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ItemModel>();
                foreach (var item in listItemPaging)
                {
                    var itemCategory = _dbContext.ItemCategory.Find(item.itemCategoryId);
                    var tmp = new ItemModel
                    {
                        id = item.id,
                        itemCategoryId = item.itemCategoryId,
                        itemCategoryName = itemCategory!.name,
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
                        Procedures = item.ProcedureItems.Select(_ => new _Procedure
                        {
                            procedureId = _.Procedure.id,
                            procedureName = _.Procedure.name,
                        }).ToList(),
                        Materials = item.ItemMaterials.Select(_ => new _Material
                        {
                            materialId = _.Material.id,
                            materialName = _.Material.name,
                        }).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listItemPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
        public async Task<ResultModel> GetItemById(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = await _dbContext.Item.Where(x => x.id == id && x.isDeleted != true)
                    .Include(x => x.ProcedureItems).ThenInclude(x => x.Procedure)
                    .Include(x => x.ItemMaterials).ThenInclude(x => x.Material)
                    .FirstOrDefaultAsync();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin Material!";
                }
                else
                {
                    var itemCategory = _dbContext.Users.Find(check.itemCategoryId);
                    var item = new ItemModel
                    {
                        id = check.id,
                        itemCategoryId = check.itemCategoryId,
                        itemCategoryName = itemCategory!.fullName,
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
                        Procedures = check.ProcedureItems.Select(_ => new _Procedure
                        {
                            procedureId = _.Procedure.id,
                            procedureName = _.Procedure.name,
                        }).ToList(),
                        Materials = check.ItemMaterials.Select(_ => new _Material
                        {
                            materialId = _.Material.id,
                            materialName = _.Material.name,
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
    }
}

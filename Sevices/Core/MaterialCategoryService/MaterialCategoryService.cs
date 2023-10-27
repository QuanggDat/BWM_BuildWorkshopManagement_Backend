using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.CategoryService
{
    public class MaterialCategoryService : IMaterialCategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public MaterialCategoryService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }     

        public ResultModel CreateMaterialCategory(Guid createById, CreateMaterialCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrWhiteSpace(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống !";                    
                }
                else
                {                
                    var checkExists = _dbContext.MaterialCategory.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                    if (checkExists != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên MaterialCategory này đã tồn tại !";                        
                    }
                    else
                    {
                        var newCategory = new MaterialCategory
                        {
                            createById = createById,
                            name = model.name,
                            isDeleted = false
                        };
                        _dbContext.MaterialCategory.Add(newCategory);
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = newCategory.id;
                    }
                }              
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
                var check = _dbContext.MaterialCategory.Where(x => x.id == model.id && x.isDeleted != true).SingleOrDefault();
                if(check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory !";
                    return result;
                }
                else
                {
                    //Validation
                    if (string.IsNullOrEmpty(model.name))
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên MaterialCategory không được để trống !";
                        return result;
                    }
                    else
                    {
                        if(model.name != check.name)
                        {
                            var checkExists = _dbContext.MaterialCategory.FirstOrDefault(x => x.name == model.name && !x.isDeleted);
                            if (checkExists != null)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Tên này đã tồn tại !";
                                return result;
                            }
                            else
                            {
                                check.name = model.name;
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = "Cập nhập thành công " + check.id;
                            }
                        }
                        else
                        {
                            check.name = model.name;
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

        public ResultModel GetAllMaterialCategory(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            
            try
            {
                var listMaterialCategory = _dbContext.MaterialCategory.Where(x => x.isDeleted != true)
                   .OrderByDescending(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listMaterialCategory = listMaterialCategory.Where(x => x.name.Contains(search)).ToList();
                }
                
                var listMaterialCategoryPaging = listMaterialCategory.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ResponeMaterialCategoryModel>();
                foreach (var item in listMaterialCategoryPaging)
                {
                    var createBy = _dbContext.Users.Find(item.createById);
                    var tmp = new ResponeMaterialCategoryModel
                    {
                        id = item.id,
                        createById = item.createById,
                        createByName = createBy!.fullName,
                        name = item.name,                    
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

        public ResultModel GetMaterialCategoryById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var check = _dbContext.MaterialCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory!";
                    return result;
                }
                else
                {
                    var createBy = _dbContext.Users.Find(check.createById);

                    var ResponeMaterialCategoryModel = new ResponeMaterialCategoryModel
                    {
                        id = check.id,
                        createById = check.createById,
                        createByName = createBy!.fullName,
                        name = check.name,
                    };

                    result.Data = ResponeMaterialCategoryModel;
                    result.Succeed = true;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }       

        public ResultModel DeleteMaterialCategory(Guid id)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.MaterialCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();
                
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin MaterialCategory!";
                    return result;
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
        
    }
}

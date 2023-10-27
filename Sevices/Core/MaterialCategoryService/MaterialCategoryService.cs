using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public ResultModel CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrEmpty(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên này không được để trống !";
                    return result;
                }
                else
                {
                    bool checkExists =  _dbContext.MaterialCategory.Any(x => x.name == model.name && x.isDeleted != true);
                    if (checkExists)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên MaterialCategory này đã tồn tại !";
                    }

                    //Create Category
                    else
                    {
                        var newCategory = new MaterialCategory
                        {
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
                var check = _dbContext.MaterialCategory.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();
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
                        result.ErrorMessage = "Tên MaterialCategory này không được để trống !";
                        return result;
                    }
                    else
                    {
                        bool checkExists =  _dbContext.MaterialCategory.Any(x => x.name == model.name && !x.isDeleted);
                        if (checkExists)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Tên này đã tồn tại !";
                        }
                        else
                        {
                            check.name = model.name;
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = _mapper.Map<Data.Entities.MaterialCategory, MaterialCategoryModel>(check);
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

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<MaterialCategoryModel>>(listMaterialCategoryPaging),
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
                    result.Data = _mapper.Map<MaterialCategoryModel>(check);
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

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

        public ResultModel CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                //Validation
                if (string.IsNullOrWhiteSpace(model.name))
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tên loại vật liệu không được để trống !";                    
                }
                else
                {                
                    var checkExists = _dbContext.MaterialCategory.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                    if (checkExists != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên loại vật liệu này đã tồn tại !";                        
                    }
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
                var check = _dbContext.MaterialCategory.Where(x => x.id == model.id && x.isDeleted != true).SingleOrDefault();
                if(check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu !";
                }
                else
                {
                    //Validation
                    if (string.IsNullOrEmpty(model.name))
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên loại vật liệu không được để trống !";
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
                            }
                            else
                            {
                                check.name = model.name;
                                _dbContext.SaveChanges();
                                result.Succeed = true;
                                result.Data = "Cập nhập thành công " + check.name;
                            }
                        }
                        else
                        {
                            check.name = model.name;
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

                var list = new List<MaterialCategoryModel>();
                foreach (var item in listMaterialCategoryPaging)
                {

                    var tmp = new MaterialCategoryModel
                    {
                        id = item.id,                       
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
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                }
                else
                {

                    var materialCategoryModel = new MaterialCategoryModel
                    {
                        id = check.id,
                        name = check.name,
                    };

                    result.Data = materialCategoryModel;
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
                var isExistedMaterial = _dbContext.Material.Any(x => x.materialCategoryId == id && x.isDeleted != true);
                if (isExistedMaterial)
                {
                    result.ErrorMessage = "Hãy xoá hết vật liệu trước khi xoá nhóm vật liệu ! ";
                }
                else
                {
                    var check = _dbContext.MaterialCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                    if (check == null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                    }
                    else
                    {
                        check.isDeleted = true;
                        _dbContext.SaveChanges();
                        result.Data = "Xoá thành công " + check.name;
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
    }
}

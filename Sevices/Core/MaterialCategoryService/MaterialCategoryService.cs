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

        public ResultModel Create(CreateMaterialCategoryModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;

            try
            {
                var checkExists = _dbContext.MaterialCategory.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);

                if (checkExists != null)
                {
                    result.Code = 24;
                    result.Succeed = false;
                    result.ErrorMessage = "Tên loại vật liệu đã tồn tại !";
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
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }        

        public ResultModel Update(UpdateMaterialCategoryModel model)
        {
            ResultModel result = new ResultModel();

            try
            {
                var check = _dbContext.MaterialCategory.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();

                if(check == null)
                {
                    result.Code = 25;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu !";
                }
                else
                {
                    var checkExists = _dbContext.MaterialCategory.FirstOrDefault(x => x.name == model.name && x.name != check.name && !x.isDeleted);

                    if (checkExists != null)
                    {
                        result.Code = 24;
                        result.Succeed = false;
                        result.ErrorMessage = "Tên loại vật liệu đã tồn tại!";
                    }
                    else
                    {
                        check.name = model.name;
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

        public ResultModel GetAll(string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();
            
            try
            {
                var listMaterialCategory = _dbContext.MaterialCategory.Include(x => x.Materials)
                    .Where(x => x.isDeleted != true).OrderBy(x => x.name).ToList();

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
                        quantityMaterial = item.Materials.Count,
                    };
                    list.Add(tmp);
                } 
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listMaterialCategory.Count
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
            result.Succeed = false;

            try
            {
                var check = _dbContext.MaterialCategory.Include(x => x.Materials)
                    .Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 25;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                }
                else
                {
                    var materialCategoryModel = new MaterialCategoryModel
                    {
                        id = check.id,
                        name = check.name,
                        quantityMaterial = check.Materials.Count,
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

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var isExistedMaterial = _dbContext.Material.Any(x => x.materialCategoryId == id && x.isDeleted != true);

                if (isExistedMaterial)
                {
                    result.Code = 26;
                    result.Succeed = false;
                    result.ErrorMessage = "Hãy xoá hết vật liệu trước khi xoá loại vật liệu ! ";
                }
                else
                {
                    var check = _dbContext.MaterialCategory.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                    if (check == null)
                    {
                        result.Code = 25;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin loại vật liệu!";
                    }
                    else
                    {
                        check.isDeleted = true;
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
    }
}

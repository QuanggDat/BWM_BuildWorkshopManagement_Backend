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
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CategoryService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }     

        public async Task<ResultModel> CreateMaterialCategory(CreateMaterialCategoryModel model)
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
                    bool nameExists = await _dbContext.MaterialCategory.AnyAsync(x => x.name == model.name && !x.isDeleted);
                    if (nameExists)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên này đã tồn tại !";
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
                        await _dbContext.SaveChangesAsync();
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
                var data = _dbContext.MaterialCategory.Where(x => x.id == model.id).FirstOrDefault();
                if (data != null)
                {
                    //Validation
                    if (string.IsNullOrEmpty(model.name))
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Tên này không được để trống.";
                        return result;
                    }

                    //Update Material Category
                    else
                    {
                        data.name = model.name;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Data.Entities.MaterialCategory, MaterialCategoryModel>(data);
                    }
                }
                else
                {
                    result.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
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
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.MaterialCategory.Where(c => c.id == id && c.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<MaterialCategoryModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }       

        public ResultModel DeleteMaterialCategory(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.MaterialCategory.Where(c => c.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<MaterialCategory, MaterialCategoryModel>(data);
                    resultModel.Data = view;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "MaterialCategory" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }
        
    }
}

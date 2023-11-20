using Data.DataAccess;
using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.StepService
{
    public class StepService : IStepService
    {
        private readonly AppDbContext _dbContext;

        public StepService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ResultModel Create(CreateStepModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkExists = _dbContext.Step.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                if (checkExists != null)
                {
                    result.Code = 72;
                    result.Succeed = false;
                    result.ErrorMessage = "Tên bước này đã tồn tại !";
                }
                else
                {
                    var newStep = new Step
                    {
                        name = model.name,
                        isDeleted = false
                    };

                    _dbContext.Step.Add(newStep);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = newStep.id;
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
                var check = _dbContext.Step.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 73;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin bước!";
                }
                else
                {
                    check.isDeleted = true;
                    _dbContext.SaveChanges();
                    result.Data = check.id;
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
                var listStep = _dbContext.Step.Where(x => x.isDeleted != true)
                   .OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listStep = listStep.Where(x => x.name.Contains(search)).ToList();
                }

                var listStepPaging = listStep.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<StepModel>();
                foreach (var item in listStepPaging)
                {

                    var tmp = new StepModel
                    {
                        id = item.id,
                        name = item.name,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listStep.Count
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
                var check = _dbContext.Step.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 73;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin bước!";
                }
                else
                {

                    var stepModel = new StepModel
                    {
                        id = check.id,
                        name = check.name,
                    };

                    result.Data = stepModel;
                    result.Succeed = true;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateStepModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Step.Where(x => x.id == model.id && x.isDeleted != true).SingleOrDefault();
                if (check == null)
                {
                    result.Code = 73;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin bước !";
                }
                else
                {
                    var checkExists = _dbContext.Step.FirstOrDefault(x => x.name == model.name && x.name != check.name && !x.isDeleted);
                    if (checkExists != null)
                    {
                        result.Code = 72;
                        result.Succeed = false;
                        result.ErrorMessage = "Tên bước này đã tồn tại !";
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
    }
}

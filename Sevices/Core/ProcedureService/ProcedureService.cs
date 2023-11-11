using Data.DataAccess;
using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ProcedureService
{
    public class ProcedureService : IProcedureService
    {
        private readonly AppDbContext _dbContext;

        public ProcedureService(AppDbContext dbContext)
        {
            _dbContext = dbContext;         
        }
        public ResultModel Create(CreateProcedureModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var checkExists = _dbContext.Procedure.FirstOrDefault(x => x.name == model.name && x.isDeleted == false);
                if (checkExists != null)
                {
                    result.Code = 70;
                    result.Succeed = false;
                    result.ErrorMessage = "Tên quy trình này đã tồn tại !";
                }
                else
                {
                    var newProcedure = new Procedure
                    {
                        name = model.name,
                        isDeleted = false
                    };

                    _dbContext.Procedure.Add(newProcedure);
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = newProcedure.id;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateProcedureModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var check = _dbContext.Procedure.Where(x => x.id == model.id && x.isDeleted != true).SingleOrDefault();
                if (check == null)
                {
                    result.Code = 71;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin quy trình !";
                }
                else
                {
                    if (model.name != check.name)
                    {
                        var checkExists = _dbContext.Procedure.FirstOrDefault(x => x.name == model.name && !x.isDeleted);
                        if (checkExists != null)
                        {
                            result.Code = 70;
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
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var check = _dbContext.Procedure.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 71;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin quy trình!";
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
                var listProcedure = _dbContext.Procedure.Where(x => x.isDeleted != true)
                   .OrderBy(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listProcedure = listProcedure.Where(x => x.name.Contains(search)).ToList();
                }

                var listProcedurePaging = listProcedure.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<ProcedureModel>();
                foreach (var item in listProcedurePaging)
                {

                    var tmp = new ProcedureModel
                    {
                        id = item.id,
                        name = item.name,
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listProcedure.Count
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
                var check = _dbContext.Procedure.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

                if (check == null)
                {
                    result.Code = 71;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin quy trình!";
                }
                else
                {

                    var procedureModel = new ProcedureModel
                    {
                        id = check.id,
                        name = check.name,
                    };

                    result.Data = procedureModel;
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

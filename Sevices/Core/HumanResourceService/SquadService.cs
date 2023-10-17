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

namespace Sevices.Core.HumanResourceService
{
     public class SquadService : ISquadService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SquadService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        //For factory role to see all squad in the factory.
        public ResultModel GetAllSquad(int pageIndex, int pageSize)
        {

            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Squad.Where(s => s.isDeleted != true).OrderByDescending(i => i.name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<SquadModel>>(data),
                    Total = data.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllUserBySquadId(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.squadId == id && s.banStatus != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<UserModel>(data).OrderByDescending(s=>s.fullName).ToList();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Squad" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        //Factory will create new squad 
        public async Task<ResultModel> CreateSquad(CreateSquadModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var nameExists =await _dbContext.Squad.AnyAsync(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tổ tên này đã tồn tại.";
                }
                else
                {
                    var newSquad = new Squad
                    {
                        name = model.name,
                        member = 0,
                        isDeleted = false
                    };
                    _dbContext.Squad.Add(newSquad);
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = newSquad.id;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> UpdateSquad(UpdateSquadModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = _dbContext.Squad.Where(s => s.id == model.id).FirstOrDefault();
                bool nameExists = await _dbContext.Squad.AnyAsync(s => s.name == model.name && !s.isDeleted);
                if (nameExists)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Tổ tên này đã tồn tại.";
                }
                else
                {
                    if (data != null)
                    {
                        data.name = model.name;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Squad, SquadModel>(data);
                    }
                    else
                    {
                        result.ErrorMessage = "Squad" + ErrorMessage.ID_NOT_EXISTED;
                        result.Succeed = false;
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel AddManagerToSquad(WorkerToSquad model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Include(r=>r.Role).Where(i => i.Id == model.id).FirstOrDefault();
                var squad = _dbContext.Squad.SingleOrDefault(g => g.id == model.squadId);
                if (data != null && squad != null)
                {
                    if(data.Role !=null && data.Role.Name == "Manager")
                    {
                        //Update GroupId
                        data.squadId = model.squadId;
                        squad.member += 1;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<User, UserModel>(data);
                        result.Data = _mapper.Map<Squad, SquadModel>(squad);
                    }
                    else
                    {
                        result.ErrorMessage = "User does not has valid Role!!!!";
                        result.Succeed = false;
                    }
                }
                else
                {
                    result.ErrorMessage = "User" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Fac and Manager can both use this function.
        public ResultModel AddWorkerToSquad(WorkerToSquad model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                var squad = _dbContext.Squad.SingleOrDefault(g => g.id == model.squadId);
                if (data != null && squad !=null)
                {
                    //Update GroupId
                    data.squadId = model.squadId;
                    squad.member+=1;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                    result.Data=_mapper.Map<Squad, SquadModel>(squad);
                }
                else
                {
                    result.ErrorMessage = "User" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel RemoveWorkerFromSquad(WorkerToSquad model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                var squad = _dbContext.Squad.SingleOrDefault(g => g.id == model.squadId);
                if (data != null && squad != null)
                {
                    //Update GroupId
                    data.squadId = null;
                    squad.member -= 1;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                    result.Data = _mapper.Map<Squad, SquadModel>(squad);
                }
                else
                {
                    result.ErrorMessage = "User" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Not sure about this yet
        public ResultModel DeleteSquad(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Squad.Where(s => s.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<Squad, SquadModel>(data);
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Squad" + ErrorMessage.ID_NOT_EXISTED;
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

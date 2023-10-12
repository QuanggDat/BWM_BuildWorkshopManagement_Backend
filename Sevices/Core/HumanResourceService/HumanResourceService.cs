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
    public class HumanResourceService : IHumanResourceService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public HumanResourceService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        //Factory will create new squad 
        public async Task<ResultModel> CreateSquad(CreateSquadModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                var nameExists = _dbContext.Squad.Select(s => s.name == model.name && !s.isDeleted).Distinct().ToList();
                if (nameExists.Any())
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

        //Factory and Manager can both use this 
        public async Task<ResultModel> CreateGroup(CreateGroupModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                    var newGroup = new Group
                    {
                        name = model.name,
                        member = 0,
                        squadId = model.squadId,
                        isDeleted = false
                    };
                    _dbContext.Group.Add(newGroup);
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = newGroup.id;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        //Fac and Manager can both use this function.
        public ResultModel AddWorkerToSquad(AddWorkerToSquad model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                    var squad = _dbContext.Squad.SingleOrDefault(g => g.id == data.squadId);
                    if (data != null)
                    {
                        //Update GroupId
                        data.squadId = model.squadId;
                        //squad.member++;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<User, UserModel>(data);
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

        //Need check again not complete. Fac and Manager can both use this function.
        public ResultModel AddWorkerToGroup(AddWorkerToGroup model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                if (data.squadId != null)
                {
                    //var group = _dbContext.Group.SingleOrDefault(g => g.id == model.groupId);
                    //var squad = _dbContext.Squad.SingleOrDefault(g => g.id == group.squadId);
                    if (data != null)
                    {
                        //Update GroupId
                        data.groupId = model.groupId;
                        //group.member++;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<User, UserModel>(data);
                    }
                    else
                    {
                        result.ErrorMessage = "User" + ErrorMessage.ID_NOT_EXISTED;
                        result.Succeed = false;
                    }
                }
                else
                {
                    result.ErrorMessage = "Thành viên này chưa nằm trong squad.";
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
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

        //For Factory role
        public async Task<List<HumanResources>> GetAllUser()
        {
            var result = new List<HumanResources>();
            var data = await _dbContext.User.Where(u => u.banStatus != false).ToListAsync();
            if (data == null)
            {
                return null;
            }

            foreach (var info in data)
            {
                var role = await _dbContext.Role.FindAsync(info.roleID);
                var squad = await _dbContext.Squad.FindAsync(info.squadId);
                var group = await _dbContext.Group.FindAsync(info.groupId);
                if (role != null && squad != null && group != null)
                {
                    var stuff = new HumanResources
                    {
                        fullName = info.fullName,
                        image = info.image,
                        roleName = role.Name,
                        squadName = squad.name,
                        groupName = group.name,
                        banStatus = info.banStatus,
                    };
                    result.Add(stuff);
                }
            }
            return result;
        }

        //public ResultModel GetAllUserBySquadId(Guid id)
        //{

        //    ResultModel result = new ResultModel();
        //    try
        //    {
        //        var data = _dbContext.User.Where(s => s.banStatus != true && s.squadId==id).OrderByDescending(s => s.fullName);
        //        var view = _mapper.ProjectTo<UserModel>(data);
        //        result.Data = view;
        //        result.Succeed = true;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        //    }
        //    return result;
        //}

        //public ResultModel GetAllUserByGroupId(Guid id)
        //{

        //    ResultModel result = new ResultModel();
        //    try
        //    {
        //        var data = _dbContext.User.Where(s => s.banStatus != true && s.groupId == id).OrderByDescending(s => s.fullName);
        //        var view = _mapper.ProjectTo<UserModel>(data);
        //        result.Data = view;
        //        result.Succeed = true;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        //    }
        //    return result;
        //}

        //Need attention for Update Squad and Group
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

        public async Task<ResultModel> UpdateGroup(UpdateGroupModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = _dbContext.Group.Where(s => s.id == model.id).FirstOrDefault();
                    if (data != null)
                    {
                        data.name = model.name;
                        data.squadId = model.squadId;
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Group, GroupModel>(data);
                    }
                    else
                    {
                        result.ErrorMessage = "Group" + ErrorMessage.ID_NOT_EXISTED;
                        result.Succeed = false;
                    }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetSquadById(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Squad.Where(s => s.id == id && s.isDeleted != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<GroupModel>(data).FirstOrDefault();
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

        public ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Group.Where(g => g.squadId == id && g.isDeleted != true).OrderByDescending(g => g.name).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                if (data != null)
                {
                    resultModel.Data = new PagingModel()
                    {
                        Data = _mapper.Map<List<SquadModel>>(data),
                        Total = data.Count
                    };
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

        public ResultModel GetUserByGroupId(Guid id, int pageIndex, int pageSize)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(u => u.groupId == id && u.banStatus != true).OrderByDescending(g => g.fullName).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                if (data != null)
                {
                    resultModel.Data = new PagingModel()
                    {
                        Data = _mapper.Map<List<UserModel>>(data),
                        Total = data.Count
                    };
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "User" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
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

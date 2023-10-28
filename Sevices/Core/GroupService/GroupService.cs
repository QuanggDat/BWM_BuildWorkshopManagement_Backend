using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.HumanResourceService
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GroupService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public ResultModel GetGroupBySquadId(Guid id, int pageIndex, int pageSize)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Group.Where(g => g.squadId == id && g.isDeleted != true).OrderByDescending(g => g.name).ToList();
                var dataPaging = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                if (data != null)
                {
                    resultModel.Data = new PagingModel()
                    {
                        Data = _mapper.Map<List<SquadModel>>(dataPaging),
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

        public ResultModel GetAllUserByGroupId(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.groupId == id && s.banStatus != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<UserModel>(data).OrderByDescending(s => s.fullName).ToList();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Group" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
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

        public ResultModel UpdateGroup(UpdateGroupModel model)
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

        //Need check again not complete. Fac and Manager can both use this function.
        public ResultModel AddWorkerToGroup(AddWorkerToGroupModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                var group = _dbContext.Group.SingleOrDefault(g => g.id == model.groupId);
                if (data != null && group != null)
                {
                    //Update GroupId
                    data.groupId = model.groupId;
                    group.member+=1;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                    result.Data = _mapper.Map<Group, GroupModel>(group);
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

        public ResultModel RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(i => i.Id == model.id).FirstOrDefault();
                var group = _dbContext.Group.SingleOrDefault(g => g.id == model.groupId);
                if (data != null && group != null)
                {
                    //Update GroupId
                    data.groupId = model.groupId;
                    group.member -= 1;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                    result.Data = _mapper.Map<Group, GroupModel>(group);
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
        public ResultModel DeleteGroup(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.Group.Where(s => s.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.isDeleted = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<Group, GroupModel>(data);
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.ErrorMessage = "Group" + ErrorMessage.ID_NOT_EXISTED;
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

using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;

namespace Sevices.Core.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TeamService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetTeamByGroupId(Guid groupId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var listTeam = _dbContext.Team.Where(x =>x.groupId == groupId && x.isDeleted != true)
                   .OrderByDescending(x => x.name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listTeam = listTeam.Where(x => x.name.Contains(search)).ToList();
                }

                var listTeamPaging = listTeam.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TeamModel>();
                foreach (var item in listTeamPaging)
                {
                    var tmp = new TeamModel
                    {
                        id = item.id,
                        groupId = item.groupId,
                        name = item.name,
                        member = item.member
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTeamPaging.Count
                };
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllUserByTeamId(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Where(s => s.teamId == id && !s.banStatus).OrderByDescending(s => s.fullName).ToList();
                result.Data = _mapper.Map<List<UserModel>>(listUser);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        //Factory and Manager can both use this 
        public ResultModel CreateTeam(CreateTeamModel model)
        {
            var result = new ResultModel();
            try
            {
                var newTeam = new Team
                {
                    name = model.name,
                    member = model.listUserId.Count,
                    groupId = model.groupId,
                    isDeleted = false
                };
                _dbContext.Team.Add(newTeam);

                if (model.listUserId.Any())
                {
                    var listUser = _dbContext.User.Where(x => model.listUserId.Contains(x.Id)).ToList();
                    foreach (var user in listUser)
                    {
                        user.teamId = newTeam.id;
                    }
                    _dbContext.User.UpdateRange(listUser);
                }

                _dbContext.SaveChanges();
                result.Data = newTeam.id;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateTeam(UpdateTeamModel model)
        {
            var result = new ResultModel();
            try
            {
                var data = _dbContext.Team.FirstOrDefault(s => s.id == model.id);
                if (data != null)
                {
                    data.name = model.name;
                    data.groupId = model.groupId;
                    data.member = model.listUserId.Count;
                    _dbContext.Team.Update(data);

                    var listUser = _dbContext.User.Where(x => x.teamId == data.id || model.listUserId.Contains(x.Id)).ToList();
                    foreach (var user in listUser)
                    {
                        if (model.listUserId.Contains(user.Id))
                        {
                            user.teamId = data.id;
                        }
                        else
                        {
                            user.teamId = null;
                        }
                    }
                    _dbContext.User.UpdateRange(listUser);

                    _dbContext.SaveChanges();
                    result.Data = _mapper.Map<TeamModel>(data);
                    result.Succeed = true;
                }
                else
                {
                    result.ErrorMessage = "Không tìm thấy nhóm!";
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
        public ResultModel AddWorkerToTeam(AddWorkerToTeamModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng!";
                }
                else
                {
                    var team = _dbContext.Team.FirstOrDefault(g => g.id == model.teamId);
                    if (team == null)
                    {
                        result.ErrorMessage = "Không tìm thấy nhóm!";
                    }
                    else
                    {
                        //Update TeamId
                        user.teamId = model.teamId;
                        _dbContext.User.Update(user);

                        team.member += 1;
                        _dbContext.Team.Update(team);

                        _dbContext.SaveChanges();
                        result.Data = _mapper.Map<TeamModel>(team);
                        result.Succeed = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel RemoveWorkerFromTeam(RemoveWorkerFromTeamModel model)
        {
            var result = new ResultModel();
            try
            {
                var user = _dbContext.User.FirstOrDefault(i => i.Id == model.id);
                if (user == null)
                {
                    result.ErrorMessage = "Không tìm thấy người dùng!";
                }
                else
                {
                    var team = _dbContext.Team.FirstOrDefault(g => g.id == model.teamId);
                    if (team == null)
                    {
                        result.ErrorMessage = "Không tìm thấy nhóm!";
                    }
                    else
                    {
                        //Update TeamId
                        user.teamId = null;
                        _dbContext.User.Update(user);

                        team.member -= 1;
                        _dbContext.Team.Update(team);

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<TeamModel>(team);
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        //Not sure about this yet
        public ResultModel DeleteTeam(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var isExistedUser = _dbContext.User.Any(x => x.teamId == id);
                if (isExistedUser)
                {
                    result.ErrorMessage = "Hãy xoá hết thành viên trước khi xoá nhóm!";
                }
                else
                {
                    var team = _dbContext.Team.FirstOrDefault(s => s.id == id);
                    if (team != null)
                    {
                        team.isDeleted = true;
                        _dbContext.Team.Update(team);
                        _dbContext.SaveChanges();

                        result.Data = _mapper.Map<TeamModel>(team);
                        result.Succeed = true;
                    }
                    else
                    {
                        result.ErrorMessage = "Không tìm thấy nhóm!";
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

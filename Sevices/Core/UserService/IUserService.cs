using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.UserService
{
    public interface IUserService
    {
        Task<ResultModel> CreateAdmin(UserCreateModel model);
        Task<ResultModel> CreateForeman(UserCreateModel model);
        Task<ResultModel> CreateLeader(UserCreateModel model);
        Task<ResultModel> CreateWorker(UserCreateModel model);
        Task<ResultModel> Login(LoginModel model);
        ResultModel Update(UserUpdateModel model);
        Task<ResultModel> ChangePassword(UserUpdatePasswordModel model);
        ResultModel UpdatePhone(UserUpdatePhoneModel model);
        ResultModel UpdateRole(UserUpdateUserRoleModel model);
        ResultModel GetAll();
        ResultModel GetByPhoneNumber(string phoneNumber);
        ResultModel GetByID(Guid id);
        ResultModel GetUserRole(Guid id);
        ResultModel BannedUser(Guid id);
        ResultModel UnBannedUser(Guid id);
        Task<List<ManagementUserModel>> GetAllUserForForeman();
    }
}

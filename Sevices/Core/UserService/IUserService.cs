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
        Task<ResultModel> CreateWoker(UserCreateModel model);
        Task<ResultModel> CreateFactory(UserCreateModel model);
        Task<ResultModel> Login(LoginModel model);
        ResultModel Update(UserUpdateModel model);
        Task<ResultModel> ChangePassword(UserUpdatePasswordModel model);
        ResultModel UpdatePhone(UserUpdatePhoneModel model);
        ResultModel GetAll();
        ResultModel GetByEmail(String email);
        ResultModel GetByID(Guid id);
        ResultModel GetUserRole(Guid id);
        ResultModel BannedUser(Guid id);
        ResultModel UnBannedUser(Guid id);
    }
}

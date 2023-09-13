using Data.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ResultModel> CreateUser(UserCreateModel model);
        Task<ResultModel> Login(LoginModel model);
        ResultModel Update(UserUpdateModel model);
        Task<ResultModel> ChangePassword(UserUpdatePasswordModel model);
        ResultModel UpdatePhone(UserUpdatePhoneModel model);
        ResultModel GetAll();
        ResultModel GetByEmail(String email);
        ResultModel GetByID(Guid Id);
        ResultModel GetUserRole(Guid id);
        ResultModel bannedUser(Guid id);
        ResultModel unBannedUser(Guid id);
    }
}

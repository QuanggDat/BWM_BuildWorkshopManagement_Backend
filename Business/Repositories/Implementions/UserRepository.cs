using Business.Repositories.Interfaces;
using Data.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.Implementions
{
    public class UserRepository : IUserRepository
    {
        public ResultModel bannedUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel> ChangePassword(UserUpdatePasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel> CreateUser(UserCreateModel model)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetAll()
        {
            throw new NotImplementedException();
        }

        public ResultModel GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetByID(Guid Id)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetUserRole(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel> Login(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public ResultModel unBannedUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public ResultModel Update(UserUpdateModel model)
        {
            throw new NotImplementedException();
        }

        public ResultModel UpdatePhone(UserUpdatePhoneModel model)
        {
            throw new NotImplementedException();
        }
    }
}

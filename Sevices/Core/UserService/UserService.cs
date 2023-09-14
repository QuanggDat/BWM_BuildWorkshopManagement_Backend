using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        public UserService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<ResultModel> CreateAdmin(UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new Role { Description = "Role for Admin", Name = "Admin" });
                }
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                var user = new User
                {
                    UserName = model.userName,
                    Email = model.email,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    address = model.address,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = true,
                    image = model.image,
                    roleID = role.Id
                };
                var userByPhone = _dbContext.User.Where(s => s.PhoneNumber == user.PhoneNumber).FirstOrDefault();
                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();
                if (userByPhone != null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Số Điện Thoại Đã Được Đăng Kí!";
                }
                else
                {
                    if (userByMail != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Email Đã Tồn Tại!";
                    }
                    else
                    {
                        if (user.PhoneNumber.Length < 9 || user.PhoneNumber.Length > 10)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Số Điện Thoại Không Hợp Lệ!";
                        }
                        else
                        {
                            var check = await _userManager.CreateAsync(user, model.password);

                            if (check != null)
                            {
                                var userRole = new UserRole
                                {
                                    RoleId = role.Id,
                                    UserId = user.Id
                                };
                                _dbContext.UserRoles.Add(userRole);
                                await _dbContext.SaveChangesAsync();
                                result.Succeed = true;
                                result.Data = user.Id;
                            }
                            else
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Validate user wrong ";
                            }
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;

        }
        public async Task<ResultModel> CreateWoker (UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                if (!await _roleManager.RoleExistsAsync("Woker"))
                {
                    await _roleManager.CreateAsync(new Role { Description = "Role for Woker", Name = "Woker" });
                }
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Woker");
                var user = new User
                {
                    UserName = model.userName,
                    Email = model.email,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    address = model.address,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = true,
                    image = model.image,
                    roleID = role.Id
                };
                var userByPhone = _dbContext.User.Where(s => s.PhoneNumber == user.PhoneNumber).FirstOrDefault();
                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();
                if (userByPhone != null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Số Điện Thoại Đã Được Đăng Kí!";
                }
                else
                {
                    if (userByMail != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Email Đã Tồn Tại!";
                    }
                    else
                    {
                        if (user.PhoneNumber.Length < 9 || user.PhoneNumber.Length > 10)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Số Điện Thoại Không Hợp Lệ!";
                        }
                        else
                        {
                            var check = await _userManager.CreateAsync(user, model.password);

                            if (check != null)
                            {
                                var userRole = new UserRole
                                {
                                    RoleId = role.Id,
                                    UserId = user.Id
                                };
                                _dbContext.UserRoles.Add(userRole);
                                await _dbContext.SaveChangesAsync();
                                result.Succeed = true;
                                result.Data = user.Id;
                            }
                            else
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Validate user wrong ";
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;

        }
        public async Task<ResultModel> CreateFactory(UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;
            try
            {
                if (!await _roleManager.RoleExistsAsync("Factory"))
                {
                    await _roleManager.CreateAsync(new Role { Description = "Role for Factory", Name = "Factory" });
                }
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Factory");
                var user = new User
                {
                    UserName = model.userName,
                    Email = model.email,
                    firstName = model.firstName,
                    lastName = model.lastName,
                    address = model.address,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = true,
                    image = model.image,
                    roleID = role.Id
                };
                var userByPhone = _dbContext.User.Where(s => s.PhoneNumber == user.PhoneNumber).FirstOrDefault();
                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();
                if (userByPhone != null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Số Điện Thoại Đã Được Đăng Kí!";
                }
                else
                {
                    if (userByMail != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Email Đã Tồn Tại!";
                    }
                    else
                    {
                        if (user.PhoneNumber.Length < 9 || user.PhoneNumber.Length > 10)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Số Điện Thoại Không Hợp Lệ!";
                        }
                        else
                        {
                            var check = await _userManager.CreateAsync(user, model.password);
                            if (check != null)
                            {
                                var userRole = new UserRole
                                {
                                    RoleId = role.Id,
                                    UserId = user.Id
                                };
                                _dbContext.UserRoles.Add(userRole);
                                await _dbContext.SaveChangesAsync();
                                result.Succeed = true;
                                result.Data = user.Id;
                            }
                            else
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Validate user wrong ";
                            }
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;

        }
        public async Task<ResultModel> Login(LoginModel model)
        {

            var result = new ResultModel();

            var userByEmail = _dbContext.User.Where(s => s.Email == model.email).FirstOrDefault();

            if (userByEmail != null)
            {
                var user = await _userManager.FindByNameAsync(userByEmail.UserName);
                var check = await _signInManager.CheckPasswordSignInAsync(user, model.password, false);
                if (!check.Succeeded)
                {
                    if (!user.EmailConfirmed)
                    {
                        result.Succeed = false;
                        //await SendMailConfirm(user);
                        result.ErrorMessage = "Email chưa được xác nhận. Vui lòng kiểm tra email để xác nhận!";
                    }

                    else
                    {
                        result.ErrorMessage = "Sai Mật Khẩu!";
                    }
                }
                else
                {
                    if (user.banStatus)
                    {
                        result.ErrorMessage = "Tài Khoản Đã Bị Khóa!";
                    }
                    else
                    {
                        var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                        var roles = new List<string>();
                        foreach (var userRole in userRoles)
                        {
                            var role = await _dbContext.Role.FindAsync(userRole.RoleId);
                            if (role != null) roles.Add(role.Name);
                        }
                        var token = GetAccessToken(user, roles);
                        result.Succeed = true;
                        result.Data = token;
                    }
                }
            }
            else
            {
                result.ErrorMessage = "Không tìm thấy số điện thoại!";
            }
            return result;
        }

        
        private async Task<Token> GetAccessToken(User user, List<string> role)
        {
            List<Claim> claims = GetClaims(user, role);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:ExpireTimes"])),
              //int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600
              signingCredentials: creds);

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token
            {
                Access_token = serializedToken,
                Token_type = "Bearer",
                Expires_in = int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600,
                userID = user.Id.ToString(),
                username = user.UserName,
                firstName = user.firstName,
                PhoneNumber = user.PhoneNumber,
                lastName = user.lastName,
                Role = user.Role
            };
        }
        private List<Claim> GetClaims(User user, List<string> roles)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("FullName", user.firstName),

                new Claim("UserName", user.UserName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber)) claims.Add(new Claim("PhoneNumber", user.PhoneNumber));
            return claims;
        }

        public ResultModel Update(UserUpdateModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Id == model.id).FirstOrDefault();
                //DateOnly dob = new DateOnly(model.dob.Year, model.dob.Month, model.dob.Day);
                if (data != null)
                {
                    data.firstName = model.firstName;
                    data.lastName = model.lastName;
                    data.address = model.address;
                    data.image = model.image;
                    data.dob = model.dob;
                    data.gender = model.gender;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.User, UserModel>(data);
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

        public async Task<ResultModel> ChangePassword(UserUpdatePasswordModel model)
        {
            ResultModel result = new ResultModel();

            try
            {
                var data = _dbContext.User.Where(s => s.Id == model.id).FirstOrDefault();

                //DateOnly dob = new DateOnly(model.dob.Year, model.dob.Month, model.dob.Day);
                if (data != null)
                {

                    var check = await _userManager.CheckPasswordAsync(data, model.oldPassword);
                    if (check != null)
                    {
                        var change = _userManager.ChangePasswordAsync(data, model.oldPassword, model.newPassword);
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = _mapper.Map<Data.Entities.User, UserModel>(data);
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

        public ResultModel UpdatePhone(UserUpdatePhoneModel model)
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Id == model.id).FirstOrDefault();
                //DateOnly dob = new DateOnly(model.dob.Year, model.dob.Month, model.dob.Day);
                if (data != null)
                {
                    data.PhoneNumber = model.phoneNumber;

                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.User, UserModel>(data);
                    result.ErrorMessage = "Cập Nhật Số Điện Thoại Thành Công!";
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

        public ResultModel GetAll()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.User;
                var view = _mapper.ProjectTo<UserModel>(data);
                result.Data = view;
                result.Succeed = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetByEmail(string email)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Email == email && !s.banStatus == true);
                if (data != null)
                {
                    var view = _mapper.ProjectTo<UserModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
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

        public ResultModel GetByID(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Id == id && s.banStatus != true);
                if (data != null)
                {

                    var view = _mapper.ProjectTo<UserModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
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

        public ResultModel GetUserRole(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var role = _dbContext.UserRoles.Where(s => s.UserId == id).FirstOrDefault();
                if (role != null)
                {
                    var data = _dbContext.Role.Where(s => s.Id == role.RoleId).FirstOrDefault();

                    if (data != null)
                    {
                        resultModel.Data = data;
                        resultModel.Succeed = true;
                    }
                    else
                    {
                        resultModel.ErrorMessage = "Role" + ErrorMessage.ID_NOT_EXISTED;
                        resultModel.Succeed = false;
                    }
                }
                else
                {
                    resultModel.ErrorMessage = "UserRole" + ErrorMessage.ID_NOT_EXISTED;
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel BannedUser(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Id == id).FirstOrDefault();
                if (data != null)
                {
                    data.banStatus = true;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<User, UserModel>(data);
                    resultModel.Data = view;
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

        public ResultModel UnBannedUser(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var data = _dbContext.User.Where(s => s.Id == id).FirstOrDefault();
                if (data != null)
                {
                    data.banStatus = false;
                    _dbContext.SaveChanges();
                    var view = _mapper.Map<User, UserModel>(data);
                    resultModel.Data = view;
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
    }
}

using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

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
                    await _roleManager.CreateAsync(new Role { description = "Role for Admin", Name = "Admin" });
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

                if (string.IsNullOrEmpty(model.image))                    
                {
                        model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fuser.jpg?alt=media&token=e87bcb1c-87d1-4969-bb32-bbe012cc4a7d&_gl=1%2A1a966xl%2A_ga%2ANzMzMjUwODQ2LjE2OTY2NTU2NjA.%2A_ga_CW55HF8NVT%2AMTY5ODIyMjgyNC40LjEuMTY5ODIyMzI4My4xMS4wLjA&fbclid=IwAR3ddevBAa093zzwoZcb5f5leF0RFyzOuOWeDQnVWtuK9jzDxyyklHoemnY";
                }

                var user = new User
                {
                    Email = model.email,
                    fullName = model.fullName,
                    address = model.address,
                    UserName = model.phoneNumber,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = model.gender,
                    image = model.image,
                    roleId = role.Id
                };

                var userByPhone = _dbContext.User.Where(s => s.UserName == user.UserName).FirstOrDefault();

                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();

                if (user.UserName.Length < 9 || user.UserName.Length > 10)
                {
                    result.Code = 1;
                    result.Succeed = false;
                    result.ErrorMessage = "Số điện thoại không hợp lệ!";
                }
                else
                {
                    if (!IsValidEmail(model.email))
                    {
                        result.Code = 2;
                        result.Succeed = false;
                        result.ErrorMessage = "Email không hợp lệ!";                        
                    }
                    else
                    {
                        if (userByPhone != null)
                        {
                            result.Code = 3;
                            result.Succeed = false;
                            result.ErrorMessage = "Số điện thoại này đã được đăng ký!";
                        }
                        else
                        {
                            if (userByMail != null)
                            {
                                result.Code = 4;
                                result.Succeed = false;
                                result.ErrorMessage = "Email đã tồn tạii!";
                            }
                            else
                            {
                                var ageDifference = DateTime.Now - model.dob;

                                int age = (int)(ageDifference.TotalDays / 365.25);// Tính tuổi xấp xỉ

                                if (age < 18 || age > 60)
                                {
                                    result.Code = 5;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Người này không trong độ tuổi lao động!";
                                    return result;
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
                                        result.Code = 6;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Xác thực sai!";
                                    }
                                }
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

        public async Task<ResultModel> CreateForeman(UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;

            try
            {
                if (!await _roleManager.RoleExistsAsync("Foreman"))
                {
                    await _roleManager.CreateAsync(new Role { description = "Role for Foreman", Name = "Foreman" });
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Foreman");

                if (string.IsNullOrEmpty(model.image))
                {
                    model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fuser.jpg?alt=media&token=e87bcb1c-87d1-4969-bb32-bbe012cc4a7d&_gl=1%2A1a966xl%2A_ga%2ANzMzMjUwODQ2LjE2OTY2NTU2NjA.%2A_ga_CW55HF8NVT%2AMTY5ODIyMjgyNC40LjEuMTY5ODIyMzI4My4xMS4wLjA&fbclid=IwAR3ddevBAa093zzwoZcb5f5leF0RFyzOuOWeDQnVWtuK9jzDxyyklHoemnY";
                }

                var user = new User
                {
                    Email = model.email,
                    fullName = model.fullName,
                    address = model.address,
                    UserName = model.phoneNumber,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = model.gender,
                    image = model.image,
                    roleId = role.Id
                };

                var userByPhone = _dbContext.User.Where(s => s.UserName == user.UserName).FirstOrDefault();

                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();

                if (user.UserName.Length < 9 || user.UserName.Length > 10)
                {
                    result.Code = 1;
                    result.Succeed = false;
                    result.ErrorMessage = "Số điện thoại không hợp lệ!";
                }
                else
                {
                    if (!IsValidEmail(model.email))
                    {
                        result.Code = 2;
                        result.Succeed = false;
                        result.ErrorMessage = "Email không hợp lệ!";                        
                    }
                    else
                    {
                        if (userByPhone != null)
                        {
                            result.Code = 3;
                            result.Succeed = false;
                            result.ErrorMessage = "Số điện thoại này đã được đăng ký!";
                        }
                        else
                        {
                            if (userByMail != null)
                            {
                                result.Code = 4;
                                result.Succeed = false;
                                result.ErrorMessage = "Email đã tồn tạii!";
                            }
                            else
                            {
                                var ageDifference = DateTime.Now - model.dob;

                                int age = (int)(ageDifference.TotalDays / 365.25);// Tính tuổi xấp xỉ

                                if (age < 18 || age > 60)
                                {
                                    result.Code = 5;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Người này không trong độ tuổi lao động!";
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
                                        result.Code = 6;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Xác thực sai!";
                                    }
                                }
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

        public async Task<ResultModel> CreateLeader(UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;

            try
            {
                if (!await _roleManager.RoleExistsAsync("Leader"))
                {
                    await _roleManager.CreateAsync(new Role { description = "Role for Leader", Name = "Leader" });
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Leader");

                if (string.IsNullOrEmpty(model.image))
                {
                    model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fuser.jpg?alt=media&token=e87bcb1c-87d1-4969-bb32-bbe012cc4a7d&_gl=1%2A1a966xl%2A_ga%2ANzMzMjUwODQ2LjE2OTY2NTU2NjA.%2A_ga_CW55HF8NVT%2AMTY5ODIyMjgyNC40LjEuMTY5ODIyMzI4My4xMS4wLjA&fbclid=IwAR3ddevBAa093zzwoZcb5f5leF0RFyzOuOWeDQnVWtuK9jzDxyyklHoemnY";
                }

                var user = new User
                {
                    Email = model.email,
                    fullName = model.fullName,
                    address = model.address,
                    UserName = model.phoneNumber,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = model.gender,
                    image = model.image,
                    roleId = role.Id
                };

                var userByPhone = _dbContext.User.Where(s => s.UserName == user.UserName).FirstOrDefault();

                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();

                if (user.UserName.Length < 9 || user.UserName.Length > 10)
                {
                    result.Code = 1;
                    result.Succeed = false;
                    result.ErrorMessage = "Số điện thoại không hợp lệ!";
                }
                else
                {
                    if (!IsValidEmail(model.email))
                    {
                        result.Code = 2;
                        result.Succeed = false;
                        result.ErrorMessage = "Email không hợp lệ!";
                    }
                    else
                    {
                        if (userByPhone != null)
                        {
                            result.Code = 3;
                            result.Succeed = false;
                            result.ErrorMessage = "Số điện thoại này đã được đăng ký!";
                        }
                        else
                        {
                            if (userByMail != null)
                            {
                                result.Code = 4;
                                result.Succeed = false;
                                result.ErrorMessage = "Email đã tồn tạii!";
                            }
                            else
                            {
                                var ageDifference = DateTime.Now - model.dob;

                                int age = (int)(ageDifference.TotalDays / 365.25);// Tính tuổi xấp xỉ

                                if (age < 18 || age > 60)
                                {
                                    result.Code = 5;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Người này không trong độ tuổi lao động!";
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
                                        result.Code = 6;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Xác thực sai!";
                                    }
                                }
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

        public async Task<ResultModel> CreateWorker(UserCreateModel model)
        {
            var result = new ResultModel();
            result.Succeed = false;            

            try
            {
                if (!await _roleManager.RoleExistsAsync("Worker"))
                {
                    await _roleManager.CreateAsync(new Role { description = "Role for Worker", Name = "Worker" });
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Worker");

                if (string.IsNullOrEmpty(model.image))
                {
                    model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fuser.jpg?alt=media&token=e87bcb1c-87d1-4969-bb32-bbe012cc4a7d&_gl=1%2A1a966xl%2A_ga%2ANzMzMjUwODQ2LjE2OTY2NTU2NjA.%2A_ga_CW55HF8NVT%2AMTY5ODIyMjgyNC40LjEuMTY5ODIyMzI4My4xMS4wLjA&fbclid=IwAR3ddevBAa093zzwoZcb5f5leF0RFyzOuOWeDQnVWtuK9jzDxyyklHoemnY";
                }

                var user = new User
                {
                    Email = model.email,
                    fullName = model.fullName,
                    address = model.address,
                    UserName = model.phoneNumber,
                    PhoneNumber = model.phoneNumber,
                    NormalizedEmail = model.email,
                    dob = model.dob,
                    banStatus = false,
                    gender = model.gender,
                    image = model.image,
                    roleId = role.Id
                };

                var userByPhone = _dbContext.User.Where(s => s.UserName == user.UserName).FirstOrDefault();

                var userByMail = _dbContext.User.Where(s => s.Email == user.Email).FirstOrDefault();

                if (user.UserName.Length < 9 || user.UserName.Length > 10)
                {
                    result.Code = 1;
                    result.Succeed = false;
                    result.ErrorMessage = "Số điện thoại không hợp lệ!";
                }
                else
                {
                    if (!IsValidEmail(model.email))
                    {
                        result.Code = 2;
                        result.Succeed = false;
                        result.ErrorMessage = "Email không hợp lệ!";
                    }
                    else
                    {
                        if (userByPhone != null)
                        {
                            result.Code = 3;
                            result.Succeed = false;
                            result.ErrorMessage = "Số điện thoại này đã được đăng ký!";
                        }
                        else
                        {
                            if (userByMail != null)
                            {
                                result.Code = 4;
                                result.Succeed = false;
                                result.ErrorMessage = "Email đã tồn tạii!";
                            }
                            else
                            {
                                var ageDifference = DateTime.Now - model.dob;
                                int age = (int)(ageDifference.TotalDays / 365.25);// Tính tuổi xấp xỉ

                                if (age < 18 || age > 60)
                                {
                                    result.Code = 5;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Người này không trong độ tuổi lao động!";
                                    return result;
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
                                        result.Code = 6;
                                        result.Succeed = false;
                                        result.ErrorMessage = "Xác thực sai!";
                                    }
                                }
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

        static bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true; // Nếu không có lỗi ngoại lệ, email là hợp lệ
            }
            catch
            {
                return false; // Nếu có lỗi ngoại lệ, email không hợp lệ
            }
        }

        public async Task<ResultModel> Login(LoginModel model)
        {

            var result = new ResultModel();
            result.Succeed = false;

            var userByPhone = _dbContext.User.Where(s => s.UserName == model.phoneNumber).FirstOrDefault();

            if (userByPhone != null)
            {
                var user = await _userManager.FindByNameAsync(userByPhone.UserName);

                var check = await _signInManager.CheckPasswordSignInAsync(user, model.password, false);

                if (!check.Succeeded)
                {
                    result.Code = 7;
                    result.Succeed = false;
                    result.ErrorMessage = "Sai mật khẩu!";
                }
                else
                {
                    if (user.banStatus)
                    {
                        result.Code = 8;
                        result.Succeed = false;
                        result.ErrorMessage = "Tài khoản đã bị khoá!";
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
                result.Code = 9;
                result.Succeed = false;
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
                userId = user.Id.ToString(),
                fullName = user.fullName,
                PhoneNumber = user.UserName,
                Role = user.Role
            };
        }

        private List<Claim> GetClaims(User user, List<string> roles)
        {
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("FullName", user.fullName),
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
            result.Succeed = false;
            try
            {
                var data = _dbContext.User.Where(s => s.Id == model.id).FirstOrDefault();
                
                if (data != null)
                {
                    if (string.IsNullOrEmpty(model.image))
                    {
                        model.image = "https://firebasestorage.googleapis.com/v0/b/capstonebwm.appspot.com/o/Picture%2Fuser.jpg?alt=media&token=e87bcb1c-87d1-4969-bb32-bbe012cc4a7d&_gl=1%2A1a966xl%2A_ga%2ANzMzMjUwODQ2LjE2OTY2NTU2NjA.%2A_ga_CW55HF8NVT%2AMTY5ODIyMjgyNC40LjEuMTY5ODIyMzI4My4xMS4wLjA&fbclid=IwAR3ddevBAa093zzwoZcb5f5leF0RFyzOuOWeDQnVWtuK9jzDxyyklHoemnY";
                    }
                    data.fullName = model.fullName;
                    data.address = model.address;
                    data.image = model.image;
                    data.dob = model.dob;
                    data.gender = model.gender;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                }
                else
                {
                    result.Code = 10;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
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
            result.Succeed = false;
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
                        result.Data = _mapper.Map<User, UserModel>(data);
                    }
                }
                else
                {
                    result.Code = 10;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public async Task<ResultModel> ResetPassword(ResetPasswordModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = _dbContext.User.Where(s => s.UserName == model.phoneNumber).FirstOrDefault();
                
                if(data == null)
                {
                    result.Code = 10;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    result.Succeed = false;
                }
                else
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(data);
                    var change = await _userManager.ResetPasswordAsync(data, resetToken, model.newPassword);

                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public async Task<ResultModel> ForgotPasswordByPhone(string phoneNumber)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var data = await _dbContext.User.Where(s => s.UserName == phoneNumber).FirstOrDefaultAsync();

                if (data == null)
                {
                    result.Code = 10;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    result.Succeed = false;
                }
                else
                {
                    var verificationCode = GenerateRandomNumber();

                    SendVerificationCode(phoneNumber, verificationCode);

                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<User, UserModel>(data);
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
            result.Succeed = false;

            try
            {
                var data = _dbContext.User.Where(s => s.Id == model.id).FirstOrDefault();
                //DateOnly dob = new DateOnly(model.dob.Year, model.dob.Month, model.dob.Day);

                if (data != null)
                {
                    data.PhoneNumber = model.phoneNumber;
                    data.UserName = model.phoneNumber;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = _mapper.Map<Data.Entities.User, UserModel>(data);
                    result.ErrorMessage = "Cập nhập số điênh thoại thành công!";
                }
                else
                {
                    result.Code = 10;
                    result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    result.Succeed = false;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel UpdateRole(UserUpdateUserRoleModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {               
                var checkRole = _dbContext.Role.Find(model.roleId);

                if (checkRole == null)
                {
                    result.Code = 11;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin vai trò người dùng !";
                    return result;
                }
                
                else
                {
                    var checkUser = _dbContext.User.Include(x => x.Role).FirstOrDefault(x => x.Id == model.userId);

                    if (checkUser == null)
                    {
                        result.Succeed = false;
                        result.Code = 10;
                        result.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                        return result;
                    }
                    else 
                    {
                        var checkWorkerInTask = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                        .FirstOrDefault(x => x.userId == model.userId && x.WorkerTask.status != EWorkerTaskStatus.Completed && x.WorkerTask.isDeleted == false);

                        if (checkUser.Role != null && checkUser.Role.Name == "Worker" && checkWorkerInTask != null)
                        {
                            result.Code = 81;
                            result.ErrorMessage = "Công nhân đang thực hiện công việc, hiện tại không thay đổi vai trò!";
                            result.Succeed = false;
                        }
                        else
                        {
                            var checkLeaderInTask = _dbContext.LeaderTask.FirstOrDefault(x => x.leaderId == model.userId && x.status != ETaskStatus.Completed && x.isDeleted == false);

                            if (checkUser.Role != null && checkUser.Role.Name == "Leader" && checkLeaderInTask != null)
                            {
                                result.Code = 83;
                                result.ErrorMessage = "Tổ trưởng đang thực hiện công việc, hiện tại không thay đổi vai trò!";
                                result.Succeed = false;
                            }
                            else
                            {
                                var listStatus = new List<OrderStatus>() {
                                OrderStatus.Pending,
                                OrderStatus.Request,
                                OrderStatus.Approve,
                                OrderStatus.InProgress
                                };

                                var checkForemanInOrder = _dbContext.Order.FirstOrDefault(x => x.assignToId == model.userId && listStatus.Contains(x.status));

                                if (checkUser.Role != null && checkUser.Role.Name == "Foreman" && checkForemanInOrder != null)
                                {
                                    result.Code = 84;
                                    result.ErrorMessage = "Quản đốc đang thực hiện đơn hàng, hiện tại không thể thay đổi vai trò!";
                                    result.Succeed = false;
                                }
                                else
                                {
                                    if (checkUser.Role != null && checkUser.Role.Name == "Admin")
                                    {
                                        result.Code = 85;
                                        result.ErrorMessage = "Không thể thay đổi vai trò của quản trị viên!";
                                        result.Succeed = false;
                                    }
                                    else
                                    {
                                        checkUser.roleId = model.roleId;

                                        // Remove all old UserRole
                                        var currentUserRole = _dbContext.UserRole
                                            .Where(x => x.UserId == model.userId)
                                            .ToList();
                                        if (currentUserRole != null && currentUserRole.Count > 0)
                                        {
                                            _dbContext.UserRole.RemoveRange(currentUserRole);
                                        }

                                        // Set new role
                                        var userRole = new UserRole
                                        {
                                            UserId = model.userId,
                                            RoleId = model.roleId
                                        };

                                        var currentUserInGroup = _dbContext.User.Include(x => x.Group).FirstOrDefault(x => x.Id == model.userId && x.groupId != null);

                                        if (currentUserInGroup != null)
                                        {
                                            currentUserInGroup.groupId = null;
                                            currentUserInGroup.Group = null;
                                            _dbContext.User.Update(currentUserInGroup);
                                        }

                                        _dbContext.UserRoles.Add(userRole);
                                        _dbContext.SaveChanges();
                                        result.Succeed = true;
                                        result.Data = model.userId;
                                    }
                                }                               
                            }                      
                        }                      
                    }                   
                }              
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetAllWithSearchAndPaging(int pageIndex, int pageSize, string? search = null)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false; 

            try
            {
                var listUser = _dbContext.User.Include(r => r.Role).Include(r => r.Group)
                   .OrderBy(x => x.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    var searchValue = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listUser = listUser.Where(x =>
                                              (!string.IsNullOrWhiteSpace(x.fullName) && FnUtil.Remove_VN_Accents(x.fullName).ToUpper().Contains(searchValue)) ||
                                               (!string.IsNullOrWhiteSpace(x.address) && FnUtil.Remove_VN_Accents(x.address).ToUpper().Contains(searchValue)) ||
                                                (!string.IsNullOrWhiteSpace(x.Email) && FnUtil.Remove_VN_Accents(x.Email).ToUpper().Contains(searchValue)) ||
                                                 (!string.IsNullOrWhiteSpace(x.UserName) && FnUtil.Remove_VN_Accents(x.UserName).ToUpper().Contains(searchValue)) ||
                                                   x.dob.ToString().Contains(searchValue) ||
                                                   (x.Role != null && !string.IsNullOrWhiteSpace(x.Role.Name) && FnUtil.Remove_VN_Accents(x.Role.Name).ToUpper().Contains(searchValue))).ToList();
                }

                var listUserPaging = listUser.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
               
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUserPaging),
                    Total = listUser.Count
                };
                result.Succeed = true;
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
            result.Succeed = false;

            try
            {
                var listUser = _dbContext.User.Include(r => r.Role).Include(r => r.Group).OrderBy(x => x.fullName).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUser),
                    Total = listUser.Count
                };
                result.Succeed = true;
            }

            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel GetByPhoneNumber(string phoneNumber)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.Succeed = false;

            try
            {
                var data = _dbContext.User.Where(s => s.UserName == phoneNumber && !s.banStatus == true);

                if (data != null)
                {
                    var view = _mapper.ProjectTo<UserModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.Code = 10;
                    resultModel.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel GetById(Guid id)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.Succeed = false;

            try
            {
                var data = _dbContext.User.Where(s => s.Id == id);

                if (data != null)
                {
                    var view = _mapper.ProjectTo<UserModel>(data).FirstOrDefault();
                    resultModel.Data = view!;
                    resultModel.Succeed = true;
                }
                else
                {
                    resultModel.Code = 10;
                    resultModel.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        public ResultModel GetByRoleId(Guid roleId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Include(r => r.Role).Include(r => r.Group)
                    .Where(x => x.roleId == roleId && !x.banStatus).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

                var listUserPaging = listUser.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUser),
                    Total = listUser.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByLeaderRole(string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listUser = _dbContext.User.Include(r => r.Role).Include(r => r.Group)
                    .Where(x => x.Role != null && x.Role.Name == "Leader" && !x.banStatus).OrderBy(s => s.fullName).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    listUser = listUser.Where(x => x.fullName.Contains(search)).ToList();
                }

                var listUserPaging = listUser.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<UserModel>>(listUser),
                    Total = listUser.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetRole()
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var role = _dbContext.Role.ToList();

                var list = new List<RoleModel>();
                foreach (var item in role)
                {

                    var tmp = new RoleModel
                    {
                        id = item.Id,
                        name = item.Name,
                    };
                    list.Add(tmp);
                }

                resultModel.Data = new PagingModel()
                {
                    Data = list,
                    Total = list.Count
                };                          
                resultModel.Succeed = true;

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
                var data = _dbContext.User.Include(x => x.Role).Where(s => s.Id == id).FirstOrDefault();

                if (data == null)
                {
                    resultModel.Code = 10;
                    resultModel.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    resultModel.Succeed = false;
                }
                else
                {
                    var checkWorkerInTask = _dbContext.WorkerTaskDetail.Include(x => x.WorkerTask)
                        .FirstOrDefault(x => x.userId == id && x.WorkerTask.status != EWorkerTaskStatus.Completed && x.WorkerTask.isDeleted == false);

                    if (data.Role != null && data.Role.Name == "Worker" && checkWorkerInTask != null)
                    {
                        resultModel.Code = 78;
                        resultModel.ErrorMessage = "Công nhân đang thực hiện công việc, hiện tại không thể khoá tài khoản!";
                        resultModel.Succeed = false;
                    }
                    else
                    {
                        var checkLeaderInTask = _dbContext.LeaderTask.FirstOrDefault(x => x.leaderId == id && x.status != ETaskStatus.Completed && x.isDeleted == false);

                        if (data.Role != null && data.Role.Name == "Leader" && checkLeaderInTask != null)
                        {
                            resultModel.Code = 79;
                            resultModel.ErrorMessage = "Tổ trưởng đang thực hiện công việc, hiện tại không thể khoá tài khoản!";
                            resultModel.Succeed = false;
                        }
                        else
                        {
                            var listStatus = new List<OrderStatus>() {
                            OrderStatus.Pending,
                            OrderStatus.Request,
                            OrderStatus.Approve,
                            OrderStatus.InProgress
                            };

                            var checkForemanInOrder = _dbContext.Order.FirstOrDefault(x => x.assignToId == id && listStatus.Contains(x.status));

                            if (data.Role != null && data.Role.Name == "Foreman" && checkForemanInOrder != null)
                            {
                                resultModel.Code = 80;
                                resultModel.ErrorMessage = "Quản đốc đang thực hiện đơn hàng, hiện tại không thể khoá tài khoản!";
                                resultModel.Succeed = false;
                            }
                            else
                            {
                                var currentUserInGroup = _dbContext.User.Include(x => x.Group).FirstOrDefault(x => x.Id == id && x.groupId != null);

                                if (currentUserInGroup != null)
                                {
                                    currentUserInGroup.groupId = null;
                                    currentUserInGroup.Group = null;
                                    _dbContext.User.Update(currentUserInGroup);
                                }

                                data.banStatus = true;
                                _dbContext.SaveChanges();
                                var view = _mapper.Map<User, UserModel>(data);
                                resultModel.Data = view;
                                resultModel.Succeed = true;
                            }
                        }
                    }                   
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
                    resultModel.Code = 10;
                    resultModel.ErrorMessage = "Không tìm thấy thông tin người dùng !";
                    resultModel.Succeed = false;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return resultModel;
        }

        #region SendVerificationCode
        private static Random random = new Random();
        public static string GenerateRandomNumber()
        {
            // Tạo chuỗi ngẫu nhiên bằng cách kết hợp 6 số ngẫu nhiên.
            string randomNumber = "";
            for (int i = 0; i < 6; i++)
            {
                randomNumber += random.Next(0, 10).ToString();
            }

            return randomNumber;
        }

        private string accountSid = "YOUR_TWILIO_ACCOUNT_SID";
        private string authToken = "YOUR_TWILIO_AUTH_TOKEN";
        private string twilioPhoneNumber = "YOUR_TWILIO_PHONE_NUMBER";

        private void SendVerificationCode(string phoneNumber, string verificationCode)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Mã xác thực của bạn là: {verificationCode}",
                from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );

            // Bạn cũng có thể xử lý kết quả gửi tin nhắn ở đây nếu cần thiết.
        }
        #endregion
    }
}

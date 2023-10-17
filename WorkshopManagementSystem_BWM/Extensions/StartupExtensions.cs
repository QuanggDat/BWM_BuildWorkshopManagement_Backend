using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sevices.Core.CategoryService;
using Sevices.Core.HumanResourceService;
using Sevices.Core.ItemService;
using Sevices.Core.ManagerTaskService;
using Sevices.Core.MaterialService;
using Sevices.Core.OrderDetailService;
using Sevices.Core.OrderReportService;
using Sevices.Core.OrderService;
using Sevices.Core.ReportService;
using Sevices.Core.SquadService;
using Sevices.Core.UserService;
using Sevices.Core.UtilsService;
using Sevices.Mapping;
using System.Text;
using System.Text.Json;

namespace WorkshopManagementSystem_BWM.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigIdentityService(this IServiceCollection services)
        {
            var build = services.AddIdentityCore<User>(option =>
            {
                option.SignIn.RequireConfirmedAccount = false;
                option.User.RequireUniqueEmail = false;
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;

                option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedAccount = false;
            }); build.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            build.AddSignInManager<SignInManager<User>>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddDefaultTokenProviders();
            services.AddAuthorization();


        }
        //AddScoped
        public static void AddBussinessService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>(); 
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IManagerTaskService, ManagerTaskService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<ISquadService, SquadService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITaskReportService, TaskReportService>();
            services.AddScoped<IUtilsService, UtilsService>();
            services.AddScoped<IOrderReportService, OrderReportService>();
        }
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        public static void AddJWTAuthentication(this IServiceCollection services, string key, string issuer)
        {
            services
                .AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtconfig =>
                {
                    jwtconfig.SaveToken = true;
                    jwtconfig.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateAudience = false,
                        ValidIssuer = issuer,
                        ValidateIssuer = true,
                        ValidateLifetime = false,
                        RequireAudience = false,
                    };
                    jwtconfig.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            // Ensure we always have an error and error description.
                            if (string.IsNullOrEmpty(context.Error))
                                context.Error = "invalid_token";
                            if (string.IsNullOrEmpty(context.ErrorDescription))
                                context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                            // Add some extra context for expired tokens.
                            if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                                context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                                context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                            }

                            return context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = context.Error,
                                error_description = context.ErrorDescription
                            }));
                        },
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/notificationHub"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            /*services.AddAuthentication().AddTwoFactorRememberMeCookie();
            services.AddAuthentication().AddTwoFactorUserIdCookie();*/
        }

        public static void AddSwaggerWithAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkshopManagementSystem_BWM_APP", Version = "1.0" }); opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                          Array.Empty<string>()
                    }
                    });
            });
        }
    }
}

using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using SignalRHubs.Hubs.CommentHub;
using SignalRHubs.Hubs.NotificationHub;
using WorkshopManagementSystem_BWM.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
});
builder.Services.AddAutoMapper();
builder.Services.ConfigIdentityService();

builder.Services.AddBussinessService();
builder.Services.AddJWTAuthentication(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"]);
builder.Services.AddSwaggerWithAuthentication();
#if DEBUG
    builder.Services.AddSignalR();
#else
    builder.Services.AddSignalR().AddAzureSignalR("Endpoint=https://workshopmanagementsystembwm.service.signalr.net;AccessKey=KxJdWvt/OftGZgb2XOOCDl1py1Esix6yx60YOjwlyW4=;Version=1.0;");
#endif

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true);
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//app cors
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();
app.UseStaticFiles();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<CommentHub>("/commentHub");

app.Run();

using AutoMapper.Execution;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.UtilsService;
using SignalRHubs.Extensions;
using SignalRHubs.Hubs.NotificationHub;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly INotificationHub _notiHub;

        public TestController(INotificationHub notiHub)
        {
            _notiHub = notiHub;
        }

        [HttpGet("SignalR")]
        public IActionResult TestSignalR()
        {
            var demoNoti = new NewNotificationModel()
            {
                CountUnseen = new Random().Next(0, 100),
                Notification = new()
                {
                    id = Guid.NewGuid(),
                    userId = Guid.NewGuid(),
                    title = FnUtils.GenerateCode(),
                    content  = FnUtils.GenerateCode(),
                    dateCreated = DateTime.Now,
                }
            };
            _notiHub.NewNotification(User.GetId().ToString(), demoNoti);
            return Ok(demoNoti);

        }
    }
}

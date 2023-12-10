using AutoMapper.Execution;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.UtilsService;
using SignalRHubs.Extensions;
using SignalRHubs.Hubs.CommentHub;
using SignalRHubs.Hubs.NotificationHub;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly INotificationHub _notiHub;
        private readonly ICommentHub _cmtHub;

        public TestController(INotificationHub notiHub, ICommentHub cmtHub)
        {
            _notiHub = notiHub;
            _cmtHub = cmtHub;
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
                    title = FnUtil.GenerateCode(),
                    content  = FnUtil.GenerateCode(),
                    createdDate = DateTime.Now,
                }
            };
            _notiHub.NewNotification(User.GetId().ToString(), demoNoti);
            return Ok(demoNoti);

        }
        [HttpGet("SignalRByUserId")]
        public IActionResult TestSignalRByUser(Guid userId)
        {
            var demoNoti = new NewNotificationModel()
            {
                CountUnseen = new Random().Next(0, 100),
                Notification = new()
                {
                    id = Guid.NewGuid(),
                    userId = Guid.NewGuid(),
                    title = FnUtil.GenerateCode(),
                    content  = FnUtil.GenerateCode(),
                    createdDate = DateTime.Now,
                }
            };
            _notiHub.NewNotification(userId.ToString(), demoNoti);
            return Ok(demoNoti);

        }

        [HttpGet("SignalR_Comment")]
        public IActionResult SignalR_Comment()
        {
            var model = new CommentModel()
            {
                id = Guid.NewGuid(),
                userId = Guid.NewGuid(),
                commentContent = FnUtil.GenerateCode(),
            };
            _cmtHub.ChangeComment(new() { Guid.Parse(User.GetId()) }, model); 
            return Ok(model);

        }
        [HttpGet("SignalRByUserId_Comment")]
        public IActionResult SignalRByUserId_Comment(Guid userId)
        {
            var model = new CommentModel()
            {
                id = Guid.NewGuid(),
                userId = Guid.NewGuid(),
                commentContent = FnUtil.GenerateCode(),
            };
            _cmtHub.ChangeComment(new() { userId }, model);
            return Ok(model);
        }
    }
}

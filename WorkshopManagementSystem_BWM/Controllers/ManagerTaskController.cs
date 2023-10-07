using Data.Entities;
using Data.Models;
using FitmarAgencyTemplate.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ManagerTaskService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerTaskController : Controller
    {
        private readonly IManagerTaskService _managerTaskService;

        public ManagerTaskController(IManagerTaskService managerTaskService)
        {
            _managerTaskService = managerTaskService;
        }

        [HttpPost("CreateTaskManager")]
        public async Task<ActionResult> CreateManagerTask(CreateManagerTaskModel model)
        {
            var userId = User.GetId();
            var result = await _managerTaskService.CreatedManagerTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        
        [HttpGet("{GetManagerTaskByOrderId}")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByOrderId(Guid orderId)
        {
            if (orderId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _managerTaskService.GetManagerTaskByOrderId(orderId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("{GetManagerTaskByManagerId}")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByManagerId()
        {
            var userId = User.GetId();         
            var result = await _managerTaskService.GetManagerTaskByOrderId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }
        [HttpGet("{GetManagerTaskByFactory}")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByFactory()
        {
            var userId = User.GetId();
            var result = await _managerTaskService.GetManagerTaskByOrderId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }
    }
}

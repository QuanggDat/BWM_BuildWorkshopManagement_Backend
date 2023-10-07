using Data.Entities;
using Data.Models;
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
            var result = await _managerTaskService.CreatedManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        
        [HttpGet("{orderId}")]
        public async Task<ActionResult<ManagerTask>> GetProjectByID(Guid orderId)
        {
            if (orderId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _managerTaskService.GetManagerTaskByOrderId(orderId);
            if (result == null) return BadRequest("Không tùm thấy Project!");
            return Ok(result);
        }
        
    }
}

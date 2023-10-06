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
    }
}

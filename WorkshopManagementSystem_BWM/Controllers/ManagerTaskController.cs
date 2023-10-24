using Data.Entities;
using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ManagerTaskService;
using System.Data.Common;

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

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateManagerTask(CreateManagerTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được id!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = await _managerTaskService.CreatedManagerTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        
        [HttpGet("[action]/{orderId}")]
        public async Task<ActionResult> GetManagerTaskByOrderId(Guid orderId)
        {
            var result = await _managerTaskService.GetManagerTaskByOrderId(orderId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetManagerTaskByManagerId()
        {
            var userId = User.GetId();         
            var result = await _managerTaskService.GetManagerTaskByManagerId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetManagerTaskByFactory()
        {
            var userId = User.GetId();
            var result = await _managerTaskService.GetManagerTaskByFactory(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }        

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateManagerTask(UpdateManagerTaskModel model)
        {            
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = await _managerTaskService.UpdateManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{managerTaskId}")]
        public async Task<ActionResult> UpdateTaskStatus(Guid managerTaskId, TaskStatus status)
        {
            var result = await _managerTaskService.UpdateManagerTaskStatus(managerTaskId, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("[action]/{managerTaskId}")]
        public async Task<ActionResult> DeleteTask(Guid managerTaskId)
        {
            var result = await _managerTaskService.DeleteManagerTask(managerTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{managerTaskId}/{groupId}")]
        public async Task<ActionResult> AssignManagerTask(Guid managerTaskId, Guid groupId)
        {
            var result = await _managerTaskService.AssignManagerTask(managerTaskId, groupId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }       
    }
}

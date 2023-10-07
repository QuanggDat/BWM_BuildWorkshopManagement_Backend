using Data.Entities;
using Data.Models;
using FitmarAgencyTemplate.Extensions;
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

        [HttpPost("CreateTaskManager")]
        public async Task<ActionResult> CreateManagerTask(CreateManagerTaskModel model)
        {
            var userId = User.GetId();
            var result = await _managerTaskService.CreatedManagerTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        
        [HttpGet("[action]/{orderId}")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByOrderId(Guid orderId)
        {
            if (orderId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _managerTaskService.GetManagerTaskByOrderId(orderId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("GetManagerTaskByManagerId")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByManagerId()
        {
            var userId = User.GetId();         
            var result = await _managerTaskService.GetManagerTaskByOrderId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }

        [HttpGet("GetManagerTaskByFactory")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByFactory()
        {
            var userId = User.GetId();
            var result = await _managerTaskService.GetManagerTaskByOrderId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }
        [HttpPut("{managerId}")]
        public async Task<ActionResult> UpdateTaskStatus(Guid managerTaskId, TaskStatus status)
        {
            var success = await _managerTaskService.UpdateManagerTaskStatus(managerTaskId, status);
            return Ok(success);
        }

        [HttpDelete("{managerId}")]
        public async Task<ActionResult> DeleteTask(Guid managerTaskId)
        {
            if (managerTaskId == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _managerTaskService.DeleteManagerTask(managerTaskId);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Không tìm thấy Manager Task.");
                else return Ok("Manager Task đã được xoá thành công.");
            }
            catch (DbException e)
            {
                return BadRequest("Không thành công.");
            }
        }
        [HttpPost("UpdateTaskManager")]
        public async Task<ActionResult> UpdateManagerTask(UpdateManagerTaskModel model)
        {
            var result = await _managerTaskService.UpdateManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

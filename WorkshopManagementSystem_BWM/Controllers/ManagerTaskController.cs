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
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được đơn hàng!");
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
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
            var result = await _managerTaskService.GetManagerTaskByManagerId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }

        [HttpGet("GetManagerTaskByFactory")]
        public async Task<ActionResult<ManagerTask>> GetManagerTaskByFactory()
        {
            var userId = User.GetId();
            var result = await _managerTaskService.GetManagerTaskByFactory(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }
        [HttpPut("{managerId}")]
        public async Task<ActionResult> UpdateTaskStatus(Guid managerTaskId, TaskStatus status)
        {
            var success = await _managerTaskService.UpdateManagerTaskStatus(managerTaskId, status);
            return Ok(success);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTask(Guid managerTaskId)
        {
            var result = await _managerTaskService.DeleteManagerTask(managerTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateTaskManager")]
        public async Task<ActionResult> UpdateManagerTask(UpdateManagerTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được đơn hàng!");
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = await _managerTaskService.UpdateManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AssignManagerTask")]
        public async Task<ActionResult> AssignManagerTask(AssignManagerTaskModel model)
        {
            if (model == null) return BadRequest("Không nhận được dữ liệu.");
            var result = await _managerTaskService.AssignManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UnAssignManagerTask")]
        public async Task<ActionResult> UnAssignManagerTask(AssignManagerTaskModel model)
        {
            if (model == null) return BadRequest("Không nhận được dữ liệu.");
            var result = await _managerTaskService.UnAssignManagerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

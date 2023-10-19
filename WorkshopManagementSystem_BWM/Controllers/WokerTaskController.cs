using FitmarAgencyTemplate.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.WokerTaskService;
using static Data.Models.WokerTaskModel;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WokerTaskController : ControllerBase
    {
        private readonly IWokerTaskService _wokerTaskService;

        public WokerTaskController(IWokerTaskService wokerTaskService)
        {
            _wokerTaskService = wokerTaskService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateWokerTask(CreateWokerTaskModel model)
        {
            if (model.managerTaskId == Guid.Empty) return BadRequest("Không nhận được managerTaskId!");
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = await _wokerTaskService.CreateWokerTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateWokerTask(UpdateWokerTaskModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = await _wokerTaskService.UpdateWokerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{wokerTaskId}/{status}")]
        public async Task<ActionResult> UpdateWokerTaskStatus(Guid wokerTaskId, TaskStatus status)
        {
            var result = await _wokerTaskService.UpdateWokerTaskStatus(wokerTaskId, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("[action]/{wokerTaskId}")]
        public async Task<ActionResult> DeleteWokerTask(Guid wokerTaskId)
        {
            var result = await _wokerTaskService.DeleteWokerTask(wokerTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> AssignWokerTask(AssignWokerTaskModel model)
        {
            var result = await _wokerTaskService.AssignWokerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UnAssignWokerTask(AssignWokerTaskModel model)
        {
            var result = await _wokerTaskService.UnAssignWokerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{managerTaskId}")]
        public async Task<ActionResult> GetAllWokerTask(Guid managerTaskId)
        {
            var result = await _wokerTaskService.GetAllWokerTask(managerTaskId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }
    }
}

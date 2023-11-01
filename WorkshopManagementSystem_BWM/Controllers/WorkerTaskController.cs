using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.WorkerTaskService;
using static Data.Models.WorkerTaskModel;
using Data.Models;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerTaskController : ControllerBase
    {
        private readonly IWorkerTaskService _workerTaskService;

        public WorkerTaskController(IWorkerTaskService workerTaskService)
        {
            _workerTaskService = workerTaskService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateWokerTask(CreateWorkerTaskModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = await _workerTaskService.CreateWorkerTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateWokerTask(UpdateWorkerTaskModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = await _workerTaskService.UpdateWorkerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{wokerTaskId}/{status}")]
        public async Task<ActionResult> UpdateWokerTaskStatus(Guid wokerTaskId, TaskStatus status)
        {
            var result = await _workerTaskService.UpdateWorkerTaskStatus(wokerTaskId, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{wokerTaskId}")]
        public async Task<ActionResult> DeleteWokerTask(Guid wokerTaskId)
        {
            var result = await _workerTaskService.DeleteWorkerTask(wokerTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> AssignWokerTask(AssignWorkerTaskModel model)
        {
            var result = await _workerTaskService.AssignWorkerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UnAssignWokerTask(AssignWorkerTaskModel model)
        {
            var result = await _workerTaskService.UnAssignWorkerTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public async Task<ActionResult> GetAllWokerTask(Guid leaderTaskId)
        {
            var result = await _workerTaskService.GetAllWorkerTask(leaderTaskId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }
    }
}

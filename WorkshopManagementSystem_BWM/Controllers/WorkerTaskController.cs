using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.WorkerTaskService;
using static Data.Models.WorkerTaskModel;
using Data.Models;
using Data.Enums;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class WorkerTaskController : ControllerBase
    {
        private readonly IWorkerTaskService _workerTaskService;

        public WorkerTaskController(IWorkerTaskService workerTaskService)
        {
            _workerTaskService = workerTaskService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateWorkerTaskModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            var userId = User.GetId();
            var result = _workerTaskService.Create(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _workerTaskService.GetByLeaderTaskId(leaderTaskId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{userId}")]
        public IActionResult GetByUserId(Guid userId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _workerTaskService.GetByUserId(userId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _workerTaskService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{workerTaskDetailId}")]
        public IActionResult GetWorkerTaskDetail(Guid workerTaskDetailId)
        {
            var result = _workerTaskService.GetWorkerTaskDetail(workerTaskDetailId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateWorkerTaskModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            var result = _workerTaskService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}/{status}")]
        public IActionResult UpdateStatus(Guid id, EWorkerTaskStatus status)
        {
            var result = _workerTaskService.UpdateStatus(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{workerTaskDetailId}/{status}")]
        public IActionResult UpdateStatusWorkerTaskDetail(Guid workerTaskDetailId, EWorkerTaskDetailsStatus status)
        {
            var result = _workerTaskService.UpdateStatusWorkerTaskDetail(workerTaskDetailId, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult SendFeedback(SendFeedbackModel model)
        {
            var result = _workerTaskService.SendFeedback(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Assign(AssignWorkerTaskModel model)
        {
            var result = _workerTaskService.Assign(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult UnAssign(AssignWorkerTaskModel model)
        {
            var result = _workerTaskService.UnAssign(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _workerTaskService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }       
    }
}

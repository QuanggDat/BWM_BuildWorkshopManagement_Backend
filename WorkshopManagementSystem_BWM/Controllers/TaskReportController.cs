using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ReportService;
using static Data.Models.TaskReportModel;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Data.Enums;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class TaskReportController : ControllerBase
    {
        private readonly ITaskReportService _reportService;

        public TaskReportController(ITaskReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("[action]")]
        public IActionResult CreateProgressReport(CreateProgressReportModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = _reportService.CreateProgressReport(userId,model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public IActionResult CreateProblemReport(CreateProblemReportModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = _reportService.CreateProblemReport(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public IActionResult CreateAcceptanceReport(CreateAcceptanceReportModel model)
        {
            if (model.acceptanceTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = _reportService.CreateAcceptanceReport(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById (Guid id)
        {
            var result = _reportService.GetById(id);
            if (result == null) return BadRequest("Không tìm thấy reportId");
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetProblemReportsByLeaderTaskId(Guid leaderTaskId,string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _reportService.GetProblemReportsByLeaderTaskId(leaderTaskId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetProgressReportsByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _reportService.GetProgressReportsByLeaderTaskId(leaderTaskId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetReportByLeaderId")]
        public IActionResult GetReportByLeaderId(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _reportService.GetReportByLeaderId(User.GetId(), search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _reportService.GetByLeaderTaskId(leaderTaskId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult SendProgressReportFeedback(SendProgressReportFeedbackModel model)
        {
            var result = _reportService.SendProgressReportFeedback(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult SendProblemReportFeedback(SendProblemReportFeedbackModel model)
        {
            var result = _reportService.SendProblemReportFeedback(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateTaskReportModel model)
        {
            var result = _reportService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult UpdateProblemTaskReport(UpdateProblemTaskReportModel model)
        {
            var result = _reportService.UpdateProblemTaskReport(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}/{status}")]
        public IActionResult UpdateStatusReport(Guid id, ReportStatus status)
        {
            var result = _reportService.UpdateStatusReport(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

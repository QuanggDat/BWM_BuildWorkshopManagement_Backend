using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ReportService;
using static Data.Models.TaskReportModel;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskReportController : ControllerBase
    {
        private readonly ITaskReportService _reportService;

        public TaskReportController(ITaskReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendReport(CreateTaskReportModel model)
        {
            if (model.managerTaskId == Guid.Empty) return BadRequest("Không nhận được managerTaskId!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = await _reportService.CreateTaskReport(userId,model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{reportId}")]
        public async Task<ActionResult> GetReportByReportId(Guid reportId)
        {
            var result = await _reportService.GetTaskReportById(reportId);
            if (result == null) return BadRequest("Không tìm thấy reportId");
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> ReportResponse(ReviewsReportModel model)
        {
            var result = await _reportService.TaskReportResponse(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetProgressReportsByManagerId()
        {
            var managerId = User.GetId();
            var result = await _reportService.GetProgressTaskReportsByManagerId(managerId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetProblemReportsByManagerId()
        {
            var managerId = User.GetId();
            var result = await _reportService.GetProblemTaskReportsByManagerId(managerId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }
    }
}

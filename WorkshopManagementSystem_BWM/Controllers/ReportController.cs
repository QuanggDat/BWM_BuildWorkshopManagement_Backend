using Data.Models;
using FitmarAgencyTemplate.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ReportService;
using static Data.Models.ReportModel;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendReport(CreateReportModel model)
        {
            if (model.managerTaskId == Guid.Empty) return BadRequest("Không nhận được managerTaskId!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = await _reportService.CreateReport(userId,model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{reportId}")]
        public async Task<ActionResult> GetReportByReportId(Guid reportId)
        {
            var result = await _reportService.GetReportByReportId(reportId);
            if (result == null) return BadRequest("Không tìm thấy reportId");
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> ReportResponse(ReviewsReportModel model)
        {
            var result = await _reportService.ReportResponse(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetProgressReportsByManagerId()
        {
            var managerId = User.GetId();
            var result = await _reportService.GetProgressReportsByManagerId(managerId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetProblemReportsByManagerId()
        {
            var managerId = User.GetId();
            var result = await _reportService.GetProblemReportsByManagerId(managerId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }
    }
}

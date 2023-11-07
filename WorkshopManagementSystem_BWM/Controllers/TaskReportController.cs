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
        public IActionResult Create (CreateTaskReportModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = _reportService.Create(userId,model);
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
        public IActionResult GetProblemReportsByLeaderId(Guid leaderTaskId)
        {
            var result = _reportService.GetProblemTaskReportsByLeaderTaskId(leaderTaskId);
            if (result == null) return BadRequest("Không tìm thấy công việc trưởng nhóm!");
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetProgressTaskReportsByLeaderTaskId(Guid leaderTaskId)
        {
            var result = _reportService.GetProgressTaskReportsByLeaderTaskId(leaderTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult SendResponse (SendResponseModel model)
        {
            var result = _reportService.SendResponse(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }       
    }
}

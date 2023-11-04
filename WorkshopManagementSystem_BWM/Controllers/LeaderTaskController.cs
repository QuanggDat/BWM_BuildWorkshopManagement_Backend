using Data.Entities;
using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Sevices.Core.LeaderTaskService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderTaskController : Controller
    {
        private readonly ILeaderTaskService _leaderTaskService;

        public LeaderTaskController(ILeaderTaskService leaderTaskService)
        {
            _leaderTaskService = leaderTaskService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateLeaderTask(CreateLeaderTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được id!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = await _leaderTaskService.CreatedLeaderTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        
        [HttpGet("[action]/{orderId}")]
        public async Task<ActionResult> GetLeaderTaskByOrderId(Guid orderId)
        {
            var result = await _leaderTaskService.GetLeaderTaskByOrderId(orderId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetLeaderTaskByLeaderId()
        {
            var userId = User.GetId();         
            var result = await _leaderTaskService.GetLeaderTaskByLeaderId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetLeaderTaskByForemanId()
        {
            var userId = User.GetId();
            var result = await _leaderTaskService.GetLeaderTaskByForemanId(userId);
            if (result == null) return BadRequest("Không tìm thấy công việc");
            return Ok(result);
        }        

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateLeaderTask(UpdateLeaderTaskModel model)
        {            
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = await _leaderTaskService.UpdateLeaderTask(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{leaderTaskId}")]
        public async Task<ActionResult> UpdateTaskStatus(Guid leaderTaskId, TaskStatus status)
        {
            var result = await _leaderTaskService.UpdateLeaderTaskStatus(leaderTaskId, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{leaderTaskId}")]
        public async Task<ActionResult> DeleteTask(Guid leaderTaskId)
        {
            var result = await _leaderTaskService.DeleteLeaderTask(leaderTaskId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        /*
        [HttpPut("[action]/{leaderTaskId}/{groupId}")]
        public async Task<ActionResult> AssignLeaderTask(Guid leaderTaskId, Guid groupId)
        {
            var result = await _leaderTaskService.AssignLeaderTask(leaderTaskId, groupId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }      
        */
    }
}

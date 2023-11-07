using Data.Entities;
using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Sevices.Core.LeaderTaskService;
using Data.Enums;
using Data.Utils;

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
        public IActionResult Created(CreateLeaderTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được id!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = _leaderTaskService.Created(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        
        [HttpGet("[action]/{orderId}")]
        public IActionResult GetByOrderId(Guid orderId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _leaderTaskService.GetByOrderId(orderId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _leaderTaskService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderId}")]
        public IActionResult GetByLeaderId(Guid leaderId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _leaderTaskService.GetByLeaderId(leaderId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
     
        [HttpPut("[action]")]
        public IActionResult Update(UpdateLeaderTaskModel model)
        {            
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = _leaderTaskService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}")]
        public IActionResult UpdateStatus(Guid id, ETaskStatus status)
        {
            var result =  _leaderTaskService.UpdateStatus(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _leaderTaskService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

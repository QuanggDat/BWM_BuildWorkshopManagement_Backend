using Data.Entities;
using Data.Models;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Sevices.Core.LeaderTaskService;
using Data.Enums;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class LeaderTaskController : Controller
    {
        private readonly ILeaderTaskService _leaderTaskService;

        public LeaderTaskController(ILeaderTaskService leaderTaskService)
        {
            _leaderTaskService = leaderTaskService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateLeaderTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được id đơn hàng!");
            if (!ValidateCreateTask(model))
            {
                return BadRequest(ModelState);
            }
            var userId = User.GetId();
            var result = _leaderTaskService.Create(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public IActionResult CreateAcceptanceTask(CreateAcceptanceTaskModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được id đơn hàng!");
            var userId = User.GetId();
            var result = _leaderTaskService.CreateAcceptanceTask(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetByOrderDetailId")]
        public IActionResult GetByOrderDetailId(Guid orderDetailId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _leaderTaskService.GetByOrderDetailId(orderDetailId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{orderId}/{itemId}")]
        public IActionResult GetByOrderIdAndItemId(Guid orderId,Guid itemId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _leaderTaskService.GetByOrderIdAndItemId(orderId, itemId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll (string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _leaderTaskService.GetAll(search, pageIndex, pageSize);
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

        [HttpGet("GetMaterialByLeaderId")]
        public IActionResult GetMaterialByLeaderId(Guid leaderId, string? search)
        {
            var result = _leaderTaskService.GetMaterialByLeaderTaskId(leaderId, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateLeaderTaskModel model)
        {
            if (!ValidateUpdateTask(model))
            {
                return BadRequest(ModelState);
            }
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

        #region Validate
        private bool ValidateCreateTask(CreateLeaderTaskModel model)
        {
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }

        private bool ValidateUpdateTask(UpdateLeaderTaskModel model)
        {
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }
        #endregion
    }
}

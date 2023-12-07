using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.GroupService;
using WorkshopManagementSystem_BWM.Extensions;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateGroupModel model)
        {
            var result = _groupService.Create(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUsersByGroupId(Guid id, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _groupService.GetAllUsersByGroupId(id, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetWorkersByGroupId(Guid id, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _groupService.GetWorkersByGroupId(id, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUsersNotInGroupId(Guid id, string? search)
        {
            var result = _groupService.GetAllUsersNotInGroupId(id, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetWorkersNotAtWorkByGroupId(Guid id, string? search)
        {
            var result = _groupService.GetWorkersNotAtWorkByGroupId(id, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAllWorkerNotYetGroup(string? search)
        {
            var result = _groupService.GetAllWorkerNotYetGroup(search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAllLeaderHaveGroup(string? search)
        {
            var result = _groupService.GetAllLeaderHaveGroup(search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAllLeaderNoHaveGroup(string? search)
        {
            var result = _groupService.GetAllLeaderNoHaveGroup(search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _groupService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _groupService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateGroupModel model)
        {
            var result = _groupService.Update(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult ChangeLeader(ChangeLeaderModel model)
        {
            var result = _groupService.ChangeLeader(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult AddWorkersToGroup(AddWorkersToGroupModel model)
        {
            var result = _groupService.AddWorkersToGroup(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = _groupService.RemoveUserFromGroup(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _groupService.Delete(id, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetAllLogOnGroup")]
        public IActionResult GetAllLogOnGroup(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _groupService.GetAllLogOnGroup(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

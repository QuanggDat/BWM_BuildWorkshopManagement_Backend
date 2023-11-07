using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.GroupService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var result = _groupService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserByGroupId(Guid id)
        {
            var result = _groupService.GetAllUserByGroupId(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserNotInGroupId(Guid id)
        {
            var result = _groupService.GetAllUserNotInGroupId(id);
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

        [HttpPut("[action]")]
        public IActionResult Update(UpdateGroupModel model)
        {
            var result = _groupService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult AddLeaderToGroup(AddWorkerToGroupModel model)
        {
            var result = _groupService.AddLeaderToGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult AddWorkerToGroup(AddWorkerToGroupModel model)
        {
            var result = _groupService.AddWorkerToGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = _groupService.RemoveUserFromGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _groupService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }        
    }
}

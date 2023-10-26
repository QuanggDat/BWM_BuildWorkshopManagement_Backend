using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.HumanResourceService;
using Sevices.Core.SquadService;
using Sevices.Core.UserService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupController(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [HttpPost("CreateGroup")]
        public async Task<ActionResult> CreateGroup(CreateGroupModel model)
        {
            var result = await _groupService.CreateGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateGroup")]
        public IActionResult UpdateGroup(UpdateGroupModel model)
        {
            var result = _groupService.UpdateGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToGroup")]
        public IActionResult AddWorkerToGroup(AddWorkerToGroupModel model)
        {
            var result = _groupService.AddWorkerToGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("RemoveWorkerFromGroup")]
        public IActionResult RemoveWorkerFromGroup(RemoveWorkerFromGroupModel model)
        {
            var result = _groupService.RemoveWorkerFromGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{id}")]
        public IActionResult DeleteGroup(Guid id)
        {
            var result = _groupService.DeleteGroup(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserByGroupId(Guid id)
        {
            var result = _groupService.GetAllUserByGroupId(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

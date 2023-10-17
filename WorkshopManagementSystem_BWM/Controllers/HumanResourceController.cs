using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.CategoryService;
using Sevices.Core.HumanResourceService;
using Sevices.Core.UserService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumanResourceController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly ISquadService _squadService;
        private readonly IUserService _userService;

        public HumanResourceController(IGroupService groupService, ISquadService squadService, IUserService userService)
        {
            _groupService=groupService;
            _squadService=squadService;
            _userService=userService;
        }

        [HttpPost("CreateSquad")]
        public async Task<ActionResult> CreateSquad(CreateSquadModel model)
        {
            var result = await _squadService.CreateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateGroup")]
        public async Task<ActionResult> CreateGroup(CreateGroupModel model)
        {
            var result = await _groupService.CreateGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateSquad")]
        public async Task<IActionResult> UpdateSquad(UpdateSquadModel model)
        {
            var result =await _squadService.UpdateSquad(model);
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

        [HttpPut("AddManagerToSquad")]
        public IActionResult AddManagerToSquad(WorkerToSquad model)
        {
            var result = _squadService.AddManagerToSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToSquad")]
        public IActionResult AddWorkerToSquad(WorkerToSquad model)
        {
            var result = _squadService.AddWorkerToSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("RemoveWorkerFromSquad")]
        public IActionResult RemoveWorkerFromSquad(WorkerToSquad model)
        {
            var result = _squadService.RemoveWorkerFromSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToGroup")]
        public IActionResult AddWorkerToGroup(WorkerToGroup model)
        {
            var result = _groupService.AddWorkerToGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("RemoveWorkerFromGroup")]
        public IActionResult RemoveWorkerFromGroup(WorkerToGroup model)
        {
            var result = _groupService.RemoveWorkerFromGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllUserWithSquadandGroup")]
        public async Task<ActionResult<HumanResources>> GetAllHumanResource()
        {
            var result =await _userService.GetAllHumanResource();
            if (result==null) return BadRequest("Không tìm thấy nhân công");
            return Ok(result);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteGroup(Guid id)
        {
            var result = _groupService.DeleteGroup(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteSquad(Guid id)
        {
            var result = _squadService.DeleteSquad(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        //[HttpGet("GetAllItemCategory")]
        //public Task<ActionResult> GetAllSquad(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        //{
        //    var result = _humanResourceService.GetAllSquad(pageIndex, pageSize);
        //    if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
        //    return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        //}

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserBySquadId(Guid id)
        {
            var result = _squadService.GetAllUserBySquadId(id);
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

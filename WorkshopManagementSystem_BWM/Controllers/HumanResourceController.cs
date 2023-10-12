using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.CategoryService;
using Sevices.Core.HumanResourceService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumanResourceController : Controller
    {
        private readonly IHumanResourceService _humanResourceService;

        public HumanResourceController(IHumanResourceService humanResourceService)
        {
            _humanResourceService = humanResourceService;
        }

        [HttpPost("CreateSquad")]
        public async Task<ActionResult> CreateSquad(CreateSquadModel model)
        {
            var result = await _humanResourceService.CreateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateGroup")]
        public async Task<ActionResult> CreateGroup(CreateGroupModel model)
        {
            var result = await _humanResourceService.CreateGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateSquad")]
        public async Task<IActionResult> UpdateSquad(UpdateSquadModel model)
        {
            var result =await _humanResourceService.UpdateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> UpdateGroup(UpdateGroupModel model)
        {
            var result = await _humanResourceService.UpdateGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToSquad")]
        public IActionResult AddWorkerToSquad(AddWorkerToSquad model)
        {
            var result = _humanResourceService.AddWorkerToSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToGroup")]
        public IActionResult AddWorkerToGroup(AddWorkerToGroup model)
        {
            var result = _humanResourceService.AddWorkerToGroup(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllUserWithSquadandGroup")]
        public async Task<ActionResult<HumanResources>> GetAllUser()
        {
            var result =await _humanResourceService.GetAllUser();
            if (result==null) return BadRequest("Không tìm thấy nhân công");
            return Ok(result);
        }

        //[HttpGet("GetAllItemCategory")]
        //public Task<ActionResult> GetAllSquad(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        //{
        //    var result = _humanResourceService.GetAllSquad(pageIndex, pageSize);
        //    if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
        //    return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        //}

        //[HttpGet("[action]/{id}")]
        //public IActionResult GetAllUserBySquadId(Guid id)
        //{
        //    var result = _humanResourceService.GetAllUserBySquadId(id);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        //[HttpGet("[action]/{id}")]
        //public IActionResult GetAllUserByGroupId(Guid id)
        //{
        //    var result = _humanResourceService.GetAllUserByGroupId(id);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}
    }
}

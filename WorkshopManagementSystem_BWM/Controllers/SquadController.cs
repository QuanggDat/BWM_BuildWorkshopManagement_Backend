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
    public class SquadController : Controller
    {
        private readonly ISquadService _squadService;
        private readonly IUserService _userService;

        public SquadController(ISquadService squadService, IUserService userService)
        {
            _squadService = squadService;
            _userService = userService;
        }

        [HttpPost("CreateSquad")]
        public async Task<ActionResult> CreateSquad(CreateSquadModel model)
        {
            var result = await _squadService.CreateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateSquad")]
        public async Task<IActionResult> UpdateSquad(UpdateSquadModel model)
        {
            var result = await _squadService.UpdateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddManagerToSquad")]
        public IActionResult AddManagerToSquad(AddWorkerToSquadModel model)
        {
            var result = _squadService.AddManagerToSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToSquad")]
        public IActionResult AddWorkerToSquad(AddWorkerToSquadModel model)
        {
            var result = _squadService.AddWorkerToSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("RemoveWorkerFromSquad")]
        public IActionResult RemoveWorkerFromSquad(RemoveWorkerFromSquadModel model)
        {
            var result = _squadService.RemoveWorkerFromSquad(model);
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

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserBySquadId(Guid id)
        {
            var result = _squadService.GetAllUserBySquadId(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

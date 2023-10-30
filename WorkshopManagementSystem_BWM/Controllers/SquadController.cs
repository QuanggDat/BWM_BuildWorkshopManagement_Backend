using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.SquadService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SquadController : Controller
    {
        private readonly ISquadService _squadService;

        public SquadController(ISquadService squadService)
        {
            _squadService = squadService;
        }

        [HttpPost("CreateSquad")]
        public IActionResult CreateSquad(CreateSquadModel model)
        {
            var result = _squadService.CreateSquad(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateSquad")]
        public IActionResult UpdateSquad(UpdateSquadModel model)
        {
            var result = _squadService.UpdateSquad(model);
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
            var result = _squadService.RemoveUserFromSquad(model);
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

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserNotInSquadId(Guid id)
        {
            var result = _squadService.GetAllUserNotInSquadId(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

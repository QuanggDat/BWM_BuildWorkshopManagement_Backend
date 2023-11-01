using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.TeamService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("CreateTeam")]
        public IActionResult CreateTeam(CreateTeamModel model)
        {
            var result = _teamService.CreateTeam(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateTeam")]
        public IActionResult UpdateTeam(UpdateTeamModel model)
        {
            var result = _teamService.UpdateTeam(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("AddWorkerToTeam")]
        public IActionResult AddWorkerToTeam(AddWorkerToTeamModel model)
        {
            var result = _teamService.AddWorkerToTeam(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("RemoveWorkerFromTeam")]
        public IActionResult RemoveWorkerFromTeam(RemoveWorkerFromTeamModel model)
        {
            var result = _teamService.RemoveWorkerFromTeam(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{id}")]
        public IActionResult DeleteTeam(Guid id)
        {
            var result = _teamService.DeleteTeam(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetTeamByGroupId(Guid groupId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _teamService.GetTeamByGroupId(groupId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAllUserByTeamId(Guid id)
        {
            var result = _teamService.GetAllUserByTeamId(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

    }
}

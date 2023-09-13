using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.UserService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("CreateWoker")]
        public async Task<ActionResult> CreateWoker([FromBody] UserCreateModel model)
        {
            var result = await _userService.CreateWoker(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost("CreateFactory")]
        public async Task<ActionResult> CeatedFactory([FromBody] UserCreateModel model)
        {
            var result = await _userService.CreateFactory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _userService.Login(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

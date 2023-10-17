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
        [HttpPost("CreateAdmin")]
        public async Task<ActionResult> CreateAdmin([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");        
            var result = await _userService.CreateAdmin(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        
        [HttpPost("CreateFactory")]
        public async Task<ActionResult> CeatedFactory([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateFactory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateManager")]
        public async Task<ActionResult> CeatedManager([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateManager(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateWoker")]
        public async Task<ActionResult> CreateWoker([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateWoker(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.password)) return BadRequest("Không nhận được mật khẩu!");
            var result = await _userService.Login(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = _userService.GetAll();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("{email}")]
        public IActionResult GetByEmail(string email)
        {
            var result = _userService.GetByEmail(email);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _userService.GetByID(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("[action]/{id}")]
        public IActionResult GetUserRole(Guid id)
        {
            var result = _userService.GetUserRole(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut]
        public IActionResult Update(UserUpdateModel model)
        {
            var result = _userService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("[action]/{id}")]
        public IActionResult BanUser(Guid id)
        {
            var result = _userService.BannedUser(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("[action]/{id}")]
        public IActionResult UnBanUser(Guid id)
        {
            var result = _userService.UnBannedUser(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("[action]")]
        public IActionResult UpdatePhone(UserUpdatePhoneModel model)
        {
            var result = _userService.UpdatePhone(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserUpdatePasswordModel model)
        {
            var result = await _userService.ChangePassword(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllUserWithSquadandGroup")]
        public async Task<ActionResult<HumanResourceModel>> GetAllUserWithSquadAndGroup()
        {
            var result = await _userService.GetAllUserWithSquadAndGroup();
            if (result == null) return BadRequest("Không tìm thấy nhân công");
            return Ok(result);
        }
    }
}

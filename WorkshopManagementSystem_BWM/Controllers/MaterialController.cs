using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.MaterialService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost("CreateMaterialCategory")]
        public async Task<ActionResult> CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var result = await _materialService.CreateCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateMaterial")]
        public async Task<ActionResult> CreateMaterial(CreateMaterialModel model)
        {
            var result = await _materialService.CreateMaterial(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterialCategory")]
        public IActionResult UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            var result = _materialService.UpdateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterial")]
        public IActionResult UpdateMaterial(UpdateMaterialModel model)
        {
            var result = _materialService.UpdateMaterial(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterialAmount")]
        public IActionResult UpdateMaterialAmount(UpdateMaterialAmountModel model)
        {
            var result = _materialService.UpdateMaterialAmount(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllMaterial")]
        public async Task<ActionResult> GetAllMaterial()
        {
            var result = _materialService.GetAllMaterial();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllMaterialCategory")]
        public async Task<ActionResult> GetAllMaterialCategory()
        {
            var result = _materialService.GetAllCategory();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialCategoryId(Guid id)
        {
            var result = _materialService.GetCategoryById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialById(Guid id)
        {
            var result = _materialService.GetMaterialById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteMaterialCategory(Guid id)
        {
            var result = _materialService.DeleteCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteMaterial(Guid id)
        {
            var result = _materialService.DeleteMaterial(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.MaterialService;
using System.Drawing.Printing;

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

        [HttpPost("SearchMaterial")]
        public Task<ActionResult> SearchItem(string search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.Search(search, pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        [HttpPost("CreateMaterial")]
        public async Task<ActionResult> CreateMaterial(Guid id, CreateMaterialModel model)
        {
            var result = await _materialService.CreateMaterial(id, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterial")]
        public IActionResult UpdateMaterial(Guid id, UpdateMaterialModel model)
        {
            var result = _materialService.UpdateMaterial(id, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterialAmount")]
        public IActionResult UpdateMaterialAmount(Guid id, UpdateMaterialAmountModel model)
        {
            var result = _materialService.UpdateMaterialAmount(id, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllMaterial")]
        public Task<ActionResult> GetAllMaterial(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.GetAllMaterial(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        [HttpGet("SortMaterialbyPrice")]
        public Task<ActionResult> SortMaterialbyPrice(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.SortMaterialByPrice(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        [HttpGet("SortMaterialbyThickness")]
        public Task<ActionResult> SortMaterialbyThickness(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.SortMaterialByThickness(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }


        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialById(Guid id)
        {
            var result = _materialService.GetMaterialById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]/{id}")]
        public IActionResult DeleteMaterial(Guid id)
        {
            var result = _materialService.DeleteMaterial(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

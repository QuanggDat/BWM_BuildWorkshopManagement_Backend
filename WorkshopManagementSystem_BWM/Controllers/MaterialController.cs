using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.MaterialService;
using System.Drawing.Printing;
using WorkshopManagementSystem_BWM.Extensions;

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
        
        [HttpPost("CreateMaterial")]
        public ActionResult CreateMaterial(CreateMaterialModel model)
        {
            if (!ValidateCreateMaterial(model))
            {
                return BadRequest(ModelState);
            }
            var createdById = User.GetId();
            var result =  _materialService.CreateMaterial(createdById, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        private bool ValidateCreateMaterial(CreateMaterialModel model)
        {
            if (model.materialCategoryId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.name),
                    $"Không nhận được {model.materialCategoryId}!");
            }
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.color))
            {
                ModelState.AddModelError(nameof(model.color),
                    $"{model.color} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.supplier))
            {
                ModelState.AddModelError(nameof(model.supplier),
                    $"{model.supplier} không được để trống !");
            }
            if (model.thickness <= 0)
            {
                ModelState.AddModelError(nameof(model.thickness),
                    $"{model.thickness} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.unit))
            {
                ModelState.AddModelError(nameof(model.unit),
                    $"{model.unit} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.importPlace))
            {
                ModelState.AddModelError(nameof(model.importPlace),
                    $"{model.importPlace} không được để trống !");
            }
            if (model.amount<= 0)
            {
                ModelState.AddModelError(nameof(model.amount),
                    $"{model.amount} nhỏ hơn hoặc bằng 0 !");
            }
            if (model.price <= 0)
            {
                ModelState.AddModelError(nameof(model.price),
                    $"{model.price} nhỏ hơn hoặc bằng 0 !");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        /*
        [HttpPost("SearchMaterial")]
        public Task<ActionResult> SearchItem(string search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.Search(search, pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
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

        //[HttpGet("SortMaterialbyPrice")]
        //public Task<ActionResult> SortMaterialbyPrice(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        //{
        //    var result = _materialService.SortMaterialbyPrice(pageIndex, pageSize);
        //    if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
        //    return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        //}

        //[HttpGet("SortMaterialbyThickness")]
        //public Task<ActionResult> SortMaterialbyThickness(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        //{
        //    var result = _materialService.SortMaterialbyThickness(pageIndex, pageSize);
        //    if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
        //    return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        //}
>
        public IActionResult GetMaterialById(Guid id)
        {
            var result = _materialService.GetMaterialById(id);
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
        */

    }
}

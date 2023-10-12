using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.CategoryService;
using Sevices.Core.ItemService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[HttpPost("CreateItemCategory")]
        //public async Task<ActionResult> CreateItemCategory(CreateItemCategoryModel model)
        //{
        //    var result = await _categoryService.CreateItemCategory(model);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        [HttpPost("CreateMaterialCategory")]
        public async Task<ActionResult> CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var result = await _categoryService.CreateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        //[HttpPut("UpdateItemCategory")]
        //public IActionResult UpdateItemCategory(UpdateItemCategoryModel model)
        //{
        //    var result = _categoryService.UpdateItemCategory(model);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        [HttpPut("UpdateMaterialCategory")]
        public IActionResult UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            var result = _categoryService.UpdateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        //[HttpGet("GetAllItemCategory")]
        //public Task<ActionResult> GetAllItemCategory(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        //{
        //    var result = _categoryService.GetAllItemCategory(pageIndex, pageSize);
        //    if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
        //    return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        //}

        [HttpGet("GetAllMaterialCategory")]
        public Task<ActionResult> GetAllMaterialCategory(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _categoryService.GetAllMaterialCategory(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        //[HttpGet("[action]/{id}")]
        //public IActionResult GetItemCategoryById(Guid id)
        //{
        //    var result = _categoryService.GetItemCategoryById(id);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialCategoryById(Guid id)
        {
            var result = _categoryService.GetMaterialCategoryById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        //[HttpGet("[action]/{id}")]
        //public IActionResult DeleteItemCategory(Guid id)
        //{
        //    var result = _categoryService.DeleteItemCategory(id);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteMaterialCategory(Guid id)
        {
            var result = _categoryService.DeleteMaterialCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

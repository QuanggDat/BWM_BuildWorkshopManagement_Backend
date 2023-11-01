using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ItemCategoryService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryService _itemCategoryService;

        public ItemCategoryController(IItemCategoryService itemCategoryService)
        {
            _itemCategoryService = itemCategoryService;
        }

        [HttpPost("[action]")]
        public ActionResult CreateItemCategory(CreateItemCategoryModel model)
        {
            var result = _itemCategoryService.CreateItemCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("[action]")]
        public IActionResult UpdateItemCategory(UpdateItemCategoryModel model)
        {
            var result = _itemCategoryService.UpdateItemCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]")]
        public Task<ActionResult> GetAllItemCategory(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemCategoryService.GetAllItemCategory(search, pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetItemCategoryById(Guid id)
        {
            var result = _itemCategoryService.GetItemCategoryById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteItemCategory(Guid id)
        {
            var result = _itemCategoryService.DeleteItemCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

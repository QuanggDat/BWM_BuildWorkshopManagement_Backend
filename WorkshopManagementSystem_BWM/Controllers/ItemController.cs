using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ItemService;
using Sevices.Core.MaterialService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("CreateItemCategory")]
        public async Task<ActionResult> CreateItemCategory(CreateItemCategoryModel model)
        {
            var result = await _itemService.CreateCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateItem")]
        public async Task<ActionResult> CreateItem(CreateItemModel model)
        {
            var result = await _itemService.CreateItem(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateItemCategory")]
        public IActionResult UpdateItemCategory(UpdateItemCategoryModel model)
        {
            var result = _itemService.UpdateItemCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateItem")]
        public IActionResult UpdateItem(UpdateItemModel model)
        {
            var result = _itemService.UpdateItem(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllItem")]
        public async Task<ActionResult> GetAllItem()
        {
            var result = _itemService.GetAllItem();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllItemCategory")]
        public async Task<ActionResult> GetAllItemCategory()
        {
            var result = _itemService.GetAllItem();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteItemCategory(int id)
        {
            var result = _itemService.DeleteCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteItem(int id)
        {
            var result = _itemService.DeleteItem(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

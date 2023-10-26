using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ItemService;
using System.Drawing.Printing;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("SearchItem")]
        public Task<ActionResult> SearchItem(string search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemService.Search(search, pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }

        [HttpPost("CreateItem")]
        public async Task<ActionResult> CreateItem(Guid id, CreateItemModel model)
        {
            var result = await _itemService.CreateItem(id, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("AddMaterialToItem")]
        public async Task<ActionResult> AddMaterialToItem(Guid id, Guid itemId, AddMaterialToItemModel model)
        {
            var result = await _itemService.AddMaterialToItem(id, itemId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateMaterialToItem")]
        public IActionResult UpdateMaterialToItem(Guid id, Guid userId, UpdateMaterialToItemModel model)
        {
            var result = _itemService.UpdateMaterialToItem(id, userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateItem")]
        public IActionResult UpdateItem(Guid id, Guid userId, UpdateItemModel model)
        {
            var result = _itemService.UpdateItem(id, userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllItem")]
        public Task<ActionResult> GetAllItem(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemService.GetAllItem(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }


        [HttpGet("SortItembyPrice")]
        public Task<ActionResult> SortItembyPrice(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemService.SortItemByPrice(pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }


        [HttpGet("[action]/{id}")]
        public IActionResult GetItemById(Guid id)
        {
            var result = _itemService.GetItemById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var result = _itemService.DeleteItem(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

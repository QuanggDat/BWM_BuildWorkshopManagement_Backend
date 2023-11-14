using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ItemCategoryService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ItemCategoryController : ControllerBase
    {
        private readonly IItemCategoryService _itemCategoryService;

        public ItemCategoryController(IItemCategoryService itemCategoryService)
        {
            _itemCategoryService = itemCategoryService;
        }

        [HttpPost("[action]")]
        public IActionResult Create (CreateItemCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại mặt hàng!"); 
            var result = _itemCategoryService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemCategoryService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _itemCategoryService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateItemCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại mặt hàng!");
            var result = _itemCategoryService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }     

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _itemCategoryService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ItemService;
using System.Drawing.Printing;
using WorkshopManagementSystem_BWM.Extensions;

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

        [HttpPost("[action]")]
        public IActionResult Create(CreateItemModel model)
        {
            if (!ValidateCreatItem(model))
            {
                return BadRequest(ModelState);
            }
            
            var result = _itemService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }       

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _itemService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{itemCategoryId}")]
        public IActionResult GetByItemCategoryId(Guid itemCategoryId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _itemService.GetByItemCategoryId(itemCategoryId,search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateItemModel model)
        {
            if (!ValidateUpdateItem(model))
            {
                return BadRequest(ModelState);
            }
            var result = _itemService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _itemService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        #region Validate
        private bool ValidateUpdateItem(UpdateItemModel model)
        {
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (model.length <= 0)
            {
                ModelState.AddModelError(nameof(model.length),
                    $"{model.length} nhỏ hơn hoặc bằng 0 !");
            }
            if (model.depth <= 0)
            {
                ModelState.AddModelError(nameof(model.depth),
                    $"{model.depth} nhỏ hơn hoặc bằng 0 !");
            }
            if (model.height <= 0)
            {
                ModelState.AddModelError(nameof(model.height),
                    $"{model.height} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.unit))
            {
                ModelState.AddModelError(nameof(model.unit),
                    $"{model.unit} không được để trống !");
            }
            if (model.mass <= 0)
            {
                ModelState.AddModelError(nameof(model.mass),
                    $"{model.mass} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.drawingsTechnical))
            {
                ModelState.AddModelError(nameof(model.drawingsTechnical),
                    $"{model.drawingsTechnical} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.drawings2D))
            {
                ModelState.AddModelError(nameof(model.drawings2D),
                    $"{model.drawings2D} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.drawings3D))
            {
                ModelState.AddModelError(nameof(model.drawings3D),
                    $"{model.drawings3D} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.description))
            {
                ModelState.AddModelError(nameof(model.description),
                    $"{model.description} không được để trống !");
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

        private bool ValidateCreatItem(CreateItemModel model)
        {        
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }          
            if (model.length <= 0)
            {
                ModelState.AddModelError(nameof(model.length),
                    $"{model.length} nhỏ hơn hoặc bằng 0 !");
            }
            if (model.depth <= 0)
            {
                ModelState.AddModelError(nameof(model.depth),
                    $"{model.depth} nhỏ hơn hoặc bằng 0 !");
            }
            if (model.height <= 0)
            {
                ModelState.AddModelError(nameof(model.height),
                    $"{model.height} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.unit))
            {
                ModelState.AddModelError(nameof(model.unit),
                    $"{model.unit} không được để trống !");
            }
            if (model.mass <= 0)
            {
                ModelState.AddModelError(nameof(model.mass),
                    $"{model.mass} nhỏ hơn hoặc bằng 0 !");
            }
            if(string.IsNullOrWhiteSpace(model.drawingsTechnical))
            {
                ModelState.AddModelError(nameof(model.drawingsTechnical),
                    $"{model.drawingsTechnical} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.drawings2D))
            {
                ModelState.AddModelError(nameof(model.drawings2D),
                    $"{model.drawings2D} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.drawings3D))
            {
                ModelState.AddModelError(nameof(model.drawings3D),
                    $"{model.drawings3D} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.description))
            {
                ModelState.AddModelError(nameof(model.description),
                    $"{model.description} không được để trống !");
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
        #endregion
    }
}

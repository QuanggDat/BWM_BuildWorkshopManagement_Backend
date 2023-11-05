using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.CategoryService;
using Sevices.Core.ItemService;
using WorkshopManagementSystem_BWM.Extensions;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialCategoryController : Controller
    {
        private readonly IMaterialCategoryService _materialCategoryService;

        public MaterialCategoryController(IMaterialCategoryService materialCategoryService)
        {
            _materialCategoryService = materialCategoryService;
        }       

        [HttpPost("[action]")]
        public ActionResult CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại vật liệu!");
            var result = _materialCategoryService.CreateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }             
        [HttpGet("[action]")]
        public IActionResult GetAllMaterialCategory(string? search,int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialCategoryService.GetAllMaterialCategory(search,pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }    

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialCategoryById(Guid id)
        {
            var result = _materialCategoryService.GetMaterialCategoryById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        [HttpPut("[action]")]
        public IActionResult UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại vật liệu!");
            var result = _materialCategoryService.UpdateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteMaterialCategory(Guid id)
        {
            var result = _materialCategoryService.DeleteMaterialCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

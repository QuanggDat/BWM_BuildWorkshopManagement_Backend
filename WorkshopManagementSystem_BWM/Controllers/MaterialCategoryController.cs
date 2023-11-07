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
        public ActionResult Create(CreateMaterialCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại vật liệu!");
            var result = _materialCategoryService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }             
        [HttpGet("[action]")]
        public IActionResult GetAll(string? search,int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialCategoryService.GetAll(search,pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }    

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _materialCategoryService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateMaterialCategoryModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên loại vật liệu!");
            var result = _materialCategoryService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _materialCategoryService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

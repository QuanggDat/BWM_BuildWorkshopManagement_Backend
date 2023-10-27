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
        private readonly IMaterialCategoryService _categoryService;

        public MaterialCategoryController(IMaterialCategoryService categoryService)
        {
            _categoryService = categoryService;
        }       

        [HttpPost("CreateMaterialCategory")]
        public ActionResult CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var createdById = User.GetId();
            var result =  _categoryService.CreateMaterialCategory(createdById,model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }      

        [HttpPut("UpdateMaterialCategory")]
        public IActionResult UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            var result = _categoryService.UpdateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }       

        [HttpGet("[action]")]
        public Task<ActionResult> GetAllMaterialCategory(string? search,int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _categoryService.GetAllMaterialCategory(search,pageIndex, pageSize);
            if (result.Succeed) return Task.FromResult<ActionResult>(Ok(result.Data));
            return Task.FromResult<ActionResult>(BadRequest(result.ErrorMessage));
        }    

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialCategoryById(Guid id)
        {
            var result = _categoryService.GetMaterialCategoryById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
       
        [HttpPut("[action]/{id}")]
        public IActionResult DeleteMaterialCategory(Guid id)
        {
            var result = _categoryService.DeleteMaterialCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

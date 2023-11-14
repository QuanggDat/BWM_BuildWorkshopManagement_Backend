using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.ProcedureService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureService _procedureService;

        public ProcedureController(IProcedureService procedureService)
        {
            _procedureService = procedureService;
        }

        [HttpPost("[action]")]
        public ActionResult Create(CreateProcedureModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên quy trình!");
            var result = _procedureService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _procedureService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _procedureService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateProcedureModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên quy trình!");
            var result = _procedureService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _procedureService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

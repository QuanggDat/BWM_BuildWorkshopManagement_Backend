using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderReportService;
using static Data.Models.OrderReportModel;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _orderReportService;

        public OrderReportController(IOrderReportService orderReportService)
        {
            _orderReportService = orderReportService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateOrderReportModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được orderId!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = _orderReportService.Create(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _orderReportService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        
        [HttpGet("[action]/{foremanId}")]
        public IActionResult GetByForemanId(Guid foremanId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {           
            var result = _orderReportService.GetByForemanId(foremanId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{orderId}")]
        public IActionResult GetByOrderId(Guid orderId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _orderReportService.GetByOrderId(orderId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll (string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _orderReportService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        /*
        [HttpPut("[action]")]
        public IActionResult SendReviews(ReviewsOrderReportModel model)
        {
            var result = _orderReportService.SendReviews(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        */
    }
}

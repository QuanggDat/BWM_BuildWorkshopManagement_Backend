using FitmarAgencyTemplate.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderReportService;
using static Data.Models.OrderReportModel;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReportController : ControllerBase
    {
        private readonly IOrderReportService _orderReportService;

        public OrderReportController(IOrderReportService orderReportService)
        {
            _orderReportService = orderReportService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SendOrderReport(CreateOrderReportModel model)
        {
            if (model.orderId == Guid.Empty) return BadRequest("Không nhận được orderId!");
            if (string.IsNullOrEmpty(model.title)) return BadRequest("Không nhận được tiêu đề!");
            var userId = User.GetId();
            var result = await _orderReportService.CreateOrderReport(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{reportId}")]
        public async Task<ActionResult> GetReportByReportId(Guid reportId)
        {
            var result = await _orderReportService.GetOrderReportByReportId(reportId);
            if (result == null) return BadRequest("Không tìm thấy reportId");
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> ReviewsOrderReport(ReviewsOrderReportModel model)
        {
            var result = await _orderReportService.ReviewsOrderReport(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetOrderReportsByFactoryId()
        {
            var factoryId = User.GetId();
            var result = await _orderReportService.GetOrderReportsByFactoryId(factoryId);
            if (result == null) return BadRequest("Không tìm thấy công việc!");
            return Ok(result);
        }
        
    }
}

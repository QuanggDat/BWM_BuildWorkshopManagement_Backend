using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderDetailService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("GetByOrderIdWithPaging")]
        public IActionResult GetByOrderIdWithPaging(Guid orderId, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size, string? search = null)
        {
            var result = _orderDetailService.GetByOrderIdWithPaging(orderId, pageIndex, pageSize, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateOrderDetailModel model)
        {
            var result = _orderDetailService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

    }
}

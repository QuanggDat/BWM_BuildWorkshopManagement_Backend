using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderDetailMaterialService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderDetailMaterial : ControllerBase
    {
        private readonly IOrderDetailMaterialService _orderDetailMaterialService;

        public OrderDetailMaterial(IOrderDetailMaterialService orderDetailMaterialService)
        {
            _orderDetailMaterialService = orderDetailMaterialService;
        }

        [HttpGet("GetByOrderDetailIdWidthPaging/{orderDetailId}")]
        public IActionResult GetByOrderDetailIdWidthPaging(Guid orderDetailId, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size, string? search = null)
        {
            var result = _orderDetailMaterialService.GetByOrderDetailIdWidthPaging(orderDetailId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

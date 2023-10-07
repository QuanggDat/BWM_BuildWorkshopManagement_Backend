using Data.Enums;
using Data.Models;
using Data.Utils;
using FitmarAgencyTemplate.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("AllWithPaging")]
        public IActionResult GetAllWithPaging(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _orderService.GetAllWithPaging( pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("QuotesByUserWithPaging")]
        public IActionResult GetQuotesByUserWithPaging(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var userId = User.GetId();
            var result = _orderService.GetQuotesByUserWithPaging(userId, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _orderService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateOrderModel model)
        {
            var result = await _orderService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Status/{status}/{id}")]
        public IActionResult UpdateStatus(OrderStatus status, Guid id)
        {
            var result = _orderService.UpdateStatus(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

    }
}

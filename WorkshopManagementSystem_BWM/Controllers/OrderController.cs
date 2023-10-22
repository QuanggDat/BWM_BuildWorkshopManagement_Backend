using Data.Enums;
using Data.Models;
using Data.Utils;
using WorkshopManagementSystem_BWM.Extensions;
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

        [HttpGet("GetAllWithPaging")]
        public IActionResult GetAllWithPaging(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _orderService.GetAllWithPaging(pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetQuotesByUserWithPaging")]
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

        [HttpGet("GetQuoteMaterialById/{id}")]
        public IActionResult GetQuoteMaterialById(Guid id)
        {
            var result = _orderService.GetQuoteMaterialById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("ExportQuoteAsPDF/{id}")]
        public async Task<IActionResult> ExportQuoteAsPDF(Guid id)
        {
            var result = await _orderService.ExportQuoteToPDF(id);
            if (result.Succeed) return File(result.Data!, result.ContentType!, result.FileName); 
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateOrderModel model)
        {
            var result = await _orderService.Create(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("UpdateStatus/{status}/{id}")]
        public IActionResult UpdateStatus(OrderStatus status, Guid id)
        {
            var result = _orderService.UpdateStatus(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

    }
}

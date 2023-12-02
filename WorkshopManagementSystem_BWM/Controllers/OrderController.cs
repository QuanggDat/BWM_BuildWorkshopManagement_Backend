using Data.Enums;
using Data.Models;
using Data.Utils;
using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.OrderService;
using Microsoft.AspNetCore.Authorization;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> Create(CreateOrderModel model)
        {
            var result = await _orderService.Create(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        [HttpPut("UpdateOrder")]
        public IActionResult Update(UpdateOrderModel model)
        {
            var result =  _orderService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetAllWithPaging")]
        public IActionResult GetAllWithPaging(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size, string? search = null)
        {
            var result = _orderService.GetAllWithPaging(pageIndex, pageSize, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{foremanId}")]
        public IActionResult GetByForemanId(Guid foremanId, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size, string? search = null)
        {
            var result = _orderService.GetByForemanId(foremanId,pageIndex, pageSize, search);
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

        [HttpGet("GetOrderById")]
        public IActionResult GetById(Guid id)
        {
            var result = _orderService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetQuoteMaterialByOrderId/{id}")]
        public IActionResult GetQuoteMaterialById(Guid id)
        {
            var result = _orderService.GetQuoteMaterialById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("ExportQuoteAsPDF/{id}")]
        public async Task<IActionResult> ExportQuoteAsPDF(Guid id)
        {
            var result = await _orderService.ExportQuoteToPDF(id);
            if (result.Succeed) return File(result.Data!, result.ContentType!, result.FileName);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("UpdateOrderStatus/{status}/{id}")]
        public IActionResult UpdateStatus(OrderStatus status, Guid id)
        {
            var userId = User.GetId();
            var result = _orderService.UpdateStatus(id, status, userId);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        /*
        [HttpPut("ReCalculatePriceOfOrder/{id}")]
        public IActionResult ReCalculatePrice(Guid id)
        {
            var result = _orderService.ReCalculatePrice(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        */
        [HttpPut("SyncItem/{id}")]
        public IActionResult SyncItem(Guid id)
        {
            var result = _orderService.SyncItem(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}

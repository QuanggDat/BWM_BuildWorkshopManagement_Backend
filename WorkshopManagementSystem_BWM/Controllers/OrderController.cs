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
            if (!ValidateCreateOrder(model))
            {
                return BadRequest(ModelState);
            }
            var result = await _orderService.Create(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
        [HttpPut("UpdateOrder")]
        public IActionResult Update(UpdateOrderModel model)
        {
            if (!ValidateUpdateOrder(model))
            {
                return BadRequest(ModelState);
            }
            var result =  _orderService.Update(model, User.GetId());
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

        [HttpGet("GetAllOrderWithTaskAndOrderDetail")]
        public IActionResult GetAllWithSearchAndPaging(int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size, string? search = null)
        {
            var result = _orderService.GetAllWithSearchAndPaging(pageIndex, pageSize, search);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("GetAllOrderWithTaskAndOrderDetailByOrderId")]
        public IActionResult GetAllByOrderId(Guid orderId)
        {
            var result = _orderService.GetAllByOrderId(orderId);
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

        [HttpGet("GetQuoteMaterialByOrderId/{id}")]
        public IActionResult GetQuoteMaterialByOrderId(Guid id)
        {
            var result = _orderService.GetQuoteMaterialByOrderId(id);
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
            var result = _orderService.SyncItem(id, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetAllLogOnOrder")]
        public IActionResult GetAllLogOnOrder(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _orderService.GetAllLogOnOrder(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        #region Validate
        private bool ValidateCreateOrder(CreateOrderModel model)
        {
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.customerName))
            {
                ModelState.AddModelError(nameof(model.customerName),
                    $"{model.customerName} không được để trống !");
            }
            if(ModelState.ErrorCount > 0) return false;
            return true;
        }

        private bool ValidateUpdateOrder(UpdateOrderModel model)
        {
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.customerName))
            {
                ModelState.AddModelError(nameof(model.customerName),
                    $"{model.customerName} không được để trống !");
            }
            if (ModelState.ErrorCount > 0) return false;
            return true;
        }
        #endregion
    }
}

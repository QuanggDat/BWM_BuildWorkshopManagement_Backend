using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetAllWithPaging(int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listOrder = _dbContext.Order.OrderByDescending(x => x.orderDate)
                                                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderModel>>(listOrder),
                    Total = listOrder.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetQuotesByUserWithPaging(Guid userId, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            var listStatus = new List<OrderStatus>() { 
                OrderStatus.Pending , 
                OrderStatus.Request, 
                OrderStatus.Approve 
            };
            try
            {
                var listOrder = _dbContext.Order.Where(x => x.assignToId == userId && listStatus.Contains(x.status))
                                                .OrderByDescending(x => x.orderDate)
                                                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderModel>>(listOrder),
                    Total = listOrder.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.FirstOrDefault(x => x.id == id);
                result.Data = _mapper.Map<OrderModel>(order);
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> Create(CreateOrderModel model)
        {
            var result = new ResultModel();
            try
            {
                var order = _mapper.Map<Order>(model);
                order.orderDate = DateTime.Now;
                order.status = OrderStatus.Pending;

                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(model.fileQuote);

                if (response.IsSuccessStatusCode)
                {
                    var stream = response.Content.ReadAsStream();
                    var excelPackage = new ExcelPackage(stream);

                    var worksheet = excelPackage?.Workbook?.Worksheets?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel UpdateStatus(Guid id, OrderStatus status)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.FirstOrDefault(x => x.id == id);
                if(status == OrderStatus.Request)
                {
                    order.quoteDate = DateTime.Now;
                }
                order.status = status;

                _dbContext.Update(order);
                _dbContext.SaveChanges();

                result.Data = true;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
    }
}

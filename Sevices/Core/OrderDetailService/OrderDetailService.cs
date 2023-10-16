using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.OrderDetailService
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listOrderDetail = _dbContext.OrderDetail.Where(x => x.orderId == orderId).Include(x => x.Item)
                                                        .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderDetailModel>>(listOrderDetail),
                    Total = listOrderDetail.Count
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateOrderDetailModel model)
        {
            var result = new ResultModel();
            try
            {
                var orderDetail = _dbContext.OrderDetail.FirstOrDefault(x => x.id == model.id);
                if(orderDetail == null)
                {
                    result.ErrorMessage = "Không tìm thấy thông tin hợp lệ!";
                }
                else
                {
                    orderDetail.quantity = model.quantity;
                    orderDetail.price = model.price;  
                    orderDetail.description = model.description;
                    _dbContext.Update(orderDetail);
                    _dbContext.SaveChanges();

                    result.Data = orderDetail;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

    }
}

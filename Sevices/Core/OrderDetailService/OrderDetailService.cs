using AutoMapper;
using Data.DataAccess;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;

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

        public ResultModel GetByOrderIdWithPaging(Guid orderId, int pageIndex, int pageSize, string? search = null)
        {
            var result = new ResultModel();
            try
            {
                var listOrderDetail = _dbContext.OrderDetail.Where(x => x.orderId == orderId).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchValue = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listOrderDetail = listOrderDetail.Where(x =>
                                                            (!string.IsNullOrWhiteSpace(x.itemName) && FnUtil.Remove_VN_Accents(x.itemName).ToUpper().Contains(searchValue)) ||
                                                            x.quantity.ToString().Contains(searchValue) ||
                                                            x.price.ToString().Contains(searchValue) ||
                                                            x.totalPrice.ToString().Contains(searchValue) ||
                                                            (!string.IsNullOrWhiteSpace(x.description) && FnUtil.Remove_VN_Accents(x.description).ToUpper().Contains(searchValue))
                                                    ).ToList();
                }

                var listOrderDetailPaging = listOrderDetail.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderDetailModel>>(listOrderDetailPaging),
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
                if (orderDetail == null)
                {
                    result.Code = 37;
                    result.ErrorMessage = "Không tìm thấy thông tin hợp lệ!";
                }
                else
                {
                    orderDetail.quantity = model.quantity;
                    orderDetail.price = model.price;
                    orderDetail.totalPrice = model.price * model.quantity;
                    orderDetail.description = model.description;
                    _dbContext.OrderDetail.Update(orderDetail);

                    var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == orderDetail.orderId);

                    if (order != null)
                    {
                        double total = 0;
                        foreach (var detail in order.OrderDetails)
                        {
                            total += detail.totalPrice;
                        }
                        order.totalPrice = total;
                        _dbContext.Order.Update(order);
                    }

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

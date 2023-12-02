using Aspose.Cells.Drawing;
using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Twilio.Types;

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

        public ResultModel CreateOrderDetail (CreateOrderDetailModel model, Guid userId)
        {
            var result = new ResultModel();
            var listNewOrderDetailMaterial = new List<OrderDetailMaterial>(); 
            try
            {
                var order = _dbContext.Order.Include(x=>x.OrderDetails).FirstOrDefault(x => x.id == model.orderId);
                var existItemCode = order.OrderDetails.Select(x=>x.itemCode).ToList();
                if (order == null)
                {
                    result.Code = 0;
                    result.Succeed = false;
                    result.ErrorMessage = "Đơn hàng không tồn tại !!!";
                }
                else
                {
                    var item = _dbContext.Item.FirstOrDefault(x => x.id == model.itemId && x.isDeleted != true);
                    if (item == null)
                    {
                        result.Code = 0;
                        result.Succeed = false;
                        result.ErrorMessage = "Sản phẩm không tồn tại !!!";
                    }
                    else
                    {
                        var code=existItemCode.Contains(item.code);
                        if(code)
                        {
                            result.Code = 0;
                            result.Succeed = false;
                            result.ErrorMessage = "Sản phẩm đã tồn tại trong chi tiết đơn hàng khác !!!";
                        }
                        else
                        {
                            var itemMate = _dbContext.ItemMaterial.Where(x => x.itemId == model.itemId).ToList();

                            var newOrderDetail = new OrderDetail
                            {
                                itemId = model.itemId,
                                orderId = model.orderId,
                                itemCategoryName = item.ItemCategory?.name ?? "",
                                itemName = item.name,
                                itemCode = item.code,
                                itemLength = item.length,
                                itemDepth = item.depth,
                                itemHeight = item.height,
                                itemUnit = item.unit,
                                itemMass = item.mass,
                                itemDrawings2D = item.drawings2D,
                                itemDrawings3D = item.drawings3D,
                                itemDrawingsTechnical = item.drawingsTechnical,
                                description = model.description,
                                price = item.price,
                                quantity = model.quantity,
                                totalPrice = item.price * model.quantity,
                            };
                            _dbContext.OrderDetail.Add(newOrderDetail);

                            foreach (var mate in itemMate)
                            {
                                var material = _dbContext.Material.FirstOrDefault(x => x.id == mate.materialId && x.isDeleted != true);
                                if (material == null)
                                {
                                    result.Code = 0;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Vật liệu không tồn tại !!!";
                                }
                                else
                                {
                                    var orderDetailMaterial = new OrderDetailMaterial
                                    {
                                        materialId = mate.materialId,
                                        orderDetailId = newOrderDetail.id,
                                        materialName = material.name,
                                        materialSupplier = material.supplier,
                                        materialThickness = material.thickness,
                                        materialUnit = material.unit,
                                        materialColor = material.color,
                                        materialSku = material.sku,
                                        quantity = mate.quantity,
                                        price = mate.price,
                                        totalPrice = mate.totalPrice,
                                    };
                                    listNewOrderDetailMaterial.Add(orderDetailMaterial);
                                }
                            }
                            _dbContext.OrderDetailMaterial.AddRange(listNewOrderDetailMaterial);

                            if (order != null)
                            {
                                double total = 0;
                                foreach (var detail in order.OrderDetails)
                                {
                                    total += detail.totalPrice;
                                }
                                order.totalPrice = total + newOrderDetail.totalPrice;
                                _dbContext.Order.Update(order);
                            }

                            var log = new Data.Entities.Log()
                            {
                                orderId = order.id,
                                orderDetailId = newOrderDetail.id,
                                userId = userId,
                                modifiedTime = DateTime.UtcNow.AddHours(7),
                                action = "Thêm chi tiết đơn hàng trong đơn hàng" + order.name,
                            };
                            _dbContext.Log.Add(log);
                            _dbContext.SaveChanges();

                            result.Data = newOrderDetail.id;
                            result.Succeed = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Update(UpdateOrderDetailModel model, Guid userId)
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
                    orderDetail.itemDrawings2D = model.itemDrawings2D;
                    orderDetail.itemDrawings3D =model.itemDrawings3D;
                    orderDetail.itemDrawingsTechnical = model.itemDrawingsTechnical;
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

                    var log = new Data.Entities.Log()
                    {
                        orderId = orderDetail.orderId,
                        orderDetailId = orderDetail.id,
                        userId = userId,
                        modifiedTime = DateTime.UtcNow.AddHours(7),
                        action = "Cập nhật chi tiết đơn hàng trong đơn hàng" + order.name,
                    };
                    _dbContext.Log.Add(log);

                    _dbContext.SaveChanges();

                    result.Data = orderDetail.id;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Delete (Guid id, Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var orderDetail = _dbContext.OrderDetail.FirstOrDefault(x => x.id == id);
                if (orderDetail == null)
                {
                    result.Code = 37;
                    result.ErrorMessage = "Không tìm thấy thông tin hợp lệ!";
                }
                else
                {
                    orderDetail.isDeleted = true;
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

                    var log = new Data.Entities.Log()
                    {
                        orderId = orderDetail.orderId,
                        orderDetailId = orderDetail.id,
                        userId = userId,
                        modifiedTime = DateTime.UtcNow.AddHours(7),
                        action = "Xóa chi tiết đơn hàng trong đơn hàng" + order.name,
                    };
                    _dbContext.Log.Add(log);

                    _dbContext.SaveChanges();

                    result.Data = true;
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

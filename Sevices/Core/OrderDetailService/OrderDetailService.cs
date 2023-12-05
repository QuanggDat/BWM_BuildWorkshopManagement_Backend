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
                var listOrderDetail = _dbContext.OrderDetail.Where(x => x.orderId == orderId && x.isDeleted!=true).ToList();

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

        public ResultModel Update(UpdateOrderDetailModel model, Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var orderDetail = _dbContext.OrderDetail.FirstOrDefault(x => x.id == model.id);
                if (orderDetail == null)
                {
                    result.Code = 37;
                    result.ErrorMessage = "Không tìm thấy thông tin chi tiết đơn hàng!";
                }
                else
                {
                    orderDetail.itemDrawings2D = model.itemDrawings2D;
                    orderDetail.itemDrawings3D = model.itemDrawings3D;
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
                        action = "Cập nhật sản phẩm trong đơn hàng",
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

        public ResultModel CreateOrderDetail (CreateOrderDetailModel model, Guid userId)
        {
            var result = new ResultModel();
            var listNewOrderDetailMaterial = new List<OrderDetailMaterial>(); 

            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == model.orderId);
               
                if (order == null)
                {
                    result.Code = 107;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var item = _dbContext.Item.FirstOrDefault(x => x.id == model.itemId && x.isDeleted != true);
                    if (item == null)
                    {
                        result.Code = 108;
                        result.Succeed = false;
                        result.ErrorMessage = "Không tìm thấy thông tin của sản phẩm!";
                    }
                    else
                    {
                        var existItemCode = order.OrderDetails.Select(x => x.itemCode).ToList();

                        var code = existItemCode.Contains(item.code);
                        if(code)
                        {
                            result.Code = 109;
                            result.Succeed = false;
                            result.ErrorMessage = "Sản phẩm đã tồn tại trong đơn hàng!";
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
                                    result.Code = 110;
                                    result.Succeed = false;
                                    result.ErrorMessage = "Không tìm thấy thông tin vật liệu!";
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

                            double total = 0;
                            foreach (var detail in order.OrderDetails)
                            {
                                total += detail.totalPrice;
                            }
                            order.totalPrice = total + newOrderDetail.totalPrice;
                            _dbContext.Order.Update(order);

                            var log = new Data.Entities.Log()
                            {
                                orderId = order.id,
                                orderDetailId = newOrderDetail.id,
                                userId = userId,
                                modifiedTime = DateTime.UtcNow.AddHours(7),
                                action = "Thêm sản phẩm vào đơn hàng",
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

        public ResultModel Delete (Guid id, Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var orderDetail = _dbContext.OrderDetail.FirstOrDefault(x => x.id == id);
                if (orderDetail == null)
                {
                    result.Code = 37;
                    result.ErrorMessage = "Không tìm thấy thông tin chi tiết đơn hàng!";
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
                        action = "Xóa sản phẩm trong đơn hàng",
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

        public ResultModel GetLogOnOrderDetailByOrderId(Guid orderId, string? search, int pageIndex, int pageSize)
        {
            ResultModel result = new ResultModel();

            try
            {
                var order = _dbContext.Order.Where(x => x.id == orderId).FirstOrDefault();
                if (order == null)
                {
                    result.Code = 97;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var listLog = _dbContext.Log.Where(x => x.orderId==orderId && x.orderDetailId != null).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        listLog = listLog.Where(x => x.action.Contains(search)).ToList();
                    }

                    var listLogPaging = listLog.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<LogModel>();
                    foreach (var item in listLogPaging)
                    {

                        var tmp = new LogModel
                        {
                            id = item.id,
                            orderId = item.orderId,
                            orderDetailId = item.orderDetailId,
                            userId = item.userId,
                            modifiedTime = item.modifiedTime,
                            action = item.action,
                        };
                        list.Add(tmp);
                    }
                    result.Data = new PagingModel()
                    {
                        Data = list,
                        Total = listLog.Count
                    };
                    result.Succeed = true;
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}

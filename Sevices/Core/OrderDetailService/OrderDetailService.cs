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

                var list = new List<OrderDetailViewlModel>();
                foreach(var item in listOrderDetailPaging)
                {
                    var listLeaderTask = _dbContext.LeaderTask.Include(x => x.WorkerTasks.Where(x=>x.isDeleted!=true)).ThenInclude(x=>x.CreateBy).Include(x=>x.Leader)
                        .Include(x=>x.CreateBy).Include(x=>x.Item).Where(x => x.itemId == item.itemId && x.orderId == orderId && x.isDeleted!=true).ToList();
                    var tmp = new OrderDetailViewlModel
                    {
                        id = item.id,
                        itemId = item.itemId,
                        itemCategoryName = item.Item?.ItemCategory?.name ?? "",
                        itemName = item.itemName,
                        itemCode = item.itemCode,
                        itemLength = item.itemLength,
                        itemDepth = item.itemDepth,
                        itemHeight = item.itemHeight,
                        itemUnit = item.itemUnit,
                        itemMass = item.itemMass,
                        itemDrawings2D = item.itemDrawings2D,
                        itemDrawings3D = item.itemDrawings3D,
                        itemDrawingsTechnical = item.itemDrawingsTechnical,
                        description = item.description,
                        price = item.price,
                        quantity = item.quantity,
                        totalPrice = item.totalPrice,
                        leaderTasks = listLeaderTask.Select(x => new ViewLeaderTask
                        {
                            id = x.id,
                            leaderId = x.leaderId,
                            leaderName = x.Leader?.fullName,
                            createdById = x.createById,
                            createdByName = x.CreateBy?.fullName,
                            name = x.name,
                            priority = x.priority,
                            itemId = x.itemId,
                            itemName = x.Item?.name,
                            itemQuantity = x.itemQuantity,
                            itemCompleted = x.itemCompleted,
                            itemFailed = x.itemFailed,
                            startTime = x.startTime,
                            endTime = x.endTime,
                            completedTime = x.completedTime,
                            status = x.status,
                            description = x.description,
                            isDeleted = x.isDeleted,
                            workerTask = x.WorkerTasks.Select(x => new WorkerTaskViewModel
                            {
                                id = x.id,
                                createById = x.createById,
                                createByName = x.CreateBy?.fullName,
                                name = x.name,
                                priority = x.priority,
                                startTime = x.startTime,
                                endTime = x.endTime,
                                completeTime = x.completedTime,
                                description = x.description,
                                status = x.status,
                                feedbackTitle = x.feedbackTitle,
                                feedbackContent = x.feedbackContent,
                                isDeleted = x.isDeleted,
                                members = x.WorkerTaskDetails.Select(x => new TaskMember
                                {
                                    memberId = x.userId,
                                    memberFullName = x.User.fullName,
                                }).ToList(),
                            }).ToList(),
                        }).ToList(),
                    };
                    list.Add(tmp);
                }
                result.Data = new PagingModel()
                {
                    Data = list,
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
                    result.Code = 112;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var item = _dbContext.Item.FirstOrDefault(x => x.id == model.itemId && x.isDeleted != true);
                    if (item == null)
                    {
                        result.Code = 113;
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
                                itemImage = item.image,
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

        public ResultModel GetAllByOrderDetailId(Guid id)
        {
            ResultModel result = new ResultModel();

            try
            {
                var orderDetail = _dbContext.OrderDetail.Include(x => x.Item).ThenInclude(x=>x.ItemCategory).FirstOrDefault(x => x.id == id && x.isDeleted!=true);
                if (orderDetail == null)
                {
                    result.Code = 37;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin hợp lệ!";
                }
                else
                {
                    var listLeaderTask = _dbContext.LeaderTask.Include(x=>x.WorkerTasks.Where(x => x.isDeleted != true)).ThenInclude(x=>x.WorkerTaskDetails).ThenInclude(x=>x.User).Where(x=>x.itemId == orderDetail.itemId && x.orderId==orderDetail.orderId && x.isDeleted!=true).ToList();
                    var item = new OrderDetailViewlModel
                    {
                        id = orderDetail.id,
                        itemCategoryName = orderDetail.Item?.ItemCategory?.name ?? "",
                        itemId = orderDetail.itemId,
                        itemName = orderDetail.itemName,
                        itemCode = orderDetail.itemCode,
                        itemLength = orderDetail.itemLength,
                        itemDepth = orderDetail.itemDepth,
                        itemHeight = orderDetail.itemHeight,
                        itemUnit = orderDetail.itemUnit,
                        itemMass = orderDetail.itemMass,
                        itemDrawings2D = orderDetail.itemDrawings2D,
                        itemDrawings3D = orderDetail.itemDrawings3D,
                        itemDrawingsTechnical = orderDetail.itemDrawingsTechnical,
                        description = orderDetail.description,
                        price = orderDetail.price,
                        quantity = orderDetail.quantity,
                        totalPrice = orderDetail.totalPrice,
                        leaderTasks = listLeaderTask.Select(x => new ViewLeaderTask
                        {
                            id =x.id,
                            leaderId = x.leaderId,
                            createdById = x.createById,
                            name = x.name,
                            priority = x.priority,
                            itemId = x.itemId,
                            itemQuantity = x.itemQuantity,
                            itemCompleted = x.itemCompleted,
                            itemFailed  = x.itemFailed,
                            startTime = x.startTime,
                            endTime = x.endTime,
                            completedTime = x.completedTime,
                            status = x.status,
                            description = x.description,
                            isDeleted = x.isDeleted,
                            workerTask = x.WorkerTasks.Select(x => new WorkerTaskViewModel
                            {
                                id = x.id,
                                createById = x.createById,
                                name = x.name,
                                priority = x.priority,
                                startTime = x.startTime,
                                endTime = x.endTime,
                                completeTime = x.completedTime,
                                description =x.description,
                                status = x.status,
                                feedbackTitle = x.feedbackTitle,
                                feedbackContent = x.feedbackContent,
                                isDeleted = x.isDeleted,
                                members = x.WorkerTaskDetails.Select(x => new TaskMember
                                {
                                    memberId = x.userId,
                                    memberFullName = x.User.fullName,
                                }).ToList(),
                            }).ToList(),
                        }).ToList(),
                    };
                    result.Data = item;
                    result.Succeed = true;
                }

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
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
                    var listLog = _dbContext.Log.Include(x => x.User).Include(x=>x.OrderDetail).Include(x=>x.Order).Where(x => x.orderId==orderId && x.orderDetailId != null).OrderByDescending(x=>x.modifiedTime).ToList();

                    if (!string.IsNullOrEmpty(search))
                    {
                        search = FnUtil.Remove_VN_Accents(search).ToUpper();
                        listLog = listLog.Where(x => FnUtil.Remove_VN_Accents(x.action).Contains(search)).ToList();
                    }

                    var listLogPaging = listLog.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    var list = new List<LogModel>();
                    foreach (var item in listLogPaging)
                    {

                        var tmp = new LogModel
                        {
                            id = item.id,
                            orderId = item.orderId,
                            orderName = item.Order?.name,
                            orderDetailId = item.orderDetailId,
                            orderDetailName = item.OrderDetail?.itemName,
                            userId = item.userId,
                            userName = item.User?.fullName,
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

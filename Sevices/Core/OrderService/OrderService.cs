using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Sevices.Core.UtilsService;
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
        private readonly IUtilsService _utilsService;

        public OrderService(AppDbContext dbContext, IMapper mapper, IUtilsService utilsService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _utilsService = utilsService;
        }

        public ResultModel GetAllWithPaging(int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listOrder = _dbContext.Order.OrderByDescending(x => x.orderDate).ToList();

                var listOrderPaging = listOrder.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderModel>>(listOrderPaging),
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
                var listOrder = _dbContext.Order.Where(x => x.assignToId == userId && listStatus.Contains(x.status)).OrderByDescending(x => x.orderDate).ToList();

                var listOrderPaging = listOrder.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderModel>>(listOrderPaging),
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
                if (order == null)
                {
                    result.ErrorMessage = "Không tìm thấy thông tin Order!";
                }
                else
                {
                    result.Data = _mapper.Map<OrderModel>(order);
                    result.Succeed = true;
                }
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
                var converData = await ConvertExcelToListOrder(model.fileQuote);
                if (!string.IsNullOrWhiteSpace(converData.Error))
                {
                    result.ErrorMessage = converData.Error;
                }
                else
                {
                    // Kiểm tra mã item có hợp lệ không
                    var listItem = _dbContext.Item.Where(x => !x.isDeleted).ToList();
                    var listItemCodeDB = listItem.Select(x => x.code).Distinct().ToList();
                    var listCodeErr = new List<string>();
                    if (converData.ListCodeItem.Any())
                    {
                         listCodeErr = converData.ListCodeItem.Where(x => !listItem.Any(i => i.code.ToUpper() == x)).ToList();
                        result.ErrorMessage = "Những mã sản phẩm không tìm thấy trong hệ thống: " + string.Join(", ", listCodeErr);
                    }

                    if(!listCodeErr.Any())
                    {
                        // Tạo order
                        var orderCreate = _mapper.Map<Order>(model);
                        orderCreate.orderDate = DateTime.Now;
                        orderCreate.status = OrderStatus.Pending;

                        _dbContext.Order.Add(orderCreate);

                        var orderId = orderCreate.id;

                        var listOrderDetailCreate = new List<OrderDetail>();
                        var listItemCodeCreated = new List<string>();

                        // Tầng
                        foreach (var item in converData.ListConverted)
                        {

                            // Tạo tầng
                            var floor = new Area()
                            {
                                name = item.name,
                            };
                            _dbContext.Area.Add(floor);
                            double priceFloor = 0;


                            // Khu vực
                            foreach (var child in item.children)
                            {
                                // Tạo khu vực
                                double priceArea = 0;

                                var area = new Area()
                                {
                                    name = child.name,
                                    parentId = floor.id
                                };
                                _dbContext.Area.Add(area);

                                var listNewItem = child.listOrderItem.Where(x => string.IsNullOrWhiteSpace(x.code)).ToList();
                                var listOldItem = child.listOrderItem.Where(x => !string.IsNullOrWhiteSpace(x.code)).ToList();
                                // Kiểm tra và tạo item mới + thêm vào list order detail mới
                                foreach (var newItem in listNewItem)
                                {
                                    var randomCode = _utilsService.GenerateItemCode(listItemCodeDB, listItemCodeCreated);
                                    listItemCodeCreated.Add(randomCode);
                                    newItem.code = randomCode;
                                    var itemNew = new Item()
                                    {

                                        code = randomCode,
                                        name = newItem.name,
                                        length = newItem.length,
                                        depth = newItem.depth,
                                        height = newItem.height,
                                        unit = newItem.unit,
                                        mass = newItem.mass,
                                        description = "",
                                        threeD = "",
                                        twoD = "",
                                        technical = "",
                                        //areaId = area.id,
                                    };
                                    _dbContext.Item.Add(itemNew);

                                    listOrderDetailCreate.Add(new()
                                    {
                                        itemId = itemNew.id,
                                        quantity = newItem.quantity,
                                        description = newItem.description ?? "",
                                        orderId = orderId,
                                        areaId = area.id,
                                        isDeleted = false,
                                    });
                                }

                                // Kiểm tra và item cũ + thêm vào list order detail mới
                                foreach (var oldItem in listOldItem)
                                {
                                    var itemFounded = listItem.FirstOrDefault(x => x.code == oldItem.code);
                                    double detailPrice = oldItem.quantity * itemFounded.price;
                                    listOrderDetailCreate.Add(new()
                                    {
                                        itemId = itemFounded.id,
                                        price = itemFounded.price,
                                        quantity = oldItem.quantity,
                                        totalPrice = detailPrice,
                                        description = oldItem.description ?? "",
                                        orderId = orderId,
                                        areaId = area.id,
                                        isDeleted = false,
                                    });
                                    priceArea += detailPrice;
                                }
                                priceFloor += priceArea;
                            }
                        }

                        _dbContext.OrderDetail.AddRange(listOrderDetailCreate);
                        orderCreate.totalPrice = listOrderDetailCreate.Sum(x => x.totalPrice);


                        //_dbContext.Order.Update(orderCreate);

                        _dbContext.SaveChanges();

                        var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == orderCreate.id);
                        result.Data = _mapper.Map<OrderModel>(order);
                        result.Succeed = true;
                    }
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
                if (status == OrderStatus.Request)
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


        #region PRIVATE 
        private static async Task<ConvertOrderExcelModel> ConvertExcelToListOrder(string url)
        {
            var result = new ConvertOrderExcelModel();
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    result.Error = "Vui lòng thêm file báo giá để tạo đơn đặt hàng";
                }
                else
                {
                    // tải file excel về
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        // đọc file excel vừa tải về
                        var stream = response.Content.ReadAsStream();
                        var excelPackage = new ExcelPackage(stream);

                        var worksheet = excelPackage?.Workbook?.Worksheets?.FirstOrDefault();

                        int rowCount = worksheet?.Dimension?.Rows ?? 0;
                        int colCount = worksheet?.Dimension?.Columns ?? 0;

                        int rowStart = 14;

                        bool isHasItem = false;

                        if (rowCount >= rowStart + 2)
                        {
                            // chuyển đổi data từ excle sang obj
                            var listFromExcel = new List<OrderExcelModel>();

                            OrderExcelModel floor = null!;
                            OrderChildrenExcelModel area = null!;

                            for (var row = rowStart; row <= rowCount; row++)
                            {
                                var cellColA = worksheet?.Cells[row, 1]?.Value?.ToString() ?? "";
                                var cellColB = worksheet?.Cells[row, 2]?.Value?.ToString() ?? "";


                                if (FnUtils.IsAlphabetOnly(cellColA) && !FnUtils.IsFirstWordValid(cellColB, "TANG") && !FnUtils.IsFirstWordValid(cellColB, "PHONG")) break;

                                // Nếu là chữ cái in hoa + bắt đầu bằng từ "Tầng"
                                if (FnUtils.IsAlphabetOnly(cellColA) && FnUtils.IsFirstWordValid(cellColB, "TANG"))
                                {
                                    floor = new OrderExcelModel()
                                    {
                                        name = cellColB
                                    };
                                    listFromExcel.Add(floor);
                                }
                                // Nếu là số la mã hợp lệ + bắt đầu bằng từ "Phòng"
                                else if (FnUtils.IsValidRomanNumber(cellColA) && FnUtils.IsFirstWordValid(cellColB, "PHONG"))
                                {
                                    area = new OrderChildrenExcelModel()
                                    {
                                        name = cellColB
                                    };
                                    floor.children.Add(area);
                                }
                                else
                                {
                                    isHasItem = true;

                                    var listError = new List<string>();

                                    // name
                                    var cellName = worksheet?.Cells[row, 3]?.Value?.ToString() ?? "";
                                    // length
                                    var cellLength = worksheet?.Cells[row, 5]?.Value?.ToString() ?? "";
                                    // depth
                                    var cellDepth = worksheet?.Cells[row, 6]?.Value?.ToString() ?? "";
                                    // height
                                    var cellHeight = worksheet?.Cells[row, 7]?.Value?.ToString() ?? "";
                                    // unit
                                    var cellUnit = worksheet?.Cells[row, 8]?.Value?.ToString() ?? "";
                                    // mass
                                    var cellMass = worksheet?.Cells[row, 9]?.Value?.ToString() ?? "";
                                    // quantity
                                    var cellQty = worksheet?.Cells[row, 10]?.Value?.ToString() ?? "";
                                    // description
                                    var cellDescr = worksheet?.Cells[row, 11]?.Value?.ToString() ?? "";

                                    // Nếu ko có mã sản phẩm => kiểm tra lỗi các thuộc tính còn lại
                                    if (string.IsNullOrWhiteSpace(cellColB))
                                    {
                                        if (string.IsNullOrWhiteSpace(cellName))
                                        {
                                            listError.Add("Tên sản phẩm không được trống!");
                                        }

                                        if (string.IsNullOrWhiteSpace(cellUnit))
                                        {
                                            listError.Add("Đơn vị không được trống!");
                                        }
                                    }
                                    else
                                    {
                                        result.ListCodeItem.Add(cellColB.ToUpper());
                                    }

                                    // xác định có lỗi => ngừng tạo order
                                    if (listError.Any())
                                    {
                                        result.Error = string.Join("\n", listError);
                                        break;
                                    }

                                    area.listOrderItem.Add(new()
                                    {
                                        code = cellColB,
                                        name = cellName,
                                        length = FnUtils.ParseStringToInt(cellLength),
                                        depth = FnUtils.ParseStringToInt(cellDepth),
                                        height = FnUtils.ParseStringToInt(cellHeight),
                                        unit = cellUnit,
                                        mass = FnUtils.ParseStringToDouble(cellMass),
                                        quantity = FnUtils.ParseStringToInt(cellQty),
                                        description = cellDescr,

                                    });
                                }
                            }
                            result.ListConverted = listFromExcel;
                        }

                        if (!isHasItem)
                        {
                            result.Error = "Vui lòng thêm sản phẩm vào file báo giá để tạo đơn đặt hàng!";
                        }
                    }
                    else
                    {
                        result.Error = "Không thể đọc file báo giá!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
        #endregion

    }
}

using Aspose.Cells;
using Aspose.Cells.Drawing;
using Aspose.Cells.Rendering;
using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using Sevices.Core.NotificationService;
using Sevices.Core.UtilsService;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Sevices.Core.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUtilsService _utilsService;
        private readonly INotificationService _notificationService;

        public OrderService(AppDbContext dbContext, IMapper mapper, IUtilsService utilsService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _utilsService = utilsService;
            _notificationService = notificationService;
        }

        public ResultModel GetAllWithPaging(int pageIndex, int pageSize, string? search = null)
        {
            var result = new ResultModel();
            try
            {
                var listOrder = _dbContext.Order.OrderByDescending(x => x.createTime).ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchValue = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listOrder = listOrder.Where(x =>
                                            (!string.IsNullOrWhiteSpace(x.name) && FnUtil.Remove_VN_Accents(x.name).ToUpper().Contains(searchValue)) ||
                                            (!string.IsNullOrWhiteSpace(x.customerName) && FnUtil.Remove_VN_Accents(x.customerName).ToUpper().Contains(searchValue)) ||
                                            x.totalPrice.ToString().Contains(searchValue) ||
                                            x.createTime.ToString("dd/MM/yyyy").Contains(searchValue) ||
                                            (x.acceptanceTime != null && x.acceptanceTime.Value.ToString("dd/MM/yyyy").Contains(searchValue))
                                        ).ToList();
                }

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
                OrderStatus.Pending,
                OrderStatus.Request,
                OrderStatus.Approve
            };
            try
            {
                var listOrder = _dbContext.Order
                    .Where(x => x.assignToId == userId && listStatus.Contains(x.status))
                    .OrderByDescending(x => x.createTime).ToList();

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
                var order = _dbContext.Order.Include(x => x.Resources).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    var data = _mapper.Map<OrderModel>(order);
                    data.resources = order.Resources.Select(x => x.link).Distinct().ToList();
                    result.Data = data;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetQuoteMaterialById(Guid id)
        {
            var result = new ResultModel();
            var listStatusDamage = new List<ESupplyStatus>() {
                ESupplyStatus.Fail,
                ESupplyStatus.RejectByCustomer,
            };
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    // get from order
                    var listItemId = order.OrderDetails.Select(x => x.itemId).Distinct().ToList();
                    var listItemMaterial = _dbContext.ItemMaterial.Include(x => x.Material).Where(x => listItemId.Contains(x.itemId)).ToList();

                    var dict = new Dictionary<Guid, QuoteMaterialDetailModel>();
                    foreach (var itemMate in listItemMaterial)
                    {
                        if (dict.ContainsKey(itemMate.materialId))
                        {
                            dict[itemMate.materialId].quantity += itemMate.quantity;
                            dict[itemMate.materialId].totalPrice = dict[itemMate.materialId].quantity * dict[itemMate.materialId].price;
                        }
                        else
                        {
                            var quoteMaterialModel = new QuoteMaterialDetailModel()
                            {
                                materialId = itemMate.materialId,
                                name = itemMate.Material.name,
                                sku = itemMate.Material.sku,
                                quantity = itemMate.quantity,
                                price = itemMate.price,
                                totalPrice = itemMate.totalPrice,
                            };
                            dict.Add(itemMate.materialId, quoteMaterialModel);
                        }
                    }

                    // get from supply
                    var listReportByOrder = _dbContext.Report.Where(x => x.orderId == order.id).ToList();
                    var listReportId = listReportByOrder.Select(x => x.id).ToList();

                    var listSupplyDamageByReport = _dbContext.Supply.Include(x => x.Material)
                                                                    .Where(x => listReportId.Contains(x.reportId) && listStatusDamage.Contains(x.status))
                                                                    .Select(x => new QuoteMaterialDetailModel()
                                                                    {
                                                                        materialId = x.materialId,
                                                                        name = x.Material.name,
                                                                        sku = x.Material.sku,
                                                                        quantity = x.amount,
                                                                        price = x.price,
                                                                        totalPrice = x.totalPrice,
                                                                    }).ToList();

                    double totalPriceSupplyDamage = listSupplyDamageByReport.Sum(x => x.totalPrice);

                    double percentDamage = 0;
                    if (order.totalPrice > 0)
                    {
                        percentDamage = totalPriceSupplyDamage / order.totalPrice * 100;
                    }

                    result.Data = new QuoteMaterialOrderModel()
                    {
                        orderId = order.id,

                        totalPriceOrder = order.totalPrice,
                        listFromOrder = dict.Values.ToList(),

                        totalPriceSupplyDamage = totalPriceSupplyDamage,
                        listFromSupplyDamage = listSupplyDamageByReport,

                        percentDamage = percentDamage
                    };
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<ResultModel> Create(CreateOrderModel model, Guid createdById)
        {
            var result = new ResultModel();
            try
            {
                var converData = await ConvertExcelToListOrder(model.fileQuote);
                if (!string.IsNullOrWhiteSpace(converData.Error))
                {
                    result.Code = converData.ErrCode;
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
                    }

                    if (listCodeErr.Any())
                    {
                        result.Code = 68;
                        result.ErrorMessage = "Những mã sản phẩm không tìm thấy trong hệ thống: " + string.Join(", ", listCodeErr);
                    }
                    else
                    {
                        // Tạo order
                        var orderCreate = _mapper.Map<Order>(model);
                        orderCreate.createdById = createdById;
                        orderCreate.createTime = DateTime.Now;
                        orderCreate.status = OrderStatus.Pending;

                        _dbContext.Order.Add(orderCreate);

                        var orderId = orderCreate.id;

                        var listOrderDetailCreate = new List<OrderDetail>();
                        var listOrderDetailMaterialCreate = new List<OrderDetailMaterial>();
                        var listItemCodeCreated = new List<string>();

                        var listNewItem = converData.ListOrderItem.Where(x => string.IsNullOrWhiteSpace(x.code)).ToList();
                        var listOldItem = converData.ListOrderItem.Where(x => !string.IsNullOrWhiteSpace(x.code)).ToList();
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
                                drawingsTechnical = "",
                                drawings2D = "",
                                drawings3D = "",
                            };
                            _dbContext.Item.Add(itemNew);

                            listOrderDetailCreate.Add(new()
                            {
                                itemId = itemNew.id,
                                quantity = newItem.quantity,
                                description = newItem.description ?? "",
                                orderId = orderId,
                            });
                        }

                        // Kiểm tra và item cũ + thêm vào list order detail mới
                        foreach (var oldItem in listOldItem)
                        {
                            var itemFounded = listItem.FirstOrDefault(x => x.code == oldItem.code);
                            double detailPrice = oldItem.quantity * itemFounded?.price ?? 0;
                            listOrderDetailCreate.Add(new()
                            {
                                itemId = itemFounded.id,
                                price = itemFounded.price,
                                quantity = oldItem.quantity,
                                totalPrice = detailPrice,
                                description = oldItem.description ?? "",
                                orderId = orderId
                            });

                            // Cứ mỗi orderdetail được tạo từ các item cũ, dựa vào id của item cũ lấy bảng itemMaterial ra để đồng bộ với bảng orderDetailMaterial mới 
                            // được tạo theo tương ứng
                            foreach (var orderDetail in listOrderDetailCreate)
                            {
                                var itemMaterial = _dbContext.ItemMaterial.Where(x => x.itemId == itemFounded.id).ToList();
                                foreach (var orderDetailMaterial in itemMaterial)
                                {
                                    var material = _dbContext.Material.FirstOrDefault(x => x.id == orderDetailMaterial.id);
                                    listOrderDetailMaterialCreate.Add(new()
                                    {
                                        orderDetailId = orderDetail.id,
                                        materialName = material.name,
                                        materialSupplier = material.supplier,
                                        materialThickness = material.thickness,
                                        materialSku = material.sku,
                                        materialId = orderDetailMaterial.materialId,
                                        quantity = orderDetailMaterial.quantity,
                                        price = material.price,
                                        totalPrice = material.price * orderDetailMaterial.quantity,
                                    });
                                }
                            }
                        }

                        _dbContext.OrderDetail.AddRange(listOrderDetailCreate);
                        _dbContext.OrderDetailMaterial.AddRange(listOrderDetailMaterialCreate);
                        orderCreate.totalPrice = listOrderDetailCreate.Sum(x => x.totalPrice);

                        _dbContext.SaveChanges();

                        var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == orderCreate.id);

                        var noti = new Notification()
                        {
                            userId = order.assignToId,
                            title = "Đơn đặt hàng mới",
                            content = "Bạn có đơn đặt hàng mới cần báo giá",
                            type = NotificationType.Order,
                            orderId = order.id
                        };
                        _notificationService.Create(noti);

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

        public ResultModel Update(UpdateOrderModel model)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.FirstOrDefault(x => x.id == model.id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    if (model.startTime != null && DateTime.Compare(model.startTime.Value, DateTime.Now) < 0)
                    {
                        result.Code = 69;
                        result.ErrorMessage = "Ngày giờ bắt đầu phải lớn hơn hoặc bằng hiện tại!";
                    }
                    else if (model.endTime != null && model.startTime == null)
                    {
                        result.Code = 74;
                        result.ErrorMessage = "Vui lòng nhập ngày giờ bắt đầu trước khi thêm/sửa ngày kết thúc!";
                    }
                    else if (model.endTime != null && model.startTime != null && DateTime.Compare(model.startTime.Value, model.endTime.Value) >= 0)
                    {
                        result.Code = 75;
                        result.ErrorMessage = "Ngày giờ kết thúc phải lớn hơn ngày giờ bắt đầu!";
                    }
                    else
                    {
                        order.name = model.name;
                        order.customerName = model.customerName;
                        order.fileContract = model.fileContract;
                        order.assignToId = model.assignToId;
                        order.description = model.description;
                        order.startTime = model.startTime;
                        order.endTime = model.endTime;

                        _dbContext.Order.Update(order);
                        _dbContext.SaveChanges();

                        result.Data = true;
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

        public ResultModel ReCalculatePrice(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).ThenInclude(x => x.Item).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    double total = 0;
                    foreach (var detail in order.OrderDetails)
                    {
                        detail.price = detail.Item?.price ?? 0;
                        detail.totalPrice = detail.price * detail.quantity;
                        total += detail.totalPrice;
                    }
                    order.totalPrice = total;

                    _dbContext.Update(order);
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

        public ResultModel UpdateStatus(Guid id, OrderStatus status, Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).ThenInclude(x => x.Item).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    if (status == OrderStatus.Request)
                    {
                        var hasError = false;

                        foreach (var detail in order.OrderDetails)
                        {
                            var listError = new List<string>();
                            if (detail.Item != null)
                            {
                                if (string.IsNullOrWhiteSpace(detail.Item.drawingsTechnical))
                                {
                                    listError.Add("Bản vẽ kỹ thuật");
                                }

                                if (string.IsNullOrWhiteSpace(detail.Item.drawings2D))
                                {
                                    listError.Add("Bản vẽ 2D");
                                }

                                if (string.IsNullOrWhiteSpace(detail.Item.drawings3D))
                                {
                                    listError.Add("Bản vẽ 3d");
                                }

                                if (listError.Any())
                                {
                                    result.Code = 106;
                                    result.ErrorMessage = $"Sản phẩm \"{detail.Item.name}\" chưa được cập nhật: {string.Join(", ", listError)}";

                                    hasError = true;
                                    break;
                                }
                            }

                        }

                        if (hasError)
                        {
                            order.quoteTime = DateTime.Now;

                            var noti = new Notification()
                            {
                                userId = order.createdById,
                                title = "Báo giá đơn đặt hàng",
                                content = "Bạn vừa nhận được báo giá đơn hàng",
                                type = NotificationType.Order,
                                orderId = order.id
                            };
                            _notificationService.Create(noti);
                        }
                    }
                    else if (status == OrderStatus.InProgress)
                    {
                        // order
                        order.inProgressTime = DateTime.Now;

                        // gen task
                        GenerateTaskByOrder(order, userId);
                    }
                    else if (status == OrderStatus.Completed)
                    {
                        order.acceptanceTime = DateTime.Now;
                    }
                    order.status = status;

                    _dbContext.Update(order);
                    _dbContext.SaveChanges();

                    result.Data = order.id;
                    result.Succeed = true;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public async Task<FileResultModel> ExportQuoteToPDF(Guid id)
        {
            var result = new FileResultModel();
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).ThenInclude(x => x.Item).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    if (!order.OrderDetails.Any())
                    {

                        result.Code = 67;
                        result.ErrorMessage = "Đơn hàng không có sản phẩm không thể tạo file báo giá!";
                    }
                    else
                    {
                        var dictItemImage = order.OrderDetails.Select(x => x.Item).Where(x => !string.IsNullOrWhiteSpace(x?.image))
                                                                .DistinctBy(x => x?.id).ToDictionary(x => x!.id, y => y?.image!);

                        var dictItemImgStream = await FetchListImageItem(dictItemImage);

                        var workbook = new Workbook(Path.Combine("Template/TemplateQuote.xlsx"));
                        var worksheet = workbook.Worksheets.FirstOrDefault();

                        var rowCount = worksheet?.Cells?.MaxDataRow ?? 0;
                        var colCount = worksheet?.Cells?.MaxDataColumn ?? 0;
                        var rowDataBegin = -1;
                        var rowData = -1;

                        for (var row = 0; row <= rowCount; row++)
                        {
                            var cellValue = worksheet?.Cells[row, 0]?.Value?.ToString();
                            if (FnUtil.Remove_VN_Accents(cellValue)?.ToUpper() == "PHAN HOAN THIEN NOI THAT")
                            {
                                var cell = worksheet.Cells[row, 10];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{order.totalPrice:n0}";

                                rowDataBegin = row + 1;
                                rowData = row + 1;
                                break;
                            }

                            cellValue = worksheet?.Cells[row, 1]?.Value?.ToString();
                            if (FnUtil.Remove_VN_Accents(cellValue)?.ToUpper() == "KHACH HANG:")
                            {
                                worksheet.Cells[row, 1].Value = $"Khách hàng: {order.customerName?.ToUpper()}";
                            }
                        }

                        if (rowData > -1)
                        {
                            for (var i = 0; i < order.OrderDetails.Count; i++)
                            {
                                var isAutoFitRow = true;

                                var detail = order.OrderDetails[i];
                                // Check Hình ảnh đầu tiên để xem có cần auto fit row hay shink to fit
                                // Hình ảnh minh hoạ
                                var cell = worksheet.Cells[rowData, 2];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                if (dictItemImgStream != null && dictItemImgStream.Count > 0 && detail.Item != null && !string.IsNullOrWhiteSpace(detail.Item.image) && dictItemImage.ContainsKey(detail.Item.id))
                                {
                                    var image = SKImage.FromEncodedData(dictItemImgStream[detail.Item.id]);
                                    image = FnUtil.ResizeImage(image);
                                    worksheet.Cells.SetRowHeightPixel(rowData, image.Height + 10);

                                    var skiaStream = image.Encode().AsStream();
                                    // Create a Stream from the SKStream
                                    var stream = new MemoryStream();
                                    skiaStream.CopyTo(stream);

                                    //var streamImg = new MemoryStream();
                                    //image.Save(streamImg, ImageFormat.Jpeg);

                                    worksheet.Pictures.Add(rowData, 2, skiaStream);

                                    isAutoFitRow = false;
                                }
                                // STT
                                cell = worksheet.Cells[rowData, 0];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = i + 1;
                                // Hạng mục
                                cell = worksheet.Cells[rowData, 1];
                                cell.SetStyle(FnExcel.ApplyWrapTextStyle(cell.GetStyle(), true));
                                cell.Value = detail.Item?.name;
                                // Dài
                                cell = worksheet.Cells[rowData, 3];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.Item?.length:n0}";
                                // Sâu
                                cell = worksheet.Cells[rowData, 4];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.Item?.depth:n0}";
                                // Cao
                                cell = worksheet.Cells[rowData, 5];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.Item?.height:n0}";
                                // ĐVT
                                cell = worksheet.Cells[rowData, 6];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = detail.Item?.unit;
                                // KL
                                cell = worksheet.Cells[rowData, 7];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.Item?.mass:n0}";
                                // SL
                                cell = worksheet.Cells[rowData, 8];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = detail.quantity;
                                // Đơn giá
                                cell = worksheet.Cells[rowData, 9];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.price:n0}";
                                // Thành tiền
                                cell = worksheet.Cells[rowData, 10];
                                cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                                cell.Value = $"{detail.totalPrice:n0}";
                                // Ghi chú vật liệu
                                cell = worksheet.Cells[rowData, 11];
                                cell.SetStyle(FnExcel.ApplyWrapTextStyle(cell.GetStyle(), false, !isAutoFitRow));
                                cell.Value = detail.description;

                                if (isAutoFitRow)
                                {
                                    worksheet.AutoFitRow(rowData);
                                }

                                rowData++;
                            }

                            var range = worksheet.Cells.CreateRange(rowDataBegin, 0, rowData - rowDataBegin, 12);
                            FnExcel.ApplyRangeCommonStyle(range, workbook.CreateCellsColor());

                            using var pdfStream = new MemoryStream();
                            var pdfOptions = new PdfSaveOptions
                            {
                                AllColumnsInOnePagePerSheet = true,
                                Compliance = PdfCompliance.PdfA1b
                            };

                            workbook.Save(pdfStream, pdfOptions);

                            result.Data = pdfStream.ToArray();
                            result.Succeed = true;
                            result.FileName = $"JAMA - BG - {order.customerName}.pdf";
                            result.ContentType = "application/pdf";
                        }
                        else
                        {
                            result.Code = 36;
                            result.ErrorMessage = "Template lỗi!";
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

        #region PRIVATE 
        private static async Task<ConvertOrderExcelModel> ConvertExcelToListOrder(string url)
        {
            var result = new ConvertOrderExcelModel();
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    result.ErrCode = 62;
                    result.Error = "Vui lòng thêm file báo giá để tạo đơn đặt hàng!";
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
                        var workbook = new Workbook(stream);

                        var worksheet = workbook.Worksheets.FirstOrDefault();

                        int rowCount = worksheet?.Cells?.MaxDataRow ?? 0;
                        int colCount = worksheet?.Cells?.MaxDataColumn ?? 0;

                        var isDataBegin = false;
                        var hasItem = false;

                        // chuyển đổi data từ excle sang obj

                        for (var row = 0; row <= rowCount; row++)
                        {
                            var cellColA = worksheet?.Cells[row, 0]?.Value?.ToString() ?? "";
                            if (FnUtil.Remove_VN_Accents(cellColA) == "TONG CONG")
                            {
                                isDataBegin = true;
                            }
                            else if (isDataBegin)
                            {
                                if (!FnUtil.IsNumberOnly(cellColA))
                                {
                                    break;
                                }
                                // code
                                var cellCode = worksheet?.Cells[row, 1]?.Value?.ToString() ?? "";
                                // name
                                var cellName = worksheet?.Cells[row, 2]?.Value?.ToString() ?? "";
                                // length
                                var cellLength = worksheet?.Cells[row, 4]?.Value?.ToString() ?? "";
                                // depth
                                var cellDepth = worksheet?.Cells[row, 5]?.Value?.ToString() ?? "";
                                // height
                                var cellHeight = worksheet?.Cells[row, 6]?.Value?.ToString() ?? "";
                                // unit
                                var cellUnit = worksheet?.Cells[row, 7]?.Value?.ToString() ?? "";
                                // mass
                                var cellMass = worksheet?.Cells[row, 8]?.Value?.ToString() ?? "";
                                // quantity
                                var cellQty = worksheet?.Cells[row, 9]?.Value?.ToString() ?? "";
                                // description
                                var cellDescr = worksheet?.Cells[row, 10]?.Value?.ToString() ?? "";

                                // Nếu ko có mã sản phẩm => kiểm tra lỗi các thuộc tính còn lại
                                if (string.IsNullOrWhiteSpace(cellCode))
                                {
                                    if (string.IsNullOrWhiteSpace(cellName))
                                    {
                                        result.ErrCode = 65;
                                        result.Error = "Tên sản phẩm không được trống!";
                                        break;
                                    }

                                    if (string.IsNullOrWhiteSpace(cellUnit))
                                    {
                                        result.ErrCode = 66;
                                        result.Error = "Đơn vị không được trống!";
                                        break;
                                    }
                                }
                                else
                                {
                                    result.ListCodeItem.Add(cellCode.ToUpper());
                                }

                                result.ListOrderItem.Add(new()
                                {
                                    code = cellCode,
                                    name = cellName,
                                    length = FnUtil.ParseStringToInt(cellLength),
                                    depth = FnUtil.ParseStringToInt(cellDepth),
                                    height = FnUtil.ParseStringToInt(cellHeight),
                                    unit = cellUnit,
                                    mass = FnUtil.ParseStringToDouble(cellMass),
                                    quantity = FnUtil.ParseStringToInt(cellQty),
                                    description = cellDescr,

                                });
                                hasItem = true;
                            }
                        }

                        if (!hasItem)
                        {
                            result.ErrCode = 63;
                            result.Error = "Vui lòng thêm sản phẩm vào file báo giá để tạo đơn đặt hàng!";
                        }
                    }
                    else
                    {
                        result.ErrCode = 64;
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

        private static async Task<Dictionary<Guid, Stream>> FetchListImageItem(Dictionary<Guid, string> dictItemImage)
        {
            var res = new Dictionary<Guid, Stream>();
            var listTask = new List<Task>();
            var httpClient = new HttpClient();

            foreach (var item in dictItemImage)
            {
                var task = Task.Run(async () =>
                {
                    try
                    {
                        var response = await httpClient.GetAsync(item.Value);
                        if (response.IsSuccessStatusCode)
                        {
                            var stream = response.Content.ReadAsStream();
                            res.Add(item.Key, stream);
                        }
                    }
                    catch (Exception) { }
                });
                listTask.Add(task);
            }
            await Task.WhenAll(listTask);
            return res;
        }

        private void GenerateTaskByOrder(Order order, Guid userId)
        {
            // Chuan bi tao task
            var listItemId = order.OrderDetails.Select(x => x.itemId).Distinct().ToList();

            var listProcedureItem = _dbContext.ProcedureItem.Include(x => x.Item).Where(x => listItemId.Contains(x.itemId)).ToList();

            var listProcedureId = listProcedureItem.Select(x => x.procedureId).Distinct().ToList();

            var listProcedure = _dbContext.Procedure.Where(x => !x.isDeleted && listProcedureId.Contains(x.id)).ToList();

            var listProcedureStep = _dbContext.ProcedureStep.Where(x => listProcedureId.Contains(x.procedureId)).ToList();

            var listStepId = listProcedureStep.Select(x => x.stepId).Distinct().ToList();

            var listStep = _dbContext.Step.Where(x => !x.isDeleted && listStepId.Contains(x.id)).ToList();

            foreach (var itemId in listItemId)
            {
                var listProcItemByItem = listProcedureItem.Where(x => x.itemId == itemId).OrderBy(x => x.priority).ToList();
                // Tao leader task
                foreach (var procItem in listProcItemByItem)
                {
                    var leaderTask = new LeaderTask()
                    {
                        orderId = order.id,
                        createById = userId,
                        itemId = procItem.itemId,
                        name = listProcedure.FirstOrDefault(x => x.id == procItem.procedureId)?.name ?? "",
                        status = ETaskStatus.New,
                        isDeleted = false,
                        priority = procItem.priority,
                    };
                    _dbContext.LeaderTask.Add(leaderTask);

                    // Tao worker task
                    var listWorkerTask = new List<WorkerTask>();
                    var listProcStepByProcId = listProcedureStep.Where(x => x.procedureId == procItem.procedureId).OrderBy(x => x.priority).ToList();
                    foreach (var procStep in listProcStepByProcId)
                    {
                        listWorkerTask.Add(new()
                        {
                            createById = userId,
                            leaderTaskId = leaderTask.id,
                            priority = procStep.priority,
                            name = listStep.FirstOrDefault(x => x.id == procStep.stepId)?.name ?? "",
                            status = EWorkerTaskStatus.New,
                            isDeleted = false,
                        });
                    }

                    if (listWorkerTask.Any())
                    {
                        _dbContext.WorkerTask.AddRange(listWorkerTask);
                    }
                }
            }
        }

        #endregion

    }
}

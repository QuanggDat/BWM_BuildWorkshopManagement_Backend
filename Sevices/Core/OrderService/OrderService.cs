using Aspose.Cells;
using Aspose.Cells.Rendering;
using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Sevices.Core.NotificationService;
using Sevices.Core.UtilsService;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;

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
                OrderStatus.Pending,
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

        public ResultModel GetQuoteMaterialById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.ErrorMessage = "Không tìm thấy thông tin Order!";
                }
                else
                {
                    var res = _mapper.Map<QuoteMaterialOrderModel>(order);
                    var listItemId = order.OrderDetails.Select(x => x.itemId).Distinct().ToList();
                    var listItemMaterial = _dbContext.ItemMaterial.Include(x => x.Material).Where(x => listItemId.Contains(x.itemId)).ToList();

                    var dict = new Dictionary<Guid, QuoteMaterialModel>();
                    foreach (var itemMate in listItemMaterial)
                    {
                        if (dict.ContainsKey(itemMate.materialId))
                        {
                            dict[itemMate.materialId].quantity += itemMate.quantity;
                            dict[itemMate.materialId].totalPrice = dict[itemMate.materialId].quantity * dict[itemMate.materialId].price;
                        }
                        else
                        {
                            var quoteMaterialModel = new QuoteMaterialModel()
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

                    res.listQuoteMaterial = dict.Values.ToList();
                    result.Data = res;
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
            Console.WriteLine("Create Order URL: " + model.fileQuote);
            Log.Warning("Create Order URL: " + model.fileQuote);
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

                    if (!listCodeErr.Any())
                    {
                        // Tạo order
                        var orderCreate = _mapper.Map<Order>(model);
                        orderCreate.createdById = createdById;
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
                            var floor = new Floor()
                            {
                                name = item.name,
                            };
                            _dbContext.Floor.Add(floor);
                            double priceFloor = 0;

                            // Khu vực
                            foreach (var child in item.children)
                            {
                                // Tạo khu vực
                                double priceArea = 0;

                                var area = new Area()
                                {
                                    name = child.name,
                                    floorId = floor.id
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
                                area.price = priceArea;
                                priceFloor += priceArea;
                            }
                            floor.price = priceFloor;
                        }

                        _dbContext.OrderDetail.AddRange(listOrderDetailCreate);
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

        public ResultModel UpdateStatus(Guid id, OrderStatus status)
        {
            var result = new ResultModel();
            try
            {
                var order = _dbContext.Order.FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng";
                }
                else
                {
                    if (status == OrderStatus.Request)
                    {
                        order.quoteDate = DateTime.Now;

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
                    else if (status == OrderStatus.Completed)
                    {
                        order.acceptanceDate = DateTime.Now;
                    }
                    order.status = status;

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

        public async Task<FileResultModel> ExportQuoteToPDF(Guid id)
        {
            var result = new FileResultModel();
            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails).ThenInclude(x => x.Item).FirstOrDefault(x => x.id == id);
                if (order == null)
                {
                    result.ErrorMessage = "Không tìm thấy thông tin đơn đặt hàng!";
                }
                else
                {
                    var dictItemImage = order.OrderDetails.Select(x => x.Item).Where(x => !string.IsNullOrWhiteSpace(x?.image))
                                                            .DistinctBy(x => x?.id).ToDictionary(x => x!.id, y => y?.image!);

                    var dictItemImgStream = await FetchListImageItem(dictItemImage);

                    var listAreaTd = order.OrderDetails.Select(x => x.areaId).Distinct().ToList();
                    var listArea = _dbContext.Area.Where(x => listAreaTd.Contains(x.id)).ToList();

                    var listFloorId = listArea.Select(x => x.floorId).Distinct().ToList();
                    var listFloor = _dbContext.Floor.Where(x => listFloorId.Contains(x.id)).ToList();

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
                        if (listFloorId.Count > 1)
                        {
                            for (var i = 0; i < listFloor.Count; i++)
                            {
                                var floor = listFloor[i];
                                // STT
                                var cell = worksheet.Cells[rowData, 0];
                                cell.SetStyle(FnExcel.ApplyFloorStyle(cell.GetStyle()));
                                cell.Value = FnUtil.NumToAlphabets(i + 1);
                                // Name
                                worksheet.Cells.Merge(rowData, 1, 1, 9);
                                cell = worksheet.Cells[rowData, 1];
                                cell.SetStyle(FnExcel.ApplyFloorStyle(cell.GetStyle(), true));
                                cell.Value = floor.name;
                                // Price
                                cell = worksheet.Cells[rowData, 10];
                                cell.SetStyle(FnExcel.ApplyFloorStyle(cell.GetStyle()));
                                cell.Value = $"{floor.price:n0}";

                                // Others
                                cell = worksheet.Cells[rowData, 11];
                                cell.SetStyle(FnExcel.ApplyFloorStyle(cell.GetStyle()));

                                rowData++;

                                var listAreaByFloor = listArea.Where(x => x.floorId == floor.id).ToList();
                                rowData = AssignDataIntoWorkbook(worksheet, rowData, listAreaByFloor, order.OrderDetails, dictItemImgStream);
                            }
                        }
                        else
                        {
                            rowData = AssignDataIntoWorkbook(worksheet, rowData, listArea, order.OrderDetails, dictItemImgStream);
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
                        result.ErrorMessage = "Template lỗi!";
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
                    result.Error = "Vui lòng thêm file báo giá để tạo đơn đặt hàng";
                }
                else
                {
                    Log.Warning("487 - Create Order URL: " + url);
                    // tải file excel về
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(url);
                    Log.Warning("491 - Create Order URL: " + url);
                    Log.Warning("492 - Create Order URL - response.StatusCode: " + response.StatusCode);

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
                        var listFromExcel = new List<OrderExcelModel>();

                        OrderExcelModel floor = null!;
                        OrderChildrenExcelModel area = null!;

                        for (var row = 0; row <= rowCount; row++)
                        {
                            var cellColA = worksheet?.Cells[row, 0]?.Value?.ToString() ?? "";
                            if (FnUtil.Remove_VN_Accents(cellColA) == "TONG CONG")
                            {
                                isDataBegin = true;
                            }
                            else if (isDataBegin)
                            {
                                var cellColB = worksheet?.Cells[row, 1]?.Value?.ToString() ?? "";

                                if (FnUtil.IsAlphabetOnly(cellColA) && !FnUtil.IsFirstWordValid(cellColB, "TANG") && !FnUtil.IsFirstWordValid(cellColB, "PHONG")) break;

                                // Nếu là chữ cái in hoa + bắt đầu bằng từ "Tầng"
                                if (FnUtil.IsAlphabetOnly(cellColA) && FnUtil.IsFirstWordValid(cellColB, "TANG"))
                                {
                                    floor = new OrderExcelModel()
                                    {
                                        name = cellColB
                                    };
                                    listFromExcel.Add(floor);
                                }
                                // Nếu là số la mã hợp lệ + bắt đầu bằng từ "Phòng"
                                else if (FnUtil.IsValidRomanNumber(cellColA) && FnUtil.IsFirstWordValid(cellColB, "PHONG"))
                                {
                                    area = new OrderChildrenExcelModel()
                                    {
                                        name = cellColB
                                    };
                                    floor.children.Add(area);
                                }
                                else
                                {
                                    var listError = new List<string>();

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
                        }

                        if (!hasItem)
                        {
                            result.Error = "Vui lòng thêm sản phẩm vào file báo giá để tạo đơn đặt hàng!";
                        }
                        else
                        {
                            result.ListConverted = listFromExcel;
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

        private static int AssignDataIntoWorkbook(Worksheet worksheet, int rowData, List<Area> listArea, List<OrderDetail> listDetail, Dictionary<Guid, Stream> dictItemImage)
        {
            for (var a = 0; a < listArea.Count; a++)
            {
                var area = listArea[a];
                // STT
                var cell = worksheet.Cells[rowData, 0];
                cell.SetStyle(FnExcel.ApplyAreaStyle(cell.GetStyle()));
                cell.Value = FnUtil.NumToRomanNum(a + 1);
                // Name
                worksheet.Cells.Merge(rowData, 1, 1, 9);
                cell = worksheet.Cells[rowData, 1];
                cell.SetStyle(FnExcel.ApplyAreaStyle(cell.GetStyle(), true));
                cell.Value = area.name;
                // Price
                cell = worksheet.Cells[rowData, 10];
                cell.SetStyle(FnExcel.ApplyAreaStyle(cell.GetStyle()));
                cell.Value = $"{area.price:n0}";
                // Others
                cell = worksheet.Cells[rowData, 11];
                cell.SetStyle(FnExcel.ApplyAreaStyle(cell.GetStyle()));

                rowData++;

                var listDetailByArea = listDetail.Where(x => x.areaId == area.id).ToList();
                for (var d = 0; d < listDetailByArea.Count; d++)
                {
                    var isAutoFitRow = true;

                    var detail = listDetailByArea[d];
                    // Check Hình ảnh đầu tiên để xem có cần auto fit row hay shink to fit
                    // Hình ảnh minh hoạ
                    cell = worksheet.Cells[rowData, 2];
                    cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                    if (detail.Item != null && !string.IsNullOrWhiteSpace(detail.Item.image) && dictItemImage.ContainsKey(detail.Item.id))
                    {

                        var image = Image.FromStream(dictItemImage[detail.Item.id]);
                        image = FnUtil.ResizeImage(image);
                        worksheet.Cells.SetRowHeightPixel(rowData, image.Height + 10);

                        var streamImg = new MemoryStream();
                        image.Save(streamImg, ImageFormat.Jpeg);
                        worksheet.Pictures.Add(rowData, 2, streamImg);

                        isAutoFitRow = false;
                    }
                    // STT
                    cell = worksheet.Cells[rowData, 0];
                    cell.SetStyle(FnExcel.ApplyDefaultStyle(cell.GetStyle()));
                    cell.Value = d + 1;
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
                worksheet.Cells.SetColumnWidthPixel(2, 300);

            }
            return rowData;
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
        #endregion

    }
}

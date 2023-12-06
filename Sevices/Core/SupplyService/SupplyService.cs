using Data.DataAccess;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.SupplyService
{
    public class SupplyService: ISupplyService
    {
        private readonly AppDbContext _dbContext;

        public SupplyService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ResultModel GetByOrderId(Guid orderId)
        {
            var result = new ResultModel();

            var listStatusDamage = new List<ESupplyStatus>() {
                ESupplyStatus.Fail,
                ESupplyStatus.Missing,
            };

            try
            {
                var order = _dbContext.Order.Include(x => x.OrderDetails.Where(o => o.isDeleted != true)).ThenInclude(x => x.OrderDetailMaterials).FirstOrDefault(x => x.id == orderId);
                if (order == null)
                {
                    result.Code = 35;
                    result.ErrorMessage = "Không tìm thấy thông tin đơn hàng!";
                }
                else
                {
                    
                    // get from order
                    var dictOrder = new Dictionary<Guid, QuoteMaterialDetailModel>();
                    foreach (var detail in order.OrderDetails)
                    {
                        foreach (var odMate in detail.OrderDetailMaterials)
                        {
                            if (dictOrder.ContainsKey(odMate.materialId))
                            {
                                dictOrder[odMate.materialId].quantity += odMate.quantity;
                                dictOrder[odMate.materialId].totalPrice = dictOrder[odMate.materialId].quantity * dictOrder[odMate.materialId].price;
                            }
                            else
                            {
                                dictOrder.Add(odMate.materialId, new()
                                {
                                    materialId = odMate.materialId,
                                    name = odMate.materialName,
                                    sku = odMate.materialSku,
                                    supplier = odMate.materialSupplier,
                                    thickness = odMate.materialThickness,
                                    color = odMate.materialColor,
                                    unit = odMate.materialUnit,
                                    quantity = odMate.quantity,
                                    price = odMate.price,
                                    totalPrice = odMate.totalPrice,
                                });
                            }
                        }
                    }
                    
                    // get from supply
                    var dictSupply = new Dictionary<Guid, QuoteMaterialDetailModel>();
                    
                    var listReportByOrder = _dbContext.Report.Include(x => x.LeaderTask).Where(x => x.LeaderTask.orderId == order.id && x.status == ReportStatus.Approve || x.LeaderTask.orderId == order.id && x.status == ReportStatus.Provided).ToList();
                    
                    var listReportId = listReportByOrder.Select(x => x.id).ToList();

                    var listSupplyDamageByReport = _dbContext.Supply.Include(x => x.Material).Include(x => x.Report)
                                                                    .Where(x => listReportId.Contains(x.reportId) && listStatusDamage.Contains(x.status)).ToList();


                    foreach (var supply in listSupplyDamageByReport)
                    {
                        if (dictSupply.ContainsKey(supply.materialId))
                        {
                            dictSupply[supply.materialId].quantity += supply.amount;
                            dictSupply[supply.materialId].totalPrice += supply.totalPrice;
                        }
                        else
                        {
                            dictSupply.Add(supply.materialId, new()
                            {
                                materialId = supply.materialId,
                                name = supply.materialName,
                                sku = supply.materialSku,
                                supplier = supply.materialSupplier,
                                thickness = supply.materialThickness,
                                color = supply.materialColor,
                                unit = supply.materialUnit,
                                quantity = supply.amount,
                                price = supply.price,
                                totalPrice = supply.totalPrice,
                            });
                        }
                    }

                    var listFromSupplyDamage = dictSupply.Values.ToList();
                    double totalPriceSupplyDamage = listFromSupplyDamage.Sum(x => x.totalPrice);

                    double percentDamage = 0;
                    if (order.totalPrice > 0)
                    {
                        percentDamage = totalPriceSupplyDamage / order.totalPrice * 100;
                    }

                    result.Data = new QuoteMaterialOrderModel()
                    {
                        orderId = order.id,

                        totalPriceOrder = order.totalPrice,
                        listFromOrder = dictOrder.Values.ToList(),

                        totalPriceSupplyDamage = totalPriceSupplyDamage,
                        listFromSupplyDamage = listFromSupplyDamage,

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
    }
}

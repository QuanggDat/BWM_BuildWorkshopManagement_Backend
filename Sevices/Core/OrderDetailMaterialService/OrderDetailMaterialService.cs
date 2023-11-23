using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.EntityFrameworkCore;
using Sevices.Core.NotificationService;
using Sevices.Core.OrderDetailMaterialService;

namespace Sevices.Core.OrderReportService
{
    public class OrderDetailMaterialService : IOrderDetailMaterialService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderDetailMaterialService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ResultModel GetByOrderDetailIdWidthPaging(Guid orderDetailId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            try
            {
                var listData = _dbContext.OrderDetailMaterial.Where(x => x.orderDetailId == orderDetailId).ToList();

                // gộp vật liệu giống nhau
                var dict = new Dictionary<Guid, OrderDetailMaterial>();
                foreach (var data in listData)
                {
                    if (dict.ContainsKey(data.materialId))
                    {
                        dict[data.materialId].quantity += data.quantity;
                        dict[data.materialId].totalPrice += dict[data.materialId].totalPrice;
                    }
                    else
                    {
                        dict.Add(data.materialId, _mapper.Map<OrderDetailMaterial>(data));
                    }
                }
                listData = dict.Values.ToList();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchValue = FnUtil.Remove_VN_Accents(search).ToUpper();
                    listData = listData.Where(x =>
                                            (!string.IsNullOrWhiteSpace(x.materialName) && FnUtil.Remove_VN_Accents(x.materialName).ToUpper().Contains(searchValue)) ||
                                            (!string.IsNullOrWhiteSpace(x.materialSku) && FnUtil.Remove_VN_Accents(x.materialSku).ToUpper().Contains(searchValue))
                                        ).ToList();
                }

                var listDataPaging = listData.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                result.Data = new PagingModel()
                {
                    Data = _mapper.Map<List<OrderDetailMaterialModel>>(listDataPaging),
                    Total = listData.Count
                }; ;
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

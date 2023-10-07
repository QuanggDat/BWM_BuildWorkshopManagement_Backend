using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ManagerTaskService
{
    public class ManagerTaskService : IManagerTaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ManagerTaskService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ResultModel> CreatedManagerTask(CreateManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            if(model.orderId == Guid.Empty)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không nhận được order";
                return result;
            }
            if (string.IsNullOrEmpty(model.name))
            {
                result.Succeed = false;
                result.ErrorMessage = "Không nhận được tên công việc";
                return result;
            }
            if (string.IsNullOrEmpty(model.description))
            {
                result.Succeed = false;
                result.ErrorMessage = "Không nhận được mô tả";
                return result;
            }

            var check = await _dbContext.ManagerTask.SingleOrDefaultAsync(a => a.name == model.name && a.orderId == model.orderId && a.isDeleted == false);
            if (check != null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Công việc đã được tạo";
                return result;
            }               

            var managerTask = new ManagerTask
            {
                managerId = model.managerId,
                orderId = model.orderId,
                name = model.name,
                timeStart = model.timeStart,
                timeEnd = model.timeEnd,
                description = model.description,
                isDeleted = false
            };

            try
            {
                await _dbContext.ManagerTask.AddAsync(managerTask);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = managerTask.id;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
        
        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByOrderId(Guid orderId)
        {

            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.orderId == orderId && a.isDeleted == false).ToListAsync();
            if (managerTask == null) { 
                return null; 
            }
          
            foreach (var item in managerTask)
            {
                var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                var tmp = new ResponseManagerTaskModel
                {
                    managerId = item.managerId,
                    orderName = orderTmp.name,
                    createdBy = orderTmp.Assign.fullName,
                    name = item.name,
                    timeStart = item.timeStart,
                    timeEnd = item.timeEnd,
                    completedTime = item.completedTime,
                    description = item.description,
                    isDeleted = item.isDeleted,
                };
                result.Add(tmp);
            }
            return result;
        }
        
    }
}

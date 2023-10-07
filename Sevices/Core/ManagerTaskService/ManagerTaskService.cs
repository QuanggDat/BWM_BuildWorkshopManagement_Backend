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

        public async Task<ResultModel> CreatedManagerTask(Guid createById, CreateManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            if(model.orderId == Guid.Empty)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không nhận được đơn hàng";
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

            var orderTmp = await _dbContext.Order.FindAsync(model.orderId);
            if (orderTmp.status != Data.Enums.OrderStatus.InProgress)
            {
                result.Succeed = false;
                result.ErrorMessage = "Đơn hàng đang không tiến hành";
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
                createById = createById,
                orderId = model.orderId,
                name = model.name,
                timeStart = model.timeStart,
                timeEnd = model.timeEnd,
                description = model.description,
                status = model.status,
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

        public async Task<ResultModel> UpdateManagerTask(UpdateManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var managerTask = await _dbContext.ManagerTask.FindAsync(model.id);
            if (managerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy Mannager Task!";
                return result;
            }

            if (model.orderId == Guid.Empty)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không nhận được đơn hàng";
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

            var orderTmp = await _dbContext.Order.FindAsync(model.orderId);
            if (orderTmp.status != Data.Enums.OrderStatus.InProgress)
            {
                result.Succeed = false;
                result.ErrorMessage = "Đơn hàng đang không tiến hành";
                return result;
            }

            managerTask.orderId = model.orderId;
            managerTask.managerId = model.managerId;
            managerTask.name = model.name;
            managerTask.timeStart = model.timeStart;
            managerTask.timeEnd = model.timeEnd;
            managerTask.description = model.description;          

            try
            {
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
                var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                var createByTmp = await _dbContext.User.FindAsync(item.createById);
                var tmp = new ResponseManagerTaskModel
                {
                    createByName = createByTmp.fullName,
                    managerName = managerTmp.fullName,
                    orderName = orderTmp.name,
                    createdById = item.createById,
                    name = item.name,
                    timeStart = item.timeStart,
                    timeEnd = item.timeEnd,
                    status = item.status,
                    completedTime = item.completedTime,
                    description = item.description,
                    isDeleted = item.isDeleted,
                };
                result.Add(tmp);
            }
            return result;
        }

        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByManagerId(Guid managerId)
        {
            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.managerId == managerId && a.isDeleted == false).ToListAsync();
            if (managerTask == null)
            {
                return null;
            }

            foreach (var item in managerTask)
            {
                var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                var createByTmp = await _dbContext.User.FindAsync(item.createById);
                var tmp = new ResponseManagerTaskModel
                {
                    createByName = createByTmp.fullName,
                    managerName = managerTmp.fullName,
                    orderName = orderTmp.name,
                    createdById = item.createById,
                    name = item.name,
                    timeStart = item.timeStart,
                    timeEnd = item.timeEnd,
                    status = item.status,
                    description = item.description,
                    isDeleted = item.isDeleted,
                };
                result.Add(tmp);
            }
            return result;
        }

        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByFactory (Guid factoryId)
        {
            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.createById == factoryId && a.isDeleted == false).ToListAsync();
            if (managerTask == null)
            {
                return null;
            }

            foreach (var item in managerTask)
            {
                var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                var createByTmp = await _dbContext.User.FindAsync(item.createById);
                var tmp = new ResponseManagerTaskModel
                {
                    createByName = createByTmp.fullName,
                    managerName = managerTmp.fullName,
                    orderName = orderTmp.name,
                    createdById = item.createById,
                    name = item.name,
                    timeStart = item.timeStart,
                    timeEnd = item.timeEnd,
                    status = item.status,
                    description = item.description,
                    isDeleted = item.isDeleted,
                };
                result.Add(tmp);
            }
            return result;
        }

        public async Task<bool> UpdateManagerTaskStatus(Guid taskManagerId, TaskStatus status)
        {
            var task = await _dbContext.ManagerTask.FindAsync(taskManagerId);
            if (task == null)
            {
                return false;
            }

            task.status = status;

            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<int> DeleteManagerTask(Guid taskManagerId)
        {
            var check = await _dbContext.ManagerTask.FindAsync(taskManagerId);
            if (check == null)
            {
                return 1;
            }
            try
            {
                check.isDeleted = true;
                await _dbContext.SaveChangesAsync();
                return 2;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    
    }
}

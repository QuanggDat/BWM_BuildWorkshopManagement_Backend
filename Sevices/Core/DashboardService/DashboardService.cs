using Data.DataAccess;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.DashboardModel;

namespace Sevices.Core.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _dbContext;
       
        public DashboardService(AppDbContext dbContext)
        {
            _dbContext = dbContext;           
        }

        public ResultModel LeaderTaskDashboard()
        {
            ResultModel result = new ResultModel();
            try
            {
                var listStatusLeaderTask = _dbContext.LeaderTask.Select(x => x.status).Distinct().ToList();

                var list = new List<TaskDashboardModel>();
                foreach (var item in listStatusLeaderTask)
                {
                    var listLeaderTask = _dbContext.LeaderTask.Where(x => x.status == item).ToList();

                    var tmp = new TaskDashboardModel
                    {
                        taskStatus = item,
                        total = listLeaderTask.Count,
                    };
                    list.Add(tmp);
                }
                result.Data = result.Data = list;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel WorkerTaskDashboard(Guid leaderId)
        {
            throw new NotImplementedException();
        }

        public ResultModel OrderByMonthDashboard(int year)
        {
            ResultModel result = new ResultModel();
            try
            {
                var listMonthOrder = _dbContext.Order.Where(x => x.createTime.Year == year).Select(x => x.createTime.Month).Distinct().ToList();

                var list = new List<OrderByMonthDashboardModel>();
                foreach (var item in listMonthOrder)
                {
                    var listOrder = _dbContext.Order.Where(x => x.createTime.Month == item).ToList();

                    var tmp = new OrderByMonthDashboardModel
                    {
                        month = item,
                        total = listOrder.Count,
                    };
                    list.Add(tmp);
                }
                result.Data = result.Data = list;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }

        public ResultModel OrderDashboard()
        {
            ResultModel result = new ResultModel();
            try
            {
                var listStatusOrder = _dbContext.Order.Select(x => x.status).Distinct().ToList();

                var list = new List<OrderDashboardModel>();
                foreach (var item in listStatusOrder)
                {
                    var listOrder = _dbContext.Order.Where(x => x.status == item).ToList();

                    var tmp = new OrderDashboardModel
                    {
                        orderStatus = item,
                        total = listOrder.Count,
                    };
                    list.Add(tmp);
                }
                result.Data = result.Data = list;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;       
        }

        public ResultModel UserDashboard()
        {
            ResultModel result = new ResultModel();

            try
            {
                var listRole = _dbContext.Role
                   .OrderByDescending(x => x.Name).ToList();

                var list = new List<UseDashboardModel>();
                foreach (var item in listRole)
                {
                    var listUser = _dbContext.User.Where(x => x.roleId == item.Id && x.banStatus == false).ToList();

                    var tmp = new UseDashboardModel
                    {
                        roleId = item.Id,
                        roleName = item.Name,
                        totalUser = listUser.Count,
                    };
                    list.Add(tmp);
                }
                result.Data = list;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}

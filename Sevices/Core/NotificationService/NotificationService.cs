using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using SignalRHubs.Hubs.NotificationHub;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly INotificationHub _notiHub;

        public NotificationService(AppDbContext dbContext, IMapper mapper, INotificationHub notiHub)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _notiHub = notiHub;
        }

        public ResultModel Create(Notification model)
        {
            var result = new ResultModel();
            try
            {
                model.createdDate = DateTime.UtcNow.AddHours(7); ;
                _dbContext.Notification.Add(model);
                _dbContext.SaveChanges();

                var data = new NewNotificationModel()
                {
                    Notification = _mapper.Map<NotificationModel>(model),
                    CountUnseen = CountUnseen(model.userId),
                };

                _notiHub.NewNotification(model.userId.ToString(), data);

                result.Data = true;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel CreateForManyUser(Notification model, List<Guid> listUserId)
        {
            var result = new ResultModel();
            try
            {
                var listNotiCreated = new List<Notification>();
                foreach (var userId in listUserId)
                {
                    model.userId = userId;
                    model.createdDate = DateTime.UtcNow.AddHours(7); 

                    _dbContext.Notification.Add(model);
                    listNotiCreated.Add(model);
                }
                _dbContext.SaveChanges();

                foreach (var noti in listNotiCreated)
                {
                    var data = new NewNotificationModel()
                    {
                        Notification = _mapper.Map<NotificationModel>(noti),
                        CountUnseen = CountUnseen(noti.userId),
                    };

                    _notiHub.NewNotification(noti.userId.ToString(), data);
                }

                result.Data = true;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByUserLogin(Guid userId)
        {
            var result = new ResultModel();
            try
            {
                var listNoti = _dbContext.Notification.Where(x => !x.isDeleted && x.userId == userId).OrderByDescending(x => x.createdDate).ToList();

                result.Data = new
                {
                    listSeen = _mapper.Map<List<NotificationModel>>(listNoti.Where(x => x.seen).ToList()),
                    listUnseen = _mapper.Map<List<NotificationModel>>(listNoti.Where(x => !x.seen).ToList()),
                };
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel MarkAllSeen()
        {
            var result = new ResultModel();
            try
            {
                var listNoti = _dbContext.Notification.Where(x => !x.isDeleted && !x.seen).ToList();
                foreach (var noti in listNoti)
                {
                    noti.seen = true;
                }
                _dbContext.Notification.UpdateRange(listNoti);
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

        public ResultModel MarkSeenById(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var noti = _dbContext.Notification.FirstOrDefault(x => x.id == id);
                if (noti != null)
                {
                    noti.seen = true;
                    _dbContext.Notification.Update(noti);
                    _dbContext.SaveChanges();

                    result.Data = true;
                    result.Succeed = true;
                }
                else
                {
                    result.Code = 88;
                    result.ErrorMessage = "Không tìm thấy thông báo!";
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel Delete(Guid id)
        {
            var result = new ResultModel();
            try
            {
                var noti = _dbContext.Notification.FirstOrDefault(x => x.id == id);
                if (noti != null)
                {
                    noti.isDeleted = true;
                    _dbContext.Notification.Update(noti);
                    _dbContext.SaveChanges();

                    result.Data = true;
                    result.Succeed = true;
                }
                else
                {
                    result.Code = 88;
                    result.ErrorMessage = "Không tìm thấy thông báo!";
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        #region PRIVATE 
        private int CountUnseen(Guid userId)
        {
            try
            {
                return _dbContext.Notification.Count(x => !x.isDeleted && !x.seen && x.userId == userId);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}

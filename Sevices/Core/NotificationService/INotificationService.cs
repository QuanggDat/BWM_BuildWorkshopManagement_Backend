using Data.Entities;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.NotificationService
{
    public interface INotificationService
    {
        ResultModel Create(Notification model);
        ResultModel CreateForManyUser(Notification model, List<Guid> listUserId);
        ResultModel GetByUserLogin(Guid userId);
        ResultModel MarkAllSeen();
        ResultModel MarkSeenById(Guid id);
        ResultModel Delete(Guid id);
    }
}

using Data.Models;

namespace SignalRHubs.Hubs.NotificationHub
{
    public interface INotificationHub
    {
        Task NewNotification(string userId, NewNotificationModel model);
    }
}

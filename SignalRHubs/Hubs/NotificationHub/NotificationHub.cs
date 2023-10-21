using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRHubs.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRHubs.Hubs.NotificationHub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub, INotificationHub
    {
        public IHubContext<NotificationHub> Current { get; set; }
        public static ConcurrentDictionary<string, List<string>> GroupConnections = new();

        public NotificationHub(IHubContext<NotificationHub> current)
        {
            Current = current;
        }

        public async Task NewNotification(string userId, NewNotificationModel model)
        {
            await EmitSignal("newNotify", userId, model);
        }

        private async Task EmitSignal(string signalName, string group, object data)
        {
            try
            {
                GroupConnections.TryGetValue(group, out var receiverConnectionIds);
                await Current.Clients.Clients(receiverConnectionIds!).SendAsync(signalName, data);
            }
            catch (Exception) { }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                Trace.TraceInformation("MapHub started. ID: {0}", Context.ConnectionId);

                var userId = Context.User!.GetId();
                GroupConnections.TryGetValue(userId, out var existingUserConnectionIds);
                existingUserConnectionIds ??= new List<string>();
                existingUserConnectionIds.Add(Context.ConnectionId);
                GroupConnections.TryAdd(userId, existingUserConnectionIds);
            }
            catch (Exception) { }
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User!.GetId();
                if (GroupConnections.TryGetValue(userId, out var existingUserConnectionIds))
                {
                    existingUserConnectionIds.Remove(Context.ConnectionId);
                    if (!existingUserConnectionIds.Any())
                    {
                        GroupConnections.TryRemove(userId, out _);
                    }
                }
            }
            catch (Exception) { }
            return base.OnDisconnectedAsync(exception);
        }
    }

}

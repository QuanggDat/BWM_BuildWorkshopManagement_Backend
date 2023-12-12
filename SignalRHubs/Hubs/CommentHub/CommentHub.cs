using Data.Entities;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRHubs.Extensions;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRHubs.Hubs.CommentHub
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentHub : Hub, ICommentHub
    {
        public IHubContext<CommentHub> Current { get; set; }
        public static ConcurrentDictionary<string, List<string>> GroupConnections = new();

        public CommentHub(IHubContext<CommentHub> current)
        {
            Current = current;
        }

        public async Task ChangeComment(List<Guid> listUserId, CommentModel model)
        {
            await EmitMultiSignal(model.workerTaskId.ToString(), listUserId.ConvertAll(id => id.ToString()), model);
        }

        //private async Task EmitSignal(string signalName, string group, object data)
        //{
        //    try
        //    {
        //        GroupConnections.TryGetValue(group, out var receiverConnectionIds);
        //        await Current.Clients.Clients(receiverConnectionIds!).SendAsync(signalName, data);
        //    }
        //    catch (Exception) { }
        //}
        
        private async Task EmitMultiSignal(string signalName, List<string> groups, object data)
        {
            try
            {
                var connectionIds = new List<string>();
                foreach (var gr in groups)
                {
                    GroupConnections.TryGetValue(gr, out var values);

                    if(values != null && values.Any())
                    {
                        connectionIds.AddRange(values);
                    }
                }
                await Current.Clients.Clients(connectionIds).SendAsync(signalName, data);
            }
            catch (Exception) { }
        }

        public void DisconnectFromtWorkerTask(Guid workerTaskId)
        {
            try
            {
                var workerTaskIdStr = workerTaskId.ToString();
                if (GroupConnections.TryGetValue(workerTaskIdStr, out var existingUserConnectionIds))
                {
                    existingUserConnectionIds.Remove(Context.ConnectionId);
                    if (!existingUserConnectionIds.Any())
                    {
                        GroupConnections.TryRemove(workerTaskIdStr, out _);
                    }
                }
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

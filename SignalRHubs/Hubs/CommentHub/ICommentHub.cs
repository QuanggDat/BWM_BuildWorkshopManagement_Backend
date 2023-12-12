using Data.Models;

namespace SignalRHubs.Hubs.CommentHub
{
    public interface ICommentHub
    {
        Task ChangeComment(List<Guid> listUserId, CommentModel model);
    }
}

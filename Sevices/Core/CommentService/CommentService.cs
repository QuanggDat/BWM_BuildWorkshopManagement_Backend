using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using SignalRHubs.Hubs.CommentHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICommentHub _commentHub;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext dbContext, ICommentHub commentHub, IMapper mapper)
        {
            _dbContext = dbContext;
            _commentHub = commentHub;
            _mapper = mapper;
        }

        public ResultModel Create(Guid userId, CreateCommentModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = _dbContext.WorkerTask
                .Include(x => x.LeaderTask)
                .Include(x => x.WorkerTaskDetails)
                .FirstOrDefault(x => x.id == model.workerTaskId);

            if (workerTask == null)
            {
                result.Code = 89;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin công việc công nhân!";
            }
            else
            {
                if (workerTask.status == EWorkerTaskStatus.Completed)
                {
                    result.Code = 90;
                    result.Succeed = false;
                    result.ErrorMessage = "Công việc đã hoàn thành, không thể gửi bình luận!";
                }
                else
                {
                    var comment = new Comment
                    {
                        workerTaskId = model.workerTaskId,
                        userId = userId,
                        commentContent = model.commentContent,
                        commentTime = DateTime.UtcNow.AddHours(7),
                        isDeleted = false
                    };

                    try
                    {
                        _dbContext.Comment.Add(comment);

                        if (model.resource != null)
                        {
                            foreach (var resource in model.resource)
                            {
                                _dbContext.Resource.Add(new Resource
                                {
                                    commentId = comment.id,
                                    link = resource
                                });
                            }
                        }

                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = comment.id;

                        var listUserId = workerTask.WorkerTaskDetails.Select(x => x.userId).Distinct().ToList();
                        if (workerTask.LeaderTask != null && workerTask.LeaderTask.leaderId != null)
                        {
                            listUserId.Add(workerTask.LeaderTask.leaderId.Value);
                        }
                        _commentHub.ChangeComment(listUserId, _mapper.Map<CommentModel>(GetById(comment.id)));
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }

        public ResultModel Update(UpdateCommentModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var checkComment = _dbContext.Comment
                .Include(x => x.WorkerTask).ThenInclude(x => x.WorkerTaskDetails)
                .Include(x => x.WorkerTask).ThenInclude(x => x.LeaderTask)
                .FirstOrDefault(x => x.id == model.id && x.isDeleted != true);

            if (checkComment == null)
            {
                result.Code = 99;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin bình luận!";
            }
            else
            {
                checkComment.commentContent = model.commentContent;

                // Remove all old resource
                var currentResources = _dbContext.Resource.Where(x => x.commentId == checkComment.id).ToList();

                if (currentResources != null && currentResources.Count > 0)
                {
                    _dbContext.Resource.RemoveRange(currentResources);
                }

                if (model.resource != null)
                {
                    foreach (var resource in model.resource)
                    {
                        _dbContext.Resource.Add(new Resource
                        {
                            commentId = checkComment.id,
                            link = resource
                        });
                    }
                }
                try
                {
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = checkComment.id;

                    EmitSignalr(checkComment);
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            return result;
        }

        public ResultModel Delete(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var checkComment = _dbContext.Comment
                .Include(x => x.WorkerTask).ThenInclude(x => x.WorkerTaskDetails)
                .Include(x => x.WorkerTask).ThenInclude(x => x.LeaderTask)
                .FirstOrDefault(x => x.id == id && x.isDeleted != true);

                if (checkComment == null)
                {
                    result.Code = 99;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin bình luận!";
                }
                else
                {
                    checkComment.isDeleted = true;
                    _dbContext.SaveChanges();

                    result.Data = checkComment.id;
                    result.Succeed = true;

                    EmitSignalr(checkComment);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public ResultModel GetByWorkerTaskId(Guid workerTaskId)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            try
            {
                var comments = _dbContext.Comment
                    .Include(x => x.Resources)
                    .Include(x => x.User).ThenInclude(x => x.Role)
                    .Where(x => !x.isDeleted && x.workerTaskId == workerTaskId)
                    .OrderByDescending(x => x.commentTime).ToList();

                var listComment = new List<CommentModel>();
                foreach (var cmt in comments)
                {
                    var cmtModel = _mapper.Map<CommentModel>(cmt);
                    cmtModel.resource = cmt.Resources.Select(x => x.link).Distinct().ToList();
                    listComment.Add(cmtModel);
                }

                result.Data = listComment;
                result.Succeed = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        private Comment? GetById(Guid id)
        {
            return _dbContext.Comment
                   .Include(x => x.Resources)
                   .Include(x => x.User).ThenInclude(x => x.Role)
                   .FirstOrDefault(x => x.id == id);
        }

        private void EmitSignalr(Comment comment)
        {
            var workerTask = _dbContext.WorkerTask
                .Include(x => x.LeaderTask)
                .Include(x => x.WorkerTaskDetails)
                .FirstOrDefault(x => x.id == comment.workerTaskId);

            if (workerTask != null)
            {
                var listUserId = workerTask.WorkerTaskDetails.Select(x => x.userId).Distinct().ToList();
                if (workerTask.LeaderTask != null && workerTask.LeaderTask.leaderId != null)
                {
                    listUserId.Add(workerTask.LeaderTask.leaderId.Value);
                }
                if (comment != null)
                {
                    _commentHub.ChangeComment(listUserId, _mapper.Map<CommentModel>(GetById(comment.id)));
                }
            }
        }
    }
}

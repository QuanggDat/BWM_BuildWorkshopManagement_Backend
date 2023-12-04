using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
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

        public CommentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResultModel Create(Guid userId, CreateCommentModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var workerTask = _dbContext.WorkerTask.Where(x => x.id == model.workerTaskId).FirstOrDefault();

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

            var checkComment = _dbContext.Comment.Where(x => x.id == model.id && x.isDeleted != true).FirstOrDefault();

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
                var checkComment = _dbContext.Comment.Where(x => x.id == id && x.isDeleted != true).FirstOrDefault();

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
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }       
    }
}

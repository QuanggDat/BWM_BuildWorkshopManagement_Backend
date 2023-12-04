using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.CommentService
{
    public interface ICommentService
    {
        ResultModel Create(Guid userId, CreateCommentModel model);
        ResultModel Update(UpdateCommentModel model);
        ResultModel Delete(Guid id);
    }
}

using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CommentModel
    {
        public Guid id { get; set; }
        public Guid workerTaskId { get; set; }
        public Guid userId { get; set; }
        public UserCommentModel User { get; set; } = null!;
        public string? commentContent { get; set; }
        public DateTime commentTime { get; set; }
        public bool isDeleted { get; set; }
        public List<string>? resource { get; set; }
    }
    public class CreateCommentModel
    {
        public Guid workerTaskId { get; set; }
        public string? commentContent { get; set; }
        public List<string>? resource { get; set; }
    }
    public class UpdateCommentModel
    {
        public Guid id { get; set; }
        public string? commentContent { get; set; }
        public List<string>? resource { get; set; }
    }
}

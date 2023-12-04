using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        
        [ForeignKey("workerTaskId")]
        public Guid workerTaskId { get; set; }
        public virtual WorkerTask WorkerTask { get; set; } = null!;

        [ForeignKey("userId")]
        public Guid userId { get; set; }
        public virtual User User { get; set; } = null!;

        public string? commentContent { get; set; } 
        public DateTime commentTime { get; set; }
        public bool isDeleted { get; set; }
        public virtual List<Resource> Resources { get; set; } = new();
    }
}

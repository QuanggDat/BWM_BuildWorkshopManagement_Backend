using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        //public DateTime DateCreated { get; set; } = DateTime.Now;
        //public DateTime DateUpdated { get; set; } = DateTime.Now;
        //public bool IsDisable { get; set; } = false;
        //public bool IsDeleted { get; set; } = false;
    }
}

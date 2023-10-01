using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    // đây sẽ là class xài chung cho các entity
    // chỉ những prop nào mà chắc chắn trong mọi entity sẽ phải có như là id thì sẽ thêm vào đây
    public class BaseEntity
    {
        // thư viện sẽ tự động gen ra id 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        //public bool isDeleted { get; set; } = false;
        //public DateTime dateCreated { get; set; } = DateTime.Now;
        //public DateTime dateUpdated { get; set; } = DateTime.Now;
    }
}

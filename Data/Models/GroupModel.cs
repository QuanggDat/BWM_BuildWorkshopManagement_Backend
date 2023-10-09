using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public  class GroupModel
    {
        public Guid id {  get; set; }
        public Guid squadId { get; set; }
    }

    public class AddGroupModel
    {
        public Guid squadId { get; set; }
    }
}

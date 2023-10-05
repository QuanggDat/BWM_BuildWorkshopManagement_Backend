using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Request = 1,
        Approve = 2,
        InProgress = 3,
        Cancel = 4,
        Completed = 5
    }
}

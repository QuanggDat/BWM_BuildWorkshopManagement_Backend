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
        Reject = 2,
        Approve = 3,
        InProgress = 4,
        Cancel = 5,
        Completed = 6
    }
}

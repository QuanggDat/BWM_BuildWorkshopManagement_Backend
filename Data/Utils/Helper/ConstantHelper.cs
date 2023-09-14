using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Utils.Helper
{
    public class ConstantHelper
    {
        public static class FixedPagingConstant
        {
            /// LimitationPageSize
            public const int MaxPageSize = 500;
        }

        public enum OrderCriteria
        {
            // descendant
            DESC,
            
            // ascendant
            ASC,
        }
    }
}

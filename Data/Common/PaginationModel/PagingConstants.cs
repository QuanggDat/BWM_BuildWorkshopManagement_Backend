using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common.PaginationModel
{
    public static class PagingConstants
    {
        public const int DefaultPage = 1;
        public const int DefaultPageSize = 50;
        //public const string DefaultSort = "id_asc";
        public const int MinPage = 0;
        public const int MinPageSize = 3;
        public const int MaxPageSize = 250;
    }
}

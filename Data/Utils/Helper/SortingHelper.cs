using Data.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Utils.Paging
{
    public static class SortingHelper
    {
        public static IQueryable<TObject> GetWithSorting<TObject>(this IQueryable<TObject> source,
            string sortKey, ConstantHelper.OrderCriteria sortOrder)
            where TObject : class
        {
            if (source == null) return Enumerable.Empty<TObject>().AsQueryable();

            if (sortKey != null)
            {
                var param = Expression.Parameter(typeof(TObject), "p");
                var prop = Expression.Property(param, sortKey);
                var exp = Expression.Lambda(prop, param);
                string method = "";
                switch (sortOrder)
                {
                    case ConstantHelper.OrderCriteria.ASC:
                        method = "OrderBy";
                        break;
                    default:
                        method = "OrderByDescending";
                        break;
                }
                Type[] types = new Type[] { source.ElementType, exp.Body.Type };
                var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
                return source.Provider.CreateQuery<TObject>(mce);

            }
            return source;
        }
    }
}

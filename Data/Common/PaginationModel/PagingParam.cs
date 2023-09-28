using Data.Utils.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Common.PaginationModel
{
    public class PagingParam<TKey> where TKey : System.Enum
    {
        private int _page = PagingConstants.DefaultPage;

        /// Gets or sets current page number.
        public int PageIndex
        {
            get => _page;
            set => _page = (value);
        }

        /// Gets or sets size of current page.
        [DefaultValue(PagingConstants.DefaultPageSize)]
        public int PageSize { get; set; } = PagingConstants.DefaultPageSize;

        [Description("Parameter use for sorting result. Value: {propertyName}")]
        public TKey SortKey { get; set; } = default(TKey);

        /// Gets or sets ordering criteria.
        [EnumDataType(typeof(ConstantHelper.OrderCriteria))]
        [JsonConverter(typeof(ConstantHelper.OrderCriteria))]
        public ConstantHelper.OrderCriteria SortOrder { get; set; }
    }
}

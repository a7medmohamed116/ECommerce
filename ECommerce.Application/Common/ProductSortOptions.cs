using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Application.Common
{
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductSortOptions
    {
        None,
        NameAscending,
        NameDescending,
        PriceAscending,
        PriceDescending
    }
}

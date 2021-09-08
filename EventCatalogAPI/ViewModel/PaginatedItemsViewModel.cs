using EventCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.ViewModel
{
    public class PaginatedItemsViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } //result count
        public long Count { get; set; } //total count
        public IEnumerable<EachEvent> Data { get; set; }
    }
}

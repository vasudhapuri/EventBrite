using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class EventPagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } //result count
        public long Count { get; set; } //total count
        public IEnumerable<EachEvent> Data { get; set; }
    }
}

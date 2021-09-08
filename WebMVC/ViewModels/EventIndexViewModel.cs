using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.ViewModels
{
    public class EventIndexViewModel
    {
        public IEnumerable<SelectListItem> Types { get; set; } //types dropdown
        public IEnumerable<SelectListItem> Locations { get; set; } //locations dropdown
        public IEnumerable<EachEvent> EachEvents { get; set; } //items displayed in page
        public PaginationInfo PaginationInfo { get; set; } //page info

        //tracks what the user had selected before
        public int? LocationsFilterApplied { get; set; }
        public int? TypesFilterApplied { get; set; }
    }
}


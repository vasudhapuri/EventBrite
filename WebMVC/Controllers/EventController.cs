using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _service;
        public EventController(IEventService service) //injecting the service
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? page, int? typesFilterApplied, int? locationsFilterApplied)
        {
            var itemsOnPage = 10; //hardcoding the pageSize.

            var catalog = await _service.GetEachEventAsync(page ?? 0, itemsOnPage, typesFilterApplied, locationsFilterApplied); //?? is simplified version of if statement for nullable check

            var vm = new EventIndexViewModel
            {
                Types = await _service.GetTypesAsync(),
                Locations = await _service.GetLocationsAsync(), //data for dropdown. making a service call to get the data
                EachEvents = catalog.Data,  //the middle section of the page
                PaginationInfo = new PaginationInfo
                {
                    TotalItems = catalog.Count,
                    ItemsPerPage = catalog.PageSize,
                    ActualPage = page ?? 0, //if null then 0th page otherwise whatever page it is

                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                    //Count is long value, itemsOnPage is an int. this will lose the round value when divided. eg: 33/10 = 3 pages but we need 4 pages
                    //typecaste one of them, this case numerator into decimal, then Ceiling func will round it to next whole number
                    //then converting it back to an int cuz total page is an int and result was
                },

                TypesFilterApplied = typesFilterApplied ?? 0,  //whatever user selected, if it is null then set it to 0
                LocationsFilterApplied = locationsFilterApplied ?? 0
            };
            return View(vm);
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your Application description page.";

            return View();
        }
    }
}

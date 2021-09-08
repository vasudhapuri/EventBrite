using EventCatalogAPI.Data;
using EventCatalogAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsByDateController : ControllerBase
    {
        private readonly EventContext _context;
        private readonly IConfiguration _config;

        public EventsByDateController(EventContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByDateRange()
        //[FromQuery] int minMonth,
        //[FromQuery] int minDay,
        //[FromQuery] int minYear,
        //[FromQuery] int maxMonth,
        //[FromQuery] int maxDay,
        //[FromQuery] int maxYear

        {
            //Events occuring within a range of dates selected by a user
            int minMonth = 9;
            int minDay = 1;
            int minYear = 2021;
            int maxMonth = 9;
            int maxDay = 30;
            int maxYear = 2021;

            DateTime minDate = new DateTime(minYear, minMonth, minDay);
            DateTime maxDate = new DateTime(maxYear, maxMonth, maxDay);
            var query = (IQueryable<EachEvent>)_context.Events;

            query = query.Where(t => t.Date > minDate);
            query = query.Where(t => t.Date < maxDate);

            var items = await query
               .OrderBy(t => t.EventName)
               .ToListAsync();

            items = ChangePictureUrl(items);
            return Ok(items);
        }

        // Get all the events coming up next weekend
        [HttpGet("[action]")]
        public async Task<IActionResult> NextWeekend()
        {
            DateTime today = DateTime.Today;
            //DateTime today = new DateTime(2021, 08, 22);
            DateTime weekendDay1 = today;
            DateTime weekendDay2 = today;

            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                weekendDay1 = today.AddDays(5);
                weekendDay2 = today.AddDays(6);
            }
            else if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                weekendDay1 = today.AddDays(4);
                weekendDay2 = today.AddDays(5);
            }
            else if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                weekendDay1 = today.AddDays(3);
                weekendDay2 = today.AddDays(4);
            }
            else if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                weekendDay1 = today.AddDays(2);
                weekendDay2 = today.AddDays(3);
            }
            else if (today.DayOfWeek == DayOfWeek.Friday)
            {
                weekendDay1 = today.AddDays(1);
                weekendDay2 = today.AddDays(2);
            }
            else if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                weekendDay1 = today.AddDays(0);
                weekendDay2 = today.AddDays(1);
            }
            else if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                weekendDay1 = today.AddDays(0);
                weekendDay2 = today.AddDays(7);
            }

            var query = (IQueryable<EachEvent>)_context.Events;

            query = query.Where(t => t.Date.Date == weekendDay1.Date || t.Date.Date == weekendDay2.Date);


            var items = await query
               .OrderBy(t => t.EventName)
               .ToListAsync();

            items = ChangePictureUrl(items);
            return Ok(items);
        }
        private List<EachEvent> ChangePictureUrl(List<EachEvent> items)
        {
            items.ForEach(item =>
            item.PictureUrl =
            item.PictureUrl.Replace(
                "http://externalcatalogbaseurltobereplaced", _config["ExternalCatalogUrl"]));
            return items;
        }
    }
}

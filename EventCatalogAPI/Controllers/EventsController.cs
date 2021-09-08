using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EventCatalogAPI.Data;
using EventCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogAPI.ViewModel;

namespace EventCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventContext _context;
        private readonly IConfiguration _config;

        public EventsController(EventContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Items(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 6)
        {
            var itemsCount = _context.Events.LongCountAsync();
            var items = await _context.Events
                  //.OrderBy (c=>c.EventName)
                  .Skip(pageIndex * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items
            };

            return Ok(model);
        }


        //Queryable filters

        //[HttpGet("[action]/type/{typeId}/location/{locationId}/zip/{zipCode}")]
        //public async Task<IActionResult> Items(
        //    int? typeId,
        //    int? locationId,
        //    //string zipCode,
        //    [FromQuery] int pageIndex = 0,
        //    [FromQuery] int pageSize = 6)
        //{
        //    var query = (IQueryable<EachEvent>)_context.Events;
        //    if (typeId.HasValue)
        //    {
        //        query = query.Where(c => c.TypeId == typeId);
        //    }
        //    if (locationId.HasValue)
        //    {
        //        query = query.Where(c => c.LocationId == locationId);
        //    }
        //    //if (!string.IsNullOrWhiteSpace(zipCode))
        //    //{
        //    //    query = query.Where(c => c.Zip == zipCode);
        //    //}

        //    var itemsCount = _context.Events.LongCountAsync();
        //    var items = await _context.Events
        //          //.OrderBy (c=>c.EventName)
        //          .Skip(pageIndex * pageSize)
        //          .Take(pageSize)
        //          .ToListAsync();

        //    items = ChangePictureUrl(items);

        //    var model = new PaginatedItemsViewModel
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = items.Count,
        //        Count = itemsCount.Result,
        //        Data = items
        //    };

        //    return Ok(model);
        //}

        //Route for Type and Location filters
        [HttpGet("[action]/type/{typeId}/location/{locationId}")]
        public async Task<IActionResult> Items(
           int? typeId,
           int? locationId,
           [FromQuery] int pageIndex = 0,
           [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<EachEvent>)_context.Events;
            if (typeId.HasValue)
            {
                query = query.Where(c => c.TypeId == typeId);
            }
            if (locationId.HasValue)
            {
                query = query.Where(c => c.LocationId == locationId);
            }
            

            var itemsCount = query.LongCountAsync();
            var items = await query
                  .OrderBy (c=>c.EventName)
                  .Skip(pageIndex * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items
            };

            return Ok(model);
        }

        //Route for Types filter
        [HttpGet("[action]/{typeId}")]
        public async Task<IActionResult> GetByType(
           int? typeId,
           

           [FromQuery] int pageIndex = 0,
           [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<EachEvent>)_context.Events;
            if (typeId.HasValue)
            {
                query = query.Where(c => c.TypeId == typeId);
            }

            var itemsCount = query.LongCountAsync();
            var items = await query
                  .OrderBy(c => c.EventName)
                  .Skip(pageIndex * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items
            };

            return Ok(model);
        }

        //Route for Location filter
        [HttpGet("[action]/{locationId}")]
        public async Task<IActionResult> GetByLocation(
           int? locationId,

           [FromQuery] int pageIndex = 0,
           [FromQuery] int pageSize = 6)
        {
            var query = (IQueryable<EachEvent>)_context.Events;
            if (locationId.HasValue)
            {
                query = query.Where(c => c.LocationId == locationId);
            }


            var itemsCount = query.LongCountAsync();
            var items = await query
                  .OrderBy(c => c.EventName)
                  .Skip(pageIndex * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            items = ChangePictureUrl(items);

            var model = new PaginatedItemsViewModel
            {
                PageIndex = pageIndex,
                PageSize = items.Count,
                Count = itemsCount.Result,
                Data = items
            };

            return Ok(model);
        }

        private List<EachEvent> ChangePictureUrl(List<EachEvent> items)
        {
            items.ForEach(item =>
              item.PictureUrl =
              item.PictureUrl.Replace(
                  "http://externalcatalogbaseurltobereplaced", _config["ExternalCatalogUrl"]));
            return items;
        }


        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetByType()
        //    [FromQuery] int typeId = 0
        //{
        //    var types = await this._context.Events.Where(x => x.TypeId == typeId).ToListAsync();
        //    return Ok(types);
        //}

        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetByLocation()
        //   //[FromQuery] int locationId = 0)
        //{
        //    //var locations = await this._context.Events.Where(x => x.LocationId == locationId).ToListAsync();

        //    var locations = await _context.EventLocations.ToListAsync();
        //    return Ok(locations);
        //}

        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetByZipCode()
        //   //[FromQuery] string zipcode = "0")
        //{
        //      //var zip = await this._context.Events.Where(x => x.Zip == zipcode).ToListAsync();

           
        //    return Ok(zip);
        //}


        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetByPopularity(
        //   [FromQuery] int likes = 0)
        //{
        //    var items = await this._context.Events.Where(x => x.Likes == likes).ToListAsync();
        //    return Ok(items);
        //}



        //Dropdown method for Event Types Filter
        [HttpGet("[action]")]
        public async Task<IActionResult> EventTypes()
        {
            var types = await _context.EventTypes.ToListAsync();
            return Ok(types);
        }

        //Dropdown method for Event Locations Filter
        [HttpGet("[action]")]
        public async Task<IActionResult> EventLocations()
        {
            var locations = await _context.EventLocations.ToListAsync();
            return Ok(locations);
        }
    }
}

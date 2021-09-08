using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class EventService : IEventService
    {
        private readonly string _baseUrl;
        private readonly IHttpClient _client;
        public EventService(IConfiguration config, IHttpClient client)
        {
            _baseUrl = $"{config["EventUrl"]}/api/events/"; //http://localhost:7006/api/events
            _client = client;
        }
        public async Task<EventPagination> GetEachEventAsync(int page, int size, int? type, int? location)
        {
            var eachEventsUri = ApiPaths.Event.GetAllEvents(_baseUrl, page, size, type, location);
            var dataString = await _client.GetStringAsync(eachEventsUri);
            return JsonConvert.DeserializeObject<EventPagination >(dataString); //(File.ReadAllText(datastring))
        }

        public async Task<IEnumerable<SelectListItem>> GetLocationsAsync()
        {
            var locationUri = ApiPaths.Event.GetAllLocations(_baseUrl);
            var dataString = await _client.GetStringAsync(locationUri);

            var items = new List<SelectListItem> //list of items for dropdown
            {
                new SelectListItem //first item added manually to list
                {
                    Value=null,
                    Text="All",
                    Selected = true
                }
            };

            var locations = JArray.Parse(dataString); //parse json array into regular array

            //looping through location regular array and adding to the list
            foreach (var location in locations)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = location.Value<string>("id"),
                        Text = location.Value<string>("city")
                    });
            }
            return items; //returning a liat of select list items
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = ApiPaths.Event.GetAllTypes(_baseUrl);
            var dataString = await _client.GetStringAsync(typeUri);

            var items = new List<SelectListItem>
            {
                new SelectListItem 
                {
                    Value=null,
                    Text="All",
                    Selected = true
                }
            };

            var types = JArray.Parse(dataString); //parse json array into regular array

            foreach (var type in types)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = type.Value<string>("id"),
                        Text = type.Value<string>("typeName")
                    });
            }
            return items; //returning a liat of select list items
        }
    
    }
}

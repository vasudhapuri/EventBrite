using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IEventService
    {
        Task<EventPagination> GetEachEventAsync(int page, int size, int? type, int? location);
        Task<IEnumerable<SelectListItem>> GetTypesAsync(); //Types DropDown 
        Task<IEnumerable<SelectListItem>> GetLocationsAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using WebMVC.Services;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using Polly.CircuitBreaker;

namespace WebMvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        
        private readonly ICartService _cartService;
        private readonly IEventService _eventCatalogService; //cuz it fetch data from catalog to add in cart
        private readonly IIdentityService<ApplicationUser> _identityService;

        public CartController(IIdentityService<ApplicationUser> identityService,
            ICartService cartService, IEventService eventCatalogService)
        {
            _identityService = identityService;
            _cartService = cartService;
            _eventCatalogService = eventCatalogService;
        }
        public    IActionResult  Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantities, string action)
        {
            if (action == "[ Checkout ]")
            {
                return RedirectToAction("Create", "Order");
            }


            try
            {
                var user = _identityService.Get(HttpContext.User);
                var basket = await _cartService.SetQuantities(user, quantities);
                var vm = await _cartService.UpdateCart(basket);

            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in open circuit  mode                 
                HandleBrokenCircuitException();
            }

            return View();

        }

            public async Task<IActionResult> AddToCart(EachEvent eventDetails)
        {
            try
            {
                if (eventDetails.Id > 0)
                {
                    var user = _identityService.Get(HttpContext.User);
                    var product = new CartItem()
                    {
                        Id = Guid.NewGuid().ToString(), //Guid: globally unique identifier
                        Quantity = 1,
                        EventName = eventDetails.EventName,
                        PictureUrl = eventDetails.PictureUrl,
                        UnitPrice = eventDetails.Price,
                        EventId = eventDetails.Id.ToString()
                    };
                    await _cartService.AddItemToCart(user, product);
                }
                return RedirectToAction("Index", "Event");
            }
            catch (BrokenCircuitException)
            {
                // Catch error when CartApi is in circuit-opened mode                 
                HandleBrokenCircuitException();
            }

            return RedirectToAction("Index", "Event");

        }

        private void HandleBrokenCircuitException()
        {
            TempData["BasketInoperativeMsg"] = "cart Service is inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }

    }
}
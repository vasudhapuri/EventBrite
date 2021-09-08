using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller  //this should happen after the call to token service
    {
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            var user = User as ClaimsPrincipal;
            //asking for token
            var token = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            foreach (var claim in user.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value : {claim.Value}");
            }

            if (token != null)
            {
                ViewData["access_token"] = token;

            }
            if (idToken != null)
            {

                ViewData["id_token"] = idToken;
            }
            // "Catalog" because UrlHelper doesn't support nameof() for controllers
            // https://github.com/aspnet/Mvc/issues/5853
            return RedirectToAction(nameof(EventController.About), "Event");
        }

        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //removing cookies
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

            ////// "Catalog" because UrlHelper doesn't support nameof() for controllers
            ////// https://github.com/aspnet/Mvc/issues/5853
            var homeUrl = Url.Action(nameof(EventController.Index), "Event");
            return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = homeUrl });
        }
    }
}

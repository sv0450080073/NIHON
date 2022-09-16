using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HassyaAllrightCloud.Pages
{
    public class Logout : PageModel
    {

        public async Task OnGetAsync()
        {
            var authProps = new AuthenticationProperties
            {
                RedirectUri = Url.Content("~/")
            };
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authProps);
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace HassyaAllrightCloud.Pages
{
    public class Login : PageModel
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _config;
        public Login(IWebHostEnvironment HostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = HostEnvironment;
            _config = configuration;
        }


        public async Task OnGetAsync(string redirectUri)
        {
            //var authProps = new AuthenticationProperties
            //{
            //    IsPersistent = true,
            //    //ExpiresUtc = DateTimeOffset.UtcNow.AddHours(15),
            //    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(30)
            //};

            string redirectStr = "/";
            if (_hostEnvironment.IsProduction())
            {
                redirectStr = _config.GetValue<string>("MySettings:BaseUrl") + "/";
            }

            if (string.IsNullOrWhiteSpace(redirectUri) || redirectUri.ToLower() == "login" || redirectUri.ToLower() == "logout") redirectUri = redirectStr;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            { RedirectUri = redirectUri });
        }
    }
}
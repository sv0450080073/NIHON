using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HassyaAllrightCloud.Controller
{
    public class UserController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("BackChannelLogout")]
        public async Task BackChannelLogout(string logoutToken)
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}

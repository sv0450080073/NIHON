using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ApiController
    {
        [HttpGet()]
        [Route("authorize")]
        public ActionResult Authorize(string code, string state, bool friendship_status_changed)
        {
            throw new NotImplementedException();
        }
        [HttpGet()]
        [Route("accessToken")]
        public ActionResult GetAccessToken(string access_token, int expires_in, string id_token, string refresh_token, string scope, string token_type)
        {
            throw new NotImplementedException();
        }
    }
}

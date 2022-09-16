using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/header")]
    [ApiController]
    public class HeaderController : ControllerBase
    {
        private IWebHostEnvironment hostingEnvironment { get; set; }
        public HeaderController(IWebHostEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost("upload")]
        public string Upload(Data data)
        {
            string pathFile = Path.Combine(hostingEnvironment.WebRootPath, $"audio\\{Guid.NewGuid().ToString()}.wav");
            System.IO.File.WriteAllBytes(pathFile, Convert.FromBase64String(data.file.Split(',')[1]));
            return pathFile;
        }
    }

    public class Data
    {
        public string file { get; set; }
    }
}

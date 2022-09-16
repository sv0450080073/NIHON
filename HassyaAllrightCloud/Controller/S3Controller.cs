using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly IFileHandler _AWSS3FileService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="AWSS3FileService"></param>
        public S3Controller(IFileHandler AWSS3FileService)
        {
            //this._dbContext = dbContext;
            this._AWSS3FileService = AWSS3FileService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [Route("getFile/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            try
            {
                var file = await _AWSS3FileService.DownloadFileAsync(fileName);

                if (file.Content == null)
                {
                    return Redirect("/");
                }

                var content = new System.IO.MemoryStream(file.Content);
                var contentType = "application/octet-stream";
                return File(content, contentType, file.Name);
            }
            catch
            {

                return Ok("NoFile");
            }

        }
    }
}
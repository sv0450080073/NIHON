using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SharedLibraries.Utility.Models;
using System;
using HassyaAllrightCloud.Commons.Constants;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private static IDictionary<string, FileSendData> _fileInformation = null;


        public UploadController()
        {
        }

        [HttpGet]
        [Route("FileInformation")]
        public async Task<ActionResult<FileSendData>> FileInformation(string fileName)
        {
            if (_fileInformation != null && _fileInformation.ContainsKey(fileName))
            {
                FileSendData Result = _fileInformation[fileName];
                _fileInformation.Remove(fileName);
                if (_fileInformation.Count == 0)
                {
                    _fileInformation = null;
                }
                return Result;
            }
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile myFile)
        {
            try
            {
                if (_fileInformation == null)
                {
                    _fileInformation = new Dictionary<string, FileSendData>();
                }
                if (_fileInformation.ContainsKey(myFile.FileName))
                {
                    Response.StatusCode = 400;
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        myFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();

                        FileSendData newFile = new FileSendData()
                        {
                            File = fileBytes,
                            FileName = myFile.FileName,
                            FilePath = null,
                            Password = null,
                            FileSize = Convert.ToInt32(myFile.Length),
                            TenantId = new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID,
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdPrgID = Common.UpdPrgId
                        };
                        _fileInformation.Add(myFile.FileName, newFile);
                    }
                }
                return new EmptyResult();
            }
            catch (Exception e)
            {
                Response.StatusCode = 400;
            }

            return new EmptyResult();
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Application.YoushaNotice.Commands;
using HassyaAllrightCloud.Application.YoushaNotice.Queries;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdYoushaNoticeController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> PostTkdYoushaNotice([FromBody]CarCooperationData carCooperationData)
        {
            try
            {
                TkdYoushaNotice youshaNotice = await Mediator.Send(new CollectDataYoushaNoticeQuery { carCooperationData = carCooperationData });
                return await Mediator.Send(new InsertYoushaNoticeCommand
                {
                    youshaNotice = youshaNotice,
                });
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}
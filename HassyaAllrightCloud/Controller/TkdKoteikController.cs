using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Application.Kotei.Queries;
using HassyaAllrightCloud.Application.Koteik.Commands;
using HassyaAllrightCloud.Application.Koteik.Queries;
using System.Collections.Generic;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TkdKoteikController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> PostTkdKoteik([FromBody]TKD_KoteikData TKD_KoteiFormData)
        {
            try
            {
            TkdKoteik koteik =await Mediator.Send(new CollectDatakoteikQuery { TKD_KoteiFormData = TKD_KoteiFormData });
            List<TkdKotei> listKotei = await Mediator.Send(new CollectDataKoteiQuery { TKD_KoteiFormData = TKD_KoteiFormData });
            return await  Mediator.Send(new InsertTkdKoteikCommand
            {
                koteik = koteik,
                listKotei = listKotei
            });
            }
            catch
            {
                return new BadRequestResult();
            }           
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Application.PartnerBookingInput.Commands;
using HassyaAllrightCloud.Application.PartnerBookingInput.Queries;
using HassyaAllrightCloud.Application.UpdateBookingInput.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerBookingInputController : ApiController
    {
        // POST api/<PartnerBookingInputController>
        [HttpPost]
        public async Task<ActionResult<YouShaDataInsert>> PostPartnerBookingInput([FromBody] YouShaDataInsert youShaDataInsert)
        {
            try
            {
                var ukeNoCD = await Mediator.Send(new InsertYouShaPopupCommand(youShaDataInsert));

                if (ukeNoCD == "-1")
                {
                    return BadRequest();
                }
                return Content(ukeNoCD.ToString());
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        // PUT api/<PartnerBookingInputController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartnerBookingInput(string id, [FromBody] YouShaDataInsert youShaDataInsert)
        {
            try
            {
                TkdYousha tkdYousha = await Mediator.Send(new GetYouShaByUkenoQuery { Ukeno = id, YouShaDataInsert = youShaDataInsert });
                Dictionary<CommandMode, List<TkdYouSyu>> tkdYouSyu = await Mediator.Send(new GetYouSyuByUkenoQuery { Ukeno = id, YouShaDataInsert = youShaDataInsert });
                TkdMihrim tkdMihrim = await Mediator.Send(new GetMihrimByUkenoQuery { Ukeno = id, YouShaDataInsert = youShaDataInsert });
                return await Mediator.Send(new UpdateYouShaPopupCommand
                {
                    Ukeno = id,
                    TkdYousha = tkdYousha,
                    YouSyu = tkdYouSyu,
                    TkdMihrim = tkdMihrim,
                    YouShaDataInsert = youShaDataInsert,
                    

                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
        [HttpPost("deleteYouSha")]
        public async Task<IActionResult> DeleteYouSha([FromBody] YouShaDataTable youShaItemUpdate)
        {        
            try
            {
                var ukeno = youShaItemUpdate.YOUSHA_UkeNo;
                TkdYousha tkdYousha = await Mediator.Send(new DeleteYouShaByUkenoQuery { Ukeno = ukeno, YouShaItemUpdate = youShaItemUpdate });
                Dictionary<CommandMode, List<TkdYouSyu>> tkdYouSyu = await Mediator.Send(new DeleteYouSyuByUkenoQuery { Ukeno = ukeno, YouShaItemUpdate = youShaItemUpdate });
                TkdMihrim tkdMihrim = await Mediator.Send(new DeleteMihrimByUkenoQuery { Ukeno = ukeno, YouShaItemUpdate = youShaItemUpdate });
                return await Mediator.Send(new DeleteYouShaPopupCommand
                {
                    Ukeno = ukeno,
                    TkdYousha = tkdYousha,
                    YouSyu = tkdYouSyu,
                    TkdMihrim = tkdMihrim,
                    YouShaItemUpdate = youShaItemUpdate
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
        [HttpPut("updateHaiSha/{ukeNo}")]
        public async Task<IActionResult> UpdateHaiSha (string ukeNo, [FromBody] HaiShaDataUpdate haiShaDataUpdate)
        {
            try
            {
                TkdHaisha tkdHaisha = await Mediator.Send(new GetHaiShaQuery { Ukeno = ukeNo, HaiShaDataUpdate = haiShaDataUpdate });
                return await Mediator.Send(new UpdateHaiShaPopupCommand
                {
                    Ukeno = ukeNo,
                    TkdHaisha = tkdHaisha
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return new BadRequestResult();
            }
        }
        // DELETE api/<PartnerBookingInputController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Application.BusAllocation.Commands;
using HassyaAllrightCloud.Application.BusAllocation.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusAllocationController : ApiController
    {       
        [HttpPut("updateItemHaiSha/{id}")]
        public async Task<IActionResult> UpdateItemHaiSha(string id, [FromBody] BusAllocationDataUpdate BusAllocationDataUpdate)
        {
            try
            {
                var paramValue = BusAllocationDataUpdate;
                TkdHaisha tkdHaisha = await Mediator.Send(new GetHaiShaDataQuery { BusAllocationDataUpdate = paramValue });
                TkdUnkobi tkdUnkobi = await Mediator.Send(new GetUnkobiDataQuery { Ukeno = paramValue.YYKSHO_UkeNo, UnkRen = paramValue.HAISHA_UnkRen });
                TkdYyksho tkdYyksho = await Mediator.Send(new GetYykshoDataQuery { Ukeno = paramValue.YYKSHO_UkeNo });                
                Dictionary<CommandMode, List<TkdKoban>> tkdKoban = await Mediator.Send(new GetKobanDataQuery
                {
                    Ukeno = paramValue.YYKSHO_UkeNo,
                    UnkRen = paramValue.HAISHA_UnkRen,
                    BunkRen = paramValue.HAISHA_BunkRen,
                    SyaSyuRen = paramValue.HAISHA_SyaSyuRen,
                    TaiDanNo = paramValue.HAISHA_TeiDanNo,
                    StartDate = paramValue.SyuKoYmd,
                    EndDate = paramValue.KikYmd,
                    StartTime =paramValue.SyuKoTime,
                    EndTime = paramValue.KikTime,
                    CompanyCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID
                });
                return await Mediator.Send(new UpdateItemHaiShaCommand
                {
                    TkdHaisha = tkdHaisha,
                    TkdUnkobi = tkdUnkobi,
                    TkdYyksho = tkdYyksho,
                    TkdKoban = tkdKoban,
                    BusAllocationDataUpdate = BusAllocationDataUpdate 
                });

            }
            catch (Exception ex)
            {
                var badrequest = new ContentResult();
                badrequest.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                badrequest.Content = ex.Message;
                return badrequest;
            }
        }

        [HttpPut("updateListHaiShaCopyPaste/{id}")]
        public async Task<IActionResult> UpdateListHaiShaCopyPaste(string id, [FromBody] BusAllocationDataCopyPaste BusAllocationDataCopyPaste)
        {
            try
            {
                var paramValue = BusAllocationDataCopyPaste.DataSourceItem;
                List<TkdHaisha> tkdHaishaList = await Mediator.Send(new GetHaiShaDataListQuery { BusAllocationDataCopyPaste = BusAllocationDataCopyPaste });
                TkdUnkobi tkdUnkobi = await Mediator.Send(new GetUnkobiDataQuery { Ukeno = paramValue.YYKSHO_UkeNo, UnkRen = paramValue.HAISHA_UnkRen });
                TkdYyksho tkdYyksho = await Mediator.Send(new GetYykshoDataQuery { Ukeno = paramValue.YYKSHO_UkeNo });

                Dictionary<CommandMode, List<TkdKoban>> tkdKoban = await Mediator.Send(new GetKobanDataListQuery
                {
                    TkdHaishaList = tkdHaishaList,
                    CompanyCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID
                }); 
                return await Mediator.Send(new UpdateListHaiShaCommand
                {
                    TkdHaishaList = tkdHaishaList,
                    TkdUnkobi = tkdUnkobi,
                    TkdYyksho = tkdYyksho,
                    TkdKoban = tkdKoban,
                    IsUpdateAll =false,
                    DataSourceItem = paramValue
                });
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
        [HttpPut("updateListHaiSha/")]
        public async Task<IActionResult> UpdateListHaiSha([FromBody] BusAllocationDataEditSimultaneously BusAllocationDataEditSimultaneously)
        {
            try
            {
                var paramValue = BusAllocationDataEditSimultaneously.DataSourceItem;
                List<TkdHaisha> tkdHaishaList = await Mediator.Send(new GetHaiShaDataListEditAllQuery { BusAllocationDataEditSimultaneously = BusAllocationDataEditSimultaneously });
                TkdUnkobi tkdUnkobi = await Mediator.Send(new GetUnkobiDataQuery { Ukeno = paramValue.YYKSHO_UkeNo, UnkRen = paramValue.HAISHA_UnkRen });
                TkdYyksho tkdYyksho = await Mediator.Send(new GetYykshoDataQuery { Ukeno = paramValue.YYKSHO_UkeNo });

                Dictionary<CommandMode, List<TkdKoban>> tkdKoban = await Mediator.Send(new GetKobanDataListQuery
                {
                    TkdHaishaList = tkdHaishaList,
                    CompanyCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID
                });
                return await Mediator.Send(new UpdateListHaiShaCommand
                {
                    TkdHaishaList = tkdHaishaList,
                    TkdUnkobi = tkdUnkobi,
                    TkdYyksho = tkdYyksho,
                    TkdKoban = tkdKoban,
                    BusAllocationDataUpdateAll = paramValue
                });
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}

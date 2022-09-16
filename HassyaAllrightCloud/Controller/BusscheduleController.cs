using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Application.BusSchedule.Commands;
using HassyaAllrightCloud.Application.Haisha.Queries;
using HassyaAllrightCloud.Application.Yyksho.Queries;
using HassyaAllrightCloud.Application.Unkobi.Queries;
using HassyaAllrightCloud.Application.Yyksyu.Queries;

namespace HassyaAllrightCloud.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusscheduleController : ApiController
    {
        private readonly KobodbContext _context;

        public BusscheduleController(KobodbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<BusscheduleData>> PostBusschedule([FromBody]BusscheduleData scheduleFormData)
        {
            string ukeNo = "-1";
            TkdYyksho yyksho = await Mediator.Send(new CollectDataYykshoQuery { ScheduleFormData = scheduleFormData });
            TkdUnkobi tkdunkobi = await Mediator.Send(new CollectDataUnkobiQuery { ScheduleFormData = scheduleFormData });
            List<TkdYykSyu> listYykSyu = await Mediator.Send(new CollectDataYykSyuQuery { ScheduleFormData = scheduleFormData });
            List<TkdHaisha> listHaisha = await Mediator.Send(new CollectDataHaishaQuery { ScheduleFormData = scheduleFormData, ListYykSyu = listYykSyu });
            var hodata = scheduleFormData.Itembus.Select(t => t.SyaSyuCdSeq).Distinct();
            foreach (var vehicleTypeRow in hodata)
            {
                var a = listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).Select(t => t.SyaSyuRen).ToList();
                int countbusdriver = listHaisha.Where(t => a.Contains(t.SyaSyuRen)).Sum(t => t.DrvJin);
                int countbusgui = listHaisha.Where(t => a.Contains(t.SyaSyuRen)).Sum(t => t.GuiSu);
                listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().DriverNum = (byte)countbusdriver;
                listYykSyu.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().GuiderNum = (byte)countbusgui;
            }
            await Mediator.Send(new InsertBusScheduleCommand
            {
                Yyksho = yyksho,
                Tkdunkobi = tkdunkobi,
                ListYykSyu = listYykSyu,
                ListHaisha = listHaisha
            });
            return null;
        }
    }
}
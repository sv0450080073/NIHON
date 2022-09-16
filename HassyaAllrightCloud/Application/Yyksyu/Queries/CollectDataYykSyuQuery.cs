using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using HassyaAllrightCloud.Commons.Constants;

namespace HassyaAllrightCloud.Application.Yyksyu.Queries
{
    public class CollectDataYykSyuQuery : IRequest<List<TkdYykSyu>>
    {
        public BusscheduleData ScheduleFormData { get; set; }

        public class Handler : IRequestHandler<CollectDataYykSyuQuery, List<TkdYykSyu>>
        {
            public async Task<List<TkdYykSyu>> Handle(CollectDataYykSyuQuery request, CancellationToken cancellationToken)
            {
                List<TkdYykSyu> listTkdYykSyu = new List<TkdYykSyu> { };
                var hodata = request.ScheduleFormData.Itembus.Select(t => t.SyaSyuCdSeq).Distinct();               
                short i = 1;
                foreach (var vehicleTypeRow in hodata)
                {
                    int countbus = request.ScheduleFormData.Itembus.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).Count();
                    int countofbuscate = request.ScheduleFormData.Itembus.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).Count();
                    TkdYykSyu yyksyu = new TkdYykSyu();
                    yyksyu.UnkRen = 1;
                    yyksyu.SyaSyuRen = i;
                    yyksyu.HenKai = 0;
                    yyksyu.SyaSyuCdSeq = vehicleTypeRow;
                    yyksyu.KataKbn = (byte)request.ScheduleFormData.Itembus.Where(t => t.SyaSyuCdSeq == vehicleTypeRow).First().YykSyu_KataKbn;
                    yyksyu.SyaSyuDai = (short)countbus;
                    yyksyu.SyaSyuTan = 0;
                    yyksyu.SyaRyoUnc = 0;
                    yyksyu.DriverNum = 0;
                    yyksyu.UnitBusPrice = 0;
                    yyksyu.UnitBusFee = 0;
                    yyksyu.GuiderNum = 0;
                    yyksyu.UnitGuiderPrice = 0;
                    yyksyu.UnitGuiderFee = 0;
                    yyksyu.SiyoKbn = 1;
                    yyksyu.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    yyksyu.UpdTime = DateTime.Now.ToString("HHmmss");
                    yyksyu.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    yyksyu.UpdPrgId = "KU0100";
                    listTkdYykSyu.Add(yyksyu);
                    i++;
                }

                return await Task.FromResult(listTkdYykSyu);
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleData.Queries
{
    public class GetVehiclesAssignedQuery :IRequest<IEnumerable<Vehical>>
    {
        private readonly DateTime _garageLeaveTime;
        private readonly DateTime _garageReturnTime;

        public GetVehiclesAssignedQuery(DateTime garageLeaveTime, DateTime garageReturnTime)
        {
            if (garageLeaveTime.CompareTo(garageReturnTime) >= 0)
                throw new ArgumentException("Make sure leave time earlier than return time.");

            _garageLeaveTime = garageLeaveTime;
            _garageReturnTime = garageReturnTime;
        }

        public class Handler : IRequestHandler<GetVehiclesAssignedQuery, IEnumerable<Vehical>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public Task<IEnumerable<Vehical>> Handle(GetVehiclesAssignedQuery request, CancellationToken cancellationToken)
            {
                var VPM_SyaSyus = _context.VpmSyaSyu;
                var VPM_SyaRyos = _context.VpmSyaRyo;
                var TKD_Haishas = _context.TkdHaisha;
                var TKD_Yyksho = _context.TkdYyksho;

                double LeaveGara = Convert.ToDouble(request._garageLeaveTime.ToString("yyyyMMddHHmm"));
                double ReturnGara = Convert.ToDouble(request._garageReturnTime.ToString("yyyyMMddHHmm"));

                var asignedVehical = (from hs in TKD_Haishas
                                      join sr in VPM_SyaRyos on hs.HaiSsryCdSeq equals sr.SyaRyoCdSeq
                                      join ss in VPM_SyaSyus on sr.SyaSyuCdSeq equals ss.SyaSyuCdSeq
                                      join yyksho in TKD_Yyksho on hs.UkeNo equals yyksho.UkeNo into grYyksho
                                      from yyksho in grYyksho
                                      where (hs.HaiSsryCdSeq != 0 && yyksho.YoyaSyu == 1 && hs.SiyoKbn == 1
                                            //&& hs.UkeNo != ukeno
                                            //&& new[] { 1 }.Contains(ss.KataKbn) // filter only big bus
                                            && ((LeaveGara.CompareTo(Convert.ToDouble(hs.SyuKoYmd + hs.SyuKoTime)) >= 0 && LeaveGara.CompareTo(Convert.ToDouble((hs.KikYmd + hs.KikTime))) < 0)
                                            || (LeaveGara.CompareTo(Convert.ToDouble(hs.SyuKoYmd + hs.SyuKoTime)) <= 0 && ReturnGara.CompareTo(Convert.ToDouble(hs.SyuKoYmd + hs.SyuKoTime)) > 0)))
                                      select new Vehical { Ukeno = hs.UkeNo, KataKbn = ss.KataKbn, VehicleModel = sr });

                return Task.FromResult(asignedVehical.AsEnumerable());
            }
        }
    }
}

using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.EditReservationMobile.Queries
{
    public class GetReservationDataQuery : IRequest<ReservationMobileData>
    {
        public int UkeCd { get; set; }
        public int TenantCdSeq { get; set; }

        public class Handler : IRequestHandler<GetReservationDataQuery, ReservationMobileData>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<ReservationMobileData> Handle(GetReservationDataQuery request, CancellationToken cancellationToken)
            {
                ReservationMobileData result = null;

                result = await (from y in _context.TkdYyksho.Where(_ => _.UkeCd == request.UkeCd && _.TenantCdSeq == request.TenantCdSeq)
                                join u in _context.TkdUnkobi.Where(_ => _.UnkRen == 1)
                                on y.UkeNo equals u.UkeNo into temp
                                from t in temp.DefaultIfEmpty()
                                join j in _context.VpmJyoKya
                                on t.JyoKyakuCdSeq equals j.JyoKyakuCdSeq into tempF
                                from final in tempF.DefaultIfEmpty()
                                select new ReservationMobileData()
                                {
                                    UkeNo = y.UkeNo,
                                    UkeCd = y.UkeCd,
                                    KaktYmd = y.KaktYmd,
                                    TokuiSeq = y.TokuiSeq,
                                    SitenCdSeq = y.SitenCdSeq,
                                    DantaiCdSeq = final.DantaiCdSeq,
                                    JyoKyakuCdSeq = t.JyoKyakuCdSeq,
                                    DispatchDate = string.IsNullOrEmpty(t.HaiSymd) || string.IsNullOrWhiteSpace(t.HaiSymd) ? DateTime.Now : DateTime.ParseExact(t.HaiSymd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture),
                                    DispatchTime = string.IsNullOrEmpty(t.HaiStime) || string.IsNullOrWhiteSpace(t.HaiStime) ? "00:00" : t.HaiStime.Insert(2, ":"),
                                    ArrivalDate = string.IsNullOrEmpty(t.TouYmd) || string.IsNullOrWhiteSpace(t.TouYmd) ? DateTime.Now : DateTime.ParseExact(t.TouYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture),
                                    ArrivalTime = string.IsNullOrEmpty(t.TouChTime) || string.IsNullOrWhiteSpace(t.TouChTime) ? "00:00" : t.TouChTime.Insert(2, ":"),
                                    Note = t.BikoNm,
                                    Organization = y.YoyaNm
                                }).FirstOrDefaultAsync();

                return result;
            }
        }
    }
}

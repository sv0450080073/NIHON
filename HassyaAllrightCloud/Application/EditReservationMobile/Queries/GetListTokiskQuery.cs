using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.EditReservationMobile.Queries
{
    public class GetListTokiskQuery : IRequest<List<ReservationTokiskData>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListTokiskQuery, List<ReservationTokiskData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationTokiskData>> Handle(GetListTokiskQuery request, CancellationToken cancellationToken)
            {
                var result = new List<ReservationTokiskData>();
                var currentDate = DateTime.Now.ToString(DateTimeFormat.yyyyMMdd);

                result = await (from tk in _context.VpmTokisk.Where(_ => _.SiyoStaYmd.CompareTo(currentDate) < 1 && _.SiyoEndYmd.CompareTo(currentDate) > -1 && _.TenantCdSeq == request.TenantCdSeq)
                                join ts in _context.VpmTokiSt.Where(_ => _.SiyoStaYmd.CompareTo(currentDate) < 1 && _.SiyoEndYmd.CompareTo(currentDate) > -1)
                                on tk.TokuiSeq equals ts.TokuiSeq
                                join g in _context.VpmGyosya.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                on tk.GyosyaCdSeq equals g.GyosyaCdSeq
                                orderby g.GyosyaCd, tk.TokuiCd, ts.SitenCd
                                select new ReservationTokiskData()
                                {
                                    GyosyaCdSeq = g.GyosyaCdSeq,
                                    GyosyaCd = g.GyosyaCd,
                                    TokuiSeq = tk.TokuiSeq,
                                    TokuiCd = tk.TokuiCd,
                                    SitenCdSeq = ts.SitenCdSeq,
                                    SitenCd = ts.SitenCd,
                                    Name = string.Format("{0} : {1} {2} : {3}", tk.TokuiCd.ToString().PadLeft(4, '0'), tk.RyakuNm, ts.SitenCd.ToString().PadLeft(4, '0'), ts.SitenNm)
                                }).ToListAsync();

                return result;
            }
        }
    }
}

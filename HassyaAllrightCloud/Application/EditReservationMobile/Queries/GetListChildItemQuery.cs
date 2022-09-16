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
    public class GetListChildItemQuery : IRequest<List<ReservationMobileChildItemData>>
    {
        public int UkeCd { get; set; }
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListChildItemQuery, List<ReservationMobileChildItemData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationMobileChildItemData>> Handle(GetListChildItemQuery request, CancellationToken cancellationToken)
            {
                List<ReservationMobileChildItemData> result = new List<ReservationMobileChildItemData>();

                result = await (from yo in _context.TkdYyksho.Where(_ => _.UkeCd == request.UkeCd && _.TenantCdSeq == request.TenantCdSeq)
                                join yu in _context.TkdYykSyu.Where(_ => _.UnkRen == 1)
                                on yo.UkeNo equals yu.UkeNo into temp
                                from t in temp.DefaultIfEmpty()
                                orderby t.SyaSyuRen
                                select new ReservationMobileChildItemData()
                                {
                                    SyaSyuRen = t.SyaSyuRen,
                                    SyaSyuCdSeq = t.SyaSyuCdSeq,
                                    KataKbn = t.KataKbn,
                                    BusCount = t.SyaSyuDai.ToString(),
                                    DriverCount = t.DriverNum.ToString(),
                                    GuiderCount = t.GuiderNum.ToString(),
                                    IsAddNew = false,
                                    SyaSyuTan = t.SyaSyuTan,
                                    UnitGuiderPrice = t.UnitGuiderPrice
                                }).ToListAsync();

                return result;
            }
        }
    }
}

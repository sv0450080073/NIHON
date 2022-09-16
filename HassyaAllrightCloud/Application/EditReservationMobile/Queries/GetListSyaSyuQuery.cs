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
    public class GetListSyaSyuQuery : IRequest<List<ReservationSyaSyuData>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetListSyaSyuQuery, List<ReservationSyaSyuData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationSyaSyuData>> Handle(GetListSyaSyuQuery request, CancellationToken cancellationToken)
            {
                var result = new List<ReservationSyaSyuData>();

                result = await (from s in _context.VpmSyaSyu.Where(_ => _.TenantCdSeq == request.TenantCdSeq)
                                join c in _context.VpmCodeKb.Where(_ => _.CodeSyu == CommonConstants.KATAKBN)
                                on s.KataKbn.ToString() equals c.CodeKbn
                                join cs in _context.VpmCodeSy
                                on c.CodeSyu equals cs.CodeSyu 
                                where (cs.KanriKbn == 1 && c.TenantCdSeq == 0) || (cs.KanriKbn != 1 && c.TenantCdSeq == request.TenantCdSeq)
                                orderby s.KataKbn, s.SyaSyuCd
                                select new ReservationSyaSyuData()
                                {
                                    SyaSyuCdSeq = s.SyaSyuCdSeq,
                                    SyaSyuCd = s.SyaSyuCd,
                                    SyaSyuNm = s.SyaSyuNm,
                                    KataKbn = s.KataKbn,
                                    KataNm = c.CodeKbnNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}

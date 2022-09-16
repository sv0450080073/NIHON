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
    public class GetListCodeKbQuery : IRequest<List<ReservationCodeKbnData>>
    {
        public class Handler : IRequestHandler<GetListCodeKbQuery, List<ReservationCodeKbnData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ReservationCodeKbnData>> Handle(GetListCodeKbQuery request, CancellationToken cancellationToken)
            {
                var result = new List<ReservationCodeKbnData>();

                result = await (from c in _context.VpmCodeKb.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn && _.TenantCdSeq == 0 && _.CodeSyu == CommonConstants.DANTAICD)
                                join j in _context.VpmJyoKya.Where(_ => _.SiyoKbn == CommonConstants.SiyoKbn)
                                on c.CodeKbnSeq equals j.DantaiCdSeq
                                orderby c.CodeKbn, j.JyoKyakuCd
                                select new ReservationCodeKbnData()
                                {
                                    CodeKbnSeq = c.CodeKbnSeq,
                                    CodeKbn = c.CodeKbn,
                                    CodeKbnNm = c.CodeKbnNm,
                                    JyoKyakuCdSeq = j.JyoKyakuCdSeq,
                                    JyoKyakuCd = j.JyoKyakuCd,
                                    JyoKyakuNm = j.JyoKyakuNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}

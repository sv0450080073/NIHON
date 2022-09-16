using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AttendanceReport.Queries
{
    public class GetKasSet : IRequest<List<KasSetDto>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetKasSet, List<KasSetDto>>
        {
            private KobodbContext _context;
            public Handler(KobodbContext kobodbContext)
            {
                _context = kobodbContext;
            }
            public async Task<List<KasSetDto>> Handle(GetKasSet request, CancellationToken cancellationToken)
            {
                return await _context.TkmKasSet.Where(e => e.CompanyCdSeq == request.CompanyCdSeq).Select(e => new KasSetDto()
                {
                    JisKinKyuNm01 = e.JisKinKyuNm01,
                    JisKinKyuNm02 = e.JisKinKyuNm02,
                    JisKinKyuNm03 = e.JisKinKyuNm03,
                    JisKinKyuNm04 = e.JisKinKyuNm04,
                    JisKinKyuNm05 = e.JisKinKyuNm05,
                    JisKinKyuNm06 = e.JisKinKyuNm06,
                    JisKinKyuNm07 = e.JisKinKyuNm07,
                    JisKinKyuNm08 = e.JisKinKyuNm08,
                    JisKinKyuNm09 = e.JisKinKyuNm09,
                    JisKinKyuNm10 = e.JisKinKyuNm10
                }).ToListAsync(cancellationToken);
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillCheckList.Queries
{
    public class GetBillAddressQuery : IRequest<List<BillAddress>>
    {
        public int tenantId { get; set; }
        public class Handler : IRequestHandler<GetBillAddressQuery, List<BillAddress>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<BillAddress>> Handle(GetBillAddressQuery request, CancellationToken cancellationToken)
            {
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                return await (from t in _dbContext.VpmTokisk
                             where DateAsString.CompareTo(t.SiyoStaYmd) >= 0 && DateAsString.CompareTo(t.SiyoEndYmd) <= 0 && t.TenantCdSeq == request.tenantId
                              from s in _dbContext.VpmTokiSt
                             where t.TokuiSeq == s.TokuiSeq && DateAsString.CompareTo(s.SiyoStaYmd) >= 0 && DateAsString.CompareTo(s.SiyoEndYmd) <= 0
                             from g in _dbContext.VpmGyosya
                             where t.GyosyaCdSeq == g.GyosyaCdSeq && g.SiyoKbn == 1
                             orderby g.GyosyaCd, t.TokuiCd, s.SitenCd, g.GyosyaCdSeq, t.TokuiSeq, s.SitenCdSeq
                             select new BillAddress()
                             {
                                 TokuiSeq = t.TokuiSeq,
                                 SitenCdSeq = s.SitenCdSeq,
                                 TokuiCd = t.TokuiCd,
                                 RyakuNm = t.RyakuNm,
                                 SitenCd = s.SitenCd,
                                 SitenNm = s.SitenNm,
                                 TesuRitu = s.TesuRitu,
                                 TesuRituGui = s.TesuRituGui,
                                 GyoSysSeq = g.GyosyaCdSeq,
                                 GyoSyaCd = g.GyosyaCd,
                                 GyoSyaNm = g.GyosyaNm
                             }).ToListAsync();
            }
        }
    }
}

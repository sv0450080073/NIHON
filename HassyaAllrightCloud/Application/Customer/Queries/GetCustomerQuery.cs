using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Customer.Queries
{
    public class GetCustomerQuery : IRequest<IEnumerable<LoadCustomerList>>
    {
        public class Handler : IRequestHandler<GetCustomerQuery, IEnumerable<LoadCustomerList>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<LoadCustomerList>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
            {
                var tenantCdSeq = new ClaimModel().TenantID;
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                var result = await (from t in _context.VpmTokisk 
                                    join s in _context.VpmTokiSt 
                                    on t.TokuiSeq equals s.TokuiSeq 
                                    into lj from subTo in lj.DefaultIfEmpty()
                                    join second in _context.VpmTokiSt
                                    on new { first = subTo.SeiSitenCdSeq, second = subTo.TokuiSeq } equals new { first = second.SitenCdSeq, second = second.TokuiSeq }
                                    into ls from secondTo in ls.DefaultIfEmpty()
                              where DateAsString.CompareTo(subTo.SiyoStaYmd) >= 0 &&
                                    DateAsString.CompareTo(subTo.SiyoEndYmd) <= 0
                                    && t.TenantCdSeq == tenantCdSeq
                              orderby t.TokuiCd ascending
                              select new LoadCustomerList()
                              {
                                  TokuiSeq = t.TokuiSeq,
                                  SitenCdSeq = subTo.SitenCdSeq,
                                  TokuiCd = t.TokuiCd,
                                  RyakuNm = t.RyakuNm,
                                  SitenCd = subTo.SitenCd,
                                  SitenNm = subTo.SitenNm,
                                  TesuRitu = subTo.TesuRitu,
                                  TesKbn = subTo.TesKbn,
                                  TesuRituGui = subTo.TesuRituGui,
                                  TesKbnGui = subTo.TesKbnGui,
                                  SimeD = secondTo.SimeD,
                                  TokuiTel = subTo.TelNo,
                                  TokuiFax = subTo.FaxNo,
                                  TokuiMail = subTo.TokuiMail,
                                  TokuiTanNm = subTo.TokuiTanNm,
                                  GyoSysSeq = t.GyosyaCdSeq
                              }).Distinct().ToListAsync();
                return result;
            }
        }
    }
}

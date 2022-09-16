using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetBusInfoDataQuery : IRequest<List<BusInfoData>>
    {
        public string Date { get; set; } = "";
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetBusInfoDataQuery, List<BusInfoData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BusInfoData>> Handle(GetBusInfoDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<BusInfoData>();
                try
                {
                    result = (from HENSYA in _context.VpmHenSya
                              join SYARYO in _context.VpmSyaRyo
                              on HENSYA.SyaRyoCdSeq equals SYARYO.SyaRyoCdSeq
                              join EIGYOS in _context.VpmEigyos
                              on HENSYA.EigyoCdSeq equals EIGYOS.EigyoCdSeq
                              join KAISHA in _context.VpmCompny
                              on new { K1 = EIGYOS.CompanyCdSeq, K2 = request.TenantCdSeq }
                              equals new { K1 = KAISHA.CompanyCdSeq, K2 = KAISHA.TenantCdSeq }
                              join SYASYU in _context.VpmSyaSyu
                              on new { K1 = SYARYO.SyaSyuCdSeq, K2 = 1, K3 = request.TenantCdSeq }
                              equals new { K1 = SYASYU.SyaSyuCdSeq, K2 = (int)SYASYU.SiyoKbn, K3 = SYASYU.TenantCdSeq }
                              where String.Compare(HENSYA.StaYmd, request.Date) <= 0
                              && String.Compare(HENSYA.EndYmd, request.Date) >= 0
                              orderby KAISHA.CompanyCdSeq, HENSYA.EigyoCdSeq, HENSYA.SyaRyoCdSeq
                              select new BusInfoData()
                              {
                                  SyaRyoCdSeq = HENSYA.SyaRyoCdSeq,
                                  StaYmd = HENSYA.StaYmd,
                                  EndYmd = HENSYA.EndYmd,
                                  EigyoCdSeq= HENSYA.EigyoCdSeq,
                                  EigyoCd = EIGYOS.EigyoCd,
                                  RyakuNm= EIGYOS.RyakuNm,
                                  SyaRyoCd= SYARYO.SyaRyoCd,
                                  SyaRyoNm= SYARYO.SyaRyoNm,
                                  CompanyCdSeq= KAISHA.CompanyCdSeq,
                                  KataKbn= SYASYU.KataKbn,
                                  SYARYO_SyainCdSeq = SYARYO.SyainCdSeq
                              }).ToList();
                    return result;
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}

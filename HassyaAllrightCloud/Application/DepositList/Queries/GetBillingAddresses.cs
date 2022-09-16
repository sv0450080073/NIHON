using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetBillingAddresses : IRequest<List<BillingAddressModel>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetBillingAddresses, List<BillingAddressModel>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<List<BillingAddressModel>> Handle(GetBillingAddresses request, CancellationToken cancellationToken)
            {
                string DateAsString = DateTime.Today.ToString("yyyyMMdd");
                return (from toksk in _dbContext.VpmTokisk
                        join tokst in _dbContext.VpmTokiSt
                        on new { key1 = toksk.TokuiSeq, key2 = true, key3 = true } equals new { key1 = tokst.TokuiSeq, key2 = tokst.SiyoStaYmd.CompareTo(DateAsString) <= 0, key3 = tokst.SiyoEndYmd.CompareTo(DateAsString) >= 0 } into toktok
                        from toktokTemp in toktok
                        join gyo in _dbContext.VpmGyosya
                        on new { key1 = toksk.GyosyaCdSeq, key2 = (byte)1 } equals new { key1 = gyo.GyosyaCdSeq, key2 = gyo.SiyoKbn } into tokgyo
                        from tokgypTemp in tokgyo
                        where toksk.SiyoStaYmd.CompareTo(DateAsString) <= 0 && toksk.SiyoEndYmd.CompareTo(DateAsString) >= 0 && toksk.TenantCdSeq == request.TenantCdSeq
                        orderby tokgypTemp.GyosyaCd, toksk.TokuiCd, toktokTemp.SitenCd
                        select new BillingAddressModel()
                        {
                            Code = $"{tokgypTemp.GyosyaCdSeq.ToString("D3")}" + $"{toksk.TokuiCd.ToString("D4")}" +$"{toktokTemp.SitenCd.ToString("D4")}",
                            Name = $"{toksk.TokuiCd.ToString("D4")}" +":"+toksk.RyakuNm+" "+$"{toktokTemp.SitenCd.ToString("D4")}" +":"+toktokTemp.SitenNm
                        }).ToList();
            }
        }
    }
}

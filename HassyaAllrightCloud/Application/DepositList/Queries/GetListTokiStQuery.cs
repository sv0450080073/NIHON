using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetListTokiStQuery : IRequest<List<CustomerComponentTokiStData>>
    {
        public class Handler : IRequestHandler<GetListTokiStQuery, List<CustomerComponentTokiStData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CustomerComponentTokiStData>> Handle(GetListTokiStQuery request, CancellationToken cancellationToken)
            {
                List<CustomerComponentTokiStData> result = new List<CustomerComponentTokiStData>();

                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMD);

                var data = _context.VpmTokiSt.Where(x => _context.VpmTokisk.Where(y => y.TenantCdSeq == new ClaimModel().TenantID && y.SiyoStaYmd.CompareTo(currentDate) <= 0 && y.SiyoEndYmd.CompareTo(currentDate) >= 0)
                .Select(x => x.TokuiSeq).Contains(x.TokuiSeq) && x.SiyoStaYmd.CompareTo(currentDate) <= 0 && x.SiyoEndYmd.CompareTo(currentDate) >= 0).ToList();

                foreach(var ts in data)
                {
                    result.Add(new CustomerComponentTokiStData()
                    {
                        SitenCdSeq = ts.SitenCdSeq,
                        SitenCd = ts.SitenCd,
                        TokuiSeq = ts.TokuiSeq,
                        RyakuNm = ts.RyakuNm,
                        FaxNo = ts.FaxNo,
                        TelNo = ts.TelNo,
                        TesuRitu = ts.TesuRitu,
                        TokuiMail = ts.TokuiMail,
                        TokuiTanNm = ts.TokuiTanNm
                    });
                }

                return result;
            }
        }
    }
}

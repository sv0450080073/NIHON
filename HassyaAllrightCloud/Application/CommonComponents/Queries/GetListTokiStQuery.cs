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

namespace HassyaAllrightCloud.Application.CommonComponents.Queries
{
    public class GetListTokiStQuery : IRequest<List<CustomerComponentTokiStData>>
    {
        public string strDate { get; set; }
        public string endDate { get; set; }
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

                if (string.IsNullOrEmpty(request.strDate))
                {
                    request.strDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                }
                if (string.IsNullOrEmpty(request.endDate))
                {
                    request.endDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                }
                result = await (from ts in _context.VpmTokiSt
                                join tk in _context.VpmTokisk
                                on new { A = ts.TokuiSeq, B = new ClaimModel().TenantID } equals new { A = tk.TokuiSeq, B = tk.TenantCdSeq }
                                where ts.SiyoStaYmd.CompareTo(request.endDate) <= 0 && ts.SiyoEndYmd.CompareTo(request.strDate) >= 0
                                   && tk.SiyoStaYmd.CompareTo(request.endDate) <= 0 && tk.SiyoEndYmd.CompareTo(request.strDate) >= 0
                                orderby ts.SitenCd
                                select new CustomerComponentTokiStData()
                                {
                                    SitenCdSeq = ts.SitenCdSeq,
                                    SitenCd = ts.SitenCd,
                                    TokuiSeq = ts.TokuiSeq,
                                    RyakuNm = ts.RyakuNm,
                                    FaxNo = ts.FaxNo,
                                    TelNo = ts.TelNo,
                                    TesuRitu = ts.TesuRitu,
                                    TokuiMail = ts.TokuiMail,
                                    TokuiTanNm = ts.TokuiTanNm,
                                    SitenNm = ts.SitenNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}

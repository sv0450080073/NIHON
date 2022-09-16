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
    public class GetListTokiskQuery : IRequest<List<CustomerComponentTokiskData>>
    {
        public string strDate { get; set; }
        public string endDate { get; set; }
        public class Handler : IRequestHandler<GetListTokiskQuery, List<CustomerComponentTokiskData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CustomerComponentTokiskData>> Handle(GetListTokiskQuery request, CancellationToken cancellationToken)
            {
                List<CustomerComponentTokiskData> result = new List<CustomerComponentTokiskData>();
                if (string.IsNullOrEmpty(request.strDate))
                {
                    request.strDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                }
                if (string.IsNullOrEmpty(request.endDate))
                {
                    request.endDate = DateTime.Now.ToString(CommonConstants.FormatYMD);
                }
                result = await (from t in _context.VpmTokisk
                                where t.SiyoStaYmd.CompareTo(request.endDate) <= 0 && t.SiyoEndYmd.CompareTo(request.strDate) >= 0
                                   && t.TenantCdSeq == new ClaimModel().TenantID
                                orderby t.TokuiCd
                                select new CustomerComponentTokiskData() 
                                { 
                                    GyosyaCdSeq = t.GyosyaCdSeq,
                                    TokuiSeq = t.TokuiSeq,
                                    TokuiCd = t.TokuiCd,
                                    RyakuNm = t.RyakuNm,
                                    TokuiNm = t.TokuiNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}

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
    public class GetListTokiskQuery : IRequest<List<CustomerComponentTokiskData>>
    {
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

                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMD);

                var data = _context.VpmTokisk.Where(x => x.SiyoStaYmd.CompareTo(currentDate) <= 0 && x.SiyoEndYmd.CompareTo(currentDate) >= 0 && x.TenantCdSeq == new ClaimModel().TenantID).OrderBy(x => x.TokuiCd).ToList();

                foreach(var item in data)
                {
                    result.Add(new CustomerComponentTokiskData()
                    {
                        GyosyaCdSeq = item.GyosyaCdSeq,
                        TokuiSeq = item.TokuiSeq,
                        TokuiCd = item.TokuiCd,
                        RyakuNm = item.RyakuNm
                    });
                }

                return result;
            }
        }
    }
}

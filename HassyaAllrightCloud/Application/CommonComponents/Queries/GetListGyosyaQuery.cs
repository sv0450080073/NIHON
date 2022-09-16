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
    public class GetListGyosyaQuery : IRequest<List<CustomerComponentGyosyaData>>
    {
        public class Handler : IRequestHandler<GetListGyosyaQuery, List<CustomerComponentGyosyaData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CustomerComponentGyosyaData>> Handle(GetListGyosyaQuery request, CancellationToken cancellationToken)
            {
                List<CustomerComponentGyosyaData> result = new List<CustomerComponentGyosyaData>();

                result = await (from g in _context.VpmGyosya
                                where g.SiyoKbn == CommonConstants.SiyoKbn && g.TenantCdSeq == new ClaimModel().TenantID
                                orderby g.GyosyaCd
                                select new CustomerComponentGyosyaData()
                                {
                                    GyosyaCdSeq = g.GyosyaCdSeq,
                                    GyosyaCd = g.GyosyaCd,
                                    GyosyaKbn = g.GyosyaKbn,
                                    GyosyaNm = g.GyosyaNm
                                }).ToListAsync();

                return result;
            }
        }
    }
}

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

                var data = _context.VpmGyosya.Where(x => x.SiyoKbn == 1 && x.TenantCdSeq == new ClaimModel().TenantID).OrderBy(x => x.GyosyaCd).ToList();

                foreach (var item in data)
                {
                    result.Add(new CustomerComponentGyosyaData()
                    {
                        GyosyaCd = item.GyosyaCd,
                        GyosyaCdSeq = item.GyosyaCdSeq,
                        GyosyaKbn = item.GyosyaKbn,
                        GyosyaNm = item.GyosyaNm
                    });
                }

                return result;
            }
        }
    }
}

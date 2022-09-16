using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VPMTenantData.Queries
{
    public class GetVpmTenantData : IRequest<List<VpmTenant>>
    {
        public class Handler : IRequestHandler<GetVpmTenantData, List<VpmTenant>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VpmTenant>> Handle(GetVpmTenantData request, CancellationToken cancellationToken)
            {
                var result =  _context.VpmTenant.Where(x => x.TenantCdSeq == new ClaimModel().TenantID).ToList();

                return result;
            }
        }

    }
}

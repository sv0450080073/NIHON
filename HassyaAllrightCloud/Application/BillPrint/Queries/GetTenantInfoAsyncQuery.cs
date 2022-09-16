using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetTenantInfoAsyncQuery : IRequest<PaymentRequestTenantInfo>
    {
        public class Handler : IRequestHandler<GetTenantInfoAsyncQuery, PaymentRequestTenantInfo>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<PaymentRequestTenantInfo> Handle(GetTenantInfoAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return await _context.VpmTenant.Where(x => x.TenantCdSeq == new ClaimModel().TenantID).Select(x => new PaymentRequestTenantInfo()
                {
                    TenantCdSeq = x.TenantCdSeq,
                    TenantCompanyName = x.TenantCompanyName
                }).FirstOrDefaultAsync();
            }
        }
    }
}

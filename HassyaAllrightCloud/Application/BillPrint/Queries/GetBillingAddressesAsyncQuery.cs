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
    public class GetBillingAddressesAsyncQuery : IRequest<List<DropDown>>
    {
        public class Handler : IRequestHandler<GetBillingAddressesAsyncQuery, List<DropDown>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DropDown>> Handle(GetBillingAddressesAsyncQuery request, CancellationToken cancellationToken = default)
            {
                List<DropDown> rows = null;

                _context.LoadStoredProc("PK_dBillingAddress_R")
                                .AddParam("TenantCdSeq", new ClaimModel().TenantID)
                                .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                                .Exec(r => rows = r.ToList<DropDown>());
                return rows;
            }
        }
    }
}

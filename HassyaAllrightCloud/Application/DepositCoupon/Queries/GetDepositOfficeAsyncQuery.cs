using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
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
    public class GetDepositOfficeAsyncQuery : IRequest<List<DepositOffice>>
    {
        public class Handler : IRequestHandler<GetDepositOfficeAsyncQuery, List<DepositOffice>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DepositOffice>> Handle(GetDepositOfficeAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return (from vpmEigyos in _context.VpmEigyos
                        join vpmCompny in _context.VpmCompny
                        on new { companyCdSeq = vpmEigyos.CompanyCdSeq, siyoKbn = (byte)1, tenantCdSeq = new ClaimModel().TenantID } 
                        equals new { companyCdSeq = vpmCompny.CompanyCdSeq, siyoKbn = vpmCompny.SiyoKbn, tenantCdSeq = vpmCompny.TenantCdSeq }
                        where vpmEigyos.SiyoKbn == (byte)1
                        orderby vpmEigyos.EigyoCd
                        select new DepositOffice
                        {
                            Code = vpmEigyos.EigyoCd.ToString().PadLeft(5, '0'),
                            Name = vpmEigyos.RyakuNm
                        }).ToList();
            }
        }
    }
}

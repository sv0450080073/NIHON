using HassyaAllrightCloud.Commons.Constants;
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
    public class GetBillingOfficeDataAsyncQuery : IRequest<List<DropDown>>
    {
        public class Handler : IRequestHandler<GetBillingOfficeDataAsyncQuery, List<DropDown>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<DropDown>> Handle(GetBillingOfficeDataAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return (from vpmEigyos in _context.VpmEigyos
                        join vpmCompny in _context.VpmCompny
                        on vpmEigyos.CompanyCdSeq equals vpmCompny.CompanyCdSeq    
                        where vpmEigyos.SiyoKbn == 1 &&
                        vpmCompny.SiyoKbn == 1 &&
                        vpmCompny.TenantCdSeq == new ClaimModel().TenantID
                        select new DropDown
                        {
                            Code = vpmEigyos.EigyoCdSeq.ToString(),
                            Name = string.Format("{0:D5}", vpmEigyos.EigyoCd) + " : " + vpmEigyos.RyakuNm,
                            CodeText = string.Format("{0:D5}", vpmEigyos.EigyoCd) + " : " + vpmEigyos.RyakuNm
                        }).ToList().OrderBy(x => x.Code).ToList();
            }
        }
    }
}

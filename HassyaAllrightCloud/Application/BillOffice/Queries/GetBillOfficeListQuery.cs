using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillOffice.Queries
{
    public class GetBillOfficeListQuery : IRequest<List<BillOfficeData>>
    {
        public int _tenantId { get; set; }
        public class Handler : IRequestHandler<GetBillOfficeListQuery, List<BillOfficeData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public async Task<List<BillOfficeData>> Handle(GetBillOfficeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await (from t1 in _dbContext.VpmEigyos
                                 join t2 in _dbContext.VpmCompny
                                            .Where(x => x.SiyoKbn == Constants.SiyoKbn
                                                     && x.TenantCdSeq == request._tenantId)
                                 on t1.CompanyCdSeq equals t2.CompanyCdSeq
                                 where t1.SiyoKbn == Constants.SiyoKbn
                                 orderby t1.EigyoCd
                                 select new BillOfficeData()
                                 {
                                     EigyoCdSeq = t1.EigyoCdSeq,
                                     EigyoCd = $"{t1.EigyoCd:00000}",
                                     EigyoNm = t1.RyakuNm
                                 }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }
    }
}

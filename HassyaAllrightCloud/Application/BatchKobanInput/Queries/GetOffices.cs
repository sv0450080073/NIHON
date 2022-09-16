using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Queries
{
    public class GetOffices : IRequest<List<LoadSaleBranchList>>
    {
        public int TenantCdSeq { get; set; }
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetOffices, List<LoadSaleBranchList>>
        {
            private readonly KobodbContext _dbContext;

            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<LoadSaleBranchList>> Handle(GetOffices request, CancellationToken cancellationToken)
            {
                return (from ei in _dbContext.VpmEigyos
                        join com in _dbContext.VpmCompny
                        on new { key1 = ei.CompanyCdSeq, key2 = (byte)1 } equals new { key1 = com.CompanyCdSeq, key2 = com.SiyoKbn } into eicom
                        from eicomTemp in eicom
                        join t in _dbContext.VpmTenant on new {key1 = eicomTemp.TenantCdSeq, key2 = (byte)1} equals new {key1 = t.TenantCdSeq, key2 = t.SiyoKbn} into eicomt
                        from eicomtTemp in eicomt
                        where ei.SiyoKbn == 1
                        && eicomtTemp.TenantCdSeq == request.TenantCdSeq && eicomTemp.CompanyCdSeq == request.CompanyCdSeq
                        orderby ei.EigyoCd
                        select new LoadSaleBranchList()
                        {
                            EigyoCdSeq = ei.EigyoCdSeq,
                            CompanyCdSeq = eicomTemp.CompanyCdSeq,
                            EigyoCd = ei.EigyoCd,
                            RyakuNm = ei.RyakuNm
                        }).ToList();
            }
        }
    }
}

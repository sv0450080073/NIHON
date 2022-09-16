using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Threading;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetSaleBranchData : IRequest<List<LoadSaleBranchList>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetSaleBranchData, List<LoadSaleBranchList>>
        {
            private readonly KobodbContext _dbContext;

            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<LoadSaleBranchList>> Handle(GetSaleBranchData request, CancellationToken cancellationToken)
            {
                return (from ei in _dbContext.VpmEigyos
                              join com in _dbContext.VpmCompny
                              on new { key1 = ei.CompanyCdSeq, key2 = (byte)1 } equals new { key1 = com.CompanyCdSeq, key2 = com.SiyoKbn } into eicom
                              from eicomTemp in eicom.DefaultIfEmpty()
                              where ei.SiyoKbn == 1
                              && eicomTemp.TenantCdSeq == request.TenantCdSeq
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

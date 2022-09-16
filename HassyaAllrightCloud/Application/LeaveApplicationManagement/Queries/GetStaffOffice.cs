using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetStaffOffice : IRequest<IEnumerable<Branch>>
    {
        public class Handler : IRequestHandler<GetStaffOffice, IEnumerable<Branch>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;


            public async Task<IEnumerable<Branch>> Handle(GetStaffOffice request, CancellationToken cancellationToken)
            {
                var result = (from e in _dbContext.VpmEigyos
                              join c in _dbContext.VpmCompny
                              on new { key1 = e.CompanyCdSeq, key2 = (byte)1, key3 = new ClaimModel().TenantID } equals new { key1 = c.CompanyCdSeq, key2 = c.SiyoKbn, key3 = c.TenantCdSeq }
                              where e.SiyoKbn == 1
                              orderby c.CompanyCd, e.EigyoCd
                              select new Branch()
                              {
                                  Seg = e.EigyoCdSeq,
                                  Name = $"{c.CompanyCd:00000}" + ":" + c.RyakuNm + $"{e.EigyoCd:00000}" + ":" + e.RyakuNm
                              }).ToList();

                return result;
            }
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.DepositList.Queries
{
    public class GetCompanyData : IRequest<List<CompanyData>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCompanyData, List<CompanyData>>
        {
            private readonly KobodbContext _dbContext;

            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<List<CompanyData>> Handle(GetCompanyData request, CancellationToken cancellationToken)
            {
                var companys = _dbContext.VpmCompny.Where(x => x.TenantCdSeq == request.TenantCdSeq && x.SiyoKbn == 1).OrderBy(x => x.CompanyCd).ToList();
                if(companys == null || companys.Count == 0)
                {
                    companys = _dbContext.VpmCompny.Where(x => x.TenantCdSeq == 0 && x.SiyoKbn == 1).OrderBy(x => x.CompanyCd).ToList();
                }
                var result = new List<CompanyData>();
                foreach(var item in companys)
                {
                    result.Add(new CompanyData()
                    {
                        CompanyCdSeq = item.CompanyCdSeq,
                        CompanyCd = item.CompanyCd,
                        RyakuNm = item.RyakuNm
                    });
                }
                return result;
            }
        }
    }
}

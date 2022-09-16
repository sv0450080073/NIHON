using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FareFeeCorrection.Queries
{
    public class GetCompanyValidate : IRequest<List<CompanyValidate>>
    {
        public class Handler : IRequestHandler<GetCompanyValidate, List<CompanyValidate>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<CompanyValidate>> Handle(GetCompanyValidate request, CancellationToken cancellationToken)
            {
                return (from c in _context.VpmCompny
                        join t in _context.VpmTenant on c.TenantCdSeq equals t.TenantCdSeq
                        where t.SiyoKbn == 1 && c.SiyoKbn == 1 && c.TenantCdSeq == 1 && c.CompanyCdSeq == 1
                        select new CompanyValidate()
                        {
                            TenantCdSeq = c.TenantCdSeq,
                            CompanyCdSeq = c.CompanyCdSeq,
                            CompanyCd = c.CompanyCd,
                            RyakuNm = c.RyakuNm,
                            SyoriYm = c.SyoriYm
                        }).ToList();
            }
        }
    }
}

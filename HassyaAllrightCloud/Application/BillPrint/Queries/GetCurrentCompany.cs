using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetCurrentCompany : IRequest<CompanyDto>
    {
        public class Handler : IRequestHandler<GetCurrentCompany, CompanyDto>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<CompanyDto> Handle(GetCurrentCompany request, CancellationToken cancellationToken = default)
            {
                var query = (from c in _context.VpmCompny.Where(e => e.SiyoKbn == 1 && new ClaimModel().CompanyID == e.CompanyCdSeq && e.TenantCdSeq == new ClaimModel().TenantID)
                             join t in _context.VpmTenant.Where(e => e.SiyoKbn == 1)
                             on c.TenantCdSeq equals t.TenantCdSeq
                             select new CompanyDto()
                             {
                                 CompanyCd = c.CompanyCd,
                                 RyakuNm = c.RyakuNm,
                                 CompanyCdSeq = c.CompanyCdSeq,
                                 TenantCdSeq = c.TenantCdSeq,
                                 CompanyNm = c.CompanyNm,
                                 ComSealImgFileId = c.ComSealImgFileId
                             });
                return await query.FirstOrDefaultAsync();
            }
        }
    }
}

using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class GetCauseCombobox : IRequest<List<Cause>>
    {
        public class Handler : IRequestHandler<GetCauseCombobox, List<Cause>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<Cause>> Handle(GetCauseCombobox request, CancellationToken cancellationToken)
            {
                var tenantCdSeq = _context.VpmCodeKb.Where(x => x.CodeSyu == "UPPERLOWERCAUSE" && x.SiyoKbn == CommonConstants.SiyoKbn && x.TenantCdSeq == new ClaimModel().TenantID).Count() == 0 ? 0 : 1;
                var lstCodeKbs = _context.VpmCodeKb.Where(x => x.CodeSyu == "UPPERLOWERCAUSE" && x.SiyoKbn == CommonConstants.SiyoKbn && x.TenantCdSeq == tenantCdSeq);
                var lstCauses = lstCodeKbs.Select(x => new Cause
                {
                    CodeKbn = Convert.ToInt16(x.CodeKbn),
                    CodeKbnNm = x.CodeKbnNm,
                    CodeSyu = x.CodeSyu
                });
                return lstCauses.ToList();
            }
        }
    }
}

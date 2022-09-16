using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetCodeKbnData : IRequest<List<CodeKbnDataPopup>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetCodeKbnData, List<CodeKbnDataPopup>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }
            public async Task<List<CodeKbnDataPopup>> Handle(GetCodeKbnData request, CancellationToken cancellationToken)
            {
                var result = new List<CodeKbnDataPopup>();
                try
                {
                    return await _codeSyuService.FilterTenantIdByCodeSyu<CodeKbnDataPopup>(async (tenant, codesyu) =>
                     {
                         return await (from VPM_CodeKb in _context.VpmCodeKb
                                       where VPM_CodeKb.CodeSyu == codesyu
                                      && VPM_CodeKb.TenantCdSeq == tenant
                                       orderby VPM_CodeKb.CodeKbn
                                       select new CodeKbnDataPopup()
                                       {
                                           CodeKbn = VPM_CodeKb.CodeKbn,
                                           CodeKbnNm = VPM_CodeKb.CodeKbnNm
                                       }).ToListAsync();
                     }, request.TenantCdSeq, "ZEIKBN");
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}
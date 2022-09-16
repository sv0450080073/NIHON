using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetVPM_HaichiDataQuery : IRequest<List<TPM_CodeKbDataBunruiCD>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetVPM_HaichiDataQuery, List<TPM_CodeKbDataBunruiCD>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<TPM_CodeKbDataBunruiCD>> Handle(GetVPM_HaichiDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<TPM_CodeKbDataBunruiCD>();
                try
                {
                    await _context.LoadStoredProc("PK_dHaichiBunruiCds_R")
                                  .AddParam("@TenantCdSeq", request.TenantCdSeq)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<TPM_CodeKbDataBunruiCD>());

                    return result;
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }

    }
}

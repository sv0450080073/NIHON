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
    public class GetTPM_CodeKbDataDepotQuery : IRequest<List<TPM_CodeKbDataDepot>>
    {  
        public int TenantCdSeq { get; set; } = 0;
        public string Date { get; set; } = "";
        public class Handler : IRequestHandler<GetTPM_CodeKbDataDepotQuery, List<TPM_CodeKbDataDepot>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<TPM_CodeKbDataDepot>> Handle(GetTPM_CodeKbDataDepotQuery request, CancellationToken cancellationToken)
            {
                var result = new List<TPM_CodeKbDataDepot>();
                try
                {
                    await _context.LoadStoredProc("PK_dKoutuBunruiCds_R")
                                  .AddParam("@TenantCdSeq", request.TenantCdSeq)
                                  .AddParam("@Ymd", request.Date)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<TPM_CodeKbDataDepot>());

                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }
    }
}

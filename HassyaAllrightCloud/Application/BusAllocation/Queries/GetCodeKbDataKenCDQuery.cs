using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetCodeKbDataKenCDQuery : IRequest<List<TPM_CodeKbDataKenCD>>
    {
        public int TenantCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetCodeKbDataKenCDQuery, List<TPM_CodeKbDataKenCD>>
        {
            private readonly KobodbContext _context;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _codeSyuService = codeSyuService;
            }

            public async Task<List<TPM_CodeKbDataKenCD>> Handle(GetCodeKbDataKenCDQuery request, CancellationToken cancellationToken)
            {
                var result = new List<TPM_CodeKbDataKenCD>();
                try
                {
                    await _context.LoadStoredProc("PK_dBasyoKenCds_R")
                                  .AddParam("@TenantCdSeq", request.TenantCdSeq)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<TPM_CodeKbDataKenCD>());

                    return result;
                }
                catch(Exception)
                {
                    return result;
                }
            }
        }
    }
}

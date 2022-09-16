using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetVehicleAllocationListQuery : IRequest<List<VehicleAllocationItem>>
    {
        public int TenantCdSeq { get; set; }
        public string UnkYmd { get; set; }
        public class Handler : IRequestHandler<GetVehicleAllocationListQuery, List<VehicleAllocationItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleAllocationItem>> Handle(GetVehicleAllocationListQuery request, CancellationToken cancellationToken)
            {
                List<VehicleAllocationItem> result = new List<VehicleAllocationItem>();

                try
                {
                    await _context.LoadStoredProc("PK_dVehicleAllocations_R")
                                  .AddParam("@TenantCdSeq", request.TenantCdSeq)
                                  .AddParam("@UnkYmd", request.UnkYmd)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<VehicleAllocationItem>());
                    
                    return result;
                }
                catch (Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}

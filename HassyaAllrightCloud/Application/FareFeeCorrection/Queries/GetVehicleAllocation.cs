using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FareFeeCorrection.Queries
{
    public class GetVehicleAllocationList : IRequest<List<VehicleAllocation>>
    {
        public string UkeNo { get; set; }
        public string TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetVehicleAllocationList, List<VehicleAllocation>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<VehicleAllocation>> Handle(GetVehicleAllocationList request, CancellationToken cancellationToken)
            {
                try
                {
                    var rows = new List<VehicleAllocation>();
                    _context.LoadStoredProc("dbo.PK_VehicleAllocation_R")
                             .AddParam("@TenantCdSeq", new ClaimModel().TenantID.ToString())
                             .AddParam("@UkeNo", request.UkeNo)
                             .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                         .Exec(r => rows = r.ToList<VehicleAllocation>());
                    return rows;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

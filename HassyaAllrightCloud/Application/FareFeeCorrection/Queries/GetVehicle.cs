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
    public class GetVehicle : IRequest<List<Vehicle>>
    {
        public string UkeNo { get; set; }
        public string TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetVehicle, List<Vehicle>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<Vehicle>> Handle(GetVehicle request, CancellationToken cancellationToken)
            {
                try
                {
                    var rows = new List<Vehicle>();
                    _context.LoadStoredProc("dbo.PK_Vehicle_R")
                             .AddParam("@TenantCdSeq", new ClaimModel().TenantID.ToString())
                             .AddParam("@UkeNo", request.UkeNo)
                             .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                         .Exec(r => rows = r.ToList<Vehicle>());
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

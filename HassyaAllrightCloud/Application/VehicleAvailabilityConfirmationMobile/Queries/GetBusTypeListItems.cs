using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VehicleAvailabilityConfirmationMobile.Queries
{
    public class GetBusTypeListItems : IRequest<IEnumerable<BusType>>
    {
        public class Handler : IRequestHandler<GetBusTypeListItems, IEnumerable<BusType>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;

            public async Task<IEnumerable<BusType>> Handle(GetBusTypeListItems request, CancellationToken cancellationToken)
            {
                var data = new List<BusType>();
                await _context.LoadStoredProc("PK_BusTypes")
                          .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                          .ExecAsync(async r => data = await r.ToListAsync<BusType>());
                return data;
            }
        }
    }
}

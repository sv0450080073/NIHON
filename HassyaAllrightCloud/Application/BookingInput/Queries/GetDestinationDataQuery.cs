using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DevExpress.Blazor.Internal;
using HassyaAllrightCloud.IService;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetDestinationDataQuery : IRequest<IEnumerable<DestinationData>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetDestinationDataQuery, IEnumerable<DestinationData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetDestinationDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetDestinationDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<IEnumerable<DestinationData>> Handle(GetDestinationDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<DestinationData>();
                try
                {
                    await _context.LoadStoredProc("PK_dDestinationList_R")
                                  .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<DestinationData>());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}

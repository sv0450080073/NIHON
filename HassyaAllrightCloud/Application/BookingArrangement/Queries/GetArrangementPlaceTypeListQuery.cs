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

namespace HassyaAllrightCloud.Application.BookingArrangement.Queries
{
    public class GetArrangementPlaceTypeListQuery : IRequest<List<ArrangementPlaceType>>
    {
        private readonly string _ukeNo;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetArrangementPlaceTypeListQuery(ITPM_CodeSyService codeSyuService, string ukeNo, int tenantId)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _tenantId = tenantId;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetArrangementPlaceTypeListQuery, List<ArrangementPlaceType>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ArrangementPlaceType>> Handle(GetArrangementPlaceTypeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<ArrangementPlaceType>();

                    await _context.LoadStoredProc("PK_dGetArrangementPlaceTypeList_R")
                                  .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<ArrangementPlaceType>());

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

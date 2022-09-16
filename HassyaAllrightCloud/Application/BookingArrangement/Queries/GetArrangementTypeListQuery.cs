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
    public class GetArrangementTypeListQuery : IRequest<List<ArrangementType>>
    {
        private readonly string _ukeNo;
        private readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetArrangementTypeListQuery(ITPM_CodeSyService codeSyuService, string ukeNo, int tenantId)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _tenantId = tenantId;
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
        }

        public class Handler : IRequestHandler<GetArrangementTypeListQuery, List<ArrangementType>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ArrangementType>> Handle(GetArrangementTypeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = new List<ArrangementType>();

                    await _context.LoadStoredProc("PK_dGetArrangementTypeList_R")
                                  .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                                  .ExecAsync(async rows => result = await rows.ToListAsync<ArrangementType>());

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

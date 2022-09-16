using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RegulationSetting.Queries
{
    public class GetSpecificWorkHolidayType : IRequest<List<WorkHolidayType>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetSpecificWorkHolidayType, List<WorkHolidayType>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<WorkHolidayType>> Handle(GetSpecificWorkHolidayType request, CancellationToken cancellationToken)
            {
                var result = new List<WorkHolidayType>();
                _context.LoadStoredProc("PK_dSpecificWorkHolidayType_R")
                    .AddParam("@CompanyCdSeq", request.CompanyCdSeq)
                    .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                    .Exec(e => result = e.ToList<WorkHolidayType>());
                return result;
            }
        }
    }
}

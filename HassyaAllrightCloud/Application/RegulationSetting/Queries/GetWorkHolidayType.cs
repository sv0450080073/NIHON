using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;

namespace HassyaAllrightCloud.Application.RegulationSetting.Queries
{
    public class GetWorkHolidayType : IRequest<List<WorkHolidayType>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetWorkHolidayType, List<WorkHolidayType>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<WorkHolidayType>> Handle(GetWorkHolidayType request, CancellationToken cancellationToken)
            {
                var result = new List<WorkHolidayType>();
                _context.LoadStoredProc("PK_dWorkHolidayType_R")
                    .AddParam("@CompanyCdSeq", request.CompanyCdSeq)
                    .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                    .Exec(e => result = e.ToList<WorkHolidayType>());
                return result;
            }
        }
    }
}

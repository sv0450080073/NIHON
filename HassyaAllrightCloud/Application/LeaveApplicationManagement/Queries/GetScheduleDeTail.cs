using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetScheduleDeTail : IRequest<ScheduleDetail>
    {
        public int scheduleId { get; set; }
        public class Handler : IRequestHandler<GetScheduleDeTail, ScheduleDetail>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;


            public async Task<ScheduleDetail> Handle(GetScheduleDeTail request, CancellationToken cancellationToken)
            {
                var result = new List<ScheduleDetail>();

                _dbContext.LoadStoredProc("PK_dScheduleDetail_R")
                          .AddParam("ScheduleId", request.scheduleId)
                          .AddParam("TenantCdSeq", new ClaimModel().TenantID)
                          .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                          .Exec(r => result = r.ToList<ScheduleDetail>());
                return result[0];
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetWorkHourCustomGroup : IRequest<List<WorkHourModel>>
    {
        public int GroupId { get; set; }
        public string Fre27ToDate { get; set; }
        public string ToDate { get; set; }
        public class Handler : IRequestHandler<GetWorkHourCustomGroup, List<WorkHourModel>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<WorkHourModel>> Handle(GetWorkHourCustomGroup request, CancellationToken cancellationToken)
            {
                List<WorkHourModel> result = new List<WorkHourModel>();

                _dbcontext.LoadStoredProc("PK_dWorkHourCustomGroup_R")
                         .AddParam("GroupId", request.GroupId)
                         .AddParam("Previous27DayToDate", request.Fre27ToDate)
                         .AddParam("ToDate", request.ToDate)
                         .AddParam("TenantCdSeq", new ClaimModel().TenantID)

                         .AddParam("ROWCOUNT", out IOutParam<int> rowCount)
                         .Exec(r => result = r.ToList<WorkHourModel>());

                return result;
            }
        }
    }
}

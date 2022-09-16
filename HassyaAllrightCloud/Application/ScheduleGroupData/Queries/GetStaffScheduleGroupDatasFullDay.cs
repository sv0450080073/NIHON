using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ScheduleGroupData.Queries
{
    public class GetStaffScheduleGroupDatasFullDay : IRequest<IEnumerable<StaffScheduleGroupData>>
    {
        public IEnumerable<StaffScheduleGroupData> schedule { get; set; }
        public class Handler : IRequestHandler<GetStaffScheduleGroupDatasFullDay, IEnumerable<StaffScheduleGroupData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;
            public async Task<IEnumerable<StaffScheduleGroupData>> Handle(GetStaffScheduleGroupDatasFullDay request, CancellationToken cancellationToken)
            {
                var result = request.schedule;
                foreach (var item in result)
                {
                    var endDate = item.endDate.ToShortDateString().Replace("/", string.Empty);
                    var startDate = item.startDate.ToShortDateString().Replace("/", string.Empty);
                    item.startDate = DateTime.ParseExact(startDate + "0000", "yyyyMMddHHmm", null);
                    item.endDate = DateTime.ParseExact(endDate + "2359", "yyyyMMddHHmm", null);
                }
                return result;
            }
        }
    }
}

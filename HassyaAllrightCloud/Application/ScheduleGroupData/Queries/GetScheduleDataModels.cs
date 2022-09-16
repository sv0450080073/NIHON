using HassyaAllrightCloud.Commons.Helpers;
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
    public class GetScheduleDataModels : IRequest<IEnumerable<ScheduleDataModel>>
    {
        public int groupId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public IEnumerable<StaffScheduleGroupData> scheduleData { get; set; }
        public IEnumerable<ScheduleDataModel> bookedScheduleData { get; set; }
        public class Handler : IRequestHandler<GetScheduleDataModels, IEnumerable<ScheduleDataModel>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;
            public async Task<IEnumerable<ScheduleDataModel>> Handle(GetScheduleDataModels request, CancellationToken cancellationToken)
            {
                var scheduleData = request.scheduleData;
                List<ScheduleDataModel> result = new List<ScheduleDataModel>();
                foreach (var item in scheduleData)
                {
                    result.Add(new ScheduleDataModel()
                    {
                        scheduleId = item.id,
                        Description = item.Description,
                        EmployeeId = item.EmployeeId,
                        endDate = item.endDate,
                        startDate = item.startDate,
                        scheduleType = item.ScheduleType,
                        EmployeeName = item.EmployeeName,
                        id = item.id,
                        LeaveType = item.LeaveType,
                        color = item.color,
                        Creator = item.Creator,
                        PlanComment = item.PlanComment,
                        Participant = item.Participant,
                        Destination = item.Destination,
                        IsPublic = item.IsPublic,
                        recurrenceRule = item.recurrenceRule,
                        YoteiSeq = item.scheduleId,
                        label = item.label,
                        plantype = item.plantype,
                        LeaveName = item.LeaveName,
                        isFullDay = item.isAllDay
                    });
                }
                result.AddRange(request.bookedScheduleData);
                return result;
            }
        }
    }
}

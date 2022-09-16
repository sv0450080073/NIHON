using HassyaAllrightCloud.Commons.Constants;
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
    public class GetBookingScheduleOfStaffFullDay : IRequest<IEnumerable<ScheduleDataModel>>
    {
        public IEnumerable<StaffScheduleGroupData> scheduleData { get; set; }
        public class Handler : IRequestHandler<GetBookingScheduleOfStaffFullDay, IEnumerable<ScheduleDataModel>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<IEnumerable<ScheduleDataModel>> Handle(GetBookingScheduleOfStaffFullDay request, CancellationToken cancellationToken)
            {
                List<ScheduleDataModel> result = new List<ScheduleDataModel>();
                foreach (var item in request.scheduleData)
                {
                    if (item.ScheduleType == CommonConstants.planType)
                    {
                        var bookedSchedules = _dbContext.TkdSchYotKsya.Where(x => x.YoteiSeq.ToString() == item.scheduleId).ToList();
                        foreach (var bookedSchedule in bookedSchedules)
                        {
                            result.Add(new ScheduleDataModel()
                            {
                                scheduleId = item.id,
                                Description = item.Description,
                                EmployeeId = bookedSchedule.SyainCdSeq.ToString(),
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
                                plantype = item.plantype
                            });
                        }
                    }
                }
                foreach (var res in result)
                {
                    var syainSd = _dbContext.VpmSyain.Where(v => v.SyainCdSeq.ToString() == res.EmployeeId).FirstOrDefault().SyainCd;
                    res.EmployeeId = syainSd;
                }
                return result;
            }
        }
    }
}

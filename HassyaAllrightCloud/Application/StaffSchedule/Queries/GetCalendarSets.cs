using DevExpress.Xpo;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Queries
{
    public class GetCalendarSets : IRequest<List<CalendarSetModel>>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetCalendarSets, List<CalendarSetModel>>
        {
            private readonly KobodbContext _dbcontext;
            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<CalendarSetModel>> Handle(GetCalendarSets request, CancellationToken cancellationToken)
            {
                var data = _dbcontext.VpmSyain.Where(x => x.SyainCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).FirstOrDefault();
                var calendarSets = _dbcontext.TkdSchCalendar.Where(x => x.CompanyCdSeq == request.CompanyCdSeq && x.SiyoKbn == (byte)1).ToList();

                List<CalendarSetModel> result = new List<CalendarSetModel>();

                result.Add(new CalendarSetModel()
                {
                    CalendarName = data != null ? data.SyainNm : new ClaimModel().UserName,
                    CalendarSeq = 0,
                    CompanyCdSeq = 0
                });

                foreach(var item in calendarSets)
                {
                    result.Add(new CalendarSetModel()
                    {
                        CalendarName = item.CalendarName,
                        CalendarSeq = item.CalendarSeq,
                        CompanyCdSeq = item.CompanyCdSeq
                    });
                }

                return result;
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetMemberOfTranning : IRequest<string>
    {
        public ScheduleDataModel schedule;
        public class Handler : IRequestHandler<GetMemberOfTranning, string>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<string> Handle(GetMemberOfTranning request, CancellationToken cancellationToken)
            {
                var result = string.Empty;
                var bookedSchedule = _dbContext.TkdSchYotKsya.Where(x => x.YoteiSeq == Int32.Parse(request.schedule.YoteiSeq)).ToList();
                foreach(var item in bookedSchedule)
                {
                    var member = _dbContext.VpmSyain.Where(x => x.SyainCdSeq == item.SyainCdSeq).FirstOrDefault();
                    result += member?.SyainNm + " ";
                }
                return result;
            }
        }
    } 
}

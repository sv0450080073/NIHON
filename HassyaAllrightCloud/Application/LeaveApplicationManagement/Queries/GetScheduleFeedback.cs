using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetScheduleFeedback : IRequest<string>
    {
        public ScheduleDataModel schedule { get; set; }
        public class Handler : IRequestHandler<GetScheduleFeedback, string>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<string> Handle(GetScheduleFeedback request, CancellationToken cancellationToken)
            {
                var result = string.Empty;
                var members = _dbContext.TkdSchYotKsya.Where(x => x.YoteiSeq == Int32.Parse(request.schedule.YoteiSeq)).ToList();
                foreach(var item in members)
                {
                    var syainNm = _dbContext.VpmSyain.Where(x => x.SyainCdSeq == item.SyainCdSeq).FirstOrDefault()?.SyainNm;
                    TkdSchYotKsyaFb tkdSchYotKsya = new TkdSchYotKsyaFb() { YoteiSeq = -1 };
                    tkdSchYotKsya = _dbContext.TkdSchYotKsyaFb.Where(x => x.KuriKbn == 2 && x.YoteiSeq == item.YoteiSeq && x.SyainCdSeq == item.SyainCdSeq && x.YoteiSymd == request.schedule.startDate.ToString().Substring(0, 10).Replace("/", string.Empty) && x.YoteiStime == request.schedule.startDate.ToString().Substring(11).Replace(":", string.Empty)).FirstOrDefault();
                    if(tkdSchYotKsya == null)
                    {
                        tkdSchYotKsya = _dbContext.TkdSchYotKsyaFb.Where(x => x.YoteiSeq == item.YoteiSeq && x.SyainCdSeq == item.SyainCdSeq).FirstOrDefault();
                    }
                    var resfb = tkdSchYotKsya != null ? tkdSchYotKsya.AcceptKbn : 0;
                    var temp = $"{syainNm},{resfb};";
                    result += temp;

                }
                return result;
            }
        }
    }
}

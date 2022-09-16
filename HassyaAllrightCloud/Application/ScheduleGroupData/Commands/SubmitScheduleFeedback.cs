using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ScheduleGroupData.Commands
{
    public class SubmitScheduleFeedback : IRequest<bool>
    {
        public AppointmentList schedule { get; set; }
        public int value { get; set; }
        public class Handler : IRequestHandler<SubmitScheduleFeedback, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<bool> Handle(SubmitScheduleFeedback request, CancellationToken cancellationToken)
            {
                TkdSchYotKsyaFb tkdSchYotKsyaFb = new TkdSchYotKsyaFb();
                TkdSchYotKsyaFb exitsYoteiFB = new TkdSchYotKsyaFb();
                if (request.schedule.RecurrenceRule != string.Empty)
                {
                    tkdSchYotKsyaFb.YoteiSeq = request.schedule.YoteiInfo.YoteiSeq;
                    tkdSchYotKsyaFb.AcceptKbn = (byte)request.value;
                    tkdSchYotKsyaFb.KuriKbn = 1;
                    tkdSchYotKsyaFb.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdSchYotKsyaFb.YoteiSymd = string.Empty;
                    tkdSchYotKsyaFb.YoteiStime = string.Empty;
                    tkdSchYotKsyaFb.UpdPrgId = Common.UpdPrgId;
                    tkdSchYotKsyaFb.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdSchYotKsyaFb.UpdTime = DateTime.Now.ToString("HHmmss");
                    tkdSchYotKsyaFb.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    exitsYoteiFB = _dbContext.TkdSchYotKsyaFb.Where(x => x.YoteiSeq == tkdSchYotKsyaFb.YoteiSeq && x.SyainCdSeq == tkdSchYotKsyaFb.SyainCdSeq).FirstOrDefault();
                }
                else
                {
                    tkdSchYotKsyaFb.YoteiSeq = request.schedule.YoteiInfo.YoteiSeq;
                    tkdSchYotKsyaFb.AcceptKbn = (byte)request.value;
                    tkdSchYotKsyaFb.KuriKbn = 2;
                    tkdSchYotKsyaFb.SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdSchYotKsyaFb.YoteiSymd = request.schedule.StartDate.ToString().Substring(0, 10).Replace("/", string.Empty).Replace("-", string.Empty);
                    tkdSchYotKsyaFb.YoteiStime = request.schedule.EndDate.ToString().Substring(11, 8).Replace(":", string.Empty).Replace("-", string.Empty);
                    tkdSchYotKsyaFb.UpdPrgId = Common.UpdPrgId;
                    tkdSchYotKsyaFb.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdSchYotKsyaFb.UpdTime = DateTime.Now.ToString("HHmmss");
                    tkdSchYotKsyaFb.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    exitsYoteiFB = _dbContext.TkdSchYotKsyaFb.Where(x => x.YoteiSeq == tkdSchYotKsyaFb.YoteiSeq && x.SyainCdSeq == tkdSchYotKsyaFb.SyainCdSeq && x.YoteiSymd == tkdSchYotKsyaFb.YoteiSymd && x.YoteiStime == tkdSchYotKsyaFb.YoteiStime).FirstOrDefault();
                }

                if (_dbContext.TkdSchYotKsyaFb.Contains(exitsYoteiFB))
                {
                    exitsYoteiFB.AcceptKbn = tkdSchYotKsyaFb.AcceptKbn;
                    exitsYoteiFB.UpdTime = DateTime.Now.ToString("HHmmss");
                    exitsYoteiFB.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    exitsYoteiFB.UpdSyainCd = new ClaimModel().SyainCdSeq;
                    _dbContext.TkdSchYotKsyaFb.Update(exitsYoteiFB);
                }
                else
                {
                    _dbContext.TkdSchYotKsyaFb.Add(tkdSchYotKsyaFb);
                }
                _dbContext.SaveChanges();
                return true;
            }
        }
    }
}

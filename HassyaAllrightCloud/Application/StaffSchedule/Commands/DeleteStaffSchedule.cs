using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using HassyaAllrightCloud.Commons.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.StaffSchedule.Commands
{
    public class DeleteStaffSchedule : IRequest<bool>
    {
        public AppointmentList model { get; set; }
        public class Handler : IRequestHandler<DeleteStaffSchedule, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<bool> Handle(DeleteStaffSchedule request, CancellationToken cancellationToken)
            {
                var deleteSchedule = _dbContext.TkdSchYotei.Where(x => x.YoteiSeq == request.model.YoteiInfo.YoteiSeq).FirstOrDefault();
                deleteSchedule.SiyoKbn = 2;
                deleteSchedule.UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty);
                deleteSchedule.UpdTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", string.Empty);
                deleteSchedule.UpdSyainCd = Constants.SyainCdSeq;
                deleteSchedule.UpdPrgId = "CONVERT";

                _dbContext.TkdSchYotei.Update(deleteSchedule);
                _dbContext.SaveChanges();
                return true;
            }
        }
    }
}

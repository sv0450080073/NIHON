using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Notification.Commands
{
    public class InsertNotificationCommand : IRequest<NotificationResult>
    {
        public int toStaffId { get; set; }
        public class Handler : IRequestHandler<InsertNotificationCommand, NotificationResult>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<NotificationResult> Handle(InsertNotificationCommand request, CancellationToken cancellationToken)
            {
                int ToStaffId = request.toStaffId;
                string Ymd = DateTime.Now.ToString(CommonConstants.FormatYMD);
                string Time = DateTime.Now.ToString(CommonConstants.FormatHMS);
                TkdNotification Notification = await (from notification in _context.TkdNotification
                                                      where notification.SyainCdSeq == ToStaffId
                                                        && notification.SouSymd == Ymd
                                                        && notification.SouStime == Time
                                                      select notification).FirstOrDefaultAsync();
                TkdNotification NewNotification = new TkdNotification();
                NewNotification.SyainCdSeq = ToStaffId;
                NewNotification.SouSymd = Ymd;
                NewNotification.SouStime = Time;
                NewNotification.SouSren = Notification != null ? (short)(Notification.SouSren + 1) : (short)1;
                NewNotification.ControlNo = ToStaffId.ToString("D10") + Ymd + Time + NewNotification.SouSren.ToString("D3");
                NewNotification.MailNowKbn = 0;
                NewNotification.LineNowKbn = 0;
                NewNotification.UpdYmd = Ymd;
                NewNotification.UpdTime = Time;
                NewNotification.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                NewNotification.UpdPrgId = Common.UpdPrgId;
                _context.TkdNotification.Add(NewNotification);
                await _context.SaveChangesAsync();

                NotificationResult Result = new NotificationResult();
                Result.ControlNo = NewNotification.ControlNo;
                return Result;
            }
        }
    }
}

using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.NotificationReplyToken.Commands
{
    public class UpdateNotificationReplyTokenCommand : IRequest<bool>
    {
        public NotificationSendMethod method { get; set; }
        public string controlNo { get; set; }
        public string token { get; set; }

        public class Handler : IRequestHandler<UpdateNotificationReplyTokenCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(UpdateNotificationReplyTokenCommand request, CancellationToken cancellationToken)
            {
                string dateAsString = DateTime.Today.ToString(CommonConstants.FormatYMDHMSNoSeparated);
                TkdNotiReplyToken tkdNotiReplyToken = _context.TkdNotiReplyToken.Where(x => x.ControlNo == request.controlNo && x.NotiMethodKbn == (byte)request.method
                    && x.Token == request.token && dateAsString.CompareTo(x.ExpiredDate + x.ExpiredTime) <= 0 && x.SiyoKbn == 1).FirstOrDefault();
                if (tkdNotiReplyToken == null)
                {
                    return false;
                }
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        tkdNotiReplyToken.SiyoKbn = 2;
                        _context.TkdNotiReplyToken.Update(tkdNotiReplyToken);
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}

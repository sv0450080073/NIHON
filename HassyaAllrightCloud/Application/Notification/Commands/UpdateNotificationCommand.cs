using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Notification.Commands
{
    public class UpdateNotificationCommand : IRequest<bool>
    {
        public string ControlNo { get; set; }
        public NotificationSendMethod SendMethod { get; set; }
        public List<string> Results { get; set; }
        public class Handler : IRequestHandler<UpdateNotificationCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
            {
                TkdNotification tkdNotification = await _context.TkdNotification.FindAsync(request.ControlNo);
                if (request.SendMethod == NotificationSendMethod.Both || request.SendMethod == NotificationSendMethod.Mail)
                {
                    tkdNotification.MailNowKbn = string.IsNullOrEmpty(request.Results[0]) ? (byte)2 : (byte)1;
                }
                if (request.SendMethod == NotificationSendMethod.Both || request.SendMethod == NotificationSendMethod.Line)
                {
                    tkdNotification.LineNowKbn = string.IsNullOrEmpty(request.Results[request.Results.Count - 1]) ? (byte)2 : (byte)1;
                }
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.TkdNotification.Update(tkdNotification);
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

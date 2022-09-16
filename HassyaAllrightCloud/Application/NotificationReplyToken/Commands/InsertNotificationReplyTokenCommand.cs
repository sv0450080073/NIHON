using DevExpress.Charts.Model;
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

namespace HassyaAllrightCloud.Application.NotificationReplyToken.Commands
{
    public class InsertNotificationReplyTokenCommand : IRequest<List<string>>
    {
        public NotificationSendMethod method { get; set; }
        public DateTime replyExpiredDateTime { get; set; }
        public string controlNo { get; set; }
        public class Handler : IRequestHandler<InsertNotificationReplyTokenCommand, List<string>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<string>> Handle(InsertNotificationReplyTokenCommand request, CancellationToken cancellationToken)
            {
                string ControlNo = request.controlNo;
                DateTime ReplyExpiredDateTime = request.replyExpiredDateTime;
                TkdNotiReplyToken NotiReplyToken = new TkdNotiReplyToken();
                NotificationSendMethod Method = request.method;
                NotiReplyToken.ControlNo = ControlNo;
                NotiReplyToken.ExpiredDate = ReplyExpiredDateTime.ToString(CommonConstants.FormatYMD);
                NotiReplyToken.ExpiredTime = ReplyExpiredDateTime.ToString(CommonConstants.FormatHMS);
                NotiReplyToken.SiyoKbn = 1;
                NotiReplyToken.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatYMD);
                NotiReplyToken.UpdTime = DateTime.Now.ToString(CommonConstants.FormatHMS);
                NotiReplyToken.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                NotiReplyToken.UpdPrgId = Common.UpdPrgId;
                List<string> Tokens = new List<string>();
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (Method == NotificationSendMethod.Both || Method == NotificationSendMethod.Mail)
                        {
                            NotiReplyToken.NotiMethodKbn = 1;
                            NotiReplyToken.Token = GenerateToken();
                            Tokens.Add(NotiReplyToken.Token);
                            _context.TkdNotiReplyToken.Add(NotiReplyToken);
                            await _context.SaveChangesAsync();
                        }
                        if (Method == NotificationSendMethod.Both || Method == NotificationSendMethod.Line)
                        {
                            NotiReplyToken.NotiMethodKbn = 2;
                            NotiReplyToken.Token = GenerateToken();
                            Tokens.Add(NotiReplyToken.Token);
                            _context.TkdNotiReplyToken.Add(NotiReplyToken);
                            await _context.SaveChangesAsync();
                        }
                        dbTran.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        dbTran.Rollback();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                    }
                }
                return Tokens;
            }

            private string GenerateToken()
            {
                string chars = TokenChars.Chars;
                char[] stringChars = new char[36];
                Random random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                return new string(stringChars);
            }
        }
    }
}

using DevExpress.Xpo;
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

namespace HassyaAllrightCloud.Application.Notice.Commands
{
    public class SaveNoticeCommand : IRequest<bool>
    {
        public Tkd_NoticeDto NoticeDto { get; set; }

        public class Handler : IRequestHandler<SaveNoticeCommand, bool>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(SaveNoticeCommand request, CancellationToken cancellationToken)
            {
                if (request.NoticeDto == null) return false;
                try
                {
                    if (request.NoticeDto.NoticeCdSeq != 0) // Update
                    {
                        var tkdNotice = context.TkdNotice.FirstOrDefault(x => x.NoticeCdSeq == request.NoticeDto.NoticeCdSeq);
                        if (tkdNotice == null) return false;
                        tkdNotice.NoticeContent = request.NoticeDto.NoticeContent;
                        tkdNotice.NoticeDisplayKbn = request.NoticeDto.NoticeDisplayKbn;
                        tkdNotice.SiyoKbn = Constants.SiyoKbn;
                        tkdNotice.UpdPrgId = Common.UpdPrgId;
                        tkdNotice.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdNotice.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                        tkdNotice.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);

                        context.TkdNotice.Update(tkdNotice);
                    }
                    else // Create
                    {
                        var tkdNotice = new TkdNotice
                        {
                            NoticeContent = request.NoticeDto.NoticeContent,
                            NoticeDisplayKbn = request.NoticeDto.NoticeDisplayKbn,
                            SiyoKbn = Constants.SiyoKbn,
                            UpdPrgId = Common.UpdPrgId,
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdTime = DateTime.Now.ToString(Formats.HHmmss),
                            UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd)
                        };

                        context.TkdNotice.Add(tkdNotice);
                    }

                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    return false;
                }
            }
        }
    }
}

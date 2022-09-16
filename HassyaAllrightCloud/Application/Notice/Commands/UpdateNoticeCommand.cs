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
    public class UpdateNoticeCommand : IRequest<TkdNotice>
    {
        public Tkd_NoticeDto NoticeDto { get; set; }

        public class Handler : IRequestHandler<UpdateNoticeCommand, TkdNotice>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext context)
            {
                this.context = context;
            }

            public async Task<TkdNotice> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
            {
                if (request.NoticeDto == null) return new TkdNotice();
                try
                {
                    if (request.NoticeDto.NoticeCdSeq != 0) // Update
                    {
                        var tkdNotice = context.TkdNotice.FirstOrDefault(x => x.NoticeCdSeq == request.NoticeDto.NoticeCdSeq);
                        if (tkdNotice == null) return new TkdNotice();
                        tkdNotice.NoticeContent = request.NoticeDto.NoticeContent;
                        tkdNotice.NoticeDisplayKbn = request.NoticeDto.NoticeDisplayKbn;
                        tkdNotice.SiyoKbn = Constants.SiyoKbn;
                        tkdNotice.UpdPrgId = Common.UpdPrgId;
                        tkdNotice.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                        tkdNotice.UpdTime = DateTime.Now.ToString(Formats.HHmmss);
                        tkdNotice.UpdYmd = DateTime.Now.ToString(Formats.yyyyMMdd);

                        context.TkdNotice.Update(tkdNotice);
                        await context.SaveChangesAsync();
                        return tkdNotice;
                    }
                    return new TkdNotice();
                }
                catch (Exception ex)
                {
                    // TODO: logging
                    return new TkdNotice();
                }
            }
        }
    }
}

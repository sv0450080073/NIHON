using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Notice.Queries
{
    public class GetNoticeByIdQuery : IRequest<Tkd_NoticeDto>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetNoticeByIdQuery, Tkd_NoticeDto>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext kobodbContext)
            {
                this.context = kobodbContext;
            }

            public async Task<Tkd_NoticeDto> Handle(GetNoticeByIdQuery request, CancellationToken cancellationToken)
            {
                var tkdNotice = context.TkdNotice.FirstOrDefault(x => x.NoticeCdSeq == request.Id);

                return new Tkd_NoticeDto
                {
                    NoticeCdSeq = tkdNotice.NoticeCdSeq,
                    NoticeContent = tkdNotice.NoticeContent,
                    NoticeDisplayKbn = tkdNotice.NoticeDisplayKbn,
                    SiyoKbn = tkdNotice.SiyoKbn
                };
            }
        }
    }
}

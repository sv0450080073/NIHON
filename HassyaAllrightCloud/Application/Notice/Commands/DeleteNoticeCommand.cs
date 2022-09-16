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
    public class DeleteNoticeCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<DeleteNoticeCommand, bool>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(DeleteNoticeCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == 0) return false;
                try
                {
                    var tkdNotice = context.TkdNotice.FirstOrDefault(x => x.NoticeCdSeq == request.Id);
                    if (tkdNotice == null) return false;
                    tkdNotice.SiyoKbn = (byte)SiyoKbn.NotUsable;

                    context.TkdNotice.Update(tkdNotice);

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

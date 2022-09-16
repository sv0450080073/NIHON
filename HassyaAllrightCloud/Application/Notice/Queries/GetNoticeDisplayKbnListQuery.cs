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
    public class GetNoticeDisplayKbnListQuery : IRequest<IEnumerable<NoticeDisplayKbnDto>>
    {
        public class Handler : IRequestHandler<GetNoticeDisplayKbnListQuery, IEnumerable<NoticeDisplayKbnDto>>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext kobodbContext)
            {
                this.context = kobodbContext;
            }

            public async Task<IEnumerable<NoticeDisplayKbnDto>> Handle(GetNoticeDisplayKbnListQuery request, CancellationToken cancellationToken)
            {
                List<NoticeDisplayKbnDto> result = null;
                context.LoadStoredProc("dbo.PK_hNoticeDisplayKbn_R")
                    .AddParam("@SiyoKbn", Constants.SiyoKbn)
                    .Exec(rows => result = rows.ToList<NoticeDisplayKbnDto>());
                var noticeDisplayKbnAllItem = result.FirstOrDefault(x => x.CodeKbn == Constants.NoticeDisplayKbnAll.ToString());
                if (noticeDisplayKbnAllItem != null) 
                { 
                    result.Remove(noticeDisplayKbnAllItem); 
                }
                return result;
            }
        }
    }
}

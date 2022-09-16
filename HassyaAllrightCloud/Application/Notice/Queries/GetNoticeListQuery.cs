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
    public class GetNoticeListQuery : IRequest<IEnumerable<Tkd_NoticeListDto>>
    {
        public class Handler : IRequestHandler<GetNoticeListQuery, IEnumerable<Tkd_NoticeListDto>>
        {
            private readonly KobodbContext context;

            public Handler(KobodbContext kobodbContext)
            {
                this.context = kobodbContext;
            }

            public async Task<IEnumerable<Tkd_NoticeListDto>> Handle(GetNoticeListQuery request, CancellationToken cancellationToken)
            {
                IEnumerable<Tkd_NoticeListDto> result = null;
                context.LoadStoredProc("dbo.PK_hNotice_R")
                    .AddParam("@CompanyCdSeq", new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID)
                    .AddParam("@EigyoCdSeq", new HassyaAllrightCloud.Domain.Dto.ClaimModel().EigyoCdSeq)
                    .Exec(rows => result = rows.ToList<Tkd_NoticeListDto>());

                return result;
            }
        }
    }
}

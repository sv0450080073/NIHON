using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.NotificationTemplate.Queries
{
    public class GetNotificationTemplateQuery : IRequest<List<NotificationTemplateData>>
    {
        public int TenantId { get; set; }
        public NotificationSendMethod Method { get; set; }
        public string CodeKbn { get; set; }
        public class Handler : IRequestHandler<GetNotificationTemplateQuery, List<NotificationTemplateData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public async Task<List<NotificationTemplateData>> Handle(GetNotificationTemplateQuery request, CancellationToken cancellationToken)
            {
                List<NotificationTemplateData> notiTemplateDatas = await (from notiTemplate in _dbContext.VpmNotiTemplate
                                                                  join codeKb in _dbContext.VpmCodeKb
                                                                    on notiTemplate.NotiContentKbn equals codeKb.CodeKbnSeq
                                                                  where codeKb.CodeKbn == request.CodeKbn
                                                                    && (request.Method == NotificationSendMethod.Both || notiTemplate.NotiMethodKbn == (int)request.Method)
                                                                    && notiTemplate.TenantCdSeq == request.TenantId
                                                                  orderby notiTemplate.NotiContentKbn ascending, notiTemplate.LineNum ascending
                                                                  select new NotificationTemplateData
                                                                  {
                                                                      LineNum = notiTemplate.LineNum,
                                                                      ContentKbn = notiTemplate.ContentKbn,
                                                                      LineContent = notiTemplate.LineContent,
                                                                      NotiMethod = notiTemplate.NotiMethodKbn

                                                                  }).ToListAsync();
                if (notiTemplateDatas.Count > 0 || request.TenantId == 0)
                {
                    return notiTemplateDatas;
                }
                return await Handle(new GetNotificationTemplateQuery() { TenantId = 0, Method = request.Method, CodeKbn = request.CodeKbn }, cancellationToken);
            }
        }
    }
}

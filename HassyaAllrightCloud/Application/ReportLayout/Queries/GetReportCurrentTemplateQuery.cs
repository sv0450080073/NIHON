using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ReportLayout.Queries
{
    public class GetReportCurrentTemplateQuery : IRequest<int>
    {
        public int TenantCdSeq { get; set; }
        public int EigyouCdSeq { get; set; }
        public ReportIdForSetting ReportId { get; set; }
        public class Handler : IRequestHandler<GetReportCurrentTemplateQuery, int>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<int> Handle(GetReportCurrentTemplateQuery request, CancellationToken cancellationToken)
            {
                VpmReportSetting ReportSetting = await (from reportSetting in _dbContext.VpmReportSetting
                                                        where reportSetting.SiyoKbn == 1
                                                           && reportSetting.ReportId == (int)request.ReportId
                                                           && (reportSetting.TenantCdSeq == request.TenantCdSeq && (reportSetting.EigyoCdSeq == 0 || reportSetting.EigyoCdSeq == request.EigyouCdSeq))
                                                        orderby reportSetting.EigyoCdSeq descending
                                                        select reportSetting).FirstOrDefaultAsync();
                if (ReportSetting == null)
                {
                    return 1;
                }
                return ReportSetting.CurrentTemplateId;
            }
        }
    }
}

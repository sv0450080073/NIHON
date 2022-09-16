using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AlertSetting.Queries
{
    public class GetShowAlertSettingAsyncQuery : IRequest<List<ShowAlertSettingGrid>>
    {
        public int tenantCdSeq { get; set; }
        public int syainCdSeq { get; set; }

        public class Handler : IRequestHandler<GetShowAlertSettingAsyncQuery, List<ShowAlertSettingGrid>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<ShowAlertSettingGrid>> Handle(GetShowAlertSettingAsyncQuery request, CancellationToken cancellationToken = default)
            {
                return await (from vpmAlert in _context.VpmAlert
                              join vpmAlert1 in _context.VpmAlert
                              on new { alertCd = vpmAlert.AlertCd, tenantCdSeq = request.tenantCdSeq } equals
                              new { alertCd = vpmAlert1.AlertCd, tenantCdSeq = vpmAlert1.TenantCdSeq }
                              into _vpmAlert1
                              from vpmAlert1 in _vpmAlert1.DefaultIfEmpty()
                              join vpmAlertSet in _context.VpmAlertSet
                              on new { alertCd = vpmAlert.AlertCd, syainCdSeq = request.syainCdSeq } equals
                              new { alertCd = vpmAlertSet.AlertCd, syainCdSeq = vpmAlertSet.SyainCdSeq }
                              into _vpmAlertSet
                              from vpmAlertSet in _vpmAlertSet.DefaultIfEmpty()
                              where vpmAlert.TenantCdSeq == 0 && vpmAlert.SiyoKbn == 1
                              select new ShowAlertSettingGrid()
                              {
                                  TenantCdSeq = vpmAlert.TenantCdSeq,
                                  AlertCdSeq = vpmAlert.AlertCdSeq,
                                  AlertKbn = vpmAlert.AlertKbn,
                                  AlertCd = vpmAlert.AlertCd,
                                  AlertNm = vpmAlert.AlertNm,
                                  CurTenantCdSeq = vpmAlert1.TenantCdSeq,
                                  CurDisplayKbn = vpmAlert1.DefaultDisplayKbn,
                                  SyainCdSeq = vpmAlertSet.SyainCdSeq,
                                  UserDisplayKbn = vpmAlertSet.DisplayKbn,
                                  Checked = vpmAlertSet.DisplayKbn == 1,
                                  AlertColor = vpmAlert.AlertKbn == 1 ? "#fff59d" : vpmAlert.AlertKbn == 2 ? "#ffcc80"
                                  : vpmAlert.AlertKbn == 3 ? "#c4e1a4" : vpmAlert.AlertKbn == 4 ? "#bbdefb" : string.Empty
                              }).Where(x => x.CurDisplayKbn != 2).Distinct().ToListAsync();
            }
        }
    }
}

using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCForm.Queries
{
    public class GetRyoKin : IRequest<List<RyokinDataItem>>
    {
        public class Handler : IRequestHandler<GetRyoKin, List<RyokinDataItem>>
        {
            KobodbContext _kobodbContext { get; set; }
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }
            public async Task<List<RyokinDataItem>> Handle(GetRyoKin request, CancellationToken cancellationToken)
            {
                var currentDate = CommonUtil.CurrentYYYYMMDD;
                return await _kobodbContext.VpmRyokin.Where(e => e.SiyoEndYmd.CompareTo(currentDate) > -1).OrderBy(e => e.RyokinTikuCd).ThenBy(e => e.RyokinCd).Select(e => new RyokinDataItem()
                {
                    RyokinCd = e.RyokinCd,
                    RyokinNm = e.RyokinNm,
                    RyokinTikuCd = e.RyokinTikuCd.ToString()
                }).ToListAsync(cancellationToken);
            }
        }

    }
}

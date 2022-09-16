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

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Queries
{
    public class GetRyokinSplit : IRequest<List<RyokinSplitModel>>
    {
        public RyokinSplitSearchModel Model { get; set; }
        public class Handler : IRequestHandler<GetRyokinSplit, List<RyokinSplitModel>>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<List<RyokinSplitModel>> Handle(GetRyokinSplit request, CancellationToken cancellationToken)
            {
                try
                {
                    var iriRyoChiCd = byte.Parse(request.Model.IriRyoChiCd);
                    var deRyoChiCd = byte.Parse(request.Model.DeRyoChiCd);
                    var currentDate = CommonUtil.CurrentYYYYMMDD;
                    return await _context.VpmRyokinSplit.Where(e => e.TargetEntranceRyokinTikuCd == iriRyoChiCd && 
                            e.TargetEntranceRyokinCd == request.Model.IriRyoCd &&
                            e.TargetExitRyokinTikuCd == deRyoChiCd &&
                            e.TargetExitRyokinCd == request.Model.DeRyoCd &&
                            e.SiyoEndYmd.CompareTo(currentDate) > -1).Select(e => new RyokinSplitModel() { 
                                EntranceRyokinTikuCd = e.EntranceRyokinTikuCd.ToString(),
                                EntranceRyokinCd = e.EntranceRyokinCd.ToString(),
                                ExitRyokinTikuCd = e.ExitRyokinTikuCd.ToString(),
                                ExitRyokinCd = e.ExitRyokinCd,
                                Fee = e.Fee
                            }).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

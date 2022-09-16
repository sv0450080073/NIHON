using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Queries
{
    public class IsRyokinExist : IRequest<bool>
    {
        public RyokinSearchModel Model { get; set; }
        public class Handler : IRequestHandler<IsRyokinExist, bool>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<bool> Handle(IsRyokinExist request, CancellationToken cancellationToken)
            {
                try
                {
                    var currentDate = CommonUtil.CurrentYYYYMMDD;
                    var cd = byte.Parse(request.Model.RyokinTikuCd);
                    return await _context.VpmRyokin.Where(e => e.RyokinTikuCd == cd && 
                    e.RyokinCd == request.Model.RyokinCd && 
                    e.SiyoEndYmd.CompareTo(currentDate) > -1).AnyAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

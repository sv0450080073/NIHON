using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Queries
{
    public class HaitaCheck : IRequest<bool>
    {
        public List<HaiTaParam> HaiTaParams { get; set; }
        public class Handler : IRequestHandler<HaitaCheck, bool>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }

            public async Task<bool> Handle(HaitaCheck request, CancellationToken cancellationToken)
            {
                var isValid = true;
                foreach (var item in request.HaiTaParams)
                {
                    isValid = await _dbContext.TkdYyksho.AnyAsync(_ => _.UpdYmd == item.UpdYmd && _.UpdTime == item.UpdTime && _.UkeNo == item.UkeNo);
                    if (!isValid) return isValid;
                }
                return isValid;
            }
        }
    }
}

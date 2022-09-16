using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FareFeeCorrection.Queries
{
    public class GetHaitaCheck : IRequest<HaitaCheckModel>
    {
        public string UkeNo { get; set; }
        public class Handler : IRequestHandler<GetHaitaCheck, HaitaCheckModel>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<HaitaCheckModel> Handle(GetHaitaCheck request, CancellationToken cancellationToken)
            {
                var yyksho = await _context.TkdYyksho.Select(e => new { e.UkeNo, e.UpdYmd, e.UpdTime }).FirstOrDefaultAsync(e => e.UkeNo == request.UkeNo);
                var haisha = await _context.TkdHaisha.Select(e => new { e.UkeNo, e.UpdYmd, e.UpdTime }).Where(e => e.UkeNo == request.UkeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                var yousha = await _context.TkdYousha.Select(e => new { e.UkeNo, e.UpdYmd, e.UpdTime }).Where(e => e.UkeNo == request.UkeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                long a = 0, b = 0, c = 0;
                if(yyksho != null)
                    long.TryParse(yyksho.UpdYmd + yyksho.UpdTime, out a);
                if (haisha != null)
                    long.TryParse(haisha.UpdYmd + haisha.UpdTime, out b);
                if (yousha != null)
                    long.TryParse(yousha.UpdYmd + yousha.UpdTime, out c);

                return new HaitaCheckModel()
                {
                    YykshoUpdYmdTime = a,
                    HaishaMaxUpdYmdTime = b,
                    YoushaMaxUpdYmdTime = c
                };
            }
        }
    }
}

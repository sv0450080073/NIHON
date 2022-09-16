using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class FutTumMishumMaxUpdYmdTime : IRequest<MaxUpdYmdTime>
    {
        private readonly string _ukeNo;
        private readonly IncidentalViewMode _mode;
        public FutTumMishumMaxUpdYmdTime(string ukeNo, IncidentalViewMode mode)
        {
            _ukeNo = ukeNo;
            _mode = mode;
        }

        public class Handler : IRequestHandler<FutTumMishumMaxUpdYmdTime, MaxUpdYmdTime>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<MaxUpdYmdTime> Handle(FutTumMishumMaxUpdYmdTime request, CancellationToken cancellationToken)
            {
                TkdFutTum futtum;
                TkdMishum mishum;
                if (request._mode == IncidentalViewMode.Futai)
                {
                    futtum = _context.TkdFutTum.Where(e => e.FutTumKbn == 1 && e.UkeNo == request._ukeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefault();
                    mishum = _context.TkdMishum.Where(e => e.SeiFutSyu == 2 && e.UkeNo == request._ukeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefault();
                }
                else
                {
                    futtum = _context.TkdFutTum.Where(e => e.FutTumKbn == 2 && e.UkeNo == request._ukeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefault();
                    mishum = _context.TkdMishum.Where(e => e.SeiFutSyu == 6 && e.UkeNo == request._ukeNo).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefault();
                }
                long futTumMaxUpdYmdTime = 0;
                long mishumMaxUpdYmdTime = 0;
                if (futtum != null)
                    long.TryParse(futtum.UpdYmd + futtum.UpdTime, out futTumMaxUpdYmdTime);
                if (mishum != null)
                    long.TryParse(mishum.UpdYmd + mishum.UpdTime, out mishumMaxUpdYmdTime);
                return new MaxUpdYmdTime()
                {
                    FutTumMaxUpdYmdTime = futTumMaxUpdYmdTime,
                    MishumMaxUpdYmdTime = mishumMaxUpdYmdTime
                };
            }
        }
    }
}

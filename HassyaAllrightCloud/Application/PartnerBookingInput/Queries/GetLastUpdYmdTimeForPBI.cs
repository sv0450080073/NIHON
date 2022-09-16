using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetLastUpdYmdTimeForPBI : IRequest<ParterBookingInputHaita>
    {
        private readonly string _ukeno;
        private readonly short _unkRen;
        private readonly int _youTblSeq;
        public GetLastUpdYmdTimeForPBI(string ukeNo, short unkRen, int youTblSeq)
        {
            _ukeno = ukeNo;
            _unkRen = unkRen;
            _youTblSeq = youTblSeq;
        }
        public class Handler : IRequestHandler<GetLastUpdYmdTimeForPBI, ParterBookingInputHaita>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<ParterBookingInputHaita> Handle(GetLastUpdYmdTimeForPBI request, CancellationToken cancellationToken)
            {
                if(request._youTblSeq == -1)
                {
                    var haisha = await _context.TkdHaisha.Where(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                    return new ParterBookingInputHaita()
                    {
                        HaishaUpdYmdTime = haisha == null ? string.Empty : haisha.UpdYmd + haisha.UpdTime
                    };
                }
                else
                {
                    var haisha = await _context.TkdHaisha.Where(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                    var yousha = await _context.TkdYousha.FirstOrDefaultAsync(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen && e.YouTblSeq == request._youTblSeq);
                    var youSyu = await _context.TkdYouSyu.Where(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen && e.YouTblSeq == request._youTblSeq).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                    var mihrim = await _context.TkdMihrim.Where(e => e.UkeNo == request._ukeno).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();
                    var yyksho = await _context.TkdYyksho.FirstOrDefaultAsync(e => e.UkeNo == request._ukeno);
                    var unkobi = await _context.TkdUnkobi.FirstOrDefaultAsync(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen);
                    var haiin = await _context.TkdHaiin.Where(e => e.UkeNo == request._ukeno && e.UnkRen == request._unkRen).OrderByDescending(e => e.UpdYmd + e.UpdTime).FirstOrDefaultAsync();

                    return new ParterBookingInputHaita()
                    {
                        HaishaUpdYmdTime = haisha == null ? string.Empty : haisha.UpdYmd + haisha.UpdTime,
                        YoushaUpdYmdTime = yousha == null ? string.Empty : yousha.UpdYmd + yousha.UpdTime,
                        YouSyuUpdYmdTime = youSyu == null ? string.Empty : youSyu.UpdYmd + youSyu.UpdTime,
                        MihrimUpdYmdTime = mihrim == null ? string.Empty : mihrim.UpdYmd + mihrim.UpdTime,
                        YykshoUpdYmdTime = yyksho == null ? string.Empty : yyksho.UpdYmd + yyksho.UpdTime,
                        UnkobiUpdYmdTime = unkobi == null ? string.Empty : unkobi.UpdYmd + unkobi.UpdTime,
                        HaiinUpdYmdTime = haiin == null ? string.Empty : haiin.UpdYmd + haiin.UpdTime
                    };
                }
            }
        }

    }
}

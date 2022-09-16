using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetCommonLastUpdatedYmdTime : IRequest<CommonLastUpdatedYmdTime>
    {
        public int TenantCdSeq { get; set; }
        public string Ukeno { get; set; }
        public byte SeiFutSyu { get; set; }
        public short UnkRen { get; set; }
        public short FutTumRen { get; set; }
        public byte FutTumKbn { get; set; }
        public GetCommonLastUpdatedYmdTime(int tenantCdSeq, string ukeNo, byte seiFutSyu, short unkRen, short futtumRen, byte futtumKbn)
        {
            TenantCdSeq = tenantCdSeq;
            Ukeno = ukeNo;
            SeiFutSyu = seiFutSyu;
            UnkRen = unkRen;
            FutTumRen = futtumRen;
            FutTumKbn = futtumKbn;
        }
        public class Handler : IRequestHandler<GetCommonLastUpdatedYmdTime, CommonLastUpdatedYmdTime>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<CommonLastUpdatedYmdTime> Handle(GetCommonLastUpdatedYmdTime request, CancellationToken cancellationToken)
            {
                var result = new CommonLastUpdatedYmdTime();

                var mishum = await _context.TkdMishum.FirstOrDefaultAsync(e => e.UkeNo == request.Ukeno && e.SeiFutSyu == request.SeiFutSyu);
                var yyksho = await _context.TkdYyksho.FirstOrDefaultAsync(e => e.UkeNo == request.Ukeno);
                var futtum = await _context.TkdFutTum.FirstOrDefaultAsync(e => e.UkeNo == request.Ukeno && e.UnkRen == request.UnkRen && e.FutTumRen == request.FutTumRen && e.FutTumKbn == request.FutTumKbn);
                
                if (mishum != null)
                {
                    result.MishumUpdYmd = mishum.UpdYmd;
                    result.MishumUpdTime = mishum.UpdTime;
                }

                if (yyksho != null)
                {
                    result.YykshoUpdYmd = yyksho.UpdYmd;
                    result.YykshoUpdTime = yyksho.UpdTime;
                }

                if (futtum != null)
                {
                    result.FuttumUpdYmd = futtum.UpdYmd;
                    result.FuttumUpdTime = futtum.UpdTime;
                }

                return result;
            }
        }
    }
}

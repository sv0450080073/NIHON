using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CouponPayment.Queries
{
    public class GetSpecLastUpdatedYmdTime : IRequest<SpecLastUpdatedYmdTime>
    {
        public int TenantCdSeq { get; set; }
        public string Ukeno { get; set; }
        public byte SeiFutSyu { get; set; }
        public short UnkRen { get; set; }
        public short FutTumRen { get; set; }
        public byte FutTumKbn { get; set; }
        public int NyuSihTblSeq { get; set; }
        public short NyuSihRen { get; set; }
        public GetSpecLastUpdatedYmdTime(int tenantCdSeq, string ukeNo, byte seiFutSyu, short unkRen, short futtumRen, byte futtumKbn, int nyuSihTblSeq, short nyuSihRen)
        {
            TenantCdSeq = tenantCdSeq;
            Ukeno = ukeNo;
            SeiFutSyu = seiFutSyu;
            UnkRen = unkRen;
            FutTumRen = futtumRen;
            FutTumKbn = futtumKbn;
            NyuSihTblSeq = nyuSihTblSeq;
            NyuSihRen = nyuSihRen;
        }
        public class Handler : IRequestHandler<GetSpecLastUpdatedYmdTime, SpecLastUpdatedYmdTime>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<SpecLastUpdatedYmdTime> Handle(GetSpecLastUpdatedYmdTime request, CancellationToken cancellationToken)
            {
                var result = new SpecLastUpdatedYmdTime();

                var nyuSih = await _context.TkdNyuSih.FirstOrDefaultAsync(e => e.NyuSihTblSeq == request.NyuSihTblSeq && e.TenantCdSeq == request.TenantCdSeq);
                var nyShmi = await _context.TkdNyShmi.FirstOrDefaultAsync(e => e.UkeNo == request.Ukeno && e.TenantCdSeq == request.TenantCdSeq && e.NyuSihRen == request.NyuSihRen);

                if (nyuSih != null)
                {
                    result.NyuSihUpdYmd = nyuSih.UpdYmd;
                    result.NyuSihUpdTime = nyuSih.UpdTime;
                }

                if (nyShmi != null)
                {
                    result.NyShmiUpdYmd = nyShmi.UpdYmd;
                    result.NyShmiUpdTime = nyShmi.UpdTime;
                }
                return result;
            }
        }
    }
}

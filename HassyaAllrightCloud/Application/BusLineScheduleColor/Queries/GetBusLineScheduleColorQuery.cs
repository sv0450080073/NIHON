using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusLineScheduleColor.Queries
{
    public class GetBusLineScheduleColorQuery : IRequest<(string cssColor, bool isLock)>
    {
        private readonly string _ukeNo;
        private readonly short _unkRen;
        private readonly short _teiDanNo;
        private readonly short _bunkRen;

        public GetBusLineScheduleColorQuery(string ukeNo, short unkRen, short teiDanNo, short bunkRen)
        {
            _ukeNo = ukeNo;
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
            _bunkRen = bunkRen;
        }

        public class Handler : IRequestHandler<GetBusLineScheduleColorQuery, (string cssColor, bool isLock)>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<(string cssColor, bool isLock)> Handle(GetBusLineScheduleColorQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var yyksho = await _context.TkdYyksho.SingleOrDefaultAsync(y => y.UkeNo == request._ukeNo);
                    if (yyksho is null) throw new Exception(string.Format("Booking ukeNo {0} is not exist", request._ukeNo));
                    string cssClassResult = string.Empty;
                    bool isLockResult = false;

                    var haisha = await _context.TkdHaisha
                        .SingleOrDefaultAsync(h => h.UkeNo == request._ukeNo
                                                && h.UnkRen == request._unkRen
                                                && h.TeiDanNo == request._teiDanNo
                                                && h.BunkRen == request._bunkRen);

                    if (haisha.HaiSsryCdSeq == 0 && haisha.YouTblSeq != 0)
                    {
                        cssClassResult = BusCssClass.YoushazumiColor;
                        isLockResult = await CheckLockQuery(yyksho) || haisha.NippoKbn == 2;
                    }
                    else if (haisha.HaiSsryCdSeq == 0 && haisha.YouTblSeq == 0)
                    {
                        cssClassResult = BusCssClass.MikarishaColor;
                        isLockResult = await CheckLockQuery(yyksho) || haisha.NippoKbn == 2;
                    }
                    else if (await CheckLockQuery(yyksho))
                    {
                        cssClassResult = BusCssClass.SimezumiColor;
                        isLockResult = true;
                    }
                    else if (haisha.NippoKbn == 2)
                    {
                        cssClassResult = BusCssClass.NippozumiColor;
                        isLockResult = true;
                    }
                    //else if (haisha.HaiIkbn == 2)
                    //{
                    //    cssClassResult = BusCssClass.HaiinzumiColor;
                    //    isLockResult = false;
                    //}
                    else if (haisha.HaiSkbn == 2)
                    {
                        cssClassResult = BusCssClass.HaishazumiColor;
                        isLockResult = false;
                    }
                    else if (DateTime.TryParseExact(yyksho.KaktYmd, "yyyyMMdd", null, DateTimeStyles.None, out _))
                    {
                        cssClassResult = BusCssClass.KakuteiColor;
                        isLockResult = false;
                    }
                    else if (await _context.TkdKaknin.Where(k => k.UkeNo == request._ukeNo).CountAsync() > 0)
                    {
                        cssClassResult = BusCssClass.KakuteichuColor;
                        isLockResult = false;
                    }
                    else
                    {
                        cssClassResult = BusCssClass.KarishaColor;
                        isLockResult = false;
                    }

                    return (cssColor: cssClassResult, isLock: isLockResult);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Check if satisfy (line 1) condition - 3804
            /// </summary>
            /// <param name="ukeNo"></param>
            /// <returns>true: SeiTaiYmd <= LockYmd</returns>
            private async Task<bool> CheckLockQuery(TkdYyksho yyksho)
            {
                var lockYmd = await (from l in _context.TkdLockTable
                                     where l.TenantCdSeq == yyksho.TenantCdSeq && l.EigyoCdSeq == yyksho.SeiEigCdSeq
                                     select l.LockYmd).SingleOrDefaultAsync();

                if (DateTime.TryParseExact(lockYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime lockDate)
                    && DateTime.TryParseExact(yyksho.SeiTaiYmd, "yyyyMMdd", null, DateTimeStyles.None, out DateTime seiTaiDate))
                {
                    if (seiTaiDate <= lockDate)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}

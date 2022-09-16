using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BookingInputData;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Queries
{
    public class GetRemoveRequireDataByUkenoQuery : IRequest<BookingSoftRemoveEntitiesData>
    {
        public string Ukeno { get; set; }
        public BookingFormData BookingData { get; set; }

        public class Handler : IRequestHandler<GetRemoveRequireDataByUkenoQuery, BookingSoftRemoveEntitiesData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetRemoveRequireDataByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetRemoveRequireDataByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<BookingSoftRemoveEntitiesData> Handle(GetRemoveRequireDataByUkenoQuery request, CancellationToken cancellationToken)
            {
                var result = new BookingSoftRemoveEntitiesData();
                var teidans = request.BookingData?.ListToRemove?.Select(l => l.TeidanNo).ToList() ?? new List<short>();
                var youtblseqs = request.BookingData?.ListToRemove?.SelectMany(l => l.YouTblSeqList).Where(y => y != 0).Distinct().ToList() ?? new List<int>();

                var youSyus = new List<TkdYouSyu>();
                youSyus = await _context.TkdYouSyu.Where(yousyu =>
                        yousyu.UkeNo == request.Ukeno
                        && yousyu.UnkRen == request.BookingData.UnkRen
                        && youtblseqs.Contains(yousyu.YouTblSeq)
                        && yousyu.SiyoKbn == 1)
                    .ToListAsync();
                youSyus.ForEach(yousyu => yousyu.SiyoKbn = 2);
                var youshas = new List<TkdYousha>();
                youshas = await _context.TkdYousha.Where(yousha =>
                        yousha.UkeNo == request.Ukeno
                        && yousha.UnkRen == request.BookingData.UnkRen
                        && youtblseqs.Contains(yousha.YouTblSeq)
                        && yousha.SiyoKbn == 1)
                    .ToListAsync();
                youshas.ForEach(yousha => yousha.SiyoKbn = 2);
                var ymfuTus = new List<TkdYmfuTu>();
                ymfuTus = await _context.TkdYmfuTu.Where(ymfuttu =>
                        ymfuttu.UkeNo == request.Ukeno
                        && ymfuttu.UnkRen == request.BookingData.UnkRen
                        && youtblseqs.Contains(ymfuttu.YouTblSeq)
                        && ymfuttu.SiyoKbn == 1)
                    .ToListAsync();
                ymfuTus.ForEach(ymfuttu => ymfuttu.SiyoKbn = 2);
                var yfuTus = new List<TkdYfutTu>();
                yfuTus = await _context.TkdYfutTu.Where(yfuttu =>
                        yfuttu.UkeNo == request.Ukeno
                        && yfuttu.UnkRen == request.BookingData.UnkRen
                        && youtblseqs.Contains(yfuttu.YouTblSeq)
                        && yfuttu.SiyoKbn == 1)
                    .ToListAsync();
                yfuTus.ForEach(yfuttu => yfuttu.SiyoKbn = 2);
                var mihrims = new List<TkdMihrim>();
                mihrims = await _context.TkdMihrim.Where(ymfuttu =>
                        ymfuttu.UkeNo == request.Ukeno
                        && ymfuttu.UnkRen == request.BookingData.UnkRen
                        && youtblseqs.Contains(ymfuttu.YouTblSeq)
                        && ymfuttu.SiyoKbn == 1)
                    .ToListAsync();
                mihrims.ForEach(ymfuttu => ymfuttu.SiyoKbn = 2);

                var mfutTus = new List<TkdMfutTu>();
                mfutTus = await _context.TkdMfutTu.Where(mfuttu =>
                        mfuttu.UkeNo == request.Ukeno
                        && mfuttu.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(mfuttu.TeiDanNo)
                        && mfuttu.SiyoKbn == 1)
                    .ToListAsync();
                mfutTus.ForEach(mfuttu => mfuttu.SiyoKbn = 2);
                var tehais = new List<TkdTehai>();
                tehais = await _context.TkdTehai.Where(tehai =>
                        tehai.UkeNo == request.Ukeno
                        && tehai.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(tehai.TeiDanNo)
                        && tehai.SiyoKbn == 1)
                    .ToListAsync();
                tehais.ForEach(tehai => tehai.SiyoKbn = 2);
                var koteis = new List<TkdKotei>();
                koteis = await _context.TkdKotei.Where(kotei =>
                        kotei.UkeNo == request.Ukeno
                        && kotei.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(kotei.TeiDanNo)
                        && kotei.SiyoKbn == 1)
                    .ToListAsync();
                koteis.ForEach(kotei => kotei.SiyoKbn = 2);
                var koteiks = new List<TkdKoteik>();
                koteiks = await _context.TkdKoteik.Where(koteik =>
                        koteik.UkeNo == request.Ukeno
                        && koteik.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(koteik.TeiDanNo)
                        && koteik.SiyoKbn == 1)
                    .ToListAsync();
                koteiks.ForEach(koteik => koteik.SiyoKbn = 2);

                var haiins = new List<TkdHaiin>();
                haiins = await _context.TkdHaiin.Where(haiin =>
                        haiin.UkeNo == request.Ukeno
                        && haiin.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(haiin.TeiDanNo)
                        && haiin.SiyoKbn == 1)
                    .ToListAsync();
                haiins.ForEach(haiin => haiin.SiyoKbn = 2);

                var kobans = new List<TkdKoban>();
                kobans = await _context.TkdKoban.Where(koban =>
                        koban.UkeNo == request.Ukeno
                        && koban.UnkRen == request.BookingData.UnkRen
                        && teidans.Contains(koban.TeiDanNo)
                        && koban.SiyoKbn == 1)
                    .ToListAsync();

                result.YouSyus = youSyus;
                result.Youshas = youshas;
                result.YmfuTus = ymfuTus;
                result.YfutTus = yfuTus;
                result.Mihrims = mihrims;
                result.MfutTus = mfutTus;
                result.Tehais = tehais;
                result.Koteis = koteis;
                result.Koteiks = koteiks;
                result.Haiins = haiins;
                result.Kobans = kobans;
                return result;
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetMihrimByUkenoQuery : IRequest<TkdMihrim>
    {
        public string Ukeno { get; set; }
        public YouShaDataInsert YouShaDataInsert { get; set; }

        public class Handler : IRequestHandler<GetMihrimByUkenoQuery, TkdMihrim>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetMihrimByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetMihrimByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<TkdMihrim> Handle(GetMihrimByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdMihrim tkdMihrim = _context.TkdMihrim.FirstOrDefault(e => e.UkeNo == request.Ukeno
                                                                            && e.UnkRen == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_UnkRen
                                                                            && e.YouTblSeq == request.YouShaDataInsert.YouShaDataPopup.YOUSHA_YouTblSeq);
                    SetTkdMihrimData(ref tkdMihrim, request.YouShaDataInsert);
                    return Task.FromResult(tkdMihrim);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdMihrimData(ref TkdMihrim tkdMihrim, YouShaDataInsert youShaDataInsert)
            {
                if (tkdMihrim != null && youShaDataInsert != null)
                {                 
                    tkdMihrim.HenKai++;                   
                    tkdMihrim.HaseiKin = youShaDataInsert.Sum_YOUSYU_SyaSyuTan;
                    tkdMihrim.SyaRyoSyo = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoSyo;
                    tkdMihrim.SyaRyoTes = youShaDataInsert.YouShaDataPopup.YOUSHA_SyaRyoTes;
                    tkdMihrim.YoushaGak = youShaDataInsert.YouShaDataPopup.Sum_MoneyAllShow;
                    tkdMihrim.SihRaiRui = 0;
                    tkdMihrim.CouKesRui = 0;
                    tkdMihrim.YouFutTumRen = 0;
                    tkdMihrim.SiyoKbn = 1;
                    tkdMihrim.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdMihrim.UpdTime = DateTime.Now.ToString("hhmmss");
                    tkdMihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdMihrim.UpdPrgId = "KU1700";
                }
            }
        }
    }
}

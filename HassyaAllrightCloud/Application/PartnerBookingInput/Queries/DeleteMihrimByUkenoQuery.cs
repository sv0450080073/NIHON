using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class DeleteMihrimByUkenoQuery : IRequest<TkdMihrim>
    {
        public string Ukeno { get; set; }
        public YouShaDataTable YouShaItemUpdate { get; set; }
        public class Handler : IRequestHandler<DeleteMihrimByUkenoQuery, TkdMihrim>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<DeleteMihrimByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<DeleteMihrimByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<TkdMihrim> Handle(DeleteMihrimByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdMihrim tkdMihrim = _context.TkdMihrim.FirstOrDefault(e => e.UkeNo == request.Ukeno
                                                                            && e.UnkRen == request.YouShaItemUpdate.YOUSHA_UnkRen
                                                                            && e.YouTblSeq == request.YouShaItemUpdate.YOUSHA_YouTblSeq);
                    SetTkdMihrimData(ref tkdMihrim, request.YouShaItemUpdate);
                    return Task.FromResult(tkdMihrim);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdMihrimData(ref TkdMihrim tkdMihrim, YouShaDataTable youShaItemUpdate)
            {
                if (tkdMihrim != null && youShaItemUpdate != null)
                {
                    tkdMihrim.HenKai++;                 
                    tkdMihrim.SiyoKbn = 2;
                    tkdMihrim.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdMihrim.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdMihrim.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdMihrim.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                }
            }
        }
    }
}

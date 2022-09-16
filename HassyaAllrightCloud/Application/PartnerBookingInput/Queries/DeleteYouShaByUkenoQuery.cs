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
    public class DeleteYouShaByUkenoQuery : IRequest<TkdYousha>
    {
        public string Ukeno { get; set; }
        public YouShaDataTable YouShaItemUpdate { get; set; }
        public class Handler : IRequestHandler<DeleteYouShaByUkenoQuery, TkdYousha>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<DeleteYouShaByUkenoQuery> _logger;
            public Handler(KobodbContext context, ILogger<DeleteYouShaByUkenoQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<TkdYousha> Handle(DeleteYouShaByUkenoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdYousha tkdYousha = _context.TkdYousha.FirstOrDefault(e => e.UkeNo == request.Ukeno
                                                                            && e.UnkRen == request.YouShaItemUpdate.YOUSHA_UnkRen
                                                                            && e.YouTblSeq == request.YouShaItemUpdate.YOUSHA_YouTblSeq);
                    SetTkdYoushaData(ref tkdYousha, request.YouShaItemUpdate);
                    return Task.FromResult(tkdYousha);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdYoushaData(ref TkdYousha tkdYousha, YouShaDataTable youShaItemUpdate)
            {
                if (tkdYousha != null && youShaItemUpdate != null)
                {
                    tkdYousha.HenKai++;
                    tkdYousha.SiyoKbn = 2;
                    tkdYousha.UpdYmd = DateTime.Now.ToString(CommonConstants.FormatUpdateDbDate);
                    tkdYousha.UpdTime = DateTime.Now.ToString(CommonConstants.FormatUpdateDbTime);
                    tkdYousha.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdYousha.UpdPrgId = ScreenCode.PartnerBookingInputUpdPrgId;
                }
            }
        }
    }
}

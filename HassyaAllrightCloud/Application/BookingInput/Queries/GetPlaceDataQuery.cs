using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HassyaAllrightCloud.IService;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetPlaceDataQuery : IRequest<IEnumerable<PlaceData>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetPlaceDataQuery, IEnumerable<PlaceData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetPlaceDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetPlaceDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<IEnumerable<PlaceData>> Handle(GetPlaceDataQuery request, CancellationToken cancellationToken)
            {
                var result = new List<PlaceData>();
                try
                {
                    //var id = await _codeSyuService.CheckTenantByKanriKbnAsync(new ClaimModel().TenantID, "BUNRUICD");
                    result = await (from h in _context.VpmHaichi
                                    join c in _context.VpmCodeKb on h.BunruiCdSeq equals c.CodeKbnSeq
                                    where c.CodeSyu == "BUNRUICD" && c.SiyoKbn == 1 && h.SiyoKbn == 1 && h.TenantCdSeq == new ClaimModel().TenantID
                                    orderby c.CodeKbn, h.HaiScd
                                    select new PlaceData
                                    {
                                        HaiSCd = h.HaiScd,
                                        HaiSCdSeq = h.HaiScdSeq,
                                        CodeKbn = c.CodeKbn,
                                        CodeKbnSeq = Convert.ToInt32(c.CodeKbnSeq),
                                        TpnCodeKbnRyakuNm = c.RyakuNm,
                                        VpmHaichiRyakuNm = h.RyakuNm
                                    }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
                return result;
            }
        }
    }
}

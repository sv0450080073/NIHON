using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetPassengerTypeQuery : IRequest<IEnumerable<PassengerType>>
    {
        public int TenantId { get; set; }
        public class Handler : IRequestHandler<GetPassengerTypeQuery, IEnumerable<PassengerType>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetPassengerTypeQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;

            public Handler(KobodbContext context, ILogger<GetPassengerTypeQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService;
            }

            public async Task<IEnumerable<PassengerType>> Handle(GetPassengerTypeQuery request, CancellationToken cancellationToken)
            {
                var result = new List<PassengerType>();
                try
                {
                    result = await _codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, code) =>
                    {
                        return await (from j in _context.VpmJyoKya
                                      where j.TenantCdSeq == request.TenantId
                                      join c in _context.VpmCodeKb on j.DantaiCdSeq equals c.CodeKbnSeq into gb
                                      from sgb in gb.DefaultIfEmpty()
                                      where sgb.CodeSyu == code && sgb.TenantCdSeq == tenantId
                                      select new PassengerType()
                                      {
                                          JyoKyakuCdSeq = j.JyoKyakuCdSeq,
                                          CodeKbn = sgb.CodeKbn,
                                          CodeKbnRyakuNm = sgb.RyakuNm,
                                          JyoKyakuCd = j.JyoKyakuCd,
                                          JyoKyaRyakuNm = j.JyoKyakuNm
                                      }).ToListAsync();
                    },
                    request.TenantId, "DANTAICD");
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

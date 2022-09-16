using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.AccessoryFeeListReport.Query
{
    public class GetLoadFutaiTypeQuery : IRequest<List<LoadFutaiType>>
    {
        private readonly ITPM_CodeSyService _codeSyuService;
        private readonly int _tenantId;

        public GetLoadFutaiTypeQuery(ITPM_CodeSyService codeSyuService, int tenantId)
        {
            _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            _tenantId = tenantId;
        }

        public class Handler : IRequestHandler<GetLoadFutaiTypeQuery, List<LoadFutaiType>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetLoadFutaiTypeQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetLoadFutaiTypeQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<LoadFutaiType>> Handle(GetLoadFutaiTypeQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        request._codeSyuService.FilterTenantIdByCodeSyu(async (tenantId, codeSyu) =>
                        {
                            return await Task.FromResult(
                                (from mcodeFufu in _context.VpmCodeKb
                                 where mcodeFufu.SiyoKbn == 1 
                                        && mcodeFufu.TenantCdSeq == tenantId
                                        && mcodeFufu.CodeSyu == codeSyu 
                                        && Convert.ToInt32(mcodeFufu.CodeKbn) != 5
                                 select new LoadFutaiType
                                 {
                                     FutGuiKbn = Convert.ToInt32(mcodeFufu.CodeKbn),
                                     CodeKbnSeq = mcodeFufu.CodeKbnSeq,
                                     CodeKbnNm = mcodeFufu.CodeKbnNm,
                                 }).ToList());
                        }, request._tenantId, "FUTGUIKBN");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<LoadFutaiType>();
                }
            }
        }
    }
}

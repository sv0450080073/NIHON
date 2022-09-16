using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingIncidental.Queries
{
    public class GetLoadSeisanKbnQuery : IRequest<List<LoadSeisanKbn>>
    {
        public readonly int _tenantId;
        private readonly ITPM_CodeSyService _codeSyuService;

        public GetLoadSeisanKbnQuery(ITPM_CodeSyService codeSyuService, int tenantId)
        {
            _tenantId = tenantId;
            _codeSyuService = codeSyuService;
        }

        public class Handler : IRequestHandler<GetLoadSeisanKbnQuery, List<LoadSeisanKbn>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<LoadSeisanKbn>> Handle(GetLoadSeisanKbnQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    int tenant = 0;
                    var count = _context.VpmCodeSy.Where(x => x.CodeSyu == "SEISANKBN" && x.KanriKbn != 1).ToList().Count;
                    if(count > 0)
                    {
                        tenant = new ClaimModel().TenantID;
                    }
                    var result = _context.VpmCodeKb.Where(x => x.SiyoKbn == 1 && x.CodeSyu == "SEISANKBN" && x.TenantCdSeq == tenant)
                        .Select(x => new LoadSeisanKbn()
                        {
                            RyakuName = x.RyakuNm,
                            CodeKbn = x.CodeKbn
                        }).ToList();

                    return result ?? new List<LoadSeisanKbn>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

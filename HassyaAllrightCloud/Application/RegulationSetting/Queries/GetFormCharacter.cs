using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RegulationSetting.Queries
{
    public class GetFormCharacter : IRequest<List<EiygoItem>>
    {
        public class Handler : IRequestHandler<GetFormCharacter, List<EiygoItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<EiygoItem>> Handle(GetFormCharacter request, CancellationToken cancellationToken)
            {
                var result = (from c in _context.VpmCodeKb
                              join s in _context.VpmCodeSy
                              on c.CodeSyu equals s.CodeSyu into csJoin
                              from csTemp in csJoin.DefaultIfEmpty()
                              where c.CodeSyu == "SENMJPTNKBN" && c.SiyoKbn == 1
                              && ((csTemp.KanriKbn == 1 && c.TenantCdSeq == 0) || (csTemp.KanriKbn != 1 && c.TenantCdSeq == new ClaimModel().TenantID))
                              orderby c.CodeKbn
                              select new EiygoItem() { 
                                  CodeKbnSeq = c.CodeKbnSeq,
                                  CodeKbn = c.CodeKbn,
                                  CodeKbnNm = c.CodeKbnNm,
                                  DisplayName = c.CodeKbnNm
                              }).ToList();
                return result;
            }
        }
    }
}

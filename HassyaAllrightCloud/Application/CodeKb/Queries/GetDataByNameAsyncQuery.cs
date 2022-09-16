using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CodeKb.Queries
{
    public class GetDataByNameAsyncQuery : IRequest<List<CodeDataModel>>
    {
        public string Name { get; set; }
        public class Handler : IRequestHandler<GetDataByNameAsyncQuery, List<CodeDataModel>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<List<CodeDataModel>> Handle(GetDataByNameAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.Name == null)
                {
                    return new List<CodeDataModel>();
                }

                return _context.VpmCodeKb.Where(x => x.CodeSyu.ToLower() == request.Name.ToLower() && x.SiyoKbn == 1 && x.TenantCdSeq == 0)
                                               .Select(y => new CodeDataModel()
                                               {
                                                   CodeKbn = y.CodeKbn,
                                                   CodeKbnNm = y.CodeKbnNm,
                                                   CodeKbnSeq = y.CodeKbnSeq
                                               }).ToList();
            }
        }
    }
}

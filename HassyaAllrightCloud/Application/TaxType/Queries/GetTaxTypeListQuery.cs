using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TaxType.Queries
{
    public class GetTaxTypeListQuery : IRequest<List<TaxTypeList>>
    {
        public class Handler : IRequestHandler<GetTaxTypeListQuery, List<TaxTypeList>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TaxTypeList>> Handle(GetTaxTypeListQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = _context.VpmCodeKb
                        .Where(c => c.CodeSyu == "ZEIKBN")
                        .OrderBy(_ => _.CodeKbn)
                        .ToList()
                        .Select(c => new TaxTypeList()
                        {
                            IdValue = int.Parse(c.CodeKbn),
                            StringValue = c.RyakuNm
                        }).ToList();

                    return await Task.FromResult(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

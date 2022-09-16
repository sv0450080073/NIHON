using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RegulationSetting.Queries
{
    public class GetExistKasSet : IRequest<bool>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<GetExistKasSet, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(GetExistKasSet request, CancellationToken cancellationToken)
            {
                var result = false;
                var kasSet = _context.TkmKasSet.Where(x => x.CompanyCdSeq == request.CompanyCdSeq).FirstOrDefault();
                if(kasSet != null)
                {
                    result = true;
                }
                return result;
            }
        }
    }
}

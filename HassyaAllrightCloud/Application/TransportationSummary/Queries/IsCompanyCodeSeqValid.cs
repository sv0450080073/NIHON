using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{
    public class IsCompanyCodeSeqValid : IRequest<bool>
    {
        public int CompanyCdSeq { get; set; }
        public class Handler : IRequestHandler<IsCompanyCodeSeqValid, bool>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Check if Company code is valid and active
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> Handle(IsCompanyCodeSeqValid request, CancellationToken cancellationToken)
            {
                return await _kobodbContext.VpmCompny.AnyAsync(e => e.CompanyCdSeq == request.CompanyCdSeq && e.SiyoKbn == 1, cancellationToken);
            }
        }
    }
}

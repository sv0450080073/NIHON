using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETCImportConditionSetting.Queries
{
    public class GetFutaiNmByFutaiCdSeq : IRequest<string>
    {
        public int CdSeq { get; set; }
        public class Handler : IRequestHandler<GetFutaiNmByFutaiCdSeq, string>
        {
            KobodbContext _context;
            public Handler(KobodbContext context) => _context = context;
            public async Task<string> Handle(GetFutaiNmByFutaiCdSeq request, CancellationToken cancellationToken)
            {
                var entity = await _context.VpmFutai.Where(e => e.FutaiCdSeq == request.CdSeq).FirstOrDefaultAsync();
                return entity == null ? string.Empty : entity.FutaiNm;
            }
        }
    }
}

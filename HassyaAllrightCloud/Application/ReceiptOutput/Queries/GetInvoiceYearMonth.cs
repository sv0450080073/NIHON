using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ReceiptOutput.Queries
{
    public class GetInvoiceYearMonth : IRequest<string>
    {
        public class Handler : IRequestHandler<GetInvoiceYearMonth, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<string> Handle(GetInvoiceYearMonth request, CancellationToken cancellationToken)
            {
                return _context.VpmCompny.FirstOrDefault(x => x.CompanyCdSeq == new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID).SyoriYm;
            }
        }
    }
}

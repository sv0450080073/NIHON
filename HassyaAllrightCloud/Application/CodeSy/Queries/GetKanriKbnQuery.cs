using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.CodeSy.Queries
{
    public class GetKanriKbnQuery : IRequest<TPM_CodeSyData>
    {
        private string _codeSyu { get; set; }

        public GetKanriKbnQuery(string codeSyu)
        {
            _codeSyu = codeSyu;
        }

        public class Handler : IRequestHandler<GetKanriKbnQuery, TPM_CodeSyData>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetKanriKbnQuery> _logger;
            public Handler(KobodbContext context,
                ILogger<GetKanriKbnQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<TPM_CodeSyData> Handle(GetKanriKbnQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await 
                            (from VPM_CodeSy in _context.VpmCodeSy
                            where VPM_CodeSy.CodeSyu == request._codeSyu
                            select new TPM_CodeSyData()
                            {
                                CodeSyu = VPM_CodeSy.CodeSyu,
                                KanriKbn = VPM_CodeSy.KanriKbn,
                                CodeSyuNm = VPM_CodeSy.CodeSyuNm
                            }).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return null;
                }


            }
        }
    }
}

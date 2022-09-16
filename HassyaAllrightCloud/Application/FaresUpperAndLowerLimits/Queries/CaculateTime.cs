using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using StoredProcedureEFCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class CaculateTime : IRequest<string>
    {
        public string Time1;
        public string Time2;

        public class Handler : IRequestHandler<CaculateTime, string>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(CaculateTime request, CancellationToken cancellationToken)
            {
                try
                {
                    _context.LoadStoredProc("dbo.PK_CaculateTime_R")
                      .AddParam("@Time1", request.Time1)
                      .AddParam("@Time2", request.Time2)
                      .AddParam("@ROWCOUNT", out IOutParam<int> retParam)
                  .ExecScalar(out string sumTime);
                  return sumTime;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

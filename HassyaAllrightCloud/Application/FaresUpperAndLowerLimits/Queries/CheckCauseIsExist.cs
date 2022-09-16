using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class CheckCauseIsExist : IRequest<bool>
    {
        public TkdMaxMinFareFeeCauseParam Param;
        public class Handler : IRequestHandler<CheckCauseIsExist, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckCauseIsExist request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request?.Param != null)
                    {

                        var causeIsExist = _context.TkdMaxMinFareFeeCause.Where(x => x.UkeNo.Trim() == request.Param.UkeNo.Trim() && x.UnkRen == request.Param.UnkRen
                                                                                      && x.TeiDanNo == request.Param.TeiDanNo && x.BunkRen == request.Param.BunkRen).Any();
                        if (!causeIsExist)
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

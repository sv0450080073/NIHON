using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class GetFirstOrDefaultTKDMaxMinFareFeeCause : IRequest<TkdMaxMinFareFeeCause>
    {
        public TkdMaxMinFareFeeCauseParam Param;
        public class Handler : IRequestHandler<GetFirstOrDefaultTKDMaxMinFareFeeCause, TkdMaxMinFareFeeCause>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<TkdMaxMinFareFeeCause> Handle(GetFirstOrDefaultTKDMaxMinFareFeeCause request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request?.Param != null)
                    {

                        return _context.TkdMaxMinFareFeeCause.Where(x => x.UkeNo.Trim() == request.Param.UkeNo.Trim() && x.UnkRen == request.Param.UnkRen
                                                                                      && x.TeiDanNo == request.Param.TeiDanNo && x.BunkRen == request.Param.BunkRen).FirstOrDefault();

                    }
                    return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

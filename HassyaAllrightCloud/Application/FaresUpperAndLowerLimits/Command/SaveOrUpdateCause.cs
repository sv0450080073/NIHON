using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FaresUpperAndLowerLimits.Queries
{
    public class SaveOrUpdateCause : IRequest<bool>
    {
        public TkdMaxMinFareFeeCause CauseItem;
        public bool IsSaveCause;
        public class Handler : IRequestHandler<SaveOrUpdateCause, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(SaveOrUpdateCause request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request?.CauseItem != null)
                    {
                        var causeIsExist = _context.TkdMaxMinFareFeeCause.Where(x => x.UkeNo.Trim() == request.CauseItem.UkeNo.Trim() && x.UnkRen == request.CauseItem.UnkRen
                                                                                      && x.TeiDanNo == request.CauseItem.TeiDanNo && x.BunkRen == request.CauseItem.BunkRen).Any();
                        if (request.IsSaveCause)
                        {
                            if (!causeIsExist)
                                _context.Add(request.CauseItem);
                            else
                                _context.Update(request.CauseItem);
                        }
                        else
                        {
                            if (causeIsExist)
                            {
                                request.CauseItem.SiyoKbn = 2;
                                _context.Update(request.CauseItem);
                            }
                        }
                        await _context.SaveChangesAsync();
                        return true;
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

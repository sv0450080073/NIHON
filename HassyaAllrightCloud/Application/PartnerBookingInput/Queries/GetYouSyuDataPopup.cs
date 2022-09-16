using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetYouSyuDataPopup : IRequest<List<TkdYouSyu>>
    {
        public string Ukeno { get; set; } = "";
        public string Unkren { get; set; } = "";
        public int YouTblSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetYouSyuDataPopup, List<TkdYouSyu>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TkdYouSyu>> Handle(GetYouSyuDataPopup request, CancellationToken cancellationToken)
            {
                var result = new List<TkdYouSyu>();
                try
                {
                    result = _context.TkdYouSyu.Where(x => x.UkeNo == request.Ukeno
                              && x.YouTblSeq == request.YouTblSeq && x.UnkRen.ToString() == request.Unkren).ToList();
                    return result;
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.PartnerBookingInput.Queries
{
    public class GetTKM_KasSetData : IRequest<List<TKM_KasSetData>>
    {
        public int CompanyCdSeq { get; set; } = 0;
        public class Handler : IRequestHandler<GetTKM_KasSetData, List<TKM_KasSetData>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<TKM_KasSetData>> Handle(GetTKM_KasSetData request, CancellationToken cancellationToken)
            {
                var result = new List<TKM_KasSetData>();
                try
                {
                    result = (from TKM_KasSet in _context.TkmKasSet
                              where TKM_KasSet.CompanyCdSeq == request.CompanyCdSeq
                              select new TKM_KasSetData()
                              {
                                  CompanyCdSeq = TKM_KasSet.CompanyCdSeq ,
                                  UriKbn = TKM_KasSet.UriKbn,
                                  SyohiHasu = TKM_KasSet.SyohiHasu,
                                  TesuHasu = TKM_KasSet.TesuHasu
                              }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    return result;
                }
            }
        }

    }
}

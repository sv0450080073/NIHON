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
    public class GetVATCustomer : IRequest<VATDataPopup>
    {
        public TokistData TokistData { get; set; }
        public class Handler : IRequestHandler<GetVATCustomer, VATDataPopup>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<VATDataPopup> Handle(GetVATCustomer request, CancellationToken cancellationToken)
            {
                var result = new VATDataPopup();
                try
                {
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

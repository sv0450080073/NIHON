using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetDisabledBookingStateList : IRequest<List<BookingDisableEditState>>
    {
        private readonly string _ukeNo;

        public GetDisabledBookingStateList(string ukeNo)
        {
            _ukeNo = ukeNo;
        }

        public class Handler : IRequestHandler<GetDisabledBookingStateList, List<BookingDisableEditState>>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<BookingDisableEditState>> Handle(GetDisabledBookingStateList request, CancellationToken cancellationToken)
            {
                try
                {
                    // check paid or coupon
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request._ukeNo).FirstOrDefaultAsync();
                    // check lock table
                    var lockTable = _context.TkdLockTable.SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
                                                                                && l.EigyoCdSeq == yyksho.SeiEigCdSeq);
                    var disableStateList = BookingInputHelper.CheckEditable(yyksho, lockTable);

                    return disableStateList;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

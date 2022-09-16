using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class CheckOpenCouponPopUpAsyncQuery : IRequest<bool>
    {
        public OutDataTable OutDataTable { get; set; }
        public class Handler : IRequestHandler<CheckOpenCouponPopUpAsyncQuery, bool>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckOpenCouponPopUpAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.OutDataTable == null)
                    return false;
                return (from tkdNyShCu in _context.TkdNyShCu
                        where tkdNyShCu.NyuSihKbn == (byte)1 
                        && tkdNyShCu.UkeNo == request.OutDataTable.UkeNo
                        && tkdNyShCu.SeiFutSyu == request.OutDataTable.SeiFutSyu
                        && tkdNyShCu.UnkRen == request.OutDataTable.FutuUnkRen
                        && tkdNyShCu.FutTumRen == request.OutDataTable.FutTumRen
                        select tkdNyShCu).Count() != 0;
            }
        }
    }
}

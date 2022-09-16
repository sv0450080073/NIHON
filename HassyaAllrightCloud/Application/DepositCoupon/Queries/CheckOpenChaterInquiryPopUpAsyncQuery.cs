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
    public class CheckOpenChaterInquiryPopUpAsyncQuery : IRequest<bool>
    {
        public OutDataTable outDataTable { get; set; }
        public class Handler : IRequestHandler<CheckOpenChaterInquiryPopUpAsyncQuery, bool>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckOpenChaterInquiryPopUpAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.outDataTable == null)
                    return false;
                if(request.outDataTable.SeiFutSyu == (byte)1)
                {
                    return (from tkdMihrim in _context.TkdMihrim
                            join tkdYysho in _context.TkdYyksho
                            on new { ukeNo = tkdMihrim.UkeNo, yoyaSyu = (byte)1 } equals new { ukeNo = tkdYysho.UkeNo, yoyaSyu = tkdYysho.YoyaSyu }
                            where tkdMihrim.UkeNo == request.outDataTable.UkeNo &&
                            tkdMihrim.SihFutSyu == (byte)1 &&
                            tkdMihrim.SiyoKbn == (byte)1
                            select tkdMihrim).Count() != 0;
                }
                return (from tkdMihrim in _context.TkdMihrim
                        join tkdYysho in _context.TkdYyksho
                        on new { ukeNo = tkdMihrim.UkeNo, yoyaSyu = (byte)1 } equals new { ukeNo = tkdYysho.UkeNo, yoyaSyu = tkdYysho.YoyaSyu }
                        join tkdYfutTu in _context.TkdYfutTu
                        on new { ukeNo = tkdMihrim.UkeNo, unkRen = tkdMihrim.UnkRen, youTblSeq = tkdMihrim.YouTblSeq, 
                            youFutTumRen = tkdMihrim.YouFutTumRen, futTumKbn = request.outDataTable.SeiFutSyu == (byte)6 ? (byte)2 : (byte)1, siyoKbn = (byte)1 }
                        equals new { ukeNo = tkdYfutTu.UkeNo, unkRen = tkdYfutTu.UnkRen, youTblSeq = tkdYfutTu.YouTblSeq, 
                            youFutTumRen = tkdYfutTu.YouFutTumRen, futTumKbn = tkdYfutTu.FutTumKbn, siyoKbn = tkdYfutTu.SiyoKbn }
                        where tkdMihrim.UkeNo == request.outDataTable.UkeNo && tkdMihrim.SihFutSyu == request.outDataTable.SeiFutSyu
                        && tkdMihrim.UnkRen == request.outDataTable.FutuUnkRen && tkdMihrim.SiyoKbn == (byte)1
                        select tkdMihrim).Count() != 0;
            }
        }
    }
}

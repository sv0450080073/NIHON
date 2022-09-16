using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.UpdateBookingInput.Commands
{
    public class InsertConfirmationTabCommand : IRequest<Unit>
    {
        public string UkeNo { get; set; }
        public IEnumerable<ConfirmationTabData> ConfirmationTabDataList { get; set; }

        public class Handler : IRequestHandler<InsertConfirmationTabCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<InsertConfirmationTabCommand> _logger;

            public Handler(KobodbContext context, ILogger<InsertConfirmationTabCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            /// <summary>
            /// This method use for both insert and update action, if TkdKaknin have data of ukeNo key then Update, otherwise Insert
            /// </summary>
            /// <param name="confirmationTabDatas"></param>
            /// <param name="ukeNo"></param>
            /// <returns></returns>
            public async Task<Unit> Handle(InsertConfirmationTabCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    // check paid or coupon
                    var yyksho = await _context.TkdYyksho.Where(x => x.UkeNo == request.UkeNo).FirstOrDefaultAsync();
                    // check lock table
                    var lockTable = _context.TkdLockTable.SingleOrDefault(l => l.TenantCdSeq == new ClaimModel().TenantID
                                                                                && l.EigyoCdSeq == yyksho.SeiEigCdSeq);
                    var checkEdit = BookingInputHelper.CheckEditable(yyksho, lockTable);
                    if (checkEdit.Contains(BookingDisableEditState.PaidOrCoupon))
                    {
                        throw new Exception("Booking has been paid or coupon");
                    }
                    if (checkEdit.Contains(BookingDisableEditState.Locked))
                    {
                        throw new Exception("Booking has been lock");
                    }

                    if (request.ConfirmationTabDataList is null) request.ConfirmationTabDataList = new List<ConfirmationTabData>();

                    short index = 0;
                    bool isInsert = false;
                    if (_context.TkdKaknin.Where(t => t.UkeNo == request.UkeNo).Count() == 0) isInsert = true;

                    if (!isInsert) _context.TkdKaknin.RemoveRange(_context.TkdKaknin.Where(t => t.UkeNo == request.UkeNo));
                    foreach (var confirmation in request.ConfirmationTabDataList)
                    {
                        var tkdKaknin = new TkdKaknin()
                        {
                            UkeNo = request.UkeNo,
                            KaknYmd = confirmation.KaknYmd.ToString("yyyyMMdd"),
                            KaknAit = confirmation.KaknAit,
                            KaknNin = short.Parse(confirmation.KaknNin),
                            SaikFlg = Convert.ToByte(confirmation.SaikFlg),
                            DaiSuFlg = Convert.ToByte(confirmation.DaiSuFlg),
                            KingFlg = Convert.ToByte(confirmation.KingFlg),
                            NitteiFlg = Convert.ToByte(confirmation.NitteFlag),
                            BikoTblSeq = 0,
                            HenKai = 0,
                            SiyoKbn = 1,
                            UpdPrgId = "KJ1000",
                            UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                            UpdTime = DateTime.Now.ToString("HHmmss"),
                            UpdYmd = DateTime.Now.ToString("yyyyMMdd")
                        };
                        if (isInsert)
                        {
                            tkdKaknin.KaknRen = ++index;
                        }
                        else
                        {
                            tkdKaknin.KaknRen = (short)confirmation.FixDataNo;
                        }
                        await _context.TkdKaknin.AddAsync(tkdKaknin);
                    }
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await trans.RollbackAsync();
                    throw;
                }
                return await Task.FromResult(Unit.Value);
            }
        }
    }
}

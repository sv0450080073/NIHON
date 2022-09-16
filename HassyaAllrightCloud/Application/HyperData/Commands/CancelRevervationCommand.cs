using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Commands
{
    public class CancelRevervationCommand : IRequest<bool>
    {
        public List<string> receiptNumber { get; set; }
        public List<ReservationDataToCheck> lstCheck { get; set; }
        public class Handler : IRequestHandler<CancelRevervationCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(CancelRevervationCommand request, CancellationToken cancellationToken)
            {
                List<TkdYyksho> RevervationList = await (from yksho in _context.TkdYyksho
                                                         where request.receiptNumber.Contains(yksho.UkeNo)
                                                         select yksho ).ToListAsync();
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (TkdYyksho Reservation in RevervationList)
                        {
                            string ymd = request.lstCheck
                                                .Where(x => x.ReceiptNumber == Reservation.UkeNo)
                                                .Select(y => y.HaiSymd).SingleOrDefault();

                            string temp = (string.IsNullOrEmpty(ymd.Trim()) ? DateTime.MinValue : DateTime.ParseExact(ymd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture)).AddDays(-1).ToString("yyyyMMdd");

                            Reservation.HenKai = (short)(Reservation.HenKai + 1);
                            Reservation.CanYmd = Math.Min(int.Parse(DateTime.Now.ToString("yyyyMMdd")), int.Parse(temp)).ToString();
                            // 1:　予約, 2:　予約キャンセル, 3:　見積, 4:　見積キャンセル
                            Reservation.YoyaSyu = (byte)(Reservation.YoyaSyu == 1 ? 2 : Reservation.YoyaSyu == 3 ? 4 : Reservation.YoyaSyu);
                            Reservation.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            Reservation.UpdTime = DateTime.Now.ToString("HHmmss");
                            Reservation.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                            Reservation.UpdPrgId = CommonProgramId.BookingUpdPrgId;
                            _context.TkdYyksho.Update(Reservation);
                        }
                        await _context.SaveChangesAsync();
                        dbTran.Commit();
                        return true;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                }
            }

        }
    }
}

using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Commands
{
    public class ConfirmRevervationCommand : IRequest<bool>
    {
        public List<string> receiptNumber { get; set; }
        public class Handler : IRequestHandler<ConfirmRevervationCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(ConfirmRevervationCommand request, CancellationToken cancellationToken)
            {
                List<TkdYyksho> RevervationList = await (from yksho in _context.TkdYyksho
                                                         where request.receiptNumber.Contains(yksho.UkeNo)
                                                         select yksho).ToListAsync();
                using (IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (TkdYyksho Reservation in RevervationList)
                        {
                            Reservation.HenKai = (short)(Reservation.HenKai + 1);
                            Reservation.KaktYmd = DateTime.Now.ToString("yyyyMMdd");
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

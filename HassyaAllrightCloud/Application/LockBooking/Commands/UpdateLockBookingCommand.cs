using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LockBooking.Commands
{
    public class UpdateLockBookingCommand : IRequest<Unit>
    {
        private readonly LockBookingData _lockBooking;

        public UpdateLockBookingCommand(LockBookingData lockBooking)
        {
            _lockBooking = lockBooking;
        }

        public class Handler : IRequestHandler<UpdateLockBookingCommand, Unit>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateLockBookingCommand request, CancellationToken cancellationToken)
            {
                var trans = _context.Database.BeginTransaction();
                try
                {
                    var data = CollectDataLockTable(request._lockBooking);
                    var dataDb = await _context.TkdLockTable.FirstOrDefaultAsync(l => l.TenantCdSeq == new ClaimModel().TenantID && l.EigyoCdSeq == data.EigyoCdSeq);

                    if(dataDb != null)
                    {
                        dataDb.SimpleCloneProperties(data);
                    }
                    else
                    {
                        await _context.TkdLockTable.AddAsync(data);
                    }

                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();

                    return Unit.Value;
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }

            private TkdLockTable CollectDataLockTable(LockBookingData lockBooking)
            {
                return new TkdLockTable()
                {
                    EigyoCdSeq = lockBooking.SalesOffice.EigyoCdSeq,
                    LockYmd = lockBooking.ProcessingDate.ToString("yyyyMMdd"),
                    TenantCdSeq = new ClaimModel().TenantID,
                    UpdYmd = DateTime.Now.ToString("yyyyMMdd"),
                    UpdTime = DateTime.Now.ToString("HHmmss"),
                    UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                    UpdPrgId = "CS3000P"
                };
            }
        }
    }
}

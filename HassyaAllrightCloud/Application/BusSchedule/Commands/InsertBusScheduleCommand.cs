using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusSchedule.Commands
{
    public class InsertBusScheduleCommand : IRequest<Unit>
    {
        public TkdYyksho Yyksho { get; set; }
        public TkdUnkobi Tkdunkobi { get; set; }
        public List<TkdYykSyu> ListYykSyu { get; set; }
        public List<TkdHaisha> ListHaisha { get; set; }

        public class Handler : IRequestHandler<InsertBusScheduleCommand, Unit>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(InsertBusScheduleCommand command, CancellationToken cancellationToken)
            {
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        string ukeNo = "-1";
                        await _context.TkdYyksho.AddAsync(command.Yyksho);
                        await _context.SaveChangesAsync();
                        ukeNo = command.Yyksho.UkeNo;

                        command.Tkdunkobi.UkeNo = ukeNo;
                        command.Tkdunkobi.Kskbn = command.Yyksho.Kskbn;
                        await _context.TkdUnkobi.AddAsync(command.Tkdunkobi);
                        foreach (TkdYykSyu item in command.ListYykSyu)
                        {
                            item.UkeNo = ukeNo;
                            await _context.TkdYykSyu.AddAsync(item);
                        }

                        foreach (TkdHaisha item in command.ListHaisha)
                        {
                            item.UkeNo = ukeNo;
                            await _context.TkdHaisha.AddAsync(item);
                        }

                        await _context.SaveChangesAsync();
                        dbTran.Commit();

                    }
                    catch (Exception ex)
                    {
                        //Rollback transaction if exception occurs  
                        dbTran.Rollback();
                    }
                }
                return Unit.Value;
            }
        }
    }
}
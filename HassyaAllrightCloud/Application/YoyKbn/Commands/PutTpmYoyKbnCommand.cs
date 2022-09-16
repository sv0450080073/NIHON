using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HassyaAllrightCloud.Application.YoyKbn.Commands
{
    public class PutVpmYoyKbnCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public VpmYoyKbn VpmYoyKbn { get; set; }

        public class Handler : IRequestHandler<PutVpmYoyKbnCommand, bool>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(PutVpmYoyKbnCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    _context.Entry(request.VpmYoyKbn).State = EntityState.Modified;

                    await _context.SaveChangesAsync(); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VpmYoyKbnExists(request.Id))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }

                return true;
            }

            private bool VpmYoyKbnExists(int id)
            {
                return _context.VpmYoyKbn.Any(e => e.YoyaKbnSeq == id);
            }
        }
    }
}

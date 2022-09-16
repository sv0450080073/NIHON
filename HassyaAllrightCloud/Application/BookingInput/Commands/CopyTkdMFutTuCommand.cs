using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Commands
{
    public class CopyTkdMFutTuCommand : IRequest<Unit>
    {
        private readonly string _ukeNoCopy;
        private readonly string _ukeNoNew;

        public CopyTkdMFutTuCommand(string ukeNoCopy, string ukeNoNew)
        {
            _ukeNoCopy = ukeNoCopy ?? throw new ArgumentNullException(nameof(ukeNoCopy));
            _ukeNoNew = ukeNoNew ?? throw new ArgumentNullException(nameof(ukeNoNew));
        }

        public class Handler : IRequestHandler<CopyTkdMFutTuCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<CopyTkdMFutTuCommand> _logger;

            public Handler(KobodbContext context, ILogger<CopyTkdMFutTuCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(CopyTkdMFutTuCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    var tkdMFutTu = await _context.TkdMfutTu
                        .Where(e => e.UkeNo == request._ukeNoCopy)
                        .AsNoTracking()
                        .ToListAsync();
                    foreach (var item in tkdMFutTu)
                    {
                        item.UkeNo = request._ukeNoNew;
                        item.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        item.UpdTime = DateTime.Now.ToString("HHmmss");
                    }
                    await _context.TkdMfutTu.AddRangeAsync(tkdMFutTu);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    throw;
                }
                return await Task.FromResult(Unit.Value);
            }
        }
    }
}

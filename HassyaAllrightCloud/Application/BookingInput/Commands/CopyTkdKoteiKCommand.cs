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
    public class CopyTkdKoteiKCommand : IRequest<Unit>
    {
        private readonly string _ukeNoCopy;
        private readonly string _ukeNoNew;

        public CopyTkdKoteiKCommand(string ukeNoCopy, string ukeNoNew)
        {
            _ukeNoCopy = ukeNoCopy ?? throw new ArgumentNullException(nameof(ukeNoCopy));
            _ukeNoNew = ukeNoNew ?? throw new ArgumentNullException(nameof(ukeNoNew));
        }

        public class Handler : IRequestHandler<CopyTkdKoteiKCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<CopyTkdKoteiKCommand> _logger;

            public Handler(KobodbContext context, ILogger<CopyTkdKoteiKCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(CopyTkdKoteiKCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    var tkdKoteiK = await _context.TkdKoteik
                        .Where(k => k.UkeNo == request._ukeNoCopy)
                        .AsNoTracking()
                        .ToListAsync();
                    foreach (var item in tkdKoteiK)
                    {
                        item.UkeNo = request._ukeNoNew;
                        item.UpdPrgId = Commons.Constants.Common.UpdPrgId;
                        item.UpdTime = DateTime.Now.ToString("HHmmss");
                        item.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    }
                    await _context.TkdKoteik.AddRangeAsync(tkdKoteiK);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return await Task.FromResult(Unit.Value);
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }
    }
}

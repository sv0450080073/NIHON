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
    public class CopyTkdKoteiCommand : IRequest<Unit>
    {
        private readonly string _ukeNoCopy;
        private readonly string _ukeNoNew;

        public CopyTkdKoteiCommand(string ukeNoCopy, string ukeNoNew)
        {
            _ukeNoCopy = ukeNoCopy ?? throw new ArgumentNullException(nameof(ukeNoCopy));
            _ukeNoNew = ukeNoNew ?? throw new ArgumentNullException(nameof(ukeNoNew));
        }

        public class Handler : IRequestHandler<CopyTkdKoteiCommand, Unit>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<CopyTkdKoteiCommand> _logger;

            public Handler(KobodbContext context, ILogger<CopyTkdKoteiCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(CopyTkdKoteiCommand request, CancellationToken cancellationToken)
            {
                using var trans = _context.Database.BeginTransaction();
                try
                {
                    var tkdKotei = await _context.TkdKotei
                        .Where(k => k.UkeNo == request._ukeNoCopy)
                        .AsNoTracking()
                        .OrderBy(e => e.TeiDanNo).ThenBy(e => e.KouRen)
                        .ToListAsync();
                    foreach (var item in tkdKotei)
                    {
                        item.UkeNo = request._ukeNoNew;
                        item.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                        item.UpdTime = DateTime.Now.ToString("HHmmss");
                    }
                    await _context.TkdKotei.AddRangeAsync(tkdKotei);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    throw ex;
                }
                return await Task.FromResult(Unit.Value);
            }
        }
    }
}

using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Commands
{
    public class SaveBikoNmCommand : IRequest<Unit>
    {
        private readonly string _ukeNo;
        private readonly string _bikoNm;
        private readonly short _unkRen;
        private readonly bool _isUnkobi;

        public SaveBikoNmCommand(string ukeNo, string bikoNm, short unkRen, bool isUnkobi)
        {
            _ukeNo = ukeNo;
            _bikoNm = bikoNm ?? throw new ArgumentNullException(nameof(bikoNm));
            _unkRen = unkRen;
            _isUnkobi = isUnkobi;
        }

        public class Handler : IRequestHandler<SaveBikoNmCommand, Unit>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(SaveBikoNmCommand request, CancellationToken cancellationToken)
            {
                using (var dbTran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if(request._isUnkobi)
                        {
                            var unkobi = await _context.TkdUnkobi
                            .FirstOrDefaultAsync(y => y.UkeNo == request._ukeNo && y.UnkRen == request._unkRen);

                            if (unkobi is null) throw new ArgumentNullException(nameof(request._ukeNo), "UkeNo does not exist");

                            unkobi.BikoNm = request._bikoNm;
                            unkobi.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            unkobi.UpdTime = DateTime.Now.ToString("HHmmss");
                        }
                        else
                        {
                            var yyksho = await _context.TkdYyksho
                            .FirstOrDefaultAsync(y => y.UkeNo == request._ukeNo);

                            if (yyksho is null) throw new ArgumentNullException(nameof(request._ukeNo), "UkeNo does not exist");

                            yyksho.BikoNm = request._bikoNm;
                            yyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                            yyksho.UpdTime = DateTime.Now.ToString("HHmmss");
                        }

                        await _context.SaveChangesAsync();
                        await dbTran.CommitAsync();
                        return Unit.Value;
                    }
                    catch (Exception)
                    {
                        await dbTran.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}

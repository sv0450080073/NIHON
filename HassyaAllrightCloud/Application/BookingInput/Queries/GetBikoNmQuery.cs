using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetBikoNmQuery : IRequest<string>
    {
        private readonly string _ukeNo;
        private readonly bool _isUnkobi;
        private readonly short _unkRen;

        public GetBikoNmQuery(string ukeNo, bool isUnkobi, short unkRen)
        {
            _ukeNo = ukeNo;
            _isUnkobi = isUnkobi;
            _unkRen = unkRen;
        }

        public class Handler : IRequestHandler<GetBikoNmQuery, string>
        {
            private readonly KobodbContext _context;

            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetBikoNmQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    string result = string.Empty;
                    if(request._isUnkobi)
                    {
                        result = _context.TkdUnkobi
                            .Where(y => y.UkeNo == request._ukeNo && y.UnkRen == request._unkRen).FirstOrDefault()?
                            .BikoNm;
                    }
                    else
                    {
                        result = _context.TkdYyksho
                            .Where(y => y.UkeNo == request._ukeNo).FirstOrDefault()?
                            .BikoNm;
                    }
                    if(string.IsNullOrEmpty(result))
                    {
                        result = string.Empty;
                    }
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VenderRequestReport.Queries
{
    public class GetYMFuttuByBookingKeyAndRequestKey : IRequest<List<YFuttuVenderRequestReportData>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _requestId;
        private readonly int _bunkRen;

        public GetYMFuttuByBookingKeyAndRequestKey(string ukeNo, int unkRen, int requestId, int bunkRen)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _requestId = requestId;
            _bunkRen = bunkRen;
        }

        public class Handler : IRequestHandler<GetYMFuttuByBookingKeyAndRequestKey, List<YFuttuVenderRequestReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYMFuttuByBookingKeyAndRequestKey> _logger;

            public Handler(KobodbContext context, ILogger<GetYMFuttuByBookingKeyAndRequestKey> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<YFuttuVenderRequestReportData>> Handle(GetYMFuttuByBookingKeyAndRequestKey request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        (from ymFuttu in _context.TkdYmfuTu
                         join yFuttu in _context.TkdYfutTu
                         on new { ymFuttu.UkeNo, ymFuttu.UnkRen, ymFuttu.YouTblSeq, ymFuttu.FutTumKbn, ymFuttu.YouFutTumRen } equals
                             new { yFuttu.UkeNo, yFuttu.UnkRen, yFuttu.YouTblSeq, yFuttu.FutTumKbn, yFuttu.YouFutTumRen } into grym
                         from subym in grym.DefaultIfEmpty()
                         where
                              ymFuttu.UkeNo == request._ukeNo &&
                              ymFuttu.UnkRen == request._unkRen &&
                              ymFuttu.YouTblSeq == request._requestId &&
                              ymFuttu.BunkRen == request._bunkRen
                         select new YFuttuVenderRequestReportData
                         {
                             Date = DateTime.ParseExact(subym.HasYmd, "yyyyMMdd", CultureInfo.CurrentCulture).ToString("MM/dd"),
                             FutTumNm = subym.FutTumNm,
                             SeisanNm = subym.SeisanNm,
                             Tanka = subym.TanKa.ToString("N0"),
                             Suryo = ymFuttu.Suryo.ToString("N0"),
                             Kingaku = (subym.TanKa * ymFuttu.Suryo).ToString("N"),
                             FutTumKbn = subym.FutTumKbn
                         }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<YFuttuVenderRequestReportData>();
                }
            }
        }
    }
}

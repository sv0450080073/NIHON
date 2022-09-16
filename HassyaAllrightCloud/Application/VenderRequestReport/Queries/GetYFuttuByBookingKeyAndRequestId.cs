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
    public class GetYFuttuByBookingKeyAndRequestId : IRequest<List<YFuttuVenderRequestReportData>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _requestId;

        public GetYFuttuByBookingKeyAndRequestId(string ukeNo, int unkRen, int requestId)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _requestId = requestId;
        }

        public class Handler : IRequestHandler<GetYFuttuByBookingKeyAndRequestId, List<YFuttuVenderRequestReportData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYFuttuByBookingKeyAndRequestId> _logger;

            public Handler(KobodbContext context, ILogger<GetYFuttuByBookingKeyAndRequestId> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<YFuttuVenderRequestReportData>> Handle(GetYFuttuByBookingKeyAndRequestId request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _context.TkdYfutTu.Where(_ => _.UkeNo == request._ukeNo &&
                                                              _.UnkRen == request._unkRen &&
                                                              _.YouTblSeq == request._requestId &&
                                                              _.SiyoKbn == 1)
                                                   .OrderBy(_ => _.UkeNo)
                                                   .ThenBy(_ => _.UnkRen)
                                                   .ThenBy(_ => _.FutTumKbn)
                                                   .ThenBy(_ => _.YouFutTumRen)
                                                   .ThenBy(_ => _.Nittei)
                                                   .ThenBy(_ => _.HasYmd)
                                                   .Select(_ => new YFuttuVenderRequestReportData
                                                   {
                                                       Date = DateTime.ParseExact(_.HasYmd, "yyyyMMdd", CultureInfo.CurrentCulture).ToString("MM/dd"),
                                                       FutTumNm = _.FutTumNm,
                                                       SeisanNm = _.SeisanNm,
                                                       Tanka = _.TanKa.ToString("N0"),
                                                       Suryo = _.Suryo.ToString("N0"),
                                                       Kingaku = (_.TanKa * _.Suryo).ToString("N"),
                                                       FutTumKbn = _.FutTumKbn
                                                   })
                                                   .ToListAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<YFuttuVenderRequestReportData>();
                }
            }
        }
    }
}

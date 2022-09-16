using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.VenderRequestReport.Queries
{
    public class GetKoteiForMainRpByBookingKeyQuery : IRequest<List<KoteiTehaiVenderRequestReport>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _teiDanNo;

        public GetKoteiForMainRpByBookingKeyQuery(string ukeNo, int unkRen, int teiDanNo)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
        }

        public class Handler : IRequestHandler<GetKoteiForMainRpByBookingKeyQuery, List<KoteiTehaiVenderRequestReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetKoteiForMainRpByBookingKeyQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetKoteiForMainRpByBookingKeyQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<KoteiTehaiVenderRequestReport>> Handle(GetKoteiForMainRpByBookingKeyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        (from kotei in _context.TkdKotei
                         join unkobi in _context.TkdUnkobi
                         on new { kotei.UkeNo, kotei.UnkRen } equals
                            new { unkobi.UkeNo, unkobi.UnkRen } into tu
                         from subKu in tu.DefaultIfEmpty()
                         where kotei.UkeNo == request._ukeNo &&
                               kotei.UnkRen == request._unkRen &&
                               kotei.TeiDanNo == request._teiDanNo &&
                               kotei.SiyoKbn == 1
                         select new KoteiTehaiVenderRequestReport
                         {
                             UkeNo = kotei.UkeNo,
                             UnkRen = kotei.UnkRen,
                             TeiDanNo = kotei.TeiDanNo,
                             Date = CommonHelper.GetKoteiTehaiDate(subKu.HaiSymd, subKu.TouYmd, kotei.Nittei, kotei.TomKbn).ToString("yyyyMMdd"),
                             Koutei = kotei.Koutei,
                         }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return new List<KoteiTehaiVenderRequestReport>();
                }
            }
        }
    }
}

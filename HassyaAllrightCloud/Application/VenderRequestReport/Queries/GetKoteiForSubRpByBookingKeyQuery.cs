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
    public class GetKoteiForSubRpByBookingKeyQuery : IRequest<List<KoteiTehaiVenderRequestReport>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _teiDanNo;
        private readonly int _bunkRen;

        public GetKoteiForSubRpByBookingKeyQuery(string ukeNo, int unkRen, int teiDanNo, int bunkRen)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
            _bunkRen = bunkRen;
        }

        public class Handler : IRequestHandler<GetKoteiForSubRpByBookingKeyQuery, List<KoteiTehaiVenderRequestReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetKoteiForSubRpByBookingKeyQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetKoteiForSubRpByBookingKeyQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<KoteiTehaiVenderRequestReport>> Handle(GetKoteiForSubRpByBookingKeyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        (from kotei in _context.TkdKotei
                         join haisha in _context.TkdHaisha
                         on new { kotei.UkeNo, kotei.UnkRen, kotei.TeiDanNo, kotei.BunkRen } equals
                            new { haisha.UkeNo, haisha.UnkRen, haisha.TeiDanNo, haisha.BunkRen } into tu
                         from subKu in tu.DefaultIfEmpty()
                         where kotei.UkeNo == request._ukeNo &&
                               kotei.UnkRen == request._unkRen &&
                               kotei.TeiDanNo == request._teiDanNo &&
                               kotei.BunkRen == request._bunkRen &&
                               kotei.SiyoKbn == 1
                         orderby kotei.UkeNo, kotei.UnkRen, kotei.TeiDanNo, kotei.BunkRen, kotei.TomKbn, kotei.Nittei, kotei.KouRen
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

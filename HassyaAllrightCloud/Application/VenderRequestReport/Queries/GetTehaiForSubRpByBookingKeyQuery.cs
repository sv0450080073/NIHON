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
    public class GetTehaiForSubRpByBookingKeyQuery : IRequest<List<KoteiTehaiVenderRequestReport>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _teiDanNo;
        private readonly int _bunkRen;

        public GetTehaiForSubRpByBookingKeyQuery(string ukeNo, int unkRen, int teiDanNo, int bunkRen)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
            _bunkRen = bunkRen;
        }

        public class Handler : IRequestHandler<GetTehaiForSubRpByBookingKeyQuery, List<KoteiTehaiVenderRequestReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetTehaiForSubRpByBookingKeyQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetTehaiForSubRpByBookingKeyQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<KoteiTehaiVenderRequestReport>> Handle(GetTehaiForSubRpByBookingKeyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        (from tehai in _context.TkdTehai
                         join haisha in _context.TkdHaisha
                         on new { tehai.UkeNo, tehai.UnkRen, tehai.TeiDanNo, tehai.BunkRen } equals
                            new { haisha.UkeNo, haisha.UnkRen, haisha.TeiDanNo, haisha.BunkRen } into tu
                         from subTu in tu.DefaultIfEmpty()
                         where tehai.UkeNo == request._ukeNo &&
                               tehai.UnkRen == request._unkRen &&
                               tehai.TeiDanNo == request._teiDanNo &&
                               tehai.BunkRen == request._bunkRen &&
                               tehai.SiyoKbn == 1
                         select new KoteiTehaiVenderRequestReport
                         {
                             UkeNo = tehai.UkeNo,
                             UnkRen = tehai.UnkRen,
                             TeiDanNo = tehai.TeiDanNo,
                             Date = CommonHelper.GetKoteiTehaiDate(subTu.HaiSymd, subTu.TouYmd, tehai.Nittei, tehai.TomKbn).ToString("yyyyMMdd"),
                             TehaiNm = tehai.TehNm,
                             TehaiTel = tehai.TehTel
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

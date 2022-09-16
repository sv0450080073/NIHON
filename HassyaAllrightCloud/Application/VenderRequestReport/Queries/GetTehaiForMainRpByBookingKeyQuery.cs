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
    public class GetTehaiForMainRpByBookingKeyQuery : IRequest<List<KoteiTehaiVenderRequestReport>>
    {
        private readonly string _ukeNo;
        private readonly int _unkRen;
        private readonly int _teiDanNo;

        public GetTehaiForMainRpByBookingKeyQuery(string ukeNo, int unkRen, int teiDanNo)
        {
            _ukeNo = ukeNo ?? throw new ArgumentNullException(nameof(ukeNo));
            _unkRen = unkRen;
            _teiDanNo = teiDanNo;
        }

        public class Handler : IRequestHandler<GetTehaiForMainRpByBookingKeyQuery, List<KoteiTehaiVenderRequestReport>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetTehaiForMainRpByBookingKeyQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetTehaiForMainRpByBookingKeyQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<KoteiTehaiVenderRequestReport>> Handle(GetTehaiForMainRpByBookingKeyQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await
                        (from tehai in _context.TkdTehai
                         join unkobi in _context.TkdUnkobi
                         on new { tehai.UkeNo, tehai.UnkRen } equals
                            new { unkobi.UkeNo, unkobi.UnkRen } into tu
                         from subTu in tu.DefaultIfEmpty()
                         where tehai.UkeNo == request._ukeNo &&
                               tehai.UnkRen == request._unkRen &&
                               tehai.TeiDanNo == request._teiDanNo &&
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

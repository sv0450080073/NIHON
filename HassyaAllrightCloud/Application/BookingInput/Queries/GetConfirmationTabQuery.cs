using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetConfirmationTabQuery : IRequest<IEnumerable<ConfirmationTabData>>
    {
        private readonly string _ukeNo;

        public GetConfirmationTabQuery(string ukeNo)
        {
            _ukeNo = ukeNo;
        }


        public class Handler : IRequestHandler<GetConfirmationTabQuery, IEnumerable<ConfirmationTabData>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetConfirmationTabQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetConfirmationTabQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<IEnumerable<ConfirmationTabData>> Handle(GetConfirmationTabQuery request, CancellationToken cancellationToken)
            {
                List<ConfirmationTabData> result = new List<ConfirmationTabData>();
                try
                {
                    result = await (from tkdKaknin in _context.TkdKaknin
                                    join tkdYyksho in _context.TkdYyksho
                                    on tkdKaknin.UkeNo equals tkdYyksho.UkeNo
                                    where tkdKaknin.UkeNo == request._ukeNo
                                    select new ConfirmationTabData()
                                    {
                                        FixDataNo = tkdKaknin.KaknRen,
                                        KaknYmd = DateTime.ParseExact(tkdKaknin.KaknYmd, "yyyyMMdd", null),
                                        KaknAit = tkdKaknin.KaknAit,
                                        KaknNin = tkdKaknin.KaknNin.ToString(),
                                        SaikFlg = Convert.ToBoolean(tkdKaknin.SaikFlg),
                                        DaiSuFlg = Convert.ToBoolean(tkdKaknin.DaiSuFlg),
                                        KingFlg = Convert.ToBoolean(tkdKaknin.KingFlg),
                                        NitteFlag = Convert.ToBoolean(tkdKaknin.NitteiFlg),
                                    }).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }

                return result;
            }
        }
    }
}

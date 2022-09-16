using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SyainCulture.Queries
{
    public class GetSyainCultureQuery : IRequest<string>
    {
        private int _staffId;

        public GetSyainCultureQuery(int staffId)
        {
            _staffId = staffId;
        }

        public class Handler : IRequestHandler<GetSyainCultureQuery, string>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetSyainCultureQuery> _logger;
            public IConfiguration Configuration { get; }

            public Handler(KobodbContext context, ILogger<GetSyainCultureQuery> logger, IConfiguration configuration)
            {
                _context = context;
                _logger = logger;
                Configuration = configuration;
            }

            public async Task<string> Handle(GetSyainCultureQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var supportedCultures = new List<CultureInfo> { new CultureInfo("ja-JP"), new CultureInfo("en-US"), new CultureInfo("vi-VN") };
                    var cultureNativeName = await (from items in _context.VpmUserSetItm
                                         join itemVal in _context.VpmUserSetItmVal on items.ItemSeq equals itemVal.ItemSeq into itemVals
                                         from iv in itemVals.DefaultIfEmpty()
                                         join code in _context.VpmCodeKb on iv.SetVal equals code.CodeKbnSeq.ToString() into codeVals
                                         from cv in codeVals.DefaultIfEmpty()
                                         where items.ItemCd == "LANGUAGE" && iv.SyainCdSeq == request._staffId && cv.CodeSyu == "USERLANGUAGE"
                                            select cv.CodeKbnNm
                                         ).FirstOrDefaultAsync();
                    var culture = supportedCultures.FirstOrDefault(s => s.NativeName.Contains(cultureNativeName ?? ""))?.Name;
                    return culture ?? "ja-JP";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());

                    return "ja-JP";
                }

            }
        }
    }
}

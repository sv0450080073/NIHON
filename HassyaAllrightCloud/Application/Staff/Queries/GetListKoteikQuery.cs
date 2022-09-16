using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Staff.Queries
{
    public class GetListKoteikQuery : IRequest<List<KoteikItem>>
    {
        public string UkeNo { get; set; }
        public short UnkRen { get; set; }
        public short TeiDanNo { get; set; }
        public short BunkRen { get; set; }

        public class Handler : IRequestHandler<GetListKoteikQuery, List<KoteikItem>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<KoteikItem>> Handle(GetListKoteikQuery request, CancellationToken cancellationToken)
            {
                List<KoteikItem> result = new List<KoteikItem>();

                try
                {
                    result = await _context.TkdKoteik.Where(_ => _.UkeNo == request.UkeNo && _.UnkRen == request.UnkRen
                                                              && _.TeiDanNo == request.TeiDanNo && _.BunkRen == request.BunkRen 
                                                              && _.TomKbn == 1 && _.SiyoKbn == CommonConstants.SiyoKbn)
                                               .Select(_ => new KoteikItem()
                                               {
                                                   Nittei = _.Nittei,
                                                   SyukoTime = _.SyukoTime,
                                                   KikTime = _.KikTime
                                               }).ToListAsync();
                    return result;
                }
                catch(Exception ex)
                {
                    // TODO: write log
                    throw ex;
                }
            }
        }
    }
}

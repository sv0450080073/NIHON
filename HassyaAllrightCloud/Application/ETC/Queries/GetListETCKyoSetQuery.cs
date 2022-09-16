using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.ETC.Queries
{
    public class GetListETCKyoSetQuery : IRequest<List<ETCKyoSet>>
    {
        public class Handler : IRequestHandler<GetListETCKyoSetQuery, List<ETCKyoSet>>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<ETCKyoSet>> Handle(GetListETCKyoSetQuery request, CancellationToken cancellationToken)
            {
                var result = await (from k in _context.VpmKyoSet
                                    where k.SetteiCd == "001"
                                    select new ETCKyoSet()
                                    {
                                        SetteiCd = k.SetteiCd,
                                        Zeiritsu1 = k.Zeiritsu1,
                                        Zei1StaYmd = k.Zei1StaYmd,
                                        Zei1EndYmd = k.Zei1EndYmd,
                                        Zeiritsu2 = k.Zeiritsu2,
                                        Zei2StaYmd = k.Zei2StaYmd,
                                        Zei2EndYmd = k.Zei2EndYmd,
                                        Zeiritsu3 = k.Zeiritsu3,
                                        Zei3StaYmd = k.Zei3StaYmd,
                                        Zei3EndYmd = k.Zei3EndYmd,
                                    }).ToListAsync();

                return result;
            }
        }
    }
}

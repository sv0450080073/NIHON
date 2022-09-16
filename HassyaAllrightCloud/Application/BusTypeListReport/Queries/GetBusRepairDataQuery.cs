using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.IService;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusTypeListReport.Queries
{
    public class GetBusRepairDataQuery : IRequest<List<BusRepairDataSource>>
    {
        public BusTypeListData BusTypeListDataParam;

        public class Handler : IRequestHandler<GetBusRepairDataQuery, List<BusRepairDataSource>>
        {

            private readonly KobodbContext _context;
            private readonly ILogger<GetBusRepairDataQuery> _logger;
            private readonly ITPM_CodeSyService _codeSyuService;
            public Handler(KobodbContext context, ILogger<GetBusRepairDataQuery> logger, ITPM_CodeSyService codeSyuService)
            {
                _context = context;
                _logger = logger;
                _codeSyuService = codeSyuService ?? throw new ArgumentNullException(nameof(codeSyuService));
            }

            public async Task<List<BusRepairDataSource>> Handle(GetBusRepairDataQuery request, CancellationToken cancellationToken)
            {
                List<BusRepairDataSource> result = new List<BusRepairDataSource>();
                try
                {
                    var param = request.BusTypeListDataParam;
                    result = (from Shuri in _context.TkdShuri
                              join SyaRyo in _context.VpmSyaRyo
                              on Shuri.SyaRyoCdSeq equals SyaRyo.SyaRyoCdSeq
                              into SyaRyo_join
                              from SyaRyo in SyaRyo_join.DefaultIfEmpty()
                              join SyaSyu in _context.VpmSyaSyu
                              on SyaRyo.SyaSyuCdSeq equals SyaSyu.SyaSyuCdSeq
                              into SyaSyu_join
                              from SyaSyu in SyaSyu_join.DefaultIfEmpty()
                              where Shuri.ShuriSymd.CompareTo(param.StartDate.AddDays(param.numberDay).ToString("yyyyMMdd")) <= 0
                                   && Shuri.ShuriEymd.CompareTo(param.StartDate.ToString("yyyyMMdd")) >= 0
                                   && Shuri.SiyoKbn == 1
                                   && SyaSyu.TenantCdSeq == param.TenantCdSeq
                              select new BusRepairDataSource()
                              {
                                  ShuriCdSeq = Shuri.ShuriCdSeq,
                                  ShuriSYmd = Shuri.ShuriSymd,
                                  ShuriEYmd = Shuri.ShuriEymd,
                                  SyaSyuCdSeq = SyaSyu.SyaSyuCdSeq,
                                  SyaRyoCdSeq = SyaRyo.SyaRyoCdSeq
                              }).ToList();
                    return result;
                }
                catch(Exception ex)
                {
                    return result;
                }
            }
        }
    }
}

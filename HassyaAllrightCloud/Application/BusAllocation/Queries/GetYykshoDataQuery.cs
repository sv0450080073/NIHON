using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusAllocation.Queries
{
    public class GetYykshoDataQuery : IRequest<TkdYyksho>
    {
        public string Ukeno { get; set; }
        public class Handler : IRequestHandler<GetYykshoDataQuery, TkdYyksho>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetYykshoDataQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetYykshoDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public Task<TkdYyksho> Handle(GetYykshoDataQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    TkdYyksho tkdYyksho  = _context.TkdYyksho.FirstOrDefault(e => e.UkeNo == request.Ukeno && e.TenantCdSeq== new ClaimModel().TenantID);
                   /// SetTkdYykshoData(ref tkdYyksho, request.Ukeno);
                    return Task.FromResult(tkdYyksho);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void SetTkdYykshoData(ref TkdYyksho tkdYyksho, string ukeno)
            {
                if (tkdYyksho != null && ukeno != "")
                {
                    tkdYyksho.HenKai++;
                    tkdYyksho.Kskbn = CalculateValueYykSho(ukeno, "HaiSkbn");
                    tkdYyksho.Kskbn = CalculateValueYykSho(ukeno, "Kskbn");
                    tkdYyksho.HaiIkbn = CalculateValueYykSho(ukeno, "HaiIkbn");
                    tkdYyksho.UpdYmd = DateTime.Now.ToString("yyyyMMdd");
                    tkdYyksho.UpdTime = DateTime.Now.ToString("hhmm");
                    tkdYyksho.UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq;
                    tkdYyksho.UpdPrgId = "KU0600";
                }
            }
            private byte CalculateValueYykSho(string ukeNo, string Option)
            {
                int countNumberOne = 0;
                int countNumberTwo = 0;
                int countAll = 0;
                var UnkobiList = new List<TkdUnkobi>();
                UnkobiList = _context.TkdUnkobi.Where(x => x.UkeNo == ukeNo).ToList();
                if (UnkobiList != null && UnkobiList.Count > 0)
                {
                    countAll = UnkobiList.Count();
                    if (Option == "HaiSkbn")
                    {
                        countNumberOne = UnkobiList.Where(x => x.HaiSkbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.HaiSkbn == 2).Count();
                    }
                    else if (Option == "Kskbn")
                    {
                        countNumberOne = UnkobiList.Where(x => x.Kskbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.Kskbn == 2).Count();
                    }
                    else
                    {
                        countNumberOne = UnkobiList.Where(x => x.HaiIkbn == 1).Count();
                        countNumberTwo = UnkobiList.Where(x => x.HaiIkbn == 2).Count();
                    }
                    if (countNumberOne == countAll)
                    {
                        return 1;
                    }
                    else if (countNumberTwo == countAll)
                    {
                        return 2;
                    }
                }
                return 3;
            }
        }
    }
}

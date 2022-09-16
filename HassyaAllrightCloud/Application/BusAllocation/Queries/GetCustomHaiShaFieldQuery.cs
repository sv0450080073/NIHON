using HassyaAllrightCloud.Domain.Dto;
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
    public class GetCustomHaiShaFieldQuery : IRequest<Dictionary<string, string>>
    {
        private string ukeno { get; set; }
        private short unkRen { get; set; }
        private short syasyuRen { get; set; }
        private short teidanNo { get; set; }
        private short bunkRen { get; set; }
        private List<CustomFieldConfigs> configs { get; set; }
        public GetCustomHaiShaFieldQuery(List<CustomFieldConfigs> configs, string ukeno, short unkRen, short syasyuRen, short teidanNo, short bunkRen)
        {
            this.ukeno = ukeno;
            this.configs = configs;
            this.unkRen = unkRen;
            this.syasyuRen = syasyuRen;
            this.teidanNo = teidanNo;
            this.bunkRen = bunkRen;
        }
        public class Handler : IRequestHandler<GetCustomHaiShaFieldQuery, Dictionary<string, string>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomHaiShaFieldQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetCustomHaiShaFieldQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Dictionary<string, string>> Handle(GetCustomHaiShaFieldQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var haishaData = _context.TkdHaisha.SingleOrDefault(unk => unk.UkeNo == request.ukeno && unk.UnkRen == request.unkRen && unk.SyaSyuRen ==request.syasyuRen
                    && unk.TeiDanNo ==request.teidanNo && unk.BunkRen ==request.bunkRen);
                    var result = new Dictionary<string, string>();
                    if (request.configs.Count() > 0)
                    {
                        foreach (var fieldId in request.configs.Select(c => c.id))
                        {
                            result[fieldId.ToString()] = _context.Entry(haishaData).Property($"CustomItems{fieldId}").CurrentValue.ToString();
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex.ToString());

                    return null;
                }

            }
        }
    }
}

using HassyaAllrightCloud.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BusLineScheduleColor.Queries
{
    public class GetAllBusLineScheduleColorQuery : IRequest<Dictionary<string, string>>
    {
        public class Handler : IRequestHandler<GetAllBusLineScheduleColorQuery, Dictionary<string, string>>
        {
            //private readonly KobodbContext _context;

            //public Handler(KobodbContext context)
            //{
            //    _context = context;
            //}

            public async Task<Dictionary<string, string>> Handle(GetAllBusLineScheduleColorQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return await Task.FromResult(new Dictionary<string, string>()
                    {
                        { BusCssClass.SimezumiColor, "#003399" },
                        { BusCssClass.NippozumiColor, "#673bb7" },
                        { BusCssClass.HaiinzumiColor, "#ff9802" },
                        { BusCssClass.HaishazumiColor, "#f54337" },
                        { BusCssClass.YoushazumiColor, "#5be8cc" },
                        { BusCssClass.MikarishaColor, "#3e2723" },
                        { BusCssClass.KakuteiColor, "#689e39" },
                        { BusCssClass.KakuteichuColor, "#c4e1a4" },
                        { BusCssClass.KarishaColor, "#64b5f7" },
                    });
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

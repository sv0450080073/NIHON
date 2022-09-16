using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.Unkobi.Queries
{
    public class GetTkdUnkobibyUkenoListQuery : IRequest<DatetimeData>
    {
        public string UkenoList { get; set; }
        public class Handler : IRequestHandler<GetTkdUnkobibyUkenoListQuery, DatetimeData>
        {
            private readonly KobodbContext _context;
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<DatetimeData> Handle(GetTkdUnkobibyUkenoListQuery request, CancellationToken cancellationToken)
            {
                string[] ukenolst = request.UkenoList.Split(',');
                var getlistkukeno = _context.TkdUnkobi.Select(x => new
                {
                    Field = string.Concat(x.UkeNo, x.UnkRen.ToString("D3")),
                    RowData = x,
                    x.SyukoYmd,
                    x.TouYmd
                }).ToList().Where(t => ukenolst.Contains(t.Field)).ToList();
                DatetimeData datetimeData = new DatetimeData();
                datetimeData.DateStart = DateTime.ParseExact(getlistkukeno.Min(t => t.SyukoYmd), "yyyyMMdd", null);
                datetimeData.DateEnd = DateTime.ParseExact(getlistkukeno.Max(t => t.TouYmd), "yyyyMMdd", null);
                return await Task.FromResult(datetimeData);

            }
        }
    }
}

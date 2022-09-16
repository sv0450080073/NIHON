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
    public class GetUnkobiDataQuery : IRequest<TkdUnkobi>
    {
        public string Ukeno { get; set; }
        public short UnkRen { get; set; }
        public class Handler : IRequestHandler<GetUnkobiDataQuery, TkdUnkobi>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetUnkobiDataQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetUnkobiDataQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public Task<TkdUnkobi> Handle(GetUnkobiDataQuery request, CancellationToken cancellationToken)
            {              
                try
                {
                    TkdUnkobi tkdUnkobi = _context.TkdUnkobi.FirstOrDefault(e => e.UkeNo == request.Ukeno && e.UnkRen ==request.UnkRen);
                    /*SetTkdUnkobiData(ref tkdUnkobi, request.Ukeno, request.UnkRen);*/
                    return Task.FromResult(tkdUnkobi);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }        
        }
    }
}

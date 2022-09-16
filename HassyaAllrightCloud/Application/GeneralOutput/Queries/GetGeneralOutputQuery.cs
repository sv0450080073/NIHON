using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Commons.Extensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace HassyaAllrightCloud.Application.GeneralOutput.Queries
{
    public class GetGeneralOutputQuery : IRequest<DataTable>
    {
        public static string _sqlQuery { get; set; }
        public GetGeneralOutputQuery(string sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }
        public class Handler : IRequestHandler<GetGeneralOutputQuery, DataTable> 
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetGeneralOutputQuery> _logger;

            public Handler(KobodbContext context, ILogger<GetGeneralOutputQuery> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<DataTable> Handle(GetGeneralOutputQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    return _context.DataTable(_sqlQuery);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            } 
        }
    }
}

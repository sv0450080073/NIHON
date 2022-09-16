using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BookingInput.Queries
{
    public class GetCustomFieldQuery : IRequest<Dictionary<string, string>>
    {
        private string ukeno { get; set; }
        private List<CustomFieldConfigs> configs { get; set; }

        public GetCustomFieldQuery(List<CustomFieldConfigs> configs, string ukeno)
        {
            this.ukeno = ukeno;
            this.configs = configs;
        }

        public class Handler : IRequestHandler<GetCustomFieldQuery, Dictionary<string, string>>
        {
            private readonly KobodbContext _context;
            private readonly ILogger<GetCustomFieldQuery> _logger;
            public Handler(KobodbContext context, ILogger<GetCustomFieldQuery> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Dictionary<string, string>> Handle(GetCustomFieldQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var unkobi = _context.TkdUnkobi.SingleOrDefault(unk => unk.UkeNo == request.ukeno);
                    var result = new Dictionary<string, string>(); 
                    if (request.configs.Count() > 0)
                    {
                        foreach (var fieldId in request.configs.Select(c => c.id))
                        {
                            result[fieldId.ToString()] = _context.Entry(unkobi).Property($"CustomItems{fieldId}").CurrentValue.ToString();
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

using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Queries
{
    public class GetMaxFieldId : IRequest<int>
    {
        public string FormName { get; set; }
        public int EmployeeId { get; set; }
        public class Handler : IRequestHandler<GetMaxFieldId, int>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<int> Handle(GetMaxFieldId request, CancellationToken cancellationToken)
            {
                var filterId = _dbcontext.TkdFilter.Where(x => x.FormNm == request.FormName && x.SyainCdSeq == request.EmployeeId).OrderByDescending(x => x.FilterId).FirstOrDefault();
                if(filterId != null)
                {
                    return filterId.FilterId;
                }
                return 0;
            }
        }
    }
}

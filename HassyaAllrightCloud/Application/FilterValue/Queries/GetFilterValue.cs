using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.SuperMenuFilterValue.Queries
{
    public class GetFilterValue : IRequest<List<TkdInpCon>>
    {
        public string formName { get; set; }
        public int filterId { get; set; }
        public int employeeId { get; set; }

        public class Handler : IRequestHandler<GetFilterValue, List<TkdInpCon>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<List<TkdInpCon>> Handle(GetFilterValue request, CancellationToken cancellationToken)
            {
                return _dbcontext.TkdInpCon.Where(x => x.FormNm == request.formName && x.FilterId == request.filterId && x.SyainCdSeq == request.employeeId).ToList();
            }
        }
    }
}

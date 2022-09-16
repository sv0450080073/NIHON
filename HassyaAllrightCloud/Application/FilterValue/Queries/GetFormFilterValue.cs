using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Queries
{
    public class GetFormFilterValue : IRequest<List<TkdInpCon>>
    {
        public string formName { get; set; }
        public int filterId { get; set; }
        public int employeeId { get; set; }
        public class Handler : IRequestHandler<GetFormFilterValue, List<TkdInpCon>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<List<TkdInpCon>> Handle(GetFormFilterValue request, CancellationToken cancellationToken)
            {
                var formFilterValues = _dbcontext.TkdInpCon.Where(x => x.SyainCdSeq == request.employeeId && x.FormNm == request.formName  && x.FilterId == request.filterId).ToList();

                return formFilterValues;
            }
        }
    }
}

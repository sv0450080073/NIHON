using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.GridLayout.Queries
{
    public class GetGridLayout : IRequest<List<TkdGridLy>>
    {
        public int employeeId { get; set; }
        public string formName { get; set; }
        public string gridName { get; set; }

        public class Handler : IRequestHandler<GetGridLayout, List<TkdGridLy>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                this._dbcontext = context;
            }

            public async Task<List<TkdGridLy>> Handle(GetGridLayout request, CancellationToken cancellationToken)
            {
                return _dbcontext.TkdGridLy.Where(x => x.SyainCdSeq == request.employeeId && x.FormNm == request.formName && x.GridNm == request.gridName).OrderBy(x => x.DspNo).ToList();
            }
        }
    }
}

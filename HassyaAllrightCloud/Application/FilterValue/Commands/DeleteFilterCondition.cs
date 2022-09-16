using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Entities;

namespace HassyaAllrightCloud.Application.FilterValue.Commands
{
    public class DeleteFilterCondition : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public string FormName { get; set; }
        public int FilterId { get; set; }
        public class Handler : IRequestHandler<DeleteFilterCondition, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<bool> Handle(DeleteFilterCondition request, CancellationToken cancellationToken)
            {
                List<TkdInpCon> tkdInpCons = _dbcontext.TkdInpCon.Where(x => x.SyainCdSeq == request.EmployeeId && x.FormNm == request.FormName && x.FilterId == request.FilterId).ToList();

                if(tkdInpCons.Count > 0)
                {
                    _dbcontext.TkdInpCon.RemoveRange(tkdInpCons);
                }

                _dbcontext.SaveChanges();

                return true;
            }
        }
    }
}

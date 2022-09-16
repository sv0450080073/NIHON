using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.GridLayout.Commands
{
    public class DeleteGridLayout : IRequest<bool>
    {
        public int employeeId { get; set; }
        public string formName { get; set; }
        public string gridName { get; set; }

        public class Handler : IRequestHandler<DeleteGridLayout, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                this._dbcontext = context;
            }

            public async Task<bool> Handle(DeleteGridLayout request, CancellationToken cancellationToken)
            {
                var deleteGridlayout = _dbcontext.TkdGridLy.Where(x => x.SyainCdSeq == request.employeeId && x.FormNm == request.formName && x.GridNm == request.gridName).ToList();
                if(deleteGridlayout != null)
                {
                    _dbcontext.TkdGridLy.RemoveRange(deleteGridlayout);
                    _dbcontext.SaveChanges();
                }
                return true;
            }
        }
    }
}

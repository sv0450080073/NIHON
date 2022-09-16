using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Commands
{
    public class DeleteCustomFiler : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public int FilerId { get; set; }
        public string FormName { get; set; }
        public string FilterName { get; set; }
        public class Handler : IRequestHandler<DeleteCustomFiler, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<bool> Handle(DeleteCustomFiler request, CancellationToken cancellationToken)
            {
                TkdFilter deletedTkdFilter = _dbcontext.TkdFilter.Where(x => x.SyainCdSeq == request.EmployeeId && x.FilterId == request.FilerId && x.FilterName == request.FilterName && x.FormNm == request.FormName).FirstOrDefault();
                if(deletedTkdFilter != null)
                {
                    deletedTkdFilter.SiyoKbn = 2;
                    _dbcontext.Update(deletedTkdFilter);
                    _dbcontext.SaveChanges();
                }
                return true;
            }
        }
    }
}

using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.FilterValue.Commands
{
    public class DeleteCustomFilterConditon : IRequest<bool>
    {
        public int EmployeeId { get; set; }
        public int FilterId { get; set; }
        public string FormName { get; set; }
        public class Handler : IRequestHandler<DeleteCustomFilterConditon, bool>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }
            public async Task<bool> Handle(DeleteCustomFilterConditon request, CancellationToken cancellationToken)
            {
                List<TkdInpCon> deleteConditons = _dbcontext.TkdInpCon.Where(x => x.SyainCdSeq == request.EmployeeId && x.FilterId == request.FilterId && x.FormNm == request.FormName).ToList();
                if(deleteConditons.Count > 0)
                {
                    _dbcontext.RemoveRange(deleteConditons);
                    _dbcontext.SaveChanges();
                }
                return true;
            }
        }
    }
}

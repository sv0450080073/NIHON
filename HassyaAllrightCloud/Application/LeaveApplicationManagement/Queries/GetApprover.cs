using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.LeaveApplicationManagement.Queries
{
    public class GetApprover : IRequest<Staffs>
    {
        public int syainCdSeq { get; set; }
        public class Handler : IRequestHandler<GetApprover, Staffs>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<Staffs> Handle(GetApprover request, CancellationToken cancellationToken)
            {
                var emp = _dbContext.VpmSyain.Where(x => x.SyainCdSeq == request.syainCdSeq).FirstOrDefault();
                return new Staffs()
                {
                    Seg = emp.SyainCdSeq,
                    Name = emp.SyainNm,
                    SyainCd = emp.SyainCd
                };
            }
        }
    }
}

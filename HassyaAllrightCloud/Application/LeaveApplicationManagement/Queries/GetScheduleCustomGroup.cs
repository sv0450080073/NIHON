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
    public class GetScheduleCustomGroup : IRequest<IEnumerable<CustomGroup>>
    {
        public class Handler : IRequestHandler<GetScheduleCustomGroup, IEnumerable<CustomGroup>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext kobodbContext) => _dbContext = kobodbContext;

            public async Task<IEnumerable<CustomGroup>> Handle(GetScheduleCustomGroup request, CancellationToken cancellationToken)
            {
                var result = (from cus in _dbContext.TkdSchCusGrp
                              where cus.SiyoKbn == 1 && cus.SyainCdSeq == new ClaimModel().SyainCdSeq
                              select new CustomGroup()
                              {
                                  CusGrpSeq = cus.CusGrpSeq,
                                  GrpNnm = cus.GrpNnm
                              }
                ).ToList();
                return result;
            }
        }
    }
}

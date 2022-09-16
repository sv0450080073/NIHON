using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Domain.Dto;
using System.Threading;
using HassyaAllrightCloud.Infrastructure.Persistence;

namespace HassyaAllrightCloud.Application.BatchKobanInput.Queries
{
    public class GetTask : IRequest<List<TaskModel>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetTask, List<TaskModel>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<TaskModel>> Handle(GetTask request, CancellationToken cancellationToken = default)
            {
                List<TaskModel> result = new List<TaskModel>();

                var data = _dbcontext.VpmSyokum.Where(x => x.TenantCdSeq == request.TenantCdSeq && x.SiyoKbn == 1).ToList();
                if(data == null || data.Count == 0)
                {
                    data = _dbcontext.VpmSyokum.Where(x => x.TenantCdSeq == 0 && x.SiyoKbn == 1).ToList();
                }
                foreach(var item in data)
                {
                    result.Add(new TaskModel()
                    {
                        SyokumuCd = item.SyokumuCd,
                        SyokumuNm = item.SyokumuNm
                    });
                }
                return result;
            }
        }
    }
}

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
    public class GetSyokumu : IRequest<List<SyokumuDataModel>>
    {
        public int TenantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetSyokumu, List<SyokumuDataModel>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<SyokumuDataModel>> Handle(GetSyokumu request, CancellationToken cancellationToken = default)
            {
                List<SyokumuDataModel> result = new List<SyokumuDataModel>();
                var data = _dbcontext.VpmSyokum.Where(x => x.SiyoKbn == 1 && x.TenantCdSeq == request.TenantCdSeq).ToList();
                if(data == null || data.Count == 0)
                {
                    data = _dbcontext.VpmSyokum.Where(x => x.SiyoKbn == 1 && x.TenantCdSeq == 0).ToList();
                }
                foreach(var item in data)
                {
                    result.Add(new SyokumuDataModel()
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

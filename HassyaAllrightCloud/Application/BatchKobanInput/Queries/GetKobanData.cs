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
    public class GetKobanData : IRequest<List<KobanDataModel>>
    {
        public string UnkYmd { get; set; }
        public int SyainCdSeq { get; set; }
        public class Handler : IRequestHandler<GetKobanData, List<KobanDataModel>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<KobanDataModel>> Handle(GetKobanData request, CancellationToken cancellationToken = default)
            {
                List<KobanDataModel> result = new List<KobanDataModel>();
                var data = _dbcontext.TkdKoban.Where(x => x.UnkYmd == request.UnkYmd && x.SyainCdSeq == request.SyainCdSeq && x.SiyoKbn == 1).OrderByDescending(x => x.KouBnRen).ToList();
                foreach(var item in data)
                {
                    result.Add(new KobanDataModel()
                    {
                        KouBnRen = item.KouBnRen,
                        SyainCdSeq = item.SyainCdSeq,
                        UnkYmd = item.UnkYmd
                    });
                }

                return result;
            }
        }
    }
}

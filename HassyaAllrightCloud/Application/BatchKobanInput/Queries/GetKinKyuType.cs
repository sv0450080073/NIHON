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
    public class GetKinKyuType : IRequest<List<WorkHolidayTypeDataModel>>
    {
        public int TennantCdSeq { get; set; }
        public class Handler : IRequestHandler<GetKinKyuType, List<WorkHolidayTypeDataModel>>
        {
            private readonly KobodbContext _dbcontext;

            public Handler(KobodbContext context)
            {
                _dbcontext = context;
            }

            public async Task<List<WorkHolidayTypeDataModel>> Handle(GetKinKyuType request, CancellationToken cancellationToken = default)
            {
                int tenantCdSeq = 0;
                var code = _dbcontext.VpmCodeSy.Where(x => x.CodeSyu == "KinKyuKbn" && x.KanriKbn != 1).ToList();
                if(code.Count == 0)
                {
                    tenantCdSeq = 0;
                }
                else
                {
                    tenantCdSeq = request.TennantCdSeq;
                }

                var data = (from k in _dbcontext.VpmKinKyu
                            join c in _dbcontext.VpmCodeKb
                            on new { key1 = k.KinKyuKbn.ToString(), key2 = "KinKyuKbn", key3 = (byte)1, key4 = tenantCdSeq } equals new { key1 = c.CodeKbn, key2 = c.CodeSyu, key3 = c.SiyoKbn, key4 = c.TenantCdSeq }
                            into kcJoin
                            from kc in kcJoin.DefaultIfEmpty()
                            where k.SiyoKbn == 1 && k.TenantCdSeq == new ClaimModel().TenantID
                            orderby k.KinKyuCd
                            select new WorkHolidayTypeDataModel()
                            {
                                KinKyuCd = k.KinKyuCd,
                                KinKyuCdSeq = k.KinKyuCdSeq,
                                KinKyuKbn = k.KinKyuKbn,
                                KinKyuNm = k.KinKyuNm,
                                KinKyuKbnNm = kc.CodeKbnNm
                            }
                ).ToList();


                return data;
            }
        }
    }
}

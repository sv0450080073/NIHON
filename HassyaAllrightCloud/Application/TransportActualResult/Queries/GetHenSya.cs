using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportActualResult.Queries
{
    public class GetHenSya : IRequest<int>
    {
        public HenSyaSearchModel Model { get; set; }
        public class Handler : IRequestHandler<GetHenSya, int>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get HenSya base on processing year and Eigyo range
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<int> Handle(GetHenSya request, CancellationToken cancellationToken)
            {
                var result = 0;
                if (request != null && request.Model != null)
                {
                    var storedBuilder = _kobodbContext.LoadStoredProc("dbo.PK_dHenSya_R");
                    await storedBuilder.AddParam("@SyoriYmd", request.Model.ProcessingYear)
                        .AddParam("@CompnyCd", new ClaimModel().CompanyID)
                        .AddParam("@StrEigyoCd", request.Model.EigyoFrom)
                        .AddParam("@EndEigyoCd", request.Model.EigyoTo)
                        .AddParam("@TenantCdSeq", new ClaimModel().TenantID)
                        .AddParam("@ROWCOUNT", out IOutParam<int> retParam).ExecAsync(async r =>
                        {
                            while (r.Read())
                            {
                                result = (int)r["JigyoCarSumCnt"];
                            }
                        });
                }

                return result;
            }
        }
    }
}

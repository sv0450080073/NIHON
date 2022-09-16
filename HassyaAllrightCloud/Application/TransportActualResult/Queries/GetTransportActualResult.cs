using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.TransportActualResult.Queries
{
    public class GetTransportActualResult : IRequest<IEnumerable<TransportActualResultSPModel>>
    {
        public ReportSearchModel Model { get; set; }
        public class Handler : IRequestHandler<GetTransportActualResult, IEnumerable<TransportActualResultSPModel>>
        {
            private readonly KobodbContext _kobodbContext;
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get data for Transport Actual Result Report
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<IEnumerable<TransportActualResultSPModel>> Handle(GetTransportActualResult request, CancellationToken cancellationToken)
            {
                var result = new List<TransportActualResultSPModel>();
                if (request != null && request.Model != null)
                {
                    var storedBuilder = _kobodbContext.LoadStoredProc("dbo.PK_dTransportActualResult_R");
                    await storedBuilder.AddParam("@SyoriYmdStr", $"{request.Model.ProcessingYear}04")
                        .AddParam("@SyoriYmdEnd", $"{request.Model.ProcessingYear + 1}03")
                        .AddParam("@CompnyCd", request.Model.Company)
                        .AddParam("@StrEigyoCd", request.Model.EigyoFrom)
                        .AddParam("@EndEigyoCd", request.Model.EigyoTo)
                        .AddParam("@StrUnsouKbn", request.Model.ShippingFrom)
                        .AddParam("@EndUnsouKbn", request.Model.ShippingTo)
                        .AddParam("@TenantCdSeq", request.Model.CurrentTenantId)
                        .AddParam("@ROWCOUNT", out IOutParam<int> retParam).ExecAsync(async r => result = await r.ToListAsync<TransportActualResultSPModel>(cancellationToken));
                }

                return result;
            }
        }
    }
}

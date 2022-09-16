using HassyaAllrightCloud.Commons;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Extensions;
using System.Data.Common;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.TransportationSummary.Commands
{
    public class RemoveTkdJitHou : IRequest<bool>
    {
        public TransportationSummarySearchModel Model { get; set; }
        public DbTransaction Transaction { get; set; }
        public class Handler : IRequestHandler<RemoveTkdJitHou, bool>
        {
            /// <summary>
            /// Execute SP PK_RemoveTkdJitHou_E to remove TKD_JitHou's records base on input conditions
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> Handle(RemoveTkdJitHou request, CancellationToken cancellationToken)
            {
                try
                {
                    var yyyyMM = request.Model.ProcessingDate.ToString(DateTimeFormat.yyyyMM);
                    var eigyoFrom = request.Model.EigyoFrom?.EigyoCd.ToString() ?? string.Empty;
                    var eigyoTo = request.Model.EigyoTo?.EigyoCd.ToString() ?? string.Empty;

                    using var command = request.Transaction.Connection.CreateCommand();
                    command.CommandText = @$"EXECUTE PK_RemoveTkdJitHou_E
                                                @SyoriYm
                                                ,@CompanyCdSeq
                                                ,@StrEigyoCd
                                                ,@EndEigyoCd
                                                ,@TenantCdSeq";

                    command.AddParam("@SyoriYm", yyyyMM);
                    command.AddParam("@CompanyCdSeq", request.Model.Company.CompanyCdSeq);
                    command.AddParam("@StrEigyoCd", eigyoFrom);
                    command.AddParam("@EndEigyoCd", eigyoTo);
                    command.AddParam("@TenantCdSeq", new ClaimModel().TenantID);
                    command.Transaction = request.Transaction;
                    await command.ExecuteNonQueryAsync(cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    // Todo: Log error
                    return false;
                }
            }
        }
    }
}

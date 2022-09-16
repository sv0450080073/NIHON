using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.TransportationSummary.Commands
{
    public class InsertTransportationSummary : IRequest<bool>
    {
        public TransportationSummarySearchModel Model { get; set; }
        public DbTransaction Transaction { get; set; }
        public class Handler : IRequestHandler<InsertTransportationSummary, bool>
        {
            private static string UpdPrgID = "KG1000P";
            private static string KinouId = "KG1000";

            /// <summary>
            /// Execute SP PK_YusoSyu_E to:
            /// 1: Delete existing TKD_JitHou base on input conditions
            /// 2: Insert new records to TKD_JitHou by gather data from multi tables.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<bool> Handle(InsertTransportationSummary request, CancellationToken cancellationToken)
            {
                try
                {
                    var processingDate = request.Model.ProcessingDate;
                    var startDate = new DateTime(processingDate.Year, processingDate.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);
                    var eigyoFrom = request.Model.EigyoFrom?.EigyoCd.ToString() ?? string.Empty;
                    var eigyoTo = request.Model.EigyoTo?.EigyoCd.ToString() ?? string.Empty;

                    using var command = request.Transaction.Connection.CreateCommand();
                    command.CommandText = @$"EXECUTE PK_YusoSyu_E 
                                               @StrDate
                                              ,@EndDate
                                              ,@UnsoKbn
                                              ,@CompanyCdSeq
                                              ,@StrEigyoCd
                                              ,@EndEigyoCd
                                              ,@KinouId
                                              ,@SyainCdSeq
                                              ,@UpdPrgID
                                              ,@TenantCdSeq";

                    command.AddParam("@StrDate", startDate.ToString(DateTimeFormat.yyyyMMdd));
                    command.AddParam("@EndDate", endDate.ToString(DateTimeFormat.yyyyMMdd));
                    command.AddParam("@UnsoKbn", request.Model.UnsoKbn);
                    command.AddParam("@CompanyCdSeq", request.Model.Company.CompanyCdSeq);
                    command.AddParam("@StrEigyoCd", eigyoFrom);
                    command.AddParam("@EndEigyoCd", eigyoTo);
                    command.AddParam("@KinouId", KinouId);
                    command.AddParam("@SyainCdSeq", new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    command.AddParam("@UpdPrgID", UpdPrgID);
                    command.AddParam("@TenantCdSeq", new ClaimModel().TenantID);
                    command.Transaction = request.Transaction;
                    await command.ExecuteNonQueryAsync(cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    // Todo: Log error
                    throw ex;
                }
            }
        }
    }
}

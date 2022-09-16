using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RevenueSummary.Queries
{
    public class GetEigyoListForDailyRevenueReport : IRequest<List<EigyoListItem>>
    {
        public TransportationRevenueSearchModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetEigyoListForDailyRevenueReport, List<EigyoListItem>>
        {
            private KobodbContext _kobodbContext;
            private static string CEigyoCd = "CEigyoCd";
            private static string CEigyoCdSeq = "CEigyoCdSeq";
            private static string CEigyoRyakuNm = "CEigyoRyakuNm";

            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get list of Eigyo are available for Daily Transportation Revenue
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<EigyoListItem>> Handle(GetEigyoListForDailyRevenueReport request, CancellationToken cancellationToken)
            {
                List<EigyoListItem> listResult = new List<EigyoListItem>();

                if (request == null || request.SearchModel == null) return listResult;

                var connection = _kobodbContext.Database.GetDbConnection(); 
                try
                {
                    connection.Open();
                    using var command = connection.CreateCommand();
                    command.CommandText = @$"EXECUTE PK_dEigyosForDailyRevenueReport_R 
                                                @UriYmdFrom,
		                                        @UriYmdTo,
		                                        @CompanyCd,
		                                        @EigyoCdFrom,
		                                        @EigyoCdTo,
		                                        @UkeNoFrom,
		                                        @UkeNoTo,
		                                        @YoyaKbnFrom,
		                                        @YoyaKbnTo,
		                                        @TenantCdSeq,
		                                        @EigyoKbn,
                                                @ROWCOUNT OUTPUT";
                    #region Add parameters
                    command.AddParam("@UriYmdFrom", request.SearchModel.UriYmdFrom);
                    command.AddParam("@UriYmdTo", request.SearchModel.UriYmdTo);
                    if(request.SearchModel.Company == 0)
                        command.AddParam("@CompanyCd", DBNull.Value);
                    else
                        command.AddParam("@CompanyCd", request.SearchModel.Company);

                    if(request.SearchModel.EigyoFrom == 0)
                        command.AddParam("@EigyoCdFrom", DBNull.Value);
                    else
                        command.AddParam("@EigyoCdFrom", request.SearchModel.EigyoFrom);

                    if (request.SearchModel.EigyoTo == 0)
                        command.AddParam("@EigyoCdTo", DBNull.Value);
                    else
                        command.AddParam("@EigyoCdTo", request.SearchModel.EigyoTo);

                    if (string.IsNullOrEmpty(request.SearchModel.UkeNoFrom.Trim()))
                        command.AddParam("@UkeNoFrom", DBNull.Value);
                    else
                        command.AddParam("@UkeNoFrom", request.SearchModel.UkeNoFrom);

                    if (string.IsNullOrEmpty(request.SearchModel.UkeNoTo.Trim()))
                        command.AddParam("@UkeNoTo", DBNull.Value);
                    else
                        command.AddParam("@UkeNoTo", request.SearchModel.UkeNoTo);

                    if (request.SearchModel.YoyaKbnFrom == 0)
                        command.AddParam("@YoyaKbnFrom", DBNull.Value);
                    else
                        command.AddParam("@YoyaKbnFrom", request.SearchModel.YoyaKbnFrom);

                    if (request.SearchModel.YoyaKbnTo == 0)
                        command.AddParam("@YoyaKbnTo", DBNull.Value);
                    else
                        command.AddParam("@YoyaKbnTo", request.SearchModel.YoyaKbnTo);

                    command.AddParam("@TenantCdSeq", request.SearchModel.TenantCdSeq);
                    command.AddParam("@EigyoKbn", (int)request.SearchModel.EigyoKbn);
                    command.AddOutputParam("@ROWCOUNT");
                    #endregion

                    var table = await command.ExecuteNonQueryAsync(cancellationToken);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var item = new EigyoListItem()
                            {
                                EigyoCd = reader[CEigyoCd] is DBNull ? 0 : (int)reader[CEigyoCd],
                                EigyoCdSeq = reader[CEigyoCdSeq] is DBNull ? 0 : (int)reader[CEigyoCdSeq],
                                RyakuNm = reader[CEigyoRyakuNm] is DBNull ? string.Empty : (string)reader[CEigyoRyakuNm]
                            };

                            listResult.Add(item);
                        }

                        await reader.CloseAsync();
                    }
                    return listResult;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}

using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RevenueSummary.Queries
{
    public class GetUriYmdForDailyRevenueReport : IRequest<List<string>>
    {
        public DailyRevenueSearchModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetUriYmdForDailyRevenueReport, List<string>>
        {
            private KobodbContext _kobodbContext;
            private static string CUriYmd = "CUriYmd";
            
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get list of UriYmd for Daily Revenue Report
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<List<string>> Handle(GetUriYmdForDailyRevenueReport request, CancellationToken cancellationToken)
            {
                List<string> listResult = new List<string>();

                if (request == null || request.SearchModel == null) return listResult;

                var connection = _kobodbContext.Database.GetDbConnection(); 
                try
                {
                    connection.Open();
                    using var command = connection.CreateCommand();
                    command.CommandText = @$"EXECUTE PK_dUriYmdsForDailyRevenueReport_R 
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
                    #region Add Params
                    command.AddParam("@UriYmdFrom", request.SearchModel.RevenueSearchModel.UriYmdFrom);
                    command.AddParam("@UriYmdTo", request.SearchModel.RevenueSearchModel.UriYmdTo);
                    if(request.SearchModel.RevenueSearchModel.Company == 0)
                        command.AddParam("@CompanyCd", DBNull.Value);
                    else
                        command.AddParam("@CompanyCd", request.SearchModel.RevenueSearchModel.Company);

                    if(request.SearchModel.Eigyo == null || request.SearchModel.Eigyo.EigyoCd == 0)
                    {
                        command.AddParam("@EigyoCdFrom", DBNull.Value);
                        command.AddParam("@EigyoCdTo", DBNull.Value);
                    }
                    else
                    {
                        command.AddParam("@EigyoCdFrom", request.SearchModel.Eigyo.EigyoCd);
                        command.AddParam("@EigyoCdTo", request.SearchModel.Eigyo.EigyoCd);
                    }
                   
                    if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoFrom.Trim()))
                        command.AddParam("@UkeNoFrom", DBNull.Value);
                    else
                        command.AddParam("@UkeNoFrom", request.SearchModel.RevenueSearchModel.UkeNoFrom);

                    if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoTo.Trim()))
                        command.AddParam("@UkeNoTo", DBNull.Value);
                    else
                        command.AddParam("@UkeNoTo", request.SearchModel.RevenueSearchModel.UkeNoTo);

                    if (request.SearchModel.RevenueSearchModel.YoyaKbnFrom == 0)
                        command.AddParam("@YoyaKbnFrom", DBNull.Value);
                    else
                        command.AddParam("@YoyaKbnFrom", request.SearchModel.RevenueSearchModel.YoyaKbnFrom);

                    if (request.SearchModel.RevenueSearchModel.YoyaKbnTo == 0)
                        command.AddParam("@YoyaKbnTo", DBNull.Value);
                    else
                        command.AddParam("@YoyaKbnTo", request.SearchModel.RevenueSearchModel.YoyaKbnTo);

                    command.AddParam("@TenantCdSeq", request.SearchModel.RevenueSearchModel.TenantCdSeq);
                    command.AddParam("@EigyoKbn", (int)request.SearchModel.RevenueSearchModel.EigyoKbn);
                    command.AddOutputParam("@ROWCOUNT");
                    #endregion

                    var table = await command.ExecuteNonQueryAsync(cancellationToken);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listResult.Add(reader[CUriYmd] is DBNull ? string.Empty : (string)reader[CUriYmd]);
                        }

                        await reader.CloseAsync();
                    }
                    return listResult.Select(s => s.AddSlash2YYYYMMDD()).ToList();
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

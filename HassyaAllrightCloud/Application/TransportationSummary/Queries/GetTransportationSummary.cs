using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Extensions;
using DevExpress.Blazor.Internal;
using System.Data.Common;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Application.TransportationSummary.Commands;
using Newtonsoft.Json;

namespace HassyaAllrightCloud.Application.TransportationSummary.Queries
{

    public class GetTransportationSummary : IRequest<IEnumerable<TransportationSummaryItem>>
    {
        public TransportationSummarySearchModel Model { get; set; }
        public bool GetOnly { get; set; }
        public class Handler : IRequestHandler<GetTransportationSummary, IEnumerable<TransportationSummaryItem>>
        {
            private static string SyoriYmCol = "SyoriYm";
            private static string CompanyCdCol = "CompanyCd";
            private static string CompanyNmCol = "CompanyNm";
            private static string EigyoCdCol = "EigyoCd";
            private static string EigyoNmCol = "EigyoNm";
            private static string UpdYmdCol = "UpdYmd";
            private static string UpdTimeCol = "UpdTime";

            private readonly KobodbContext _kobodbContext;
            private readonly IMediator _mediator;
            public Handler(KobodbContext kobodbContext, IMediator mediator)
            {
                _kobodbContext = kobodbContext;
                _mediator = mediator;
            }

            /// <summary>
            /// Get Transportation Summary base on input conditions by following steps:
            /// 1. 既存の集計データを削除する 
            /// 2. 新しい集計データを登録する
            /// 3. 一覧に結果データを表示する
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<IEnumerable<TransportationSummaryItem>> Handle(GetTransportationSummary request, CancellationToken cancellationToken)
            {
                var connection = _kobodbContext.Database.GetDbConnection();
                connection.Open();
                using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    if (request.Model == null) return null;
                    IEnumerable<TransportationSummaryItem> result;
                    if (request.GetOnly)
                    {
                        result = await GetTransportationSummary(request, transaction);
                    }
                    else
                    {
                        // ・UnsoKbnの値がそれぞれ1 (一般)、2 (特定)、3 (特殊)で以下の処理から新しい集計データを登録することを行う
                        var success = await _mediator.Send(new RemoveTkdJitHou() { Transaction = transaction, Model = request.Model });
                        for (var i = 1; i <= 3; i++)
                        {
                            request.Model.UnsoKbn = i;
                            if (!success) throw new Exception($"Failed execute {nameof(RemoveTkdJitHou)} with model: {JsonConvert.SerializeObject(request.Model)}");
                            success = await _mediator.Send(new InsertTransportationSummary() { Transaction = transaction, Model = request.Model });
                            if (!success) throw new Exception($"Failed execute {nameof(InsertTransportationSummary)} with model: {JsonConvert.SerializeObject(request.Model)}");
                        }

                        result = await GetTransportationSummary(request, transaction);
                    }
                    await transaction.CommitAsync();
                    await connection.CloseAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    // Todo: Log error
                    await transaction.RollbackAsync();
                    await connection.CloseAsync();
                    throw ex;
                }
            }

            /// <summary>
            /// Execute SP PK_GetTransportationSummary_R to get Transportation Summary
            /// </summary>
            /// <param name="request"></param>
            /// <param name="transaction"></param>
            /// <returns></returns>
            private async Task<IEnumerable<TransportationSummaryItem>> GetTransportationSummary(GetTransportationSummary request, DbTransaction transaction)
            {
                var queryResult = new List<SummaryTableResult>();
                var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = @$"EXECUTE PK_GetTransportationSummary_R
                                                   @SyoriYm
                                                  ,@TenantCdSeq
                                                  ,@CompnyCdSeq
                                                  ,@StrEigyoCd
                                                  ,@EndEigyoCd";
                command.AddParam("@SyoriYm", request.Model.ProcessingDate.ToString(DateTimeFormat.yyyyMM));
                command.AddParam("@TenantCdSeq", request.Model.Company.TenantCdSeq);
                command.AddParam("@CompnyCdSeq", request.Model.Company.CompanyCdSeq);
                command.AddParam("@StrEigyoCd", request.Model.EigyoFrom?.EigyoCd ?? 0);
                command.AddParam("@EndEigyoCd", request.Model.EigyoTo?.EigyoCd ?? 0);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var tableResult = new SummaryTableResult
                        {
                            SyoriYm = reader[SyoriYmCol] is DBNull ? string.Empty : (string)reader[SyoriYmCol],
                            CompanyCd = reader[CompanyCdCol] is DBNull ? 0 : (int)reader[CompanyCdCol],
                            CompanyNm = reader[CompanyNmCol] is DBNull ? string.Empty : (string)reader[CompanyNmCol],
                            EigyoCd = reader[EigyoCdCol] is DBNull ? 0 : (int)reader[EigyoCdCol],
                            EigyoNm = reader[EigyoNmCol] is DBNull ? string.Empty : (string)reader[EigyoNmCol],
                            UpdYmd = reader[UpdYmdCol] is DBNull ? string.Empty : (string)reader[UpdYmdCol],
                            UpdTime = reader[UpdTimeCol] is DBNull ? string.Empty : (string)reader[UpdTimeCol]
                        };
                        queryResult.Add(tableResult);
                    }

                    await reader.CloseAsync();
                }
                var result = queryResult.GroupBy(
                    r => r.SyoriYm,
                    r => r,
                    (key, row) => new TransportationSummaryItem()
                    {
                        ProcessingDate = key.AddSlash2YYYYMM(),
                        Companies = row.ToList().GroupBy(
                            cr => new { cr.CompanyCd, cr.CompanyNm },
                            cr => cr,
                            (ckey, crow) => new Company()
                            {
                                CompanyName = $"{(ckey.CompanyCd ?? 0).AddPaddingLeft(5)} : {ckey.CompanyNm}",
                                Eigyos = crow.Select(x => new Eigyo()
                                {
                                    EigyoName = $"{(x.EigyoCd ?? 0).AddPaddingLeft(5)} : {x.EigyoNm}",
                                    UpdateDate = $"{x.UpdYmd.AddSlash2YYYYMMDD()} {x.UpdTime.AddColon2HHMM()}"
                                })
                            })
                    });
                return result;
            }
        }
    }
}

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
    public class GetMonthlyRevenueData : IRequest<MonthlyRevenueData>
    {
        public MonthlyRevenueSearchModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetMonthlyRevenueData, MonthlyRevenueData>
        {
            private KobodbContext _kobodbContext;

            #region Column Names
            private static string MesaiKbn = "MesaiKbn";
            private static string UriYmd = "UriYmd";
            private static string KeiKin = "KeiKin";
            private static string JisSyaRyoSum = "JisSyaRyoSum";
            private static string GaiSyaRyoSum = "GaiSyaRyoSum";
            private static string EtcSyaRyoSum = "EtcSyaRyoSum";
            private static string CanSum = "CanSum";
            private static string JisSyaRyoUnc = "JisSyaRyoUnc";
            private static string JisSyaSyuDai = "JisSyaSyuDai";
            private static string JisSyaRyoSyo = "JisSyaRyoSyo";
            private static string JisSyaRyoTes = "JisSyaRyoTes";
            private static string GaiUriGakKin = "GaiUriGakKin";
            private static string GaiSyaRyoSyo = "GaiSyaRyoSyo";
            private static string GaiSyaRyoTes = "GaiSyaRyoTes";
            private static string EtcUriGakKin = "EtcUriGakKin";
            private static string EtcSyaRyoSyo = "EtcSyaRyoSyo";
            private static string EtcSyaRyoTes = "EtcSyaRyoTes";
            private static string CanUnc = "CanUnc";
            private static string CanSyoG = "CanSyoG";
            private static string YouRyakuNm = "YouRyakuNm";
            private static string EigyoCd = "EigyoCd";

            private static string YouSitRyakuNm = "YouSitRyakuNm";
            private static string YouSyaSyuDai = "YouSyaSyuDai";
            private static string YouG = "S_YouSyaRyo";
            private static string YouFutG = "S_YfuSyaRyo";
            private static string YouSyaRyoUnc = "YouSyaRyoUnc";
            private static string YouSyaRyoSyo = "YouSyaRyoSyo";
            private static string YouSyaRyoTes = "YouSyaRyoTes";
            private static string YfuUriGakKin = "YfuUriGakKin";
            private static string YfuSyaRyoSyo = "YfuSyaRyoSyo";
            private static string YfuSyaRyoTes = "YfuSyaRyoTes";

            private static string SFutSyaRyoSyo = "S_FutSyaRyoSyo";
            private static string SFutSyaRyoTes = "S_FutSyaRyoTes";
            private static string SFutUriGakKin = "S_FutUriGakKin";
            private static string SJisSyaRyoSyo = "S_JisSyaRyoSyo";
            private static string SJisSyaRyoTes = "S_JisSyaRyoTes";
            private static string SJisSyaRyoUnc = "S_JisSyaRyoUnc";
            private static string SJyuSyaRyoRui = "S_JyuSyaRyoRui";
            private static string SJyuSyaRyoSyo = "S_JyuSyaRyoSyo";
            private static string SJyuSyaRyoTes = "S_JyuSyaRyoTes";
            private static string SJyuSyaRyoUnc = "S_JyuSyaRyoUnc";
            private static string SSoneki = "S_Soneki";
            private static string SYfuSyaRyoSyo = "S_YfuSyaRyoSyo";
            private static string SYfuSyaRyoTes = "S_YfuSyaRyoTes";
            private static string SYfuUriGakKin = "S_YfuUriGakKin";
            private static string SYoushaSyo = "S_YoushaSyo";
            private static string SYoushaTes = "S_YoushaTes";
            private static string SYoushaUnc = "S_YoushaUnc";
            private static string JyuSyaRyoRui = "JyuSyaRyoRui";

            private const string YouGyosyaCd = "YouGyosyaCd";
            private const string YouCd = "YouCd";
            private const string YouSitCd = "YouSitCd";
            private const string YouGyosyaNm = "YouGyosyaNm";
            private const string YouNm = "YouNm";
            private const string YouSitenNm = "YouSitenNm";
            private const string Jippi = "Jippi";
            private const string HighwayUriGakKin = "HighwayUriGakKin";
            private const string HighwaySyaRyoSyo = "HighwaySyaRyoSyo";
            private const string HighwaySyaRyoTes = "HighwaySyaRyoTes";
            private const string HighwaySyaRyoSum = "HighwaySyaRyoSum";
            private const string HotelUriGakKin = "HotelUriGakKin";
            private const string HotelSyaRyoSyo = "HotelSyaRyoSyo";
            private const string HotelSyaRyoTes = "HotelSyaRyoTes";
            private const string HotelSyaRyoSum = "HotelSyaRyoSum";
            private const string ParkingUriGakKin = "ParkingUriGakKin";
            private const string ParkingSyaRyoSyo = "ParkingSyaRyoSyo";
            private const string ParkingSyaRyoTes = "ParkingSyaRyoTes";
            private const string ParkingSyaRyoSum = "ParkingSyaRyoSum";
            private const string OtherUriGakKin = "OtherUriGakKin";
            private const string OtherSyaRyoSyo = "OtherSyaRyoSyo";
            private const string OtherSyaRyoTes = "OtherSyaRyoTes";
            private const string OtherSyaRyoSum = "OtherSyaRyoSum";
            private const string EigyoNm = "EigyoNm";
            private const string EigyoRyak= "EigyoRyak";
            #endregion
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            /// <summary>
            /// Get Monthly Trasportation Revenue Data
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<MonthlyRevenueData> Handle(GetMonthlyRevenueData request, CancellationToken cancellationToken)
            {
                var revenueData = new MonthlyRevenueData();
                var listResult = new List<MonthlyRevenueItem>();
                var summaryResult = new List<SummaryResult>();
                if (request == null || request.SearchModel == null || request.SearchModel.RevenueSearchModel == null) return revenueData;
                var connection = _kobodbContext.Database.GetDbConnection();
                try
                {
                    connection.Open();
                    using (var command1 = connection.CreateCommand())
                    {
                        command1.CommandText = @$"EXECUTE PK_dMonthlyRevenueData_R
                                                @CompanyCd,
		                                        @UkeNoFrom,
		                                        @UkeNoTo,
		                                        @YoyaKbnFrom,
		                                        @YoyaKbnTo,
		                                        @TenantCdSeq,
		                                        @EigyoKbn,
		                                        @TesuInKbn,
		                                        @EigyoCdSeq,
                                                @UriYmdTo,
		                                        @UriYmdFrom,
                                                @ROWCOUNT OUTPUT";

                        #region Add Params For Command 1
                        if (request.SearchModel.RevenueSearchModel.Company == 0)
                            command1.AddParam("@CompanyCd", DBNull.Value);
                        else
                            command1.AddParam("@CompanyCd", request.SearchModel.RevenueSearchModel.Company);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoFrom.Trim()))
                            command1.AddParam("@UkeNoFrom", DBNull.Value);
                        else
                            command1.AddParam("@UkeNoFrom", request.SearchModel.RevenueSearchModel.UkeNoFrom);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoTo.Trim()))
                            command1.AddParam("@UkeNoTo", DBNull.Value);
                        else
                            command1.AddParam("@UkeNoTo", request.SearchModel.RevenueSearchModel.UkeNoTo);

                        if (request.SearchModel.RevenueSearchModel.YoyaKbnFrom == 0)
                            command1.AddParam("@YoyaKbnFrom", DBNull.Value);
                        else
                            command1.AddParam("@YoyaKbnFrom", request.SearchModel.RevenueSearchModel.YoyaKbnFrom);

                        if (request.SearchModel.RevenueSearchModel.YoyaKbnTo == 0)
                            command1.AddParam("@YoyaKbnTo", DBNull.Value);
                        else
                            command1.AddParam("@YoyaKbnTo", request.SearchModel.RevenueSearchModel.YoyaKbnTo);

                        if (request.SearchModel.Eigyo == null)
                            command1.AddParam("@EigyoCdSeq", DBNull.Value);
                        else
                            command1.AddParam("@EigyoCdSeq", request.SearchModel.Eigyo.EigyoCd);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UriYmdTo))
                            command1.AddParam("@UriYmdTo", DBNull.Value);
                        else
                            command1.AddParam("@UriYmdTo", request.SearchModel.RevenueSearchModel.UriYmdTo);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UriYmdFrom))
                            command1.AddParam("@UriYmdFrom", DBNull.Value);
                        else
                            command1.AddParam("@UriYmdFrom", request.SearchModel.RevenueSearchModel.UriYmdFrom);

                        command1.AddParam("@TenantCdSeq", request.SearchModel.RevenueSearchModel.TenantCdSeq);
                        command1.AddParam("@EigyoKbn", (int)request.SearchModel.RevenueSearchModel.EigyoKbn);
                        command1.AddParam("@TesuInKbn", (int)request.SearchModel.RevenueSearchModel.TesuInKbn);
                        command1.AddOutputParam("@ROWCOUNT");
                        #endregion

                        var tableResult1 = await command1.ExecuteNonQueryAsync(cancellationToken);

                        using (var reader = await command1.ExecuteReaderAsync())
                        {
                            int i = 1;
                            while (await reader.ReadAsync())
                            {
                                var item = new MonthlyRevenueItem();
                                #region DbDataReader To Model
                                item.MesaiKbn = reader[MesaiKbn] is DBNull ? 0 : (int)reader[MesaiKbn];
                                if (item.MesaiKbn == 3)
                                {
                                    item.No = i++;
                                    item.MesaiKbn = reader[MesaiKbn] is DBNull ? 0 : (int)reader[MesaiKbn];
                                    item.EigyoCd = reader[EigyoCd] is DBNull ? 0 : (int)reader[EigyoCd];
                                    item.UriYmd = reader[UriYmd] is DBNull ? string.Empty : (string)reader[UriYmd];
                                    item.KeiKin = reader[KeiKin] is DBNull ? 0 : (long)reader[KeiKin];
                                    item.JisSyaRyoSum = reader[JisSyaRyoSum] is DBNull ? 0 : (long)reader[JisSyaRyoSum];
                                    item.GaiSyaRyoSum = reader[GaiSyaRyoSum] is DBNull ? 0 : (int)reader[GaiSyaRyoSum];
                                    item.EtcSyaRyoSum = reader[EtcSyaRyoSum] is DBNull ? 0 : (int)reader[EtcSyaRyoSum];
                                    item.CanSum = reader[CanSum] is DBNull ? 0 : (int)reader[CanSum];
                                    item.JisSyaRyoUnc = reader[JisSyaRyoUnc] is DBNull ? 0 : (long)reader[JisSyaRyoUnc];
                                    item.JisSyaSyuDai = reader[JisSyaSyuDai] is DBNull ? 0 : (int)reader[JisSyaSyuDai];
                                    item.JisSyaRyoSyo = reader[JisSyaRyoSyo] is DBNull ? 0 : (long)reader[JisSyaRyoSyo];
                                    item.JisSyaRyoTes = reader[JisSyaRyoTes] is DBNull ? 0 : (long)reader[JisSyaRyoTes];
                                    item.GaiUriGakKin = reader[GaiUriGakKin] is DBNull ? 0 : (int)reader[GaiUriGakKin];
                                    item.GaiSyaRyoSyo = reader[GaiSyaRyoSyo] is DBNull ? 0 : (int)reader[GaiSyaRyoSyo];
                                    item.GaiSyaRyoTes = reader[GaiSyaRyoTes] is DBNull ? 0 : (int)reader[GaiSyaRyoTes];
                                    item.EtcUriGakKin = reader[EtcUriGakKin] is DBNull ? 0 : (int)reader[EtcUriGakKin];
                                    item.EtcSyaRyoSyo = reader[EtcSyaRyoSyo] is DBNull ? 0 : (int)reader[EtcSyaRyoSyo];
                                    item.EtcSyaRyoTes = reader[EtcSyaRyoTes] is DBNull ? 0 : (int)reader[EtcSyaRyoTes];
                                    item.CanUnc = reader[CanUnc] is DBNull ? 0 : (int)reader[CanUnc];
                                    item.CanSyoG = reader[CanSyoG] is DBNull ? 0 : (int)reader[CanSyoG];

                                    item.JyuSyaRyoRui = reader[JyuSyaRyoRui] is DBNull ? 0 : (long)reader[JyuSyaRyoRui];
                                    
                                    item.HighwayUriGakKin = reader[HighwayUriGakKin] is DBNull ? 0 : (int)reader[HighwayUriGakKin];
                                    item.HighwaySyaRyoSyo = reader[HighwaySyaRyoSyo] is DBNull ? 0 : (int)reader[HighwaySyaRyoSyo];
                                    item.HighwaySyaRyoTes = reader[HighwaySyaRyoTes] is DBNull ? 0 : (int)reader[HighwaySyaRyoTes];
                                    item.HighwaySyaRyoSum = reader[HighwaySyaRyoSum] is DBNull ? 0 : (int)reader[HighwaySyaRyoSum];
                                    item.HotelUriGakKin = reader[HotelUriGakKin] is DBNull ? 0 : (int)reader[HotelUriGakKin];
                                    item.HotelSyaRyoSyo = reader[HotelSyaRyoSyo] is DBNull ? 0 : (int)reader[HotelSyaRyoSyo];
                                    item.HotelSyaRyoTes = reader[HotelSyaRyoTes] is DBNull ? 0 : (int)reader[HotelSyaRyoTes];
                                    item.HotelSyaRyoSum = reader[HotelSyaRyoSum] is DBNull ? 0 : (int)reader[HotelSyaRyoSum];
                                    item.ParkingUriGakKin = reader[ParkingUriGakKin] is DBNull ? 0 : (int)reader[ParkingUriGakKin];
                                    item.ParkingSyaRyoSyo = reader[ParkingSyaRyoSyo] is DBNull ? 0 : (int)reader[ParkingSyaRyoSyo];
                                    item.ParkingSyaRyoTes = reader[ParkingSyaRyoTes] is DBNull ? 0 : (int)reader[ParkingSyaRyoTes];
                                    item.ParkingSyaRyoSum = reader[ParkingSyaRyoSum] is DBNull ? 0 : (int)reader[ParkingSyaRyoSum];
                                    item.OtherUriGakKin = reader[OtherUriGakKin] is DBNull ? 0 : (int)reader[OtherUriGakKin];
                                    item.OtherSyaRyoSyo = reader[OtherSyaRyoSyo] is DBNull ? 0 : (int)reader[OtherSyaRyoSyo];
                                    item.OtherSyaRyoTes = reader[OtherSyaRyoTes] is DBNull ? 0 : (int)reader[OtherSyaRyoTes];
                                    item.OtherSyaRyoSum = reader[OtherSyaRyoSum] is DBNull ? 0 : (int)reader[OtherSyaRyoSum];
                                    item.EigyoNm = reader[EigyoNm] is DBNull ? string.Empty : (string)reader[EigyoNm];
                                    item.EigyoRyak = reader[EigyoRyak] is DBNull ? string.Empty : (string)reader[EigyoRyak];

                                    item.SFutSyaRyoSyo = reader[SFutSyaRyoSyo] is DBNull ? 0 : (int)reader[SFutSyaRyoSyo];
                                    item.SFutSyaRyoTes = reader[SFutSyaRyoTes] is DBNull ? 0 : (int)reader[SFutSyaRyoTes];
                                    item.SFutUriGakKin = reader[SFutUriGakKin] is DBNull ? 0 : (int)reader[SFutUriGakKin];
                                    item.SJisSyaRyoSyo = reader[SJisSyaRyoSyo] is DBNull ? 0 : (long)reader[SJisSyaRyoSyo];
                                    item.SJisSyaRyoTes = reader[SJisSyaRyoTes] is DBNull ? 0 : (long)reader[SJisSyaRyoTes];
                                    item.SJisSyaRyoUnc = reader[SJisSyaRyoUnc] is DBNull ? 0 : (long)reader[SJisSyaRyoUnc];
                                    item.SJyuSyaRyoRui = reader[SJyuSyaRyoRui] is DBNull ? 0 : (long)reader[SJyuSyaRyoRui];
                                    item.SJyuSyaRyoSyo = reader[SJyuSyaRyoSyo] is DBNull ? 0 : (long)reader[SJyuSyaRyoSyo];
                                    item.SJyuSyaRyoTes = reader[SJyuSyaRyoTes] is DBNull ? 0 : (long)reader[SJyuSyaRyoTes];
                                    item.SJyuSyaRyoUnc = reader[SJyuSyaRyoUnc] is DBNull ? 0 : (long)reader[SJyuSyaRyoUnc];
                                    item.SSoneki = reader[SSoneki] is DBNull ? 0 : (long)reader[SSoneki];
                                    item.SYfuSyaRyoSyo = reader[SYfuSyaRyoSyo] is DBNull ? 0 : (int)reader[SYfuSyaRyoSyo];
                                    item.SYfuSyaRyoTes = reader[SYfuSyaRyoTes] is DBNull ? 0 : (int)reader[SYfuSyaRyoTes];
                                    item.SYfuUriGakKin = reader[SYfuUriGakKin] is DBNull ? 0 : (int)reader[SYfuUriGakKin];
                                    item.SYoushaSyo = reader[SYoushaSyo] is DBNull ? 0 : (int)reader[SYoushaSyo];
                                    item.SYoushaTes = reader[SYoushaTes] is DBNull ? 0 : (int)reader[SYoushaTes];
                                    item.SYoushaUnc = reader[SYoushaUnc] is DBNull ? 0 : (int)reader[SYoushaUnc];

                                    listResult.Add(item);
                                }
                                else
                                {
                                    var commonResult = new SummaryResult();
                                    commonResult.MesaiKbn = item.MesaiKbn;
                                    commonResult.SFutSyaRyoSyo = reader[SFutSyaRyoSyo] is DBNull ? 0 : (int)reader[SFutSyaRyoSyo];
                                    commonResult.SFutSyaRyoTes = reader[SFutSyaRyoTes] is DBNull ? 0 : (int)reader[SFutSyaRyoTes];
                                    commonResult.SFutUriGakKin = reader[SFutUriGakKin] is DBNull ? 0 : (int)reader[SFutUriGakKin];
                                    commonResult.SJisSyaRyoSyo = reader[SJisSyaRyoSyo] is DBNull ? 0 : (long)reader[SJisSyaRyoSyo];
                                    commonResult.SJisSyaRyoTes = reader[SJisSyaRyoTes] is DBNull ? 0 : (long)reader[SJisSyaRyoTes];
                                    commonResult.SJisSyaRyoUnc = reader[SJisSyaRyoUnc] is DBNull ? 0 : (long)reader[SJisSyaRyoUnc];
                                    commonResult.SJyuSyaRyoRui = reader[SJyuSyaRyoRui] is DBNull ? 0 : (long)reader[SJyuSyaRyoRui];
                                    commonResult.SJyuSyaRyoSyo = reader[SJyuSyaRyoSyo] is DBNull ? 0 : (long)reader[SJyuSyaRyoSyo];
                                    commonResult.SJyuSyaRyoTes = reader[SJyuSyaRyoTes] is DBNull ? 0 : (long)reader[SJyuSyaRyoTes];
                                    commonResult.SJyuSyaRyoUnc = reader[SJyuSyaRyoUnc] is DBNull ? 0 : (long)reader[SJyuSyaRyoUnc];
                                    commonResult.SSoneki = reader[SSoneki] is DBNull ? 0 : (long)reader[SSoneki];
                                    commonResult.SYfuSyaRyoSyo = reader[SYfuSyaRyoSyo] is DBNull ? 0 : (int)reader[SYfuSyaRyoSyo];
                                    commonResult.SYfuSyaRyoTes = reader[SYfuSyaRyoTes] is DBNull ? 0 : (int)reader[SYfuSyaRyoTes];
                                    commonResult.SYfuUriGakKin = reader[SYfuUriGakKin] is DBNull ? 0 : (int)reader[SYfuUriGakKin];
                                    commonResult.SYoushaSyo = reader[SYoushaSyo] is DBNull ? 0 : (int)reader[SYoushaSyo];
                                    commonResult.SYoushaTes = reader[SYoushaTes] is DBNull ? 0 : (int)reader[SYoushaTes];
                                    commonResult.SYoushaUnc = reader[SYoushaUnc] is DBNull ? 0 : (int)reader[SYoushaUnc];

                                    commonResult.JisSyaSyuDai = reader[JisSyaSyuDai] is DBNull ? 0 : (int)reader[JisSyaSyuDai];
                                    commonResult.GaiUriGakKin = reader[GaiUriGakKin] is DBNull ? 0 : (int)reader[GaiUriGakKin];
                                    commonResult.GaiSyaRyoSyo = reader[GaiSyaRyoSyo] is DBNull ? 0 : (int)reader[GaiSyaRyoSyo];
                                    commonResult.GaiSyaRyoTes = reader[GaiSyaRyoTes] is DBNull ? 0 : (int)reader[GaiSyaRyoTes];
                                    commonResult.GaiSyaRyoSum = reader[GaiSyaRyoSum] is DBNull ? 0 : (int)reader[GaiSyaRyoSum];
                                    commonResult.CanUnc = reader[CanUnc] is DBNull ? 0 : (int)reader[CanUnc];
                                    commonResult.CanSyoG = reader[CanSyoG] is DBNull ? 0 : (int)reader[CanSyoG];
                                    commonResult.CanSum = reader[CanSum] is DBNull ? 0 : (int)reader[CanSum];

                                    commonResult.EtcUriGakKin = reader[EtcUriGakKin] is DBNull ? 0 : (int)reader[EtcUriGakKin];
                                    commonResult.EtcSyaRyoSyo = reader[EtcSyaRyoSyo] is DBNull ? 0 : (int)reader[EtcSyaRyoSyo];
                                    commonResult.EtcSyaRyoTes = reader[EtcSyaRyoTes] is DBNull ? 0 : (int)reader[EtcSyaRyoTes];
                                    commonResult.EtcSyaRyoSum = reader[EtcSyaRyoSum] is DBNull ? 0 : (int)reader[EtcSyaRyoSum];
                                    summaryResult.Add(commonResult);
                                }
                                #endregion
                            }

                            await reader.CloseAsync();
                        }
                    }

                    List<MonthlyRevenueDetailItem> detailItems = new List<MonthlyRevenueDetailItem>();
                    using (var command2 = connection.CreateCommand())
                    {
                        command2.CommandText = @$"EXECUTE PK_dMonthlyRevenueDetailItems_R
                                            @CompanyCd,
		                                    @UkeNoFrom,
		                                    @UkeNoTo,
		                                    @YoyaKbnFrom,
		                                    @YoyaKbnTo,
		                                    @TenantCdSeq,
		                                    @EigyoKbn,
		                                    @TesuInKbn,
		                                    @EigyoCdSeq,
                                            @UriYmdTo,
		                                    @UriYmdFrom,
                                            @ROWCOUNT OUTPUT";

                        #region Add Params For Command2
                        if (request.SearchModel.RevenueSearchModel.Company == 0)
                            command2.AddParam("@CompanyCd", DBNull.Value);
                        else
                            command2.AddParam("@CompanyCd", request.SearchModel.RevenueSearchModel.Company);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoFrom.Trim()))
                            command2.AddParam("@UkeNoFrom", DBNull.Value);
                        else
                            command2.AddParam("@UkeNoFrom", request.SearchModel.RevenueSearchModel.UkeNoFrom);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UkeNoTo.Trim()))
                            command2.AddParam("@UkeNoTo", DBNull.Value);
                        else
                            command2.AddParam("@UkeNoTo", request.SearchModel.RevenueSearchModel.UkeNoTo);

                        if (request.SearchModel.RevenueSearchModel.YoyaKbnFrom == 0)
                            command2.AddParam("@YoyaKbnFrom", DBNull.Value);
                        else
                            command2.AddParam("@YoyaKbnFrom", request.SearchModel.RevenueSearchModel.YoyaKbnFrom);

                        if (request.SearchModel.RevenueSearchModel.YoyaKbnTo == 0)
                            command2.AddParam("@YoyaKbnTo", DBNull.Value);
                        else
                            command2.AddParam("@YoyaKbnTo", request.SearchModel.RevenueSearchModel.YoyaKbnTo);

                        if (request.SearchModel.Eigyo == null)
                            command2.AddParam("@EigyoCdSeq", DBNull.Value);
                        else
                            command2.AddParam("@EigyoCdSeq", request.SearchModel.Eigyo.EigyoCd);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UriYmdTo))
                            command2.AddParam("@UriYmdTo", DBNull.Value);
                        else
                            command2.AddParam("@UriYmdTo", request.SearchModel.RevenueSearchModel.UriYmdTo);

                        if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UriYmdFrom))
                            command2.AddParam("@UriYmdFrom", DBNull.Value);
                        else
                            command2.AddParam("@UriYmdFrom", request.SearchModel.RevenueSearchModel.UriYmdFrom);

                        command2.AddParam("@TenantCdSeq", request.SearchModel.RevenueSearchModel.TenantCdSeq);
                        command2.AddParam("@EigyoKbn", (int)request.SearchModel.RevenueSearchModel.EigyoKbn);
                        command2.AddParam("@TesuInKbn", (int)request.SearchModel.RevenueSearchModel.TesuInKbn);
                        command2.AddOutputParam("@ROWCOUNT");

                        #endregion

                        var tableResult2 = await command2.ExecuteNonQueryAsync(cancellationToken);

                        using (var reader = await command2.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = new MonthlyRevenueDetailItem();
                                
                                #region DbDataReader To Model
                                item.MesaiKbn = reader[MesaiKbn] is DBNull ? 0 : (int)reader[MesaiKbn];
                                item.EigyoCd = reader[EigyoCd] is DBNull ? 0 : (int)reader[EigyoCd];
                                item.UriYmd = reader[UriYmd] is DBNull ? string.Empty : (string)reader[UriYmd];
                                item.YouRyakuNm = reader[YouRyakuNm] is DBNull ? string.Empty : (string)reader[YouRyakuNm];
                                item.YouSitRyakuNm = reader[YouSitRyakuNm] is DBNull ? string.Empty : (string)reader[YouSitRyakuNm];
                                item.YouSyaSyuDai = reader[YouSyaSyuDai] is DBNull ? 0 : (int)reader[YouSyaSyuDai];
                                item.YouG = reader[YouG] is DBNull ? 0 : (int)reader[YouG];
                                item.YouFutG = reader[YouFutG] is DBNull ? 0 : (int)reader[YouFutG];
                                item.YouSyaRyoUnc = reader[YouSyaRyoUnc] is DBNull ? 0 : (int)reader[YouSyaRyoUnc];
                                item.YouSyaRyoSyo = reader[YouSyaRyoSyo] is DBNull ? 0 : (int)reader[YouSyaRyoSyo];
                                item.YouSyaRyoTes = reader[YouSyaRyoTes] is DBNull ? 0 : (int)reader[YouSyaRyoTes];
                                item.YfuUriGakKin = reader[YfuUriGakKin] is DBNull ? 0 : (int)reader[YfuUriGakKin];
                                item.YfuSyaRyoSyo = reader[YfuSyaRyoSyo] is DBNull ? 0 : (int)reader[YfuSyaRyoSyo];
                                item.YfuSyaRyoTes = reader[YfuSyaRyoTes] is DBNull ? 0 : (int)reader[YfuSyaRyoTes];

                                item.YouGyosyaCd = reader[YouGyosyaCd] is DBNull ? 0 : (int)reader[YouGyosyaCd];
                                item.YouCd = reader[YouCd] is DBNull ? 0 : (int)reader[YouCd];
                                item.YouSitCd = reader[YouSitCd] is DBNull ? 0 : (int)reader[YouSitCd];
                                item.YouGyosyaNm = reader[YouGyosyaNm] is DBNull ? string.Empty : (string)reader[YouGyosyaNm];
                                item.YouNm = reader[YouNm] is DBNull ? string.Empty : (string)reader[YouNm];
                                item.YouSitenNm = reader[YouSitenNm] is DBNull ? string.Empty : (string)reader[YouSitenNm];
                                #endregion
                                if (item.MesaiKbn == 2 && summaryResult.Any())
                                {
                                    var sumPage = summaryResult.FirstOrDefault(sr => sr.MesaiKbn == 1);
                                    if(sumPage != null)
                                    {
                                        sumPage.SYoushaUnc = item.YouSyaRyoUnc;
                                        sumPage.SYoushaSyo = item.YouSyaRyoSyo;
                                        sumPage.SYoushaTes = item.YouSyaRyoTes;
                                        sumPage.SYfuUriGakKin = item.YfuUriGakKin;
                                        sumPage.SYfuSyaRyoSyo = item.YfuSyaRyoSyo;
                                        sumPage.SYfuSyaRyoTes = item.YfuSyaRyoTes;
                                    }
                                }
                                
                                detailItems.Add(item);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var item in listResult)
                    {
                        item.DetailItems = detailItems.Where(i => i.EigyoCd == item.EigyoCd && i.UriYmd == item.UriYmd && i.MesaiKbn == 3);
                        item.UntSoneki = item.KeiKin - item.JisSyaRyoSum - item.DetailItems.Sum(i => i.YouG);
                    }

                    revenueData.MonthlyRevenueItems = listResult.OrderBy(i => i.UriYmd).ToList();
                    revenueData.SummaryResult = summaryResult.OrderBy(i => i.MesaiKbn).ToList();
                    revenueData.DetailItems = detailItems;
                    return revenueData;
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

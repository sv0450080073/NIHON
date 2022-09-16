using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.RevenueSummary.Queries
{
    public class GetDailyRevenueData : IRequest<DailyRevenueData>
    {
        public DailyRevenueSearchModel SearchModel { get; set; }
        public class Handler : IRequestHandler<GetDailyRevenueData, DailyRevenueData>
        {
            private KobodbContext _kobodbContext;

            #region Column Names
            private static string MesaiKbn = "MesaiKbn";
            private static string CanSyoG = "CanSyoG";
            private static string CanSyoR = "CanSyoR";
            private static string CanUnc = "CanUnc";
            private static string CanSum = "CanSum";
            private static string YfuSyaRyoSyo = "YfuSyaRyoSyo";
            private static string YfuSyaRyoTes = "YfuSyaRyoTes";
            private static string YfuUriGakKin = "YfuUriGakKin";
            private static string JisSyaRyoSyo = "JisSyaRyoSyo";
            private static string JisSyaRyoSyoRit = "JisSyaRyoSyoRit";
            private static string JisSyaRyoTes = "JisSyaRyoTes";
            private static string JisSyaRyoTesRit = "JisSyaRyoTesRit";
            private static string JisSyaRyoUnc = "JisSyaRyoUnc";
            private static string JisSyaSyuDai = "JisSyaSyuDai";
            private static string DanTaNm = "DanTaNm";
            private static string EtcSyaRyoSum = "EtcSyaRyoSum";
            private static string GaiSyaRyoSum = "GaiSyaRyoSum";
            private static string YouSyaRyoSyo = "YouSyaRyoSyo";
            private static string YouSyaRyoTes = "YouSyaRyoTes";
            private static string YouSyaRyoUnc = "YouSyaRyoUnc";
            private static string YouTesuRitu = "YouTesuRitu";
            private static string YouZeiritsu = "YouZeiritsu";
            private static string GaiSyaRyoSyo = "GaiSyaRyoSyo";
            private static string GaiSyaRyoTes = "GaiSyaRyoTes";
            private static string GaiUriGakKin = "GaiUriGakKin";
            private static string IkNm = "IkNm";
            private static string JisSyaRyoSum = "JisSyaRyoSum";
            private static string KeiKin = "KeiKin";
            private static string Nissu = "Nissu";
            private static string EtcSyaRyoSyo = "EtcSyaRyoSyo";
            private static string EtcSyaRyoTes = "EtcSyaRyoTes";
            private static string EtcUriGakKin = "EtcUriGakKin";
            private static string SirRyakuNm = "SirRyakuNm";
            private static string SirSitRyakuNm = "SirSitRyakuNm";
            private static string SitRyakuNm = "SitRyakuNm";
            private static string TokRyakuNm = "TokRyakuNm";
            private static string UkeNo = "UkeNo";
            private static string UkeRyakuNm = "UkeRyakuNm";
            private static string YouFutG = "S_YfuSyaRyo";
            private static string YouG = "S_YouSyaRyo";
            private static string YouRyakuNm = "YouRyakuNm";
            private static string YouSitRyakuNm = "YouSitRyakuNm";
            private static string YouSyaSyuDai = "YouSyaSyuDai";
            private static string UnkRen = "UnkRen";
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

            private const string UriYmd = "UriYmd";
            private const string SeiEigyoCd = "SeiEigyoCd";
            private const string UkeEigyoCd = "UkeEigyoCd";
            private const string SeiEigyoNm = "SeiEigyoNm";
            private const string UkeEigyoNm = "UkeEigyoNm";
            private const string SeiEigyoRyak = "SeiEigyoRyak";
            private const string GyosyaCd = "GyosyaCd";
            private const string TokuiCd = "TokuiCd";
            private const string SitenCd = "SitenCd";
            private const string GyosyaNm = "GyosyaNm";
            private const string TokuiNm = "TokuiNm";
            private const string SitenNm = "SitenNm";
            private const string SirGyosyaCd = "SirGyosyaCd";
            private const string SirCd = "SirCd";
            private const string SirSitenCd = "SirSitenCd";
            private const string SirGyosyaNm = "SirGyosyaNm";
            private const string SirNm = "SirNm";
            private const string SirSitenNm = "SirSitenNm";

            private const string YouGyosyaCd = "YouGyosyaCd";
            private const string YouCd = "YouCd";
            private const string YouSitCd = "YouSitCd";
            private const string YouGyosyaNm = "YouGyosyaNm";
            private const string YouNm = "YouNm";
            private const string YouSitenNm = "YouSitenNm";
            private const string YouZeiKbn = "YouZeiKbn";
            private const string YouZKbnNm = "YouZKbnNm";
            private const string JisZeiKbn = "JisZeiKbn";
            private const string JisZeiKbnNm = "JisZeiKbnNm";
            private const string CanZKbn = "CanZKbn";
            private const string CanZKbnNm = "CanZKbnNm";
            private const string OtherUriGakKin = "OtherUriGakKin";
            private const string OtherSyaRyoSyo = "OtherSyaRyoSyo";
            private const string OtherSyaRyoTes = "OtherSyaRyoTes";
            private const string OtherSyaRyoSum = "OtherSyaRyoSum";
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
            #endregion
            
            public Handler(KobodbContext kobodbContext)
            {
                _kobodbContext = kobodbContext;
            }

            private async Task ExcuteQuery1(List<DailyRevenueItem> listResult, List<SummaryResult> summaryResult, GetDailyRevenueData request, DbConnection connection, CancellationToken cancellationToken)
            {
                using (var command1 = connection.CreateCommand())
                {
                    command1.CommandText = @$"EXECUTE PK_dDailyRevenueData_R 
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

                    if (string.IsNullOrEmpty(request.SearchModel.UriYmd))
                        command1.AddParam("@UriYmdTo", DBNull.Value);
                    else
                        command1.AddParam("@UriYmdTo", request.SearchModel.UriYmd.Replace("/", ""));

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
                            var item = new DailyRevenueItem();
                            #region DbDataReader to Model
                            item.MesaiKbn = reader[MesaiKbn] is DBNull ? 0 : (int)reader[MesaiKbn];
                            if (item.MesaiKbn == 3)
                            {
                                item.UkeNo = reader[UkeNo] is DBNull ? string.Empty : (string)reader[UkeNo];
                                if (!string.IsNullOrEmpty(item.UkeNo)) item.UkeNo = item.UkeNo.Substring(5); // Remove tenant Id

                                item.No = i++;
                                item.UriYmd = reader[UriYmd] is DBNull ? string.Empty : (string)reader[UriYmd];
                                item.SeiEigyoCd = reader[SeiEigyoCd] is DBNull ? 0 : (int)reader[SeiEigyoCd];
                                item.UkeEigyoCd = reader[UkeEigyoCd] is DBNull ? 0 : (int)reader[UkeEigyoCd];
                                item.SeiEigyoNm = reader[SeiEigyoNm] is DBNull ? string.Empty : (string)reader[SeiEigyoNm];
                                item.UkeEigyoNm = reader[UkeEigyoNm] is DBNull ? string.Empty : (string)reader[UkeEigyoNm];
                                item.SeiEigyoRyak = reader[SeiEigyoRyak] is DBNull ? string.Empty : (string)reader[SeiEigyoRyak];
                                item.GyosyaCd = reader[GyosyaCd] is DBNull ? 0 : (int)reader[GyosyaCd];
                                item.TokuiCd = reader[TokuiCd] is DBNull ? 0 : (int)reader[TokuiCd];
                                item.SitenCd = reader[SitenCd] is DBNull ? 0 : (int)reader[SitenCd];
                                item.GyosyaNm = reader[GyosyaNm] is DBNull ? string.Empty : (string)reader[GyosyaNm];
                                item.TokuiNm = reader[TokuiNm] is DBNull ? string.Empty : (string)reader[TokuiNm];
                                item.SitenNm = reader[SitenNm] is DBNull ? string.Empty : (string)reader[SitenNm];
                                item.SirGyosyaCd = reader[SirGyosyaCd] is DBNull ? 0 : (int)reader[SirGyosyaCd];
                                item.SirCd = reader[SirCd] is DBNull ? 0 : (int)reader[SirCd];
                                item.SirSitenCd = reader[SirSitenCd] is DBNull ? 0 : (int)reader[SirSitenCd];
                                item.SirGyosyaNm = reader[SirGyosyaNm] is DBNull ? string.Empty : (string)reader[SirGyosyaNm];
                                item.SirNm = reader[SirNm] is DBNull ? string.Empty : (string)reader[SirNm];
                                item.SirSitenNm = reader[SirSitenNm] is DBNull ? string.Empty : (string)reader[SirSitenNm];
                                item.JisZeiKbn = reader[JisZeiKbn] is DBNull ? 0 : (int)reader[JisZeiKbn];
                                item.JisZeiKbnNm = reader[JisZeiKbnNm] is DBNull ? string.Empty : (string)reader[JisZeiKbnNm];
                                item.OtherUriGakKin = reader[OtherUriGakKin] is DBNull ? 0 : (int)reader[OtherUriGakKin];
                                item.OtherSyaRyoSyo = reader[OtherSyaRyoSyo] is DBNull ? 0 : (int)reader[OtherSyaRyoSyo];
                                item.OtherSyaRyoTes = reader[OtherSyaRyoTes] is DBNull ? 0 : (int)reader[OtherSyaRyoTes];
                                item.OtherSyaRyoSum = reader[OtherSyaRyoSum] is DBNull ? 0 : (int)reader[OtherSyaRyoSum];

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


                                item.UkeRyakuNm = reader[UkeRyakuNm] is DBNull ? string.Empty : (string)reader[UkeRyakuNm];

                                item.SirRyakuNm = reader[SirRyakuNm] is DBNull ? string.Empty : (string)reader[SirRyakuNm];
                                item.SirSitRyakuNm = reader[SirSitRyakuNm] is DBNull ? string.Empty : (string)reader[SirSitRyakuNm];
                                item.SitRyakuNm = reader[SitRyakuNm] is DBNull ? string.Empty : (string)reader[SitRyakuNm];
                                item.TokRyakuNm = reader[TokRyakuNm] is DBNull ? string.Empty : (string)reader[TokRyakuNm];

                                item.DanTaNm = reader[DanTaNm] is DBNull ? string.Empty : (string)reader[DanTaNm];
                                item.IkNm = reader[IkNm] is DBNull ? string.Empty : (string)reader[IkNm];

                                item.Nissu = reader[Nissu] is DBNull ? 0 : (int)reader[Nissu];
                                item.KeiKin = reader[KeiKin] is DBNull ? 0 : (long)reader[KeiKin];

                                item.JisSyaRyoSum = reader[JisSyaRyoSum] is DBNull ? 0 : (long)reader[JisSyaRyoSum];
                                item.GaiSyaRyoSum = reader[GaiSyaRyoSum] is DBNull ? 0 : (int)reader[GaiSyaRyoSum];
                                item.EtcSyaRyoSum = reader[EtcSyaRyoSum] is DBNull ? 0 : (int)reader[EtcSyaRyoSum];
                                item.CanSum = reader[CanSum] is DBNull ? 0 : (int)reader[CanSum];

                                item.JisSyaRyoUnc = reader[JisSyaRyoUnc] is DBNull ? 0 : (long)reader[JisSyaRyoUnc];
                                item.JisSyaSyuDai = reader[JisSyaSyuDai] is DBNull ? 0 : (int)reader[JisSyaSyuDai];
                                item.JisSyaRyoSyoRit = reader[JisSyaRyoSyoRit] is DBNull ? 0 : (decimal)reader[JisSyaRyoSyoRit];
                                item.JisSyaRyoSyo = reader[JisSyaRyoSyo] is DBNull ? 0 : (long)reader[JisSyaRyoSyo];
                                item.JisSyaRyoTesRit = reader[JisSyaRyoTesRit] is DBNull ? 0 : (decimal)reader[JisSyaRyoTesRit];
                                item.JisSyaRyoTes = reader[JisSyaRyoTes] is DBNull ? 0 : (long)reader[JisSyaRyoTes];

                                item.GaiUriGakKin = reader[GaiUriGakKin] is DBNull ? 0 : (int)reader[GaiUriGakKin];
                                item.GaiSyaRyoSyo = reader[GaiSyaRyoSyo] is DBNull ? 0 : (int)reader[GaiSyaRyoSyo];
                                item.GaiSyaRyoTes = reader[GaiSyaRyoTes] is DBNull ? 0 : (int)reader[GaiSyaRyoTes];

                                item.EtcUriGakKin = reader[EtcUriGakKin] is DBNull ? 0 : (int)reader[EtcUriGakKin];
                                item.EtcSyaRyoSyo = reader[EtcSyaRyoSyo] is DBNull ? 0 : (int)reader[EtcSyaRyoSyo];
                                item.EtcSyaRyoTes = reader[EtcSyaRyoTes] is DBNull ? 0 : (int)reader[EtcSyaRyoTes];
                                item.CanZKbn = reader[CanZKbn] is DBNull ? 0 : (int)reader[CanZKbn];
                                item.CanZKbnNm = reader[CanZKbnNm] is DBNull ? string.Empty : (string)reader[CanZKbnNm];

                                item.CanUnc = reader[CanUnc] is DBNull ? 0 : (int)reader[CanUnc];
                                item.CanSyoR = reader[CanSyoR] is DBNull ? 0 : (decimal)reader[CanSyoR];
                                item.CanSyoG = reader[CanSyoG] is DBNull ? 0 : (int)reader[CanSyoG];

                                item.UnkRen = reader[UnkRen] is DBNull ? 0 : (int)reader[UnkRen];
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
            }

            private async Task ExecuteQuery2(List<DailyRevenueDetailItem> detailItems, GetDailyRevenueData request, DbConnection connection, CancellationToken cancellationToken)
            {
                using (var command2 = connection.CreateCommand())
                {
                    command2.CommandText = @$"EXECUTE PK_dDailyRevenueDetailItems_R
                                                @CompanyCd,
		                                        @UkeNoFrom,
		                                        @UkeNoTo,
		                                        @YoyaKbnFrom,
		                                        @YoyaKbnTo,
		                                        @TenantCdSeq,
		                                        @EigyoKbn,
		                                        @TesuInKbn,
		                                        @EigyoCdSeq,
		                                        @UriYmdFrom,
                                                @UriYmdTo,
                                                @ROWCOUNT OUTPUT";

                    #region Add Params For Command 2
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

                    if (string.IsNullOrEmpty(request.SearchModel.RevenueSearchModel.UriYmdFrom))
                        command2.AddParam("@UriYmdFrom", DBNull.Value);
                    else
                        command2.AddParam("@UriYmdFrom", request.SearchModel.RevenueSearchModel.UriYmdFrom);

                    if (string.IsNullOrEmpty(request.SearchModel.UriYmd))
                        command2.AddParam("@UriYmdTo", DBNull.Value);
                    else
                        command2.AddParam("@UriYmdTo", request.SearchModel.UriYmd.Replace("/", ""));

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
                            var item = new DailyRevenueDetailItem();
                            #region DbDataReader to Model
                            item.MesaiKbn = reader[MesaiKbn] is DBNull ? 0 : (int)reader[MesaiKbn];
                            item.UkeNo = reader[UkeNo] is DBNull ? string.Empty : (string)reader[UkeNo];
                            if (!string.IsNullOrEmpty(item.UkeNo)) item.UkeNo = item.UkeNo.Substring(5); // Remove tenant Id
                            item.UnkRen = reader[UnkRen] is DBNull ? 0 : (int)reader[UnkRen];
                            item.YouSyaRyoUnc = reader[YouSyaRyoUnc] is DBNull ? 0 : (int)reader[YouSyaRyoUnc];
                            item.YouZeiritsu = reader[YouZeiritsu] is DBNull ? 0 : (decimal)reader[YouZeiritsu];
                            item.YouSyaRyoSyo = reader[YouSyaRyoSyo] is DBNull ? 0 : (int)reader[YouSyaRyoSyo];
                            item.YouTesuRitu = reader[YouTesuRitu] is DBNull ? 0 : (decimal)reader[YouTesuRitu];
                            item.YouSyaRyoTes = reader[YouSyaRyoTes] is DBNull ? 0 : (int)reader[YouSyaRyoTes];
                            item.YfuUriGakKin = reader[YfuUriGakKin] is DBNull ? 0 : (int)reader[YfuUriGakKin];
                            item.YfuSyaRyoSyo = reader[YfuSyaRyoSyo] is DBNull ? 0 : (int)reader[YfuSyaRyoSyo];
                            item.YfuSyaRyoTes = reader[YfuSyaRyoTes] is DBNull ? 0 : (int)reader[YfuSyaRyoTes];
                            item.YouRyakuNm = reader[YouRyakuNm] is DBNull ? string.Empty : (string)reader[YouRyakuNm];
                            item.YouSitRyakuNm = reader[YouSitRyakuNm] is DBNull ? string.Empty : (string)reader[YouSitRyakuNm];
                            item.YouSyaSyuDai = reader[YouSyaSyuDai] is DBNull ? 0 : (int)reader[YouSyaSyuDai];
                            item.YouG = reader[YouG] is DBNull ? 0 : (int)reader[YouG];
                            item.YouFutG = reader[YouFutG] is DBNull ? 0 : (int)reader[YouFutG];

                            item.YouCd = reader[YouCd] is DBNull ? 0 : (int)reader[YouCd];
                            item.YouSitCd = reader[YouSitCd] is DBNull ? 0 : (int)reader[YouSitCd];
                            item.YouGyosyaNm = reader[YouGyosyaNm] is DBNull ? string.Empty : (string)reader[YouGyosyaNm];
                            item.YouNm = reader[YouNm] is DBNull ? string.Empty : (string)reader[YouNm];
                            item.YouSitenNm = reader[YouSitenNm] is DBNull ? string.Empty : (string)reader[YouSitenNm];
                            item.YouZeiKbn = reader[YouZeiKbn] is DBNull ? 0 : (int)reader[YouZeiKbn];
                            item.YouZKbnNm = reader[YouZKbnNm] is DBNull ? string.Empty : (string)reader[YouZKbnNm];
                            item.YouGyosyaCd = reader[YouGyosyaCd] is DBNull ? 0 : (int)reader[YouGyosyaCd];
                            #endregion
                            detailItems.Add(item);
                        }

                        await reader.CloseAsync();
                    }
                }
            }

            /// <summary>
            /// Get Daily Transportation Revenue data
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task<DailyRevenueData> Handle(GetDailyRevenueData request, CancellationToken cancellationToken)
            {
                var revenueData = new DailyRevenueData();
                var listResult = new List<DailyRevenueItem>();
                var summaryResult = new List<SummaryResult>();
                var detailItems = new List<DailyRevenueDetailItem>();

                if (request == null || request.SearchModel == null || request.SearchModel.RevenueSearchModel == null) return revenueData;

                var connection = _kobodbContext.Database.GetDbConnection();
                try
                {
                    await connection.OpenAsync();

                    await ExcuteQuery1(listResult, summaryResult, request, connection, cancellationToken);
                    await ExecuteQuery2(detailItems, request, connection, cancellationToken);

                    foreach (var item in listResult)
                    {
                        item.DetailItems = detailItems.Where(i => i.UkeNo == item.UkeNo && i.UnkRen == item.UnkRen && i.MesaiKbn == 3);
                        item.UntSoneki = item.KeiKin - item.JisSyaRyoSum - item.DetailItems.Sum(i => i.YouG);
                    }

                    revenueData.DailyRevenueItems = listResult;
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

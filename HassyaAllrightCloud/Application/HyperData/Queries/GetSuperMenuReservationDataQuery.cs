using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.HyperData.Queries
{
    public class GetSuperMenuReservationDataQuery : IRequest<List<SuperMenuReservationData>>
    {
        public HyperFormData hyperData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public int fetch { get; set; }
        public int? offSet { get; set; } = null;
        public class Handler : IRequestHandler<GetSuperMenuReservationDataQuery, List<SuperMenuReservationData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public void CreateParameter(DbCommand command, string procedureName, HyperFormData hyperData, int CompanyId, int TenantId)
            {
                command.CommandText = "EXECUTE " + procedureName + " " +
                        " @TenantCdSeq," +
                        " @CompanyCdSeq," +
                        " @StartDispatchDate," +
                        " @EndDispatchDate," +
                        " @StartArrivalDate," +
                        " @EndArrivalDate," +
                        " @StartReservationDate," +
                        " @EndReservationDate," +
                        " @StartReceiptNumber," +
                        " @EndReceiptNumber," +
                        " @StartReservationClassification," +
                        " @EndReservationClassification," +
                        " @StartServicePerson," +
                        " @EndServicePerson," +
                        " @StartRegistrationOffice," +
                        " @EndRegistrationOffice," +
                        " @StartInputPerson," +
                        " @EndInputPerson," +
                        " @StartCustomer," +
                        " @EndCustomer," +
                        " @StartSupplier," +
                        " @EndSupplier," +
                        " @StartGroupClassification," +
                        " @EndGroupClassification, " +
                        " @StartCustomerTypeClassification," +
                        " @EndCustomerTypeClassification," +
                        " @StartDestination," +
                        " @EndDestination," +
                        " @StartDispatchPlace," +
                        " @EndDispatchPlace," +
                        " @StartOccurrencePlace," +
                        " @EndOccurrencePlace," +
                        " @StartArea," +
                        " @EndArea," +
                        " @StartReceiptCondition," +
                        " @EndReceiptCondition," +
                        " @StartCarType," +
                        " @EndCarType," +
                        " @StartCarTypePrice," +
                        " @EndCarTypePrice," +
                        " @DantaNm," +
                        " @MaxMinSetting," +
                        " @ReservationStatus";
                List<DbParameter> Parameters = new List<DbParameter>();

                // ログインユーザのテナントID
                DbParameter parameter0 = command.CreateParameter();
                parameter0.ParameterName = "@TenantCdSeq";
                parameter0.Value = TenantId;
                Parameters.Add(parameter0);

                // ログインユーザの会社のCompanyCdSeq
                DbParameter parameter1 = command.CreateParameter();
                parameter1.ParameterName = "@CompanyCdSeq";
                parameter1.Value = CompanyId;
                Parameters.Add(parameter1);

                // 配車日
                DbParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "@StartDispatchDate";
                if (hyperData.HaishaBiFrom == null)
                {
                    parameter2.Value = DBNull.Value;
                }
                else
                {
                    parameter2.Value = ((DateTime)hyperData.HaishaBiFrom).ToString("yyyyMMdd");
                }
                DbParameter parameter3 = command.CreateParameter();
                parameter3.ParameterName = "@EndDispatchDate";
                if (hyperData.HaishaBiTo == null)
                {
                    parameter3.Value = DBNull.Value;
                }
                else
                {
                    parameter3.Value = ((DateTime)hyperData.HaishaBiTo).ToString("yyyyMMdd");
                }
                Parameters.Add(parameter2);
                Parameters.Add(parameter3);

                // 到着日
                DbParameter parameter4 = command.CreateParameter();
                parameter4.ParameterName = "@StartArrivalDate";
                if (hyperData.TochakuBiFrom == null)
                {
                    parameter4.Value = DBNull.Value;
                }
                else
                {
                    parameter4.Value = ((DateTime)hyperData.TochakuBiFrom).ToString("yyyyMMdd");
                }
                DbParameter parameter5 = command.CreateParameter();
                parameter5.ParameterName = "@EndArrivalDate";
                if (hyperData.TochakuBiTo == null)
                {
                    parameter5.Value = DBNull.Value;
                }
                else
                {
                    parameter5.Value = ((DateTime)hyperData.TochakuBiTo).ToString("yyyyMMdd");
                }
                Parameters.Add(parameter4);
                Parameters.Add(parameter5);

                // 予約日
                DbParameter parameter6 = command.CreateParameter();
                parameter6.ParameterName = "@StartReservationDate";
                if (hyperData.YoyakuBiFrom == null)
                {
                    parameter6.Value = DBNull.Value;
                }
                else
                {
                    parameter6.Value = ((DateTime)hyperData.YoyakuBiFrom).ToString("yyyyMMdd");
                }
                DbParameter parameter7 = command.CreateParameter();
                parameter7.ParameterName = "@EndReservationDate";
                if (hyperData.YoyakuBiTo == null)
                {
                    parameter7.Value = DBNull.Value;
                }
                else
                {
                    parameter7.Value = ((DateTime)hyperData.YoyakuBiTo).ToString("yyyyMMdd");
                }
                Parameters.Add(parameter6);
                Parameters.Add(parameter7);

                // 受付番号
                DbParameter parameter8 = command.CreateParameter();
                parameter8.ParameterName = "@StartReceiptNumber";
                if (string.IsNullOrEmpty(hyperData.UketsukeBangoFrom))
                {
                    parameter8.Value = DBNull.Value;
                }
                else
                {
                    parameter8.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(hyperData.UketsukeBangoFrom);
                }
                DbParameter parameter9 = command.CreateParameter();
                parameter9.ParameterName = "@EndReceiptNumber";
                if (string.IsNullOrEmpty(hyperData.UketsukeBangoTo))
                {
                    parameter9.Value = DBNull.Value;
                }
                else
                {
                    parameter9.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(hyperData.UketsukeBangoTo);
                }
                Parameters.Add(parameter8);
                Parameters.Add(parameter9);

                // 予約区分
                DbParameter parameter10 = command.CreateParameter();
                parameter10.ParameterName = "@StartReservationClassification";
                if (hyperData.YoyakuFrom == null)
                {
                    parameter10.Value = DBNull.Value;
                }
                else
                {
                    parameter10.Value = hyperData.YoyakuFrom.YoyaKbn;
                }
                DbParameter parameter11 = command.CreateParameter();
                parameter11.ParameterName = "@EndReservationClassification";
                if (hyperData.YoyakuTo == null)
                {
                    parameter11.Value = DBNull.Value;
                }
                else
                {
                    parameter11.Value = hyperData.YoyakuTo.YoyaKbn;
                }
                Parameters.Add(parameter10);
                Parameters.Add(parameter11);

                // 営業担当
                DbParameter parameter12 = command.CreateParameter();
                parameter12.ParameterName = "@StartServicePerson";
                if (hyperData.EigyoTantoShaFrom == null)
                {
                    parameter12.Value = DBNull.Value;
                }
                else
                {
                    parameter12.Value = hyperData.EigyoTantoShaFrom.SyainCd;
                }
                DbParameter parameter13 = command.CreateParameter();
                parameter13.ParameterName = "@EndServicePerson";
                if (hyperData.EigyoTantoShaTo == null)
                {
                    parameter13.Value = DBNull.Value;
                }
                else
                {
                    parameter13.Value = hyperData.EigyoTantoShaTo.SyainCd;
                }
                Parameters.Add(parameter12);
                Parameters.Add(parameter13);

                // 受付営業所
                DbParameter parameter14 = command.CreateParameter();
                parameter14.ParameterName = "@StartRegistrationOffice";
                if (hyperData.UketsukeEigyoJoFrom == null)
                {
                    parameter14.Value = DBNull.Value;
                }
                else
                {
                    parameter14.Value = hyperData.UketsukeEigyoJoFrom.EigyoCd;
                }
                DbParameter parameter15 = command.CreateParameter();
                parameter15.ParameterName = "@EndRegistrationOffice";
                if (hyperData.UketsukeEigyoJoTo == null)
                {
                    parameter15.Value = DBNull.Value;
                }
                else
                {
                    parameter15.Value = hyperData.UketsukeEigyoJoTo.EigyoCd;
                }
                Parameters.Add(parameter14);
                Parameters.Add(parameter15);

                // 入力担当
                DbParameter parameter16 = command.CreateParameter();
                parameter16.ParameterName = "@StartInputPerson";
                if (hyperData.NyuryokuTantoShaFrom == null)
                {
                    parameter16.Value = DBNull.Value;
                }
                else
                {
                    parameter16.Value = hyperData.NyuryokuTantoShaFrom.SyainCd;
                }
                DbParameter parameter17 = command.CreateParameter();
                parameter17.ParameterName = "@EndInputPerson";
                if (hyperData.NyuryokuTantoShaTo == null)
                {
                    parameter17.Value = DBNull.Value;
                }
                else
                {
                    parameter17.Value = hyperData.NyuryokuTantoShaTo.SyainCd;
                }
                Parameters.Add(parameter16);
                Parameters.Add(parameter17);

                // 得意先
                DbParameter parameter18 = command.CreateParameter();
                parameter18.ParameterName = "@StartCustomer";
                if (hyperData.GyosyaTokuiSakiFrom == null)
                {
                    parameter18.Value = DBNull.Value;
                }
                else
                {
                    parameter18.Value = hyperData.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3") + (hyperData.TokiskTokuiSakiFrom == null ? "0000" : hyperData.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")) + (hyperData.TokiStTokuiSakiFrom == null ? "0000" : hyperData.TokiStTokuiSakiFrom.SitenCd.ToString("D4"));
                }

                DbParameter parameter19 = command.CreateParameter();
                parameter19.ParameterName = "@EndCustomer";
                if (hyperData.GyosyaTokuiSakiTo == null)
                {
                    parameter19.Value = DBNull.Value;
                }
                else
                {
                    parameter19.Value = hyperData.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3") + (hyperData.TokiskTokuiSakiTo == null ? "9999" : hyperData.TokiskTokuiSakiTo.TokuiCd.ToString("D4")) + (hyperData.TokiStTokuiSakiTo == null ? "9999" : hyperData.TokiStTokuiSakiTo.SitenCd.ToString("D4"));
                }
                Parameters.Add(parameter18);
                Parameters.Add(parameter19);

                // 仕入先
                DbParameter parameter20 = command.CreateParameter();
                parameter20.ParameterName = "@StartSupplier";
                if (hyperData.GyosyaShiireSakiFrom == null)
                {
                    parameter20.Value = DBNull.Value;
                }
                else
                {
                    parameter20.Value = hyperData.GyosyaShiireSakiFrom.GyosyaCd.ToString("D3") + (hyperData.TokiskShiireSakiFrom == null ? "0000" : hyperData.TokiskShiireSakiFrom.TokuiCd.ToString("D4")) + (hyperData.TokiStShiireSakiFrom == null ? "0000" : hyperData.TokiStShiireSakiFrom.SitenCd.ToString("D4"));
                }
                DbParameter parameter21 = command.CreateParameter();
                parameter21.ParameterName = "@EndSupplier";
                if (hyperData.GyosyaShiireSakiTo == null)
                {
                    parameter21.Value = DBNull.Value;
                }
                else
                {
                    parameter21.Value = hyperData.GyosyaShiireSakiTo.GyosyaCd.ToString("D3") + (hyperData.TokiskShiireSakiTo == null ? "9999" : hyperData.TokiskShiireSakiTo.TokuiCd.ToString("D4")) + (hyperData.TokiStShiireSakiTo == null ? "9999" : hyperData.TokiStShiireSakiTo.SitenCd.ToString("D4"));
                }
                Parameters.Add(parameter20);
                Parameters.Add(parameter21);

                // 団体区分
                DbParameter parameter22 = command.CreateParameter();
                parameter22.ParameterName = "@StartGroupClassification";
                if (hyperData.DantaiKbnFrom == null)
                {
                    parameter22.Value = DBNull.Value;
                }
                else
                {
                    parameter22.Value = hyperData.DantaiKbnFrom.CodeKbn;
                }
                DbParameter parameter23 = command.CreateParameter();
                parameter23.ParameterName = "@EndGroupClassification";
                if (hyperData.DantaiKbnTo == null)
                {
                    parameter23.Value = DBNull.Value;
                }
                else
                {
                    parameter23.Value = hyperData.DantaiKbnTo.CodeKbn;
                }
                Parameters.Add(parameter22);
                Parameters.Add(parameter23);

                // 客種区分
                DbParameter parameter24 = command.CreateParameter();
                parameter24.ParameterName = "@StartCustomerTypeClassification";
                if (hyperData.KyakuDaneKbnFrom == null)
                {
                    parameter24.Value = DBNull.Value;
                }
                else
                {
                    parameter24.Value = hyperData.KyakuDaneKbnFrom.JyoKyakuCd;
                }
                DbParameter parameter25 = command.CreateParameter();
                parameter25.ParameterName = "@EndCustomerTypeClassification";
                if (hyperData.KyakuDaneKbnTo == null)
                {
                    parameter25.Value = DBNull.Value;
                }
                else
                {
                    parameter25.Value = hyperData.KyakuDaneKbnTo.JyoKyakuCd;
                }
                Parameters.Add(parameter24);
                Parameters.Add(parameter25);

                // 行先
                DbParameter parameter26 = command.CreateParameter();
                parameter26.ParameterName = "@StartDestination";
                if (hyperData.YukiSakiFrom == null)
                {
                    parameter26.Value = DBNull.Value;
                }
                else
                {
                    parameter26.Value = hyperData.YukiSakiFrom.CodeKbn + hyperData.YukiSakiFrom.BasyoMapCd;
                }
                DbParameter parameter27 = command.CreateParameter();
                parameter27.ParameterName = "@EndDestination";
                if (hyperData.YukiSakiTo == null)
                {
                    parameter27.Value = DBNull.Value;
                }
                else
                {
                    parameter27.Value = hyperData.YukiSakiTo.CodeKbn + hyperData.YukiSakiTo.BasyoMapCd;
                }
                Parameters.Add(parameter26);
                Parameters.Add(parameter27);

                // 配車地
                DbParameter parameter28 = command.CreateParameter();
                parameter28.ParameterName = "@StartDispatchPlace";
                if (hyperData.HaishaChiFrom == null)
                {
                    parameter28.Value = DBNull.Value;
                }
                else
                {
                    parameter28.Value = hyperData.HaishaChiFrom.CodeKbn + hyperData.HaishaChiFrom.HaiSCd;
                }
                DbParameter parameter29 = command.CreateParameter();
                parameter29.ParameterName = "@EndDispatchPlace";
                if (hyperData.HaishaChiTo == null)
                {
                    parameter29.Value = DBNull.Value;
                }
                else
                {
                    parameter29.Value = hyperData.HaishaChiTo.CodeKbn + hyperData.HaishaChiTo.HaiSCd;
                }
                Parameters.Add(parameter28);
                Parameters.Add(parameter29);

                // 発生地
                DbParameter parameter30 = command.CreateParameter();
                parameter30.ParameterName = "@StartOccurrencePlace";
                if (hyperData.HasseiChiFrom == null)
                {
                    parameter30.Value = DBNull.Value;
                }
                else
                {
                    parameter30.Value = hyperData.HasseiChiFrom.CodeKbn + hyperData.HasseiChiFrom.BasyoMapCd;
                }
                DbParameter parameter31 = command.CreateParameter();
                parameter31.ParameterName = "@EndOccurrencePlace";
                if (hyperData.HasseiChiTo == null)
                {
                    parameter31.Value = DBNull.Value;
                }
                else
                {
                    parameter31.Value = hyperData.HasseiChiTo.CodeKbn + hyperData.HasseiChiTo.BasyoMapCd;
                }
                Parameters.Add(parameter30);
                Parameters.Add(parameter31);

                // エリア
                DbParameter parameter32 = command.CreateParameter();
                parameter32.ParameterName = "@StartArea";
                if (hyperData.AreaFrom == null)
                {
                    parameter32.Value = DBNull.Value;
                }
                else
                {
                    parameter32.Value = hyperData.AreaFrom.CodeKbn + hyperData.AreaFrom.BasyoMapCd;
                }
                DbParameter parameter33 = command.CreateParameter();
                parameter33.ParameterName = "@EndArea";
                if (hyperData.AreaTo == null)
                {
                    parameter33.Value = DBNull.Value;
                }
                else
                {
                    parameter33.Value = hyperData.AreaTo.CodeKbn + hyperData.AreaTo.BasyoMapCd;
                }
                Parameters.Add(parameter32);
                Parameters.Add(parameter33);

                // 受付条件
                DbParameter parameter34 = command.CreateParameter();
                parameter34.ParameterName = "@StartReceiptCondition";
                if (hyperData.UketsukeJokenFrom == null)
                {
                    parameter34.Value = DBNull.Value;
                }
                else
                {
                    parameter34.Value = hyperData.UketsukeJokenFrom.CodeKbn;
                }
                DbParameter parameter35 = command.CreateParameter();
                parameter35.ParameterName = "@EndReceiptCondition";
                if (hyperData.UketsukeJokenTo == null)
                {
                    parameter35.Value = DBNull.Value;
                }
                else
                {
                    parameter35.Value = hyperData.UketsukeJokenTo.CodeKbn;
                }
                Parameters.Add(parameter34);
                Parameters.Add(parameter35);

                // 車種
                DbParameter parameter36 = command.CreateParameter();
                parameter36.ParameterName = "@StartCarType";
                if (hyperData.ShashuFrom == null)
                {
                    parameter36.Value = DBNull.Value;
                }
                else
                {
                    parameter36.Value = hyperData.ShashuFrom.SyaSyuCd;
                }
                DbParameter parameter37 = command.CreateParameter();
                parameter37.ParameterName = "@EndCarType";
                if (hyperData.ShashuTo == null)
                {
                    parameter37.Value = DBNull.Value;
                }
                else
                {
                    parameter37.Value = hyperData.ShashuTo.SyaSyuCd;
                }
                Parameters.Add(parameter36);
                Parameters.Add(parameter37);

                // 車種単価
                DbParameter parameter38 = command.CreateParameter();
                parameter38.ParameterName = "@StartCarTypePrice";
                if (string.IsNullOrEmpty(hyperData.ShashuTankaFrom))
                {
                    parameter38.Value = DBNull.Value;
                }
                else
                {
                    parameter38.Value = int.Parse(hyperData.ShashuTankaFrom);
                }
                DbParameter parameter39 = command.CreateParameter();
                parameter39.ParameterName = "@EndCarTypePrice";
                if (string.IsNullOrEmpty(hyperData.ShashuTankaTo))
                {
                    parameter39.Value = DBNull.Value;
                }
                else
                {
                    parameter39.Value = int.Parse(hyperData.ShashuTankaTo);
                }
                Parameters.Add(parameter38);
                Parameters.Add(parameter39);

                // P.M.Nhat add new parameter 2020/09/04
                DbParameter parameter40 = command.CreateParameter();
                parameter40.ParameterName = "@DantaNm";
                if (string.IsNullOrEmpty(hyperData.DantaiNm))
                {
                    parameter40.Value = DBNull.Value;
                }
                else
                {
                    parameter40.Value = hyperData.DantaiNm;
                }
                Parameters.Add(parameter40);

                DbParameter parameter41 = command.CreateParameter();
                parameter41.ParameterName = "@MaxMinSetting";
                if (hyperData.MaxMinSetting == null)
                {
                    parameter41.Value = DBNull.Value;
                }
                else
                {
                    parameter41.Value = hyperData.MaxMinSetting.IdValue;
                }
                Parameters.Add(parameter41);

                DbParameter parameter42 = command.CreateParameter();
                parameter42.ParameterName = "@ReservationStatus";
                if (hyperData.ReservationStatus == null)
                {
                    parameter42.Value = DBNull.Value;
                }
                else
                {
                    parameter42.Value = hyperData.ReservationStatus.IdValue;
                }
                Parameters.Add(parameter42);

                foreach (var parameter in Parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            public string[] Mark(string KaktYmd, byte KaknKais, byte NyuKinKbn)
            {
                List<string> result = new List<string>();
                if (KaktYmd != null && KaktYmd.Trim().Length > 0)
                {
                    result.Add("Sure");
                }
                else if (KaknKais > 0)
                {
                    result.Add("Approval");
                }
                if (NyuKinKbn > 1)
                {
                    result.Add("Done");
                }
                return result.ToArray();
            }
            public DateTime? DateDisplayValue(string Ymd)
            {
                DateTime DateValue;
                string DateFormat = "yyyyMMdd";
                if (!DateTime.TryParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue))
                {
                    return null;
                }
                else
                {
                    return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture);
                }
            }
            public string DateAreaDisplayValue(string Ymd, string area)
            {
                DateTime DateValue;
                string DateFormat = "yyyyMMdd";
                if (!DateTime.TryParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue))
                {
                    return area;
                }
                else
                {
                    return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture).ToString("yyyy/MM/dd (ddd)") + ' ' + area;
                }
            }
            public async Task<List<SuperMenuReservationData>> Handle(GetSuperMenuReservationDataQuery request, CancellationToken cancellationToken)
            {
                List<SuperMenuReservationData> list = new List<SuperMenuReservationData>();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    CreateParameter(command, "PK_SpMnYyk_R", request.hyperData, request.companyId, request.tenantId);
                    command.CommandText += ","
                        + " @OffSet,"
                        + " @Fetch WITH RECOMPILE";
                    DbParameter OffsetParam = command.CreateParameter();
                    OffsetParam.ParameterName = "@OffSet";
                    if (request.offSet == null)
                    {
                        OffsetParam.Value = DBNull.Value;
                    }
                    else
                    {
                        OffsetParam.Value = request.offSet;
                    }

                    DbParameter FetchParam = command.CreateParameter();
                    FetchParam.ParameterName = "@Fetch";
                    FetchParam.Value = request.fetch;
                    command.Parameters.Add(OffsetParam);
                    command.Parameters.Add(FetchParam);
                    _dbContext.Database.OpenConnection();
                    if (request.offSet == null)
                    {
                        var totalRow = command.ExecuteReader();
                        if (totalRow.Read())
                        {
                            int Count = (int)totalRow["count"];
                            if (Count > 0)
                            {
                                list = new List<SuperMenuReservationData>(new SuperMenuReservationData[Count]);
                            }
                        }
                        totalRow.Close();
                        _dbContext.Database.CloseConnection();
                        return await Task.FromResult(list);
                    }
                    else
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SuperMenuReservationData
                                {
                                    UkeNo = (string)reader["UkeNo"],
                                    UnkRen = (short)reader["UnkRen"],
                                    Symbol = Mark((string)reader["KaktYmd"], (byte)reader["KaknKais"], (byte)reader["NyuKinKbn"]),
                                    Customer = (string)reader["TokuiRyakuNm"],
                                    Branch = (string)reader["SitenRyakuNm"],
                                    PersonInCharge = (string)reader["TokuiTanNm"],
                                    Organization = (string)reader["DanTaNm"],
                                    Secretary = (string)reader["KanJNm"],
                                    Destination = (string)reader["IkNm"],
                                    Dispatch = DateAreaDisplayValue((string)reader["HaiSYmd"], (string)reader["HaiSNm"]),
                                    Arrival = DateAreaDisplayValue((string)reader["TouYmd"], (string)reader["TouNm"]),
                                    NumberOfDrivers = (short)reader["DrvJin"],
                                    NumberOfGuides = (short)reader["GuiSu"],
                                    BusesInfo = new List<BusInfo>(),
                                    Fare = (int)reader["SyaRyoUnc"],
                                    TaxRate = (decimal)reader["Zeiritsu"],
                                    FeeTaxRate = (decimal)reader["TesuRitu"],
                                    FareTax = (int)reader["SyaRyoSyo"],
                                    FareFee = (int)reader["SyaRyoTes"],
                                    CompanyFare = (int)reader["JiSyaRyoUnc"],
                                    CompanyFareTax = (int)reader["JiSyaRyoSyo"],
                                    CompanyFareFee = (int)reader["JiSyaRyoTes"],
                                    MaxFee = (int)reader["UnitPriceMaxAmount"],
                                    MinFee = (int)reader["UnitPriceMinAmount"],
                                    // NTLanAnh add 2020/12/07
                                    UnitPriceIndex = (decimal)reader["UnitPriceIndex"],
                                    Guide = (int)reader["Gui_UriGakKin_S"],
                                    GuideTax = (int)reader["Gui_SyaRyoSyo_S"],
                                    GuideFee = (int)reader["Gui_SyaRyoTes_S"],
                                    Other = (int)reader["Fut_UriGakKin_S"] + (int)reader["Tuk_UriGakKin_S"] + (int)reader["Teh_UriGakKin_S"] + (int)reader["Tum_UriGakKin_S"],
                                    OtherTax = (int)reader["Fut_SyaRyoSyo_S"] + (int)reader["Tuk_SyaRyoSyo_S"] + (int)reader["Teh_SyaRyoSyo_S"] + (int)reader["Tum_SyaRyoSyo_S"],
                                    OtherFee = (int)reader["Fut_SyaRyoTes_S"] + (int)reader["Tuk_SyaRyoTes_S"] + (int)reader["Teh_SyaRyoTes_S"] + (int)reader["Tum_SyaRyoTes_S"],
                                    NumberOfHiredBus = (int)reader["YoushaDai"],
                                    HiredBusFare = (int)reader["YoushaUnc"],
                                    HiredBusFareTax = (int)reader["YoushaSyo"],
                                    HiredBusFareFee = (int)reader["YoushaTes"],
                                    HiredBusGuide = (int)reader["YGui_HaseiKin_S"],
                                    HiredBusGuideTax = (int)reader["YGui_SyaRyoSyo_S"],
                                    HiredBusGuideFee = (int)reader["YGui_SyaRyoTes_S"],
                                    HiredBusOther = (int)reader["Yfut_HaseiKin_S"] + (int)reader["Ytuk_HaseiKin_S"] + (int)reader["Yteh_HaseiKin_S"] + (int)reader["Ytum_HaseiKin_S"],
                                    HiredBusOtherTax = (int)reader["Yfut_SyaRyoSyo_S"] + (int)reader["Ytuk_SyaRyoSyo_S"] + (int)reader["Yteh_SyaRyoSyo_S"] + (int)reader["Ytum_SyaRyoSyo_S"],
                                    HiredBusOtherFee = (int)reader["Yfut_SyaRyoTes_S"] + (int)reader["Ytuk_SyaRyoTes_S"] + (int)reader["Yteh_SyaRyoTes_S"] + (int)reader["Ytum_SyaRyoTes_S"],
                                    Person = (short)reader["JyoSyaJin"],
                                    PersonPlus = (short)reader["PlusJin"],
                                    BillClassification = (string)reader["SeiKyuKbn_RyakuNm"],
                                    BillDate = (string)reader["SeiTaiYmd"],
                                    BillOutputDate = (string)reader["SeiHatYmd"],
                                    DeliveryReceiptOutputDate = (string)reader["UnsoHikiYmd"],
                                    ReceptOffice = (string)reader["UkeEig_RyakuNm"],
                                    SalesStaff = (string)reader["EigTan_SyainNm"],
                                    InputStaff = (string)reader["InputTan_SyainNm"],
                                    ReserveClassification = (string)reader["YoyaKbn_YoyaKbnNm"],
                                    ReceptionDate = (string)reader["UkeYmd"],
                                    ReceiptNumber = ((int)reader["UkeCd"]).ToString("D10") + "-" + ((short)reader["UnkRen"]).ToString(),
                                    Journey = (int)reader["CntInS_Kotei"] > 0,
                                    Load = (int)reader["CntInS_Tum"] > 0,
                                    Arrange = (int)reader["CntInS_Tehai"] > 0,
                                    Remarks = (int)reader["CntInS_Biko"] > 0,
                                    Incidental = (int)reader["CntInS_Fut"] > 0,
                                    SihKbn = (byte)reader["SihKbn"],
                                    SCouKbn = (byte)reader["SCouKbn"],
                                    NyuKinKbn = (byte)reader["NyuKinKbn"],
                                    NCouKbn = (byte)reader["NCouKbn"],
                                    NippoKbn = (byte)reader["NippoKbn"],
                                    YouKbn = (byte)reader["YouKbn"],
                                    HaiSKbn = (byte)reader["HaiSKbn"],
                                    HaiIKbn = (byte)reader["HaiIKbn"],
                                    GuiWNin = (short)reader["GuiWNin"],
                                    KaktYmd = (string)reader["KaktYmd"],
                                    KaknKais = (byte)reader["KaknKais"],
                                    KSKbn = (byte)reader["KSKbn"],
                                    KHinKbn = (byte)reader["KHinKbn"],
                                    HaiSYmd = (string)reader["HaiSYmd"],
                                    YykshoUpdYmdTime = (string)reader["yykSho_UpdTime"],
                                    UnkobiFileUpdYmdTime = (string)reader["unkoFile_UpdTime"]
                                });
                            }
                            reader.Close();
                        }
                        _dbContext.Database.CloseConnection();
                    }

                }
                // Add Bus Type
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    int GridIndex = 0;
                    CreateParameter(command, "PK_SpMnYykSs_R", request.hyperData, request.companyId, request.tenantId);
                    command.CommandText += ","
                        + " @StartFilterNo,"
                        + " @EndFilterNo  WITH RECOMPILE";
                    DbParameter StartFilterNo = command.CreateParameter();
                    StartFilterNo.ParameterName = "@StartFilterNo";
                    StartFilterNo.Value = list[0].UkeNo + list[0].UnkRen.ToString("D5");

                    DbParameter EndFilterNo = command.CreateParameter();
                    EndFilterNo.ParameterName = "@EndFilterNo";
                    EndFilterNo.Value = list[list.Count() - 1].UkeNo + list[list.Count() - 1].UnkRen.ToString("D5");
                    command.Parameters.Add(StartFilterNo);
                    command.Parameters.Add(EndFilterNo);
                    _dbContext.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            while (GridIndex < list.Count() && (list[GridIndex].UkeNo != (string)reader["UkeNo"] || list[GridIndex].UnkRen != (short)reader["UnkRen"]))
                            {
                                GridIndex++;
                            }
                            list[GridIndex].BusesInfo.Add(new BusInfo()
                            {
                                BusType = (string)reader["SyaSyuNm"],
                                NumberOfBuses = (short)reader["SyaSyuDai"]
                            });
                        }
                        reader.Close();
                    }
                    _dbContext.Database.CloseConnection();
                }
                return await Task.FromResult(list);
            }
        }
    }
}

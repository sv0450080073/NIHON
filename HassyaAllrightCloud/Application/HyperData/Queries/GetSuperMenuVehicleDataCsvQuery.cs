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
    public class GetSuperMenuVehicleDataCsvQuery : IRequest<List<SuperMenuVehicleCsv>>
    {
        public HyperFormData hyperData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public int fetch { get; set; }
        public int? offSet { get; set; } = null;
        public class Handler : IRequestHandler<GetSuperMenuVehicleDataCsvQuery, List<SuperMenuVehicleCsv>>
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
                        " @DantaNm";
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
                    parameter10.Value = (int)hyperData.YoyakuFrom.YoyaKbn;
                }
                DbParameter parameter11 = command.CreateParameter();
                parameter11.ParameterName = "@EndReservationClassification";
                if (hyperData.YoyakuTo == null)
                {
                    parameter11.Value = DBNull.Value;
                }
                else
                {
                    parameter11.Value = (int)hyperData.YoyakuTo.YoyaKbn;
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

                foreach (var parameter in Parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            public async Task<List<SuperMenuVehicleCsv>> Handle(GetSuperMenuVehicleDataCsvQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    string StoreName = "PK_dVehicle_R";
                    List<SuperMenuVehicleCsv> list = new List<SuperMenuVehicleCsv>();
                    using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        CreateParameter(command, StoreName, request.hyperData, request.companyId, request.tenantId);
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
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new SuperMenuVehicleCsv
                                {
                                    UkeNo = (string)reader["UkeNo"],
                                    UnkRen = (short)reader["UnkRen"],
                                    TeiDanNo = (short)reader["TeiDanNo"],
                                    BunkRen = (short)reader["BunkRen"],
                                    YoyaKbn = (byte)reader["YoyaKbn"],
                                    YoyaKbnNm = (string)reader["YoyaKbnNm"],
                                    UkeYmd = (string)reader["UkeYmd"],
                                    GyosyaCd = (short)reader["GyosyaCd"],
                                    TokuiCd = (short)reader["TokuiCd"],
                                    SitenCd = (short)reader["SitenCd"],
                                    GyosyaNm = (string)reader["GyosyaNm"],
                                    TokuiNm = (string)reader["TokuiNm"],
                                    SitenNm = (string)reader["SitenNm"],
                                    TokuiRyakuNm = (string)reader["TokuiRyakuNm"],
                                    SitenRyakuNm = (string)reader["SitenRyakuNm"],
                                    TokuiTanNm = (string)reader["TokuiTanNm"],
                                    DanTaNm = (string)reader["DanTaNm"],
                                    DanTaNm2 = (string)reader["DanTaNm2"],
                                    IkNm = (string)reader["IkNm"],
                                    SyaryoEigCd = (int)reader["SyaryoEigCd"],
                                    SyaryoEigyoNm = (string)reader["SyaryoEigyoNm"],
                                    SyaryoEigRyakuNm = (string)reader["SyaryoEigRyakuNm"],
                                    SyaRyoCd = (int)reader["SyaRyoCd"],
                                    SyaRyoNm = (string)reader["SyaRyoNm"],
                                    GoSya = (string)reader["GoSya"],
                                    YouGyoCd = (short)reader["YouGyoCd"],
                                    YouCd = (short)reader["YouCd"],
                                    YouSitCd = (short)reader["YouSitCd"],
                                    YouGyoNm = (string)reader["YouGyoNm"],
                                    YouNm = (string)reader["YouNm"],
                                    YouSitNm = (string)reader["YouSitNm"],
                                    YouRyakuNm = (string)reader["YouRyakuNm"],
                                    YouSitCdRyakuNm = (string)reader["YouSitCdRyakuNm"],
                                    HaiSYmd = (string)reader["HaiSYmd"],
                                    HaiSTime = (string)reader["HaiSTime"],
                                    HaiSNm = (string)reader["HaiSNm"],
                                    HaiSJyuS = (string)reader["HaiSJyuS1"],
                                    HaiSJyuS2 = (string)reader["HaiSJyuS2"],
                                    TouYmd = (string)reader["TouYmd"],
                                    TouChTime = (string)reader["TouChTime"],
                                    TouNm = (string)reader["TouNm"],
                                    TouJyusyo1 = (string)reader["TouJyusyo1"],
                                    TouJyusyo2 = (string)reader["TouJyusyo2"],
                                    SyuKoYmd = (string)reader["SyuKoYmd"],
                                    SyuKoTime = (string)reader["SyuKoTime"],
                                    SyuEigCd = (int)reader["SyuEigCd"],
                                    SyuEigNm = (string)reader["SyuEigNm"],
                                    SyuEigRyakuNm = (string)reader["SyuEigRyakuNm"],
                                    KikYmd = (string)reader["KikYmd"],
                                    KikTime = (string)reader["KikTime"],
                                    KikEigCd = (int)reader["KikEigCd"],
                                    KikEigNm = (string)reader["KikEigNm"],
                                    KikEigRyakuNm = (string)reader["KikEigRyakuNm"],
                                    SyuPaTime = (string)reader["SyuPaTime"],
                                    DrvJin = (short)reader["DrvJin"],
                                    GuiSu = (short)reader["GuiSu"],
                                    SyaSyuCd = (short)reader["SyaSyuCd"],
                                    SyaSyuNm = (string)reader["SyaSyuNm"],
                                    KataKbn = (byte)reader["KataKbn"],
                                    KataNm = (string)reader["KataNm"],
                                    SyaRyoUnc = (int)reader["SyaRyoUnc"],
                                    SyaRyoSyo = (int)reader["SyaRyoSyo"],
                                    SyaRyoTes = (int)reader["SyaRyoTes"],
                                    Gui_UriGakKin_S = (long)reader["Gui_UriGakKin_S"],
                                    Gui_SyaRyoSyo_S = (long)reader["Gui_SyaRyoSyo_S"],
                                    Gui_SyaRyoTes_S = (long)reader["Gui_SyaRyoTes_S"],
                                    Oth_UriGakKin_S = (long)reader["Fut_UriGakKin_S"] + (long)reader["Tuk_UriGakKin_S"] + (long)reader["Teh_UriGakKin_S"] + (long)reader["Tum_UriGakKin_S"],
                                    Oth_SyaRyoSyo_S = (long)reader["Fut_SyaRyoSyo_S"] + (long)reader["Tuk_SyaRyoSyo_S"] + (long)reader["Teh_SyaRyoSyo_S"] + (long)reader["Tum_SyaRyoSyo_S"],
                                    Oth_SyaRyoTes_S = (long)reader["Fut_SyaRyoTes_S"] + (long)reader["Tuk_SyaRyoTes_S"] + (long)reader["Teh_SyaRyoTes_S"] + (long)reader["Tum_SyaRyoTes_S"],
                                    YoushaUnc = (int)reader["YoushaUnc"],
                                    YoushaSyo = (int)reader["YoushaSyo"],
                                    YoushaTes = (int)reader["YoushaTes"],
                                    YGui_HaseiKin_S = (long)reader["YGui_HaseiKin_S"],
                                    YGui_SyaRyoSyo_S = (long)reader["YGui_SyaRyoSyo_S"],
                                    YGui_SyaRyoTes_S = (long)reader["YGui_SyaRyoTes_S"],
                                    YOth_HaseiKin_S = (long)reader["Yfut_HaseiKin_S"] + (long)reader["Ytuk_HaseiKin_S"] + (long)reader["Yteh_HaseiKin_S"] + (long)reader["Ytum_HaseiKin_S"],
                                    YOth_SyaRyoSyo_S = (long)reader["Yfut_SyaRyoSyo_S"] + (long)reader["Ytuk_SyaRyoSyo_S"] + (long)reader["Yteh_SyaRyoSyo_S"] + (long)reader["Ytum_SyaRyoSyo_S"],
                                    YOth_SyaRyoTes_S = (long)reader["Yfut_SyaRyoTes_S"] + (long)reader["Ytuk_SyaRyoTes_S"] + (long)reader["Yteh_SyaRyoTes_S"] + (long)reader["Ytum_SyaRyoTes_S"],
                                    SyainCd1 = "",
                                    SyainNm1 = "",
                                    SyainCd2 = "",
                                    SyainNm2 = "",
                                    SyainCd3 = "",
                                    SyainNm3 = "",
                                    SyainCd4 = "",
                                    SyainNm4 = "",
                                    SyainCd5 = "",
                                    SyainNm5 = "",
                                    JisaIPKm = (decimal)reader["JisaIPKm"],
                                    JisaKSKm = (decimal)reader["JisaKSKm"],
                                    KisoIPkm = (decimal)reader["KisoIPkm"],
                                    KisoKOKm = (decimal)reader["KisoKOKm"],
                                    OthKm = (decimal)reader["OthKm"],
                                    TotalKilo = (decimal)reader["JisaIPKm"] + (decimal)reader["JisaKSKm"] + (decimal)reader["KisoIPkm"] + (decimal)reader["KisoKOKm"] + (decimal)reader["OthKm"],
                                    Nenryo1 = (decimal)reader["Nenryo1"],
                                    NenryoRyak1 = (string)reader["NenryoRyak1"],
                                    Nenryo2 = (decimal)reader["Nenryo2"],
                                    NenryoRyak2 = (string)reader["NenryoRyak2"],
                                    Nenryo3 = (decimal)reader["Nenryo3"],
                                    NenryoRyak3 = (string)reader["NenryoRyak3"],
                                    JyoSyaJin = (short)reader["JyoSyaJin"],
                                    PlusJin = (short)reader["PlusJin"],
                                    UkeJyKbn = (string)reader["UkeJyKbn"],
                                    SijJoKbn1 = (string)reader["SijJoKbn1"],
                                    SijJoKbn2 = (string)reader["SijJoKbn2"],
                                    SijJoKbn3 = (string)reader["SijJoKbn3"],
                                    SijJoKbn4 = (string)reader["SijJoKbn4"],
                                    SijJoKbn5 = (string)reader["SijJoKbn5"],
                                    UkeEigCd = (int)reader["UkeEigCd"],
                                    UkeEigNm = (string)reader["UkeEigNm"],
                                    UkeEigRyakuNm = (string)reader["UkeEigRyakuNm"],
                                    EigTanSyainCd = (string)reader["EigTanSyainCd"],
                                    EigTanSyainNm = (string)reader["EigTanSyainNm"],
                                    InputTanSyainCd = (string)reader["InputTanSyainCd"],
                                    InputTanSyainNm = (string)reader["InputTanSyainNm"],
                                    DantaiCd = (string)reader["DantaiCd"],
                                    DantaiCdNm = (string)reader["DantaiCdNm"],
                                    JyokyakuCd = (byte)reader["JyoKyakuCd"],
                                    JyokyakuNm = (string)reader["JyoKyakuNm"],
                                    CanFuYmd = (string)reader["CanFuYmd"],
                                    CanFuRiy = (string)reader["CanFuRiy"],
                                    CanFutanCd = (string)reader["CanFutanCd"],
                                    CanFutanNm = (string)reader["CanFutanNm"]

                                });
                            }
                            reader.Close();
                        }
                        _dbContext.Database.CloseConnection();
                    }

                    // Add Crew
                    using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        CreateParameter(command, "PK_dVehicleCrew_R", request.hyperData, request.companyId, request.tenantId);
                        command.CommandText += ","
                            + " @StartFilterNo,"
                            + " @EndFilterNo WITH RECOMPILE";
                        DbParameter StartFilterNo = command.CreateParameter();
                        StartFilterNo.ParameterName = "@StartFilterNo";
                        StartFilterNo.Value = list[0].UkeNo + $"{list[0].UnkRen:00000}" + $"{list[0].TeiDanNo:00000}" + $"{list[0].BunkRen:00000}";

                        DbParameter EndFilterNo = command.CreateParameter();
                        EndFilterNo.ParameterName = "@EndFilterNo";
                        EndFilterNo.Value = list[list.Count() - 1].UkeNo + $"{list[list.Count() - 1].UnkRen:00000}"
                                           + $"{list[list.Count() - 1].TeiDanNo:00000}" + $"{list[list.Count() - 1].BunkRen:00000}";

                        command.Parameters.Add(StartFilterNo);
                        command.Parameters.Add(EndFilterNo);
                        _dbContext.Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var currentCrew = list.Where(x => x.UkeNo == (string)reader["UkeNo"] && x.UnkRen == (short)reader["UnkRen"]
                                                             && x.TeiDanNo == (short)reader["TeiDanNo"] && x.BunkRen == (short)reader["BunkRen"]).FirstOrDefault();

                                if (currentCrew != null && string.IsNullOrEmpty(currentCrew.SyainCd5) && string.IsNullOrEmpty(currentCrew.SyainNm5)
                                   && (!string.IsNullOrEmpty(currentCrew.SyainCd4) || !string.IsNullOrEmpty(currentCrew.SyainNm4)))
                                {
                                    currentCrew.SyainCd5 = (string)reader["SyainCd"];
                                    currentCrew.SyainNm5 = (string)reader["SyainNm"];
                                }
                                if (currentCrew != null && string.IsNullOrEmpty(currentCrew.SyainCd4) && string.IsNullOrEmpty(currentCrew.SyainNm4)
                                   && (!string.IsNullOrEmpty(currentCrew.SyainCd3) || !string.IsNullOrEmpty(currentCrew.SyainNm3)))
                                {
                                    currentCrew.SyainCd4 = (string)reader["SyainCd"];
                                    currentCrew.SyainNm4 = (string)reader["SyainNm"];
                                }
                                if (currentCrew != null && string.IsNullOrEmpty(currentCrew.SyainCd3) && string.IsNullOrEmpty(currentCrew.SyainNm3)
                                   && (!string.IsNullOrEmpty(currentCrew.SyainCd2) || !string.IsNullOrEmpty(currentCrew.SyainNm2)))
                                {
                                    currentCrew.SyainCd3 = (string)reader["SyainCd"];
                                    currentCrew.SyainNm3 = (string)reader["SyainNm"];
                                }
                                if (currentCrew != null && string.IsNullOrEmpty(currentCrew.SyainCd2) && string.IsNullOrEmpty(currentCrew.SyainNm2)
                                   && (!string.IsNullOrEmpty(currentCrew.SyainCd1) || !string.IsNullOrEmpty(currentCrew.SyainNm1)))
                                {
                                    currentCrew.SyainCd2 = (string)reader["SyainCd"];
                                    currentCrew.SyainNm2 = (string)reader["SyainNm"];
                                }
                                if (currentCrew != null && string.IsNullOrEmpty(currentCrew.SyainCd1) && string.IsNullOrEmpty(currentCrew.SyainNm1))
                                {
                                    currentCrew.SyainCd1 = (string)reader["SyainCd"];
                                    currentCrew.SyainNm1 = (string)reader["SyainNm"];
                                }
                            }
                            reader.Close();
                        }
                        _dbContext.Database.CloseConnection();
                    }
                    return await Task.FromResult(list);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
        }
    }
}

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

namespace HassyaAllrightCloud.Application.BillCheckList.Queries
{
    public class GetDataForBillCheckListGridQuery : IRequest<(int, List<BillCheckListTotalData>, List<BillCheckListGridData>)>
    {
        public BillsCheckListFormData billCheckListData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public int fetch { get; set; }
        public int? offSet { get; set; } = null;
        public class Handler : IRequestHandler<GetDataForBillCheckListGridQuery, (int, List<BillCheckListTotalData>, List<BillCheckListGridData>)>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public void CreateParameter(DbCommand command, string procedureName, BillsCheckListFormData billFormData, int CompanyId, int TenantId)
            {
                command.CommandText = "EXECUTE " + procedureName + " " +
                        " @TenantCdSeq," +
                        " @CompanyCdSeq," +
                        " @StartBillPeriod," +
                        " @EndBillPeriod," +
                        " @BillOffice," +
                        " @StartBillAddress," +
                        " @EndBillAddress," +
                        " @StartReceiptNumber," +
                        " @EndReceiptNumber," +
                        " @StartReservationClassification," +
                        " @EndReservationClassification," +
                        " @StartBillClassification," +
                        " @EndBillClassification," +
                        " @BillTypes," +
                        " @BillAddress," +
                        " @BillIssuedClassification," +
                        " @BillTypeOrderBy";
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

                // 請求対象期間
                DbParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "@StartBillPeriod";
                if (billFormData.BillPeriodFrom == null)
                {
                    parameter2.Value = DBNull.Value;
                }
                else
                {
                    parameter2.Value = ((DateTime)billFormData.BillPeriodFrom).ToString("yyyyMMdd");
                }
                DbParameter parameter3 = command.CreateParameter();
                parameter3.ParameterName = "@EndBillPeriod";
                if (billFormData.BillPeriodTo == null)
                {
                    parameter3.Value = DBNull.Value;
                }
                else
                {
                    parameter3.Value = ((DateTime)billFormData.BillPeriodTo).ToString("yyyyMMdd");
                }
                Parameters.Add(parameter2);
                Parameters.Add(parameter3);

                // 請求営業所
                DbParameter parameter4 = command.CreateParameter();
                parameter4.ParameterName = "@BillOffice";
                if (billFormData.BillOffice == null)
                {
                    parameter4.Value = DBNull.Value;
                }
                else
                {
                    parameter4.Value = billFormData.BillOffice.EigyoCdSeq;
                }
                Parameters.Add(parameter4);

                // 請求先
                DbParameter parameter5 = command.CreateParameter();
                parameter5.ParameterName = "@StartBillAddress";
                if (billFormData.GyosyaTokuiSakiFrom == null || billFormData.GyosyaTokuiSakiFrom.GyosyaCdSeq == 0)
                {
                    parameter5.Value = DBNull.Value;
                }
                else
                {
                    parameter5.Value = billFormData.GyosyaTokuiSakiFrom.GyosyaCd.ToString("D3") + (billFormData.TokiskTokuiSakiFrom == null ? "0000" : billFormData.TokiskTokuiSakiFrom.TokuiCd.ToString("D4")) + (billFormData.TokiStTokuiSakiFrom == null ? "0000" : billFormData.TokiStTokuiSakiFrom.SitenCd.ToString("D4"));
                }
                DbParameter parameter6 = command.CreateParameter();
                parameter6.ParameterName = "@EndBillAddress";
                if (billFormData.GyosyaTokuiSakiTo == null || billFormData.GyosyaTokuiSakiTo.GyosyaCdSeq == 0)
                {
                    parameter6.Value = DBNull.Value;
                }
                else
                {
                    parameter6.Value = billFormData.GyosyaTokuiSakiTo.GyosyaCd.ToString("D3") + ((billFormData.TokiskTokuiSakiTo == null || billFormData.TokiskTokuiSakiTo.TokuiSeq == 0) ? "9999" : billFormData.TokiskTokuiSakiTo.TokuiCd.ToString("D4")) + ((billFormData.TokiStTokuiSakiTo == null || billFormData.TokiStTokuiSakiTo.SitenCdSeq == 0) ? "9999" : billFormData.TokiStTokuiSakiTo.SitenCd.ToString("D4"));
                }
                Parameters.Add(parameter5);
                Parameters.Add(parameter6);

                // 予約番号
                DbParameter parameter7 = command.CreateParameter();
                parameter7.ParameterName = "@StartReceiptNumber";
                if (string.IsNullOrEmpty(billFormData.StartReceiptNumber))
                {
                    parameter7.Value = DBNull.Value;
                }
                else
                {
                    parameter7.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(billFormData.StartReceiptNumber);
                }
                DbParameter parameter8 = command.CreateParameter();
                parameter8.ParameterName = "@EndReceiptNumber";
                if (string.IsNullOrEmpty(billFormData.EndReceiptNumber))
                {
                    parameter8.Value = DBNull.Value;
                }
                else
                {
                    parameter8.Value = TenantId.ToString("D5") + CommonUtil.FormatCodeNumber(billFormData.EndReceiptNumber);
                }
                Parameters.Add(parameter7);
                Parameters.Add(parameter8);

                // 予約区分
                DbParameter parameter9 = command.CreateParameter();
                parameter9.ParameterName = "@StartReservationClassification";
                if (billFormData.YoyakuFrom == null)
                {
                    parameter9.Value = DBNull.Value;
                }
                else
                {
                    parameter9.Value = billFormData.YoyakuFrom.YoyaKbn;
                }
                DbParameter parameter10 = command.CreateParameter();
                parameter10.ParameterName = "@EndReservationClassification";
                if (billFormData.YoyakuTo == null)
                {
                    parameter10.Value = DBNull.Value;
                }
                else
                {
                    parameter10.Value = billFormData.YoyakuTo.YoyaKbn;
                }
                Parameters.Add(parameter9);
                Parameters.Add(parameter10);

                // 請求区分
                DbParameter parameter11 = command.CreateParameter();
                parameter11.ParameterName = "@StartBillClassification";
                if (billFormData.StartBillClassification == null)
                {
                    parameter11.Value = DBNull.Value;
                }
                else
                {
                    parameter11.Value = billFormData.StartBillClassification.CodeKbn;
                }
                DbParameter parameter12 = command.CreateParameter();
                parameter12.ParameterName = "@EndBillClassification";
                if (billFormData.EndBillClassification == null)
                {
                    parameter12.Value = DBNull.Value;
                }
                else
                {
                    parameter12.Value = billFormData.EndBillClassification.CodeKbn;
                }
                Parameters.Add(parameter11);
                Parameters.Add(parameter12);

                // 請求発行済区分
                DbParameter parameter13 = command.CreateParameter();
                parameter13.ParameterName = "@BillTypes";
                string lstBillType = "";
                if (billFormData.itemFare)
                {
                    lstBillType += "1,";
                }
                if (billFormData.itemIncidental)
                {
                    lstBillType += "2,";
                }
                if (billFormData.itemTollFee)
                {
                    lstBillType += "3,";
                }
                if (billFormData.itemArrangementFee)
                {
                    lstBillType += "4,";
                }
                if (billFormData.itemGuideFee)
                {
                    lstBillType += "5,";
                }
                if (billFormData.itemShippingCharge)
                {
                    lstBillType += "6,";
                }
                if (billFormData.itemCancellationCharge)
                {
                    lstBillType += "7,";
                }
                if ("".Equals(lstBillType))
                {
                    parameter13.Value = DBNull.Value;
                }
                else
                {
                    parameter13.Value = lstBillType.Substring(0, lstBillType.Length - 1);
                }
                Parameters.Add(parameter13);

                // 請求先
                DbParameter parameter14 = command.CreateParameter();
                parameter14.ParameterName = "@BillAddress";
                if (billFormData.BillAdress == null)
                {
                    parameter14.Value = DBNull.Value;
                }
                else
                {
                    parameter14.Value = billFormData.BillAdress.GyoSyaCd.ToString("D3") + billFormData.BillAdress.TokuiCd.ToString("D4") + billFormData.BillAdress.SitenCd.ToString("D4");
                }
                
                Parameters.Add(parameter14);

                // 
                DbParameter parameter15 = command.CreateParameter();
                parameter15.ParameterName = "@BillIssuedClassification";
                if (billFormData.BillIssuedClassification == null || billFormData.BillIssuedClassification.IdValue == 0)
                {
                    parameter15.Value = DBNull.Value;
                }
                else
                {
                    parameter15.Value = billFormData.BillIssuedClassification.IdValue;
                }
                Parameters.Add(parameter15);

                // @BillTypeOrderBy
                DbParameter parameter16 = command.CreateParameter();
                parameter16.ParameterName = "@BillTypeOrderBy";
                if (billFormData.BillTypeOrder == null || billFormData.BillTypeOrder.IdValue == 0)
                {
                    parameter16.Value = DBNull.Value;
                }
                else
                {
                    parameter16.Value = billFormData.BillTypeOrder.IdValue.ToString();
                }
                Parameters.Add(parameter16);

                foreach (var parameter in Parameters)
                {
                    command.Parameters.Add(parameter);
                }
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
                    return DateTime.ParseExact(Ymd, DateFormat, CultureInfo.InvariantCulture).ToString("yy/MM/dd (ddd)") + ' ' + area;
                }
            }

            public async Task<(int, List<BillCheckListTotalData>, List<BillCheckListGridData>)> Handle(GetDataForBillCheckListGridQuery request, CancellationToken cancellationToken)
            {
                    int outputRowCoutVehicle = 0;
                    int outputRowCoutVehicleCrew = 0;
                    var totalRows = 0;
                    List<BillCheckListTotalData> listTotal = new List<BillCheckListTotalData>();
                    List<BillCheckListGridData> list = new List<BillCheckListGridData>();
                    using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        CreateParameter(command, "PK_dBillCheckList_R", request.billCheckListData, request.companyId, request.tenantId);
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
                        using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    totalRows = (int)reader["rcount"];
                                }
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    listTotal.Add(new BillCheckListTotalData
                                    {
                                        Type = 3,
                                        SeiFutSyu = (byte)reader["SeiFutSyu"],
                                        BillAmountTotal = (long)reader["SeiKin"],
                                        DepositAmountTotal = (decimal)reader["NyuKinRui"],
                                        UnpaidAmountTotal = (decimal)reader["MisyuG"],
                                        SalesAmountTotal = (long)reader["UriGakKin"],
                                        TaxAmountTotal = (long)reader["SyaRyoSyo"],
                                        CommissionAmount = (long)reader["SyaRyoTes"]
                                    });
                                }
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    list.Add(new BillCheckListGridData
                                    {
                                        UkeNo = (string)reader["UkeNo"],
                                        MisyuRen = (short)reader["MisyuRen"],
                                        SeiFutSyu = (byte)reader["SeiFutSyu"],
                                        FutTumRen = (short)reader["FutTumRen"],
                                        FutuUnkRen = (short)reader["FutuUnkRen"],
                                        BillDate = DateDisplayValue((string)reader["SeiTaiYmd"]),
                                        Office = (string)reader["UkeRyakuNm"],
                                        BillAddress = (string)reader["SeiRyakuNm"] + " " + (string)reader["SeiSitRyakuNm"],
                                        GroupName = (string)reader["DanTaNm"],
                                        DestinationName = (string)reader["IkNm"],
                                        DispatchDate = DateDisplayValue((string)reader["HaiSYmd"]),
                                        ArrivalDate = DateDisplayValue((string)reader["TouYmd"]),
                                        BillIncidentTypeName = (string)reader["SeiFutSyuNm"],
                                        IncidentLoadingGoodsName = (string)reader["FutTumNm"],
                                        PaymentName = (string)reader["SeisanNm"],
                                        Quantity = (byte)reader["SeiFutSyu"] == 1 ? (string)reader["Sum_SyaSyuDai"] : (string)reader["Suryo"],
                                        BusType = "",
                                        Price = !string.IsNullOrEmpty(((byte)reader["SeiFutSyu"] == 1 ? (string)reader["Sum_SyaSyuTan"] : (string)reader["TanKa"])) ?  decimal.Parse(((byte)reader["SeiFutSyu"] == 1 ? (string)reader["Sum_SyaSyuTan"] : (string)reader["TanKa"])) : 0,
                                        BillAmount = (int)reader["SeiKin"],
                                        DepositDate = DateDisplayValue((string)reader["NyuKinYmd"]),
                                        DepositAmount = (decimal)reader["NyuKinRui"],
                                        UnpaidAmount = (decimal)reader["MisyuG"],
                                        SalesAmount = (int)reader["UriGakKin"],
                                        TaxAmount = (int)reader["SyaRyoSyo"],
                                        CommissionRate = (decimal)reader["TesuRitu"],
                                        CommissionAmount = (int)reader["SyaRyoTes"],
                                        OccurrenceDate = DateDisplayValue((string)reader["HasYmd"]),
                                        IssuedDate = DateDisplayValue((string)reader["SeiHatYmd"]),
                                        ReceiptNumber = ((string)reader["UkeNo"]).Substring(5, 10),
                                        NyuKinKbn = (byte)reader["NyuKinKbn"],
                                        NCouKbn = (byte)reader["NCouKbn"]

                                    });
                                }
                            reader.Close();
                            }
                            _dbContext.Database.CloseConnection();
                    }
                    if (listTotal.Count > 0)
                    {
                        listTotal.Add(new BillCheckListTotalData()
                        {
                            Type = 3,
                            SeiFutSyu = 0,
                            BillAmountTotal = listTotal.Sum(x => x.BillAmountTotal),
                            DepositAmountTotal = listTotal.Sum(x => x.DepositAmountTotal),
                            UnpaidAmountTotal = listTotal.Sum(x => x.UnpaidAmountTotal),
                            SalesAmountTotal = listTotal.Sum(x => x.SalesAmountTotal),
                            TaxAmountTotal = listTotal.Sum(x => x.TaxAmountTotal),
                            CommissionAmount = listTotal.Sum(x => x.CommissionAmount),
                        });
                    }
                    if (request.offSet != null)
                    {
                        // Add Crew
                        using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                        {
                            int GridIndex = 0;
                            command.CommandText = "EXECUTE PK_dAcquisitionData_R "
                                       + " @TenantCdSeq,"
                                       + " @ListUkeNo,"
                                       + " @ROWCOUNT";
                            DbParameter tenantID = command.CreateParameter();
                            tenantID.ParameterName = "@TenantCdSeq";
                            tenantID.Value = request.tenantId;

                            DbParameter StartFilterNo = command.CreateParameter();
                            StartFilterNo.ParameterName = "@ListUkeNo";
                            int i = 0;
                            string lstUkeNo = "";
                            for (i = 0; i < list.Count; i++)
                            {
                                if (list[i].SeiFutSyu == 1)
                                {
                                    lstUkeNo += "'" + list[i].UkeNo.ToString() + "',";
                                }
                            }
                            if ("".Equals(lstUkeNo))
                            {
                                StartFilterNo.Value = DBNull.Value;
                            }
                            else
                            {
                                lstUkeNo = lstUkeNo.Substring(0, lstUkeNo.Length - 1);
                                StartFilterNo.Value = lstUkeNo;
                            }

                            DbParameter rowCount = command.CreateParameter();
                            rowCount.ParameterName = "@ROWCOUNT";
                            rowCount.Value = outputRowCoutVehicleCrew;

                            command.Parameters.Add(tenantID);
                            command.Parameters.Add(StartFilterNo);
                            command.Parameters.Add(rowCount);
                            _dbContext.Database.OpenConnection();
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    while (GridIndex < list.Count() && (list[GridIndex].UkeNo != (string)reader["UkeNo"]))
                                    {
                                        GridIndex++;
                                    }
                                    if (GridIndex < list.Count() && list[GridIndex].SeiFutSyu == 1)
                                    {
                                        list[GridIndex].BusType = (string)reader["SyaSyuCd_SyaSyuNm"] + " (" + (string)reader["KataKbn_RyakuNm"] + ")";
                                    }
                                    else
                                    {
                                        GridIndex++;
                                    }
                                }
                                reader.Close();
                            }
                            _dbContext.Database.CloseConnection();
                        }
                    }
                    return (totalRows, listTotal, list);
            }
        }
    }
}

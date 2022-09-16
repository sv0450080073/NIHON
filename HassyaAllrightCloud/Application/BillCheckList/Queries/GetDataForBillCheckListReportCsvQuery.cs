using HassyaAllrightCloud.Commons.Constants;
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
    public class GetDataForBillCheckListReportCsvQuery : IRequest<List<BillCheckListModelCsvData>>
    {
        public BillsCheckListFormData billCheckListData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public class Handler : IRequestHandler<GetDataForBillCheckListReportCsvQuery, List<BillCheckListModelCsvData>>
        {
            private readonly KobodbContext _dbContext;
            public Handler(KobodbContext context)
            {
                _dbContext = context;
            }
            public void CreateParameter(DbCommand command, string procedureName, BillsCheckListFormData billFormData, int CompanyId, int TenantId, int type)
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
                        (type == 0 ? " @BillIssuedClassification," + " @BillTypeOrderBy" : " @BillIssuedClassification");
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

                // 
                DbParameter parameter14 = command.CreateParameter();
                parameter14.ParameterName = "@BillIssuedClassification";
                if (billFormData.BillIssuedClassification == null || billFormData.BillIssuedClassification.IdValue == 0)
                {
                    parameter14.Value = DBNull.Value;
                }
                else
                {
                    parameter14.Value = billFormData.BillIssuedClassification.IdValue;
                }
                Parameters.Add(parameter14);
                if (type == 0)
                {
                    // @BillTypeOrderBy
                    DbParameter parameter15 = command.CreateParameter();
                    parameter15.ParameterName = "@BillTypeOrderBy";
                    if (billFormData.BillTypeOrder == null || billFormData.BillTypeOrder.IdValue == 0)
                    {
                        parameter15.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter15.Value = billFormData.BillTypeOrder.IdValue.ToString();
                    }
                    Parameters.Add(parameter15);
                }


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
            public async Task<List<BillCheckListModelCsvData>> Handle(GetDataForBillCheckListReportCsvQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.billCheckListData;
                List<BillCheckListModelCsvData> listData = new List<BillCheckListModelCsvData>();

               // Get for list data
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    CreateParameter(command, "PK_dBillCheckListReport_R", request.billCheckListData, request.companyId, request.tenantId, 0);
                    command.CommandText += " WITH RECOMPILE";

                    _dbContext.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listData.Add(new BillCheckListModelCsvData
                            {
                                BillOfficeCode = ((int)reader["SeiEigyoCd"]),
                                BillOffice = (string)reader["SeiEigyoNm"],
                                BillOfficeAbbreviation = (string)reader["SeiEigyoRyak"],
                                BillCompanyCode = (short)reader["SeiCd"],
                                BillAddressCode = (short)reader["SeiGyosyaCd"],
                                BillBranchCode = (short)reader["SeiSitenCd"],
                                BillCompanyName = (string)reader["SeiGyosyaCdNm"],
                                BillAddress = (string)reader["SeiCdNm"],
                                BillBranchName = (string)reader["SeiSitenCdNm"],
                                BillAbbreviation = (string)reader["SeiRyakuNm"],
                                BillBranchShortName = (string)reader["SeiSitRyakuNm"],
                                BillDate = (string)reader["SeiTaiYmd"],
                                ReceiptNumber = ((string)reader["UkeNo"]).Substring(5, 10),
                                ReceiptOfficeCode = ((int)reader["UkeEigyoCd"]),
                                ReceiptOfficeName = (string)reader["UkeEigyoNm"],
                                ReceiptOfficeAbbreviationName = (string)reader["UkeRyakuNm"],
                                GroupName = (string)reader["DanTaNm"],
                                DestinationName = (string)reader["IkNm"],
                                DispatchDate = (string)reader["HaiSYmd"],
                                ArrivalDate = (string)reader["TouYmd"],
                                BillIncidentType = (byte)reader["SeiFutSyu"],
                                BillIncidentTypeName = (string)reader["SeiFutSyuNm"],
                                IncidentLoadingGoodsName = (string)reader["FutTumNm"],
                                PaymentCode = (string)reader["SeisanCd"],
                                PaymentName = (string)reader["SeisanNm"],
                                UnitNumber = (string)reader["Suryo"] != "" ? int.Parse((string)reader["Suryo"]) : 0,
                                Price = (string)reader["TanKa"],
                                BillAmount = (int)reader["SeiKin"],
                                DepositDate = (string)reader["NyuKinYmd"],
                                DepositAmount = (decimal)reader["NyuKinRui"],
                                UnpaidAmount = (decimal)reader["MisyuG"],
                                SalesAmount = (int)reader["UriGakKin"],
                                TaxAmount = (int)reader["SyaRyoSyo"],
                                CommissionRate = (decimal)reader["TesuRitu"],
                                CommissionAmount = (int)reader["SyaRyoTes"],
                                OccurrenceDate = (string)reader["HasYmd"],
                                IssuedDate = (string)reader["SeiHatYmd"],
                                TSiyoStaYmd = (string)reader["TSiyoStaYmd"],
                                TSiyoEndYmd = (string)reader["TSiyoEndYmd"],
                                SSiyoStaYmd = (string)reader["SSiyoStaYmd"],
                                SSiyoEndYmd = (string)reader["SSiyoEndYmd"],
                                QuantityNumber = (string)reader["Sum_SyaSyuDai"],
                                Sum_SyaSyuTan = (string)reader["Sum_SyaSyuTan"],
                                UkeNo = (string)reader["UkeNo"],
                                MisyuRen = (short)reader["MisyuRen"],
                                FutTumRen = (short)reader["FutTumRen"],
                                FutuUnkRen = (short)reader["FutuUnkRen"]
                            });
                        }
                        reader.Close();
                    }
                    _dbContext.Database.CloseConnection();
                    
                }
                return await Task.FromResult(listData);
            }
        }
    }
}

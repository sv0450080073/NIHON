using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Infrastructure.Persistence;
using HassyaAllrightCloud.Reports.DataSource;
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
    public class GetDataForBillCheckListReportQuery : IRequest<List<BillCheckListReportPDF>>
    {
        public BillsCheckListFormData billCheckListData { get; set; }
        public int companyId { get; set; }
        public int tenantId { get; set; }
        public class Handler : IRequestHandler<GetDataForBillCheckListReportQuery, List<BillCheckListReportPDF>>
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
                if(type == 0)
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

            public async Task<List<BillCheckListReportPDF>> Handle(GetDataForBillCheckListReportQuery request, CancellationToken cancellationToken)
            {
                var searchParam = request.billCheckListData;
                List<BillCheckListGridData> listData = new List<BillCheckListGridData>();
                List<BillCheckListTotalData> listTotal = new List<BillCheckListTotalData>();
                List<BillCheckListReportPDF> listReport = new List<BillCheckListReportPDF>();
                var currentDate = DateTime.Now.ToString(CommonConstants.FormatYMDHm);
                var page = 1;
                string startYmd = searchParam.BillPeriodFrom == null ? string.Empty : searchParam.BillPeriodFrom.Value.ToString(CommonConstants.Format2YMD);
                string endYmd = searchParam.BillPeriodTo == null ? string.Empty : searchParam.BillPeriodTo.Value.ToString(CommonConstants.Format2YMD);

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
                            listData.Add(new BillCheckListGridData
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
                                Price = !string.IsNullOrEmpty(((byte)reader["SeiFutSyu"] == 1 ? (string)reader["Sum_SyaSyuTan"] : (string)reader["TanKa"])) ? decimal.Parse(((byte)reader["SeiFutSyu"] == 1 ? (string)reader["Sum_SyaSyuTan"] : (string)reader["TanKa"])) : 0,
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
                                QuantityNumber = (string)reader["Sum_SyaSyuDai"] != "" ? int.Parse((string)reader["Sum_SyaSyuDai"]) : 0,
                                UnitNumber = (string)reader["Suryo"] != "" ? int.Parse((string)reader["Suryo"]) : 0,
                                BillAddressCode = $"{(short)reader["SeiGyosyaCd"]:000}" + "-" + $"{(short)reader["SeiCd"]:0000}" + "-" + $"{(short)reader["SeiSitenCd"]:0000}"

                            });
                        }
                        reader.Close();
                    }
                    _dbContext.Database.CloseConnection();
                }

                // Get for bill address distinct
                List<LoadCustomerList> listBillAddress = new List<LoadCustomerList>();
                using (var command = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    CreateParameter(command, "PK_dBillCheckListForDistinctBillAddress_R", request.billCheckListData, request.companyId, request.tenantId, 1);
                    command.CommandText += " WITH RECOMPILE";
                    _dbContext.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBillAddress.Add(new LoadCustomerList
                            {
                                TokuiSeq = (int)reader["TokuiSeq"],
                                SitenCdSeq = (int)reader["SitenCdSeq"],
                                TokuiCd = (short)reader["TokuiCd"],
                                RyakuNm = (string)reader["RyakuNm"],
                                SitenCd = (short)reader["SitenCd"],
                                SitenNm = (string)reader["SitenNm"],
                                TesuRitu = (decimal)reader["TesuRitu"],
                                TesuRituGui = (decimal)reader["TesuRituGui"],
                                GyoSysSeq = (int)reader["GyosyaCdSeq"],
                                GyoSyaCd = (short)reader["GyosyaCd"],
                                GyoSyaNm = (string)reader["GyosyaNm"]

                            });
                        }
                        reader.Close();
                    }
                    _dbContext.Database.CloseConnection();
                }

                // Sub for each bill address
                listBillAddress.ForEach(e =>
                {
                    var onePage = new BillCheckListReportPDF();
                    List<BillCheckListGridData> listTemp = new List<BillCheckListGridData>();
                    listTemp = listData.Where(_ => _.BillAddressCode == ($"{e.GyoSyaCd:000}" + "-" + $"{e.TokuiCd:0000}" + "-" + $"{e.SitenCd:0000}")).ToList();
                    var tempTotal = new BillCheckListTotalData()
                    {
                        QuantityNumberTotal = 0,
                        TaxAmountTotal = 0,
                        UnitNumberTotal = 0,
                        SalesAmountTotal = 0,
                        CommissionAmount = 0,
                        BillAmountTotal = 0,
                        DepositAmountTotal = 0,
                        UnpaidAmountTotal = 0
                    };
                    if (listTemp.Count >= 18)
                    {
                        var count = Math.Ceiling(listTemp.Count / 18 * 1.0);
                        for (int i = 0; i < count; i++)
                        {
                            onePage = new BillCheckListReportPDF();
                            var listPerPage = listTemp.Skip(i * 18).Take(18).ToList();
                            SetData(onePage, listTemp, listPerPage, currentDate, request.billCheckListData, page, tempTotal, e);
                            listReport.Add(onePage);
                            page++;
                        }
                        if(count * 18 < listTemp.Count)
                        {
                            onePage = new BillCheckListReportPDF();
                            var listPerPage = listTemp.Skip((int)(count * 18)).Take(listTemp.Count - (int)(count * 18)).ToList();
                            while (listPerPage.Count < 18)
                            {
                                listPerPage.Add(new BillCheckListGridData());
                            }
                            SetData(onePage, listTemp, listPerPage, currentDate, request.billCheckListData, page, tempTotal, e);
                            listReport.Add(onePage);
                            page++;
                        }
                    }
                    else
                    {
                        while (listTemp.Count < 18)
                        {
                            listTemp.Add(new BillCheckListGridData());
                        }
                        SetData(onePage, listTemp, listTemp, currentDate, request.billCheckListData, page, tempTotal, e);
                        listReport.Add(onePage);
                        page++;
                    }
                });
                //
                return await Task.FromResult(listReport);
            }
            private void SetData(BillCheckListReportPDF onePage, List<BillCheckListGridData> list,
                List<BillCheckListGridData> listPerPage, string currentDate, BillsCheckListFormData listSearch, int page, BillCheckListTotalData listTotal, dynamic item)
            {
                int i = 1;
                list.ForEach(e => {
                    if(!"".Equals(e.UkeNo) && e.UkeNo != null)
                    {
                        e.No = i;
                    }
                    else
                    {
                        e.No = 0;
                    }
                    i++;
                });
                onePage.ListData= listPerPage;
                onePage.ListTotal = InitChildList(listPerPage, listTotal);
                onePage.CurrentDate = currentDate;
                onePage.PageNumber = page;
                onePage.BillOfficeCode = listSearch.BillOffice != null ? $"{listSearch.BillOffice.EigyoCd:00000}" : "";
                onePage.BillOffice = listSearch.BillOffice != null ? listSearch.BillOffice.EigyoNm : "";
                onePage.BillAddressCode = ($"{item.GyoSyaCd:000}" + "-" + $"{item.TokuiCd:0000}" + "-" + $"{item.SitenCd:0000}"); //listSearch.BillAdress != null ? ($"{listSearch.BillAdress.GyoSyaCd:000}" + "-" + $"{listSearch.BillAdress.TokuiCd:0000}" + "-" + $"{listSearch.BillAdress.SitenCd:0000}") : "";
                onePage.BillAddress = item.RyakuNm + " " + item.SitenNm;
                onePage.StartBillAddressCode = listSearch.StartBillAddress != null ? ($"{listSearch.StartBillAddress.GyoSyaCd:000}" + "-" + $"{listSearch.StartBillAddress.TokuiCd:0000}" + "-" + $"{listSearch.StartBillAddress.SitenCd:0000}") : "";
                onePage.StartBillAddress = listSearch.StartBillAddress != null ? listSearch.StartBillAddress.RyakuNm + " " + listSearch.StartBillAddress.SitenNm : "";
                onePage.EndBillAddressCode = listSearch.EndBillAddress != null ? ($"{listSearch.EndBillAddress.GyoSyaCd:000}" + "-" + $"{listSearch.EndBillAddress.TokuiCd:0000}" + "-" + $"{listSearch.EndBillAddress.SitenCd:0000}") : "";
                onePage.EndBillAddress = listSearch.EndBillAddress != null ? listSearch.EndBillAddress.RyakuNm + " " + listSearch.EndBillAddress.SitenNm : "";
                onePage.BillPeriodFrom = listSearch.BillPeriodFrom != null ? string.Format("{0:yyyy/MM/dd}", listSearch.BillPeriodFrom) : "";
                onePage.BillPeriodTo = listSearch.BillPeriodTo != null ? string.Format("{0:yyyy/MM/dd}", listSearch.BillPeriodTo) : "";
                onePage.BillIssuedClassification = listSearch.BillIssuedClassification != null ? listSearch.BillIssuedClassification.StringValue : "";
                onePage.OutputType = listSearch.BillTypeOrder.StringValue;

            }

            private List<BillCheckListTotalData> InitChildList(List<BillCheckListGridData> listPerPage, BillCheckListTotalData listTotal)
            {
                var listChild = new List<BillCheckListTotalData>();

                var temp = new BillCheckListTotalData()
                {
                    QuantityNumberTotal = 0,
                    TaxAmountTotal = 0,
                    UnitNumberTotal = 0,
                    SalesAmountTotal = 0,
                    CommissionAmount = 0,
                    BillAmountTotal = 0,
                    DepositAmountTotal = 0,
                    UnpaidAmountTotal = 0
                };
                temp.Text = "頁計";
                listPerPage.ForEach(e =>
                {
                    temp.QuantityNumberTotal += e.QuantityNumber;
                    temp.UnitNumberTotal += e.UnitNumber;
                    temp.SalesAmountTotal += e.SalesAmount;
                    temp.TaxAmountTotal += e.TaxAmount;
                    temp.CommissionAmount += e.CommissionAmount;
                    temp.BillAmountTotal += e.BillAmount;
                    temp.DepositAmountTotal += e.DepositAmount;
                    temp.UnpaidAmountTotal += e.UnpaidAmount;
                });
                temp.UserCode = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd;
                temp.UserName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
                listChild.Add(temp);
                // Add total
                listTotal.Text = "累計";
                listPerPage.ForEach(e =>
                {
                    listTotal.QuantityNumberTotal += e.QuantityNumber;
                    listTotal.UnitNumberTotal += e.UnitNumber;
                    listTotal.SalesAmountTotal += e.SalesAmount;
                    listTotal.TaxAmountTotal += e.TaxAmount;
                    listTotal.CommissionAmount += e.CommissionAmount;
                    listTotal.BillAmountTotal += e.BillAmount;
                    listTotal.DepositAmountTotal += e.DepositAmount;
                    listTotal.UnpaidAmountTotal += e.UnpaidAmount;
                });
                listTotal.UserCode = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCd;
                listTotal.UserName = new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name;
                BillCheckListTotalData listTotalAdd = new BillCheckListTotalData(listTotal);
                listChild.Add(listTotalAdd);

                return listChild;
            }

            
        }
    }
}

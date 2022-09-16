using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.BillPrint.Queries
{
    public class GetPaymentRequestAsyncQuery : IRequest<List<PaymentRequestReport>>
    {
        public BillPrintInput billPrintInput { get; set; }
        public class Handler : IRequestHandler<GetPaymentRequestAsyncQuery, List<PaymentRequestReport>>
        {
            private readonly KobodbContext _context;
            private readonly string Asterisk = "*";
            private readonly string Reissue = "(再)";
            private readonly string SubTitle = "(プレビュー版)";
            public Handler(KobodbContext context)
            {
                _context = context;
            }

            public async Task<List<PaymentRequestReport>> Handle(GetPaymentRequestAsyncQuery request, CancellationToken cancellationToken = default)
            {
                if (request.billPrintInput == null)
                {
                    return new List<PaymentRequestReport>();
                }
                //format data: MainInfo = [if1], TableInfo = [tif11,tif12,...], SeiComInfo
                PaymentRequestReport paymentRequestReport = new PaymentRequestReport();
                List<MainInfoReport> mainInfoReports = new List<MainInfoReport>();
                var claimModel = new ClaimModel();

                paymentRequestReport.seiComInfo = await _context.TkmKasSet.Where(x => x.CompanyCdSeq == claimModel.CompanyID).Select(x => new SeiComInfo()
                {
                    SeiCom1 = x.SeiCom1,
                    SeiCom2 = x.SeiCom2,
                    SeiCom3 = x.SeiCom3,
                    SeiCom4 = x.SeiCom4,
                    SeiCom5 = x.SeiCom5,
                    SeiCom6 = x.SeiCom6,
                    SeiGenFlg = x.SeiGenFlg,
                    SeiKrksKbn = x.SeiKrksKbn
                }).FirstOrDefaultAsync();

                var startBillAddress = (request.billPrintInput.startCustomerComponentGyosyaData == null ? "000"
                    : request.billPrintInput.startCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.billPrintInput.startCustomerComponentTokiskData == null ? "0000"
                    : request.billPrintInput.startCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.billPrintInput.startCustomerComponentTokiStData == null ? "0000"
                    : request.billPrintInput.startCustomerComponentTokiStData.SitenCd.ToString("D4"));
                var endBillAddress = (request.billPrintInput.endCustomerComponentGyosyaData == null ? "999"
                    : request.billPrintInput.endCustomerComponentGyosyaData.GyosyaCd.ToString("D3")) +
                    (request.billPrintInput.endCustomerComponentTokiskData == null ? "9999"
                    : request.billPrintInput.endCustomerComponentTokiskData.TokuiCd.ToString("D4")) +
                    (request.billPrintInput.endCustomerComponentTokiStData == null ? "9999"
                    : request.billPrintInput.endCustomerComponentTokiStData.SitenCd.ToString("D4"));
                PaymentRequestInputData paymentRequestInputData = new PaymentRequestInputData()
                {
                    BillingType = request.billPrintInput.BillingType ?? string.Empty,
                    EndBillAdd = endBillAddress,
                    EndSeiCdSeq = Convert.ToInt32(request.billPrintInput.endCustomerComponentTokiskData?.TokuiSeq),
                    EndSeiSitCdSeq = Convert.ToInt32(request.billPrintInput.endCustomerComponentTokiStData?.SitenCdSeq),
                    EndUkeNo = request.billPrintInput.EndRcpNum == null ? string.Empty : request.billPrintInput.EndRcpNum.GetValueOrDefault().ToString("D10"),
                    EndYoyaKbn = Convert.ToByte(request.billPrintInput.EndRsrCatDropDown?.YoyaKbn),
                    InTanCdSeq = claimModel.SyainCdSeq,
                    InvoiceOutNum = request.billPrintInput.InvoiceOutNum.GetValueOrDefault(),
                    InvoiceSerNum = (short)request.billPrintInput.InvoiceSerNum.GetValueOrDefault(),
                    Jyus1 = request.billPrintInput.Address1 ?? string.Empty,
                    Jyus2 = request.billPrintInput.Address2 ?? string.Empty,
                    KuriSyoriKbn = Convert.ToByte(paymentRequestReport.seiComInfo?.SeiKrksKbn),
                    OutDataTable = "",
                    PrnCpysTan = 0,
                    SeiEigCdSeq = Convert.ToInt32(request.billPrintInput.BillingOfficeDropDown?.Code),
                    SeiFutCanKbn = request.billPrintInput.CancelFeeBilTyp,
                    SeiFutFutKbn = request.billPrintInput.FutaiBilTyp,
                    SeiFutGuiKbn = request.billPrintInput.GuideFeeBilTyp,
                    SeiFutTehKbn = request.billPrintInput.ArrangementFeeBilTyp,
                    SeiFutTukKbn = request.billPrintInput.TollFeeBilTyp,
                    SeiFutTumKbn = request.billPrintInput.LoadedItemBilTyp,
                    SeiFutUncKbn = request.billPrintInput.FareBilTyp,
                    SeiGenFlg = Convert.ToByte(paymentRequestReport.seiComInfo?.SeiGenFlg),
                    SeiHatYmd = request.billPrintInput.IssueYmd.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                    SeikYm = request.billPrintInput.InvoiceYm == null ? string.Empty : ((DateTime)request.billPrintInput.InvoiceYm).ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture),
                    SeiOutSyKbn = (byte)request.billPrintInput.PrintMode,
                    SeiOutTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmm),
                    SeiSitKbn = Convert.ToByte(request.billPrintInput.BillingAddressDropDown?.Code),
                    SimeD = Convert.ToByte(request.billPrintInput.ClosingDate.GetValueOrDefault()),
                    SitenNm = request.billPrintInput.CustomerBrchNm ?? string.Empty,
                    SiyoKbn = 1,
                    StartBillAdd = startBillAddress,
                    StaSeiCdSeq = Convert.ToInt32(request.billPrintInput.startCustomerComponentTokiskData?.TokuiSeq),
                    StaSeiSitCdSeq = Convert.ToInt32(request.billPrintInput.startCustomerComponentTokiStData?.SitenCdSeq),
                    StaUkeNo = request.billPrintInput.StartRcpNum == null ? string.Empty : request.billPrintInput.StartRcpNum.GetValueOrDefault().ToString("D10"),
                    StaYoyaKbn = Convert.ToByte(request.billPrintInput.StartRsrCatDropDown?.YoyaKbn),
                    TenantCdSeq = new ClaimModel().TenantID,
                    TokuiNm = request.billPrintInput.CustomerNm ?? string.Empty,
                    UpdPrgID = claimModel.SyainCd,
                    UpdSyainCd = claimModel.SyainCdSeq,
                    UpdTime = DateTime.Now.ToString(Commons.Constants.Formats.HHmmss),
                    UpdYmd = DateTime.Now.ToString(Commons.Constants.Formats.yyyyMMdd),
                    ZipCd = request.billPrintInput.ZipCode ?? string.Empty,
                    TesPrnKbn = Convert.ToByte(request.billPrintInput.HandlingCharPrtDropDown?.Code),
                    RStaUkeNo = Convert.ToByte(request.billPrintInput.StartRsrCatDropDown?.YoyaKbn),
                    REndUkeNo = Convert.ToByte(request.billPrintInput.EndRsrCatDropDown?.YoyaKbn)
                };

                if (request.billPrintInput.OutDataTables.Any())
                {
                    for (int i = 0; i < request.billPrintInput.OutDataTables.Count(); i++)
                    {
                        paymentRequestInputData.OutDataTable += request.billPrintInput.OutDataTables[i].UkeNo.ToString() +
                            request.billPrintInput.OutDataTables[i].SeiFutSyu.ToString() + request.billPrintInput.OutDataTables[i].FutuUnkRen.ToString() +
                            request.billPrintInput.OutDataTables[i].FutTumRen.ToString() + (i == request.billPrintInput.OutDataTables.Count() - 1 ? "" : ",");
                    }
                }

                _context.LoadStoredProc(request.billPrintInput.IsPreview ? "PK_dPaymentRequestPreview_R" : "PK_dPaymentRequest_R")
                    .AddParam("SeikYm", paymentRequestInputData.SeikYm)
                    .AddParam("SeiHatYmd", paymentRequestInputData.SeiHatYmd)
                    .AddParam("SeiOutTime", paymentRequestInputData.SeiOutTime)
                    .AddParam("InTanCdSeq", paymentRequestInputData.InTanCdSeq)
                    .AddParam("SeiOutSyKbn", paymentRequestInputData.SeiOutSyKbn)
                    .AddParam("SeiGenFlg", paymentRequestInputData.SeiGenFlg)
                    .AddParam("StaUkeNo", paymentRequestInputData.StaUkeNo)
                    .AddParam("EndUkeNo", paymentRequestInputData.EndUkeNo)
                    .AddParam("RStaYoyaKbn", paymentRequestInputData.RStaUkeNo)
                    .AddParam("REndYoyaKbn", paymentRequestInputData.REndUkeNo)
                    .AddParam("StaYoyaKbn", paymentRequestInputData.StaYoyaKbn)
                    .AddParam("EndYoyaKbn", paymentRequestInputData.EndYoyaKbn)
                    .AddParam("SeiEigCdSeq", paymentRequestInputData.SeiEigCdSeq)
                    .AddParam("SeiSitKbn", paymentRequestInputData.SeiSitKbn)
                    .AddParam("StaSeiCdSeq", paymentRequestInputData.StaSeiCdSeq)
                    .AddParam("StaSeiSitCdSeq", paymentRequestInputData.StaSeiSitCdSeq)
                    .AddParam("EndSeiCdSeq", paymentRequestInputData.EndSeiCdSeq)
                    .AddParam("EndSeiSitCdSeq", paymentRequestInputData.EndSeiSitCdSeq)
                    .AddParam("SimeD", paymentRequestInputData.SimeD)
                    .AddParam("PrnCpysTan", paymentRequestInputData.PrnCpysTan)
                    .AddParam("TesPrnKbn", paymentRequestInputData.TesPrnKbn)
                    .AddParam("SeiFutUncKbn", paymentRequestInputData.SeiFutUncKbn)
                    .AddParam("SeiFutFutKbn", paymentRequestInputData.SeiFutFutKbn)
                    .AddParam("SeiFutTukKbn", paymentRequestInputData.SeiFutTukKbn)
                    .AddParam("SeiFutTehKbn", paymentRequestInputData.SeiFutTehKbn)
                    .AddParam("SeiFutGuiKbn", paymentRequestInputData.SeiFutGuiKbn)
                    .AddParam("SeiFutTumKbn", paymentRequestInputData.SeiFutTumKbn)
                    .AddParam("SeiFutCanKbn", paymentRequestInputData.SeiFutCanKbn)
                    .AddParam("ZipCd", paymentRequestInputData.ZipCd)
                    .AddParam("Jyus1", paymentRequestInputData.Jyus1)
                    .AddParam("Jyus2", paymentRequestInputData.Jyus2)
                    .AddParam("TokuiNm", paymentRequestInputData.TokuiNm)
                    .AddParam("SitenNm", paymentRequestInputData.SitenNm)
                    .AddParam("SiyoKbn", paymentRequestInputData.SiyoKbn)
                    .AddParam("UpdYmd", paymentRequestInputData.UpdYmd)
                    .AddParam("UpdTime", paymentRequestInputData.UpdTime)
                    .AddParam("UpdSyainCd", paymentRequestInputData.UpdSyainCd)
                    .AddParam("UpdPrgID", paymentRequestInputData.UpdPrgID)
                    .AddParam("KuriSyoriKbn", paymentRequestInputData.KuriSyoriKbn)
                    .AddParam("TenantCdSeq", paymentRequestInputData.TenantCdSeq)
                    .AddParam("StartBillAdd", paymentRequestInputData.StartBillAdd)
                    .AddParam("EndBillAdd", paymentRequestInputData.EndBillAdd)
                    .AddParam("InvoiceOutNum", paymentRequestInputData.InvoiceOutNum)
                    .AddParam("InvoiceSerNum", paymentRequestInputData.InvoiceSerNum)
                    .AddParam("BillingType", paymentRequestInputData.BillingType)
                    .AddParam("OutDataTable", paymentRequestInputData.OutDataTable)
                    .AddParam("SeiOutSeq", out IOutParam<int> seiOutSeq)
                    .AddParam("ReturnCd", out IOutParam<int> returnCd)
                    .AddParam("RowCount", out IOutParam<int> rowCount)
                    .AddParam("ReturnMsg", " ", out IOutParam<string> returnMsg)
                    .AddParam("eProcedure", " ", out IOutParam<string> eProcedure)
                    .AddParam("eLine", " ", out IOutParam<string> eLine)
                    .Exec(r =>
                    {
                        mainInfoReports = r.ToList<MainInfoReport>();
                        r.NextResult();
                        paymentRequestReport.TableReports = r.ToList<TableReport>();
                    });

                List<PaymentRequestReport> paymentRequestReportList = new List<PaymentRequestReport>();
                mainInfoReports.ForEach(e =>
                {
                    var mainInfo = mainInfoReports.Where(x => x.SeiOutSeq == e.SeiOutSeq && x.SeiRen == e.SeiRen).FirstOrDefault();
                    if (paymentRequestReport.seiComInfo.SeiKrksKbn != 1)
                    {
                        mainInfo.ZenKurG = 0;
                    }
                    if (paymentRequestInputData.TesPrnKbn != 1)
                    {
                        mainInfo.KonTesG = 0;
                    }
                    mainInfo.SeikYmDateTime = DateTime.TryParseExact(mainInfo.SeikYm + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime DateValue) ?
                    DateTime.ParseExact(mainInfo.SeikYm + "01", "yyyyMMdd", null) : (DateTime?)null;
                    mainInfo.SeiHatYmdDateTime = DateTime.ParseExact(mainInfo.SeiHatYmd, "yyyyMMdd", CultureInfo.InvariantCulture);
                    mainInfo.Reissue = (paymentRequestReport.seiComInfo.SeiGenFlg == 0 && request.billPrintInput.PrintMode == (int)PaymentRequestPrintMode.BillNumberChosenPrint) ? Reissue : string.Empty;
                    mainInfo.SeiOutSeqNm = mainInfo.SeiOutSeq.ToString("D8");
                    mainInfo.SeiRenNm = mainInfo.SeiRen.ToString("D4");
                    mainInfo.ZipCd = "〒 " + mainInfo.ZipCd;
                    mainInfo.SeiEigZipCd = "〒 " + mainInfo.SeiEigZipCd;
                    mainInfo.SubTitle = request.billPrintInput.IsPreview ? SubTitle : string.Empty;

                    var tableInfo = paymentRequestReport.TableReports.Where(x => x.SeiOutSeq == mainInfo.SeiOutSeq && x.SeiRen == mainInfo.SeiRen).ToList();
                    foreach (var item in tableInfo)
                    {
                        item.ReissueKbn = item.SeiSaHKbn == 2 ? Asterisk : string.Empty;
                        item.HasYmd = StringExtensions.AddSlash2YYYYMMDD(item.HasYmd);
                        item.HaiSymd = StringExtensions.AddSlash2YYYYMMDD(item.HaiSymd);
                        item.TouYmd = StringExtensions.AddSlash2YYYYMMDD(item.TouYmd);
                        item.SyaSyuNmDisplay = item.Suryo == 0 ? (item.SyaSyuNm + System.Environment.NewLine + string.Empty) : (item.SyaSyuNm + System.Environment.NewLine + item.Suryo);
                    }
                    var newPaymentRequestReport = new PaymentRequestReport()
                    {
                        MainInfoReport = mainInfo,
                        TableReports = tableInfo,
                        seiComInfo = paymentRequestReport.seiComInfo
                    };
                    paymentRequestReportList.Add(newPaymentRequestReport);
                });

                return paymentRequestReportList;
            }
        }
    }
}

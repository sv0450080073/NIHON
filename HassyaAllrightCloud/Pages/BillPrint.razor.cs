using DevExpress.Blazor;
using DevExpress.Blazor.Reporting;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.CommonComponents;
using HassyaAllrightCloud.Reports.DataSource;
using HassyaAllrightCloud.Reports.ReportFactory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SharedLibraries.UI.Services;
using SharedLibraries.Utility.Constant;
using SharedLibraries.Utility.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class BillPrintBase : ComponentBase
    {
        [CascadingParameter(Name = "ClaimModel")]
        protected ClaimModel ClaimModel { get; set; }
        [Inject]
        public IStringLocalizer<BillPrint> Lang { get; set; }
        [Inject]
        public IJSRuntime jSRuntime { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        [Inject]
        public IBillPrintService _billPrintService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [Inject] protected ISharedLibrariesApi _s3Service { get; set; }
        [Inject]
        public ILoadingService loadingService { get; set; }
        [Inject] ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject] IReportLayoutSettingService _reportLayoutSettingService { get; set; }
        [Inject] protected IReservationClassComponentService ReservationClassComponentService { get; set; }
        [Inject] protected ICustomerComponentService CustomerComponentService { get; set; }
        [Inject] IWebHostEnvironment _webHostEnvironment { get; set; }
        [Parameter] public int OutSiji { get; set; } = (int)PaymentRequestPrintMode.Print;
        [Parameter] public EventCallback<int> OutSijiChanged { get; set; }
        [Parameter] public string Option { get; set; }
        [Parameter]
        public List<OutDataTable> OutDataTable { get; set; } = new List<OutDataTable>() { };
        [Parameter] public EventCallback<List<OutDataTable>> OutDataTableChanged { get; set; }
        [Parameter] public string lstInfo { get; set; }
        public readonly string BillAddress = "請求先";
        public readonly string CustomerAddress = "得意先";
        public readonly string Exist = "あり";
        public readonly string None = "なし";
        public readonly string NullTextBillAddress = "請求先コード : 請求先名　支店コード : 支店名";
        public readonly string PaymentRequestReport = "PaymentRequestReport";
        public readonly string RcpNum = "RcpNum";
        public readonly string StartRsrCat = "StartRsrCat";
        public readonly string EndRsrCat = "EndRsrCat";
        public readonly string StartBillAdd = "StartBillAdd";
        public readonly string EndBillAdd = "EndBillAdd";
        public readonly string BillingOffice = "BillingOffice";
        public readonly string BillingAddress = "BillingAddress";
        public readonly string HandlingCharPrt = "HandlingCharPrt";
        public readonly string Printed = "出力完了しました。";
        public readonly int[] printOptions = new int[] { 2, 3, 4 };
        public readonly int[] printOptions2 = new int[] { 4 };
        public readonly int[] printOptions3 = new int[] { 1, 4 };

        public BillPrintInput billPrint { get; set; } = new BillPrintInput();
        public EditContext editFormContext { get; set; }
        public List<DropDown> billingOfficeDatas { get; set; }
        public List<DropDown> specifyBillingAddresses { get; set; } = new List<DropDown>();
        public List<DropDown> handlingCharPrts { get; set; } = new List<DropDown>();
        public List<OutDataTableOutput> outDataTableOutputs { get; set; } = new List<OutDataTableOutput>();
        public List<ReservationClassComponentData> ListReservationClass { get; set; } = new List<ReservationClassComponentData>();
        protected List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        protected List<CustomerComponentTokiskData> ListTokisk { get; set; } = new List<CustomerComponentTokiskData>();
        protected List<CustomerComponentTokiStData> ListTokiSt { get; set; } = new List<CustomerComponentTokiStData>();
        public List<SharedLibraries.Utility.Models.S3File> printedFiles { get; set; } = new List<SharedLibraries.Utility.Models.S3File>();
        public DxDocumentViewer documentViewer = new DxDocumentViewer();
        public List<int> billingTypes { get; set; } = new List<int>();
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public ClaimModel claimModel { get; set; } = new ClaimModel();
        public Components.CommonComponents.CustomerComponent startCustomerComponent { get; set; }
= new Components.CommonComponents.CustomerComponent();
        public Components.CommonComponents.CustomerComponent endCustomerComponent { get; set; }
        = new Components.CommonComponents.CustomerComponent();

        public HassyaAllrightCloud.Pages.Components.CommonComponents.DateInputComponent dateInputComponent { get; set; }
            = new HassyaAllrightCloud.Pages.Components.CommonComponents.DateInputComponent();
        public long codeStartRcpNum { get; set; }
        public long codeEndRcpNum { get; set; }
        public string noDataMessage { get; set; }
        public bool iShowPrintedModal { get; set; }
        public bool isLoading { get; set; }
        public int ModeOutput { get; set; }
        public int syainCdSeq { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                 await jSRuntime.InvokeVoidAsync("EnterTab", ".enterField", false);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                editFormContext = new EditContext(billPrint);
                ModeOutput = (int)OutputInstruction.Pdf;
                billingOfficeDatas = await _billPrintService.GetBillingOfficeDatasAsync();
                ListReservationClass = await ReservationClassComponentService.GetListReservationClass();
                ListGyosya = await CustomerComponentService.GetListGyosya();
                ListTokisk = await CustomerComponentService.GetListTokisk();
                ListTokiSt = await CustomerComponentService.GetListTokiSt();
                LangDic = Lang.GetAllStrings().ToDictionary(l => l.Name, l => l.Value);
                if (lstInfo != null) {
                    var searchParams = EncryptHelper.DecryptFromUrl<OutDataTableModel>(lstInfo);
                    if(searchParams != null)
                    {
                        OutDataTable = searchParams.outDataTables;
                        OutSiji = (OutDataTable == null || !OutDataTable.Any()) ? OutSiji : OutDataTable.FirstOrDefault().supOutSiji;
                    }
                    await GetInitData();
                } else {
                    await GetInitData();
                    if (ClaimModel != null)
                        syainCdSeq = ClaimModel.SyainCdSeq;
                    List<TkdInpCon> filterValues = await FilterConditionService.GetFilterCondition(FormFilterName.BillPrint, 0, syainCdSeq);
                    if (filterValues.Any())
                    {
                        foreach (var item in filterValues)
                    {
                        var propertyInfo = billPrint.GetType().GetProperty(item.ItemNm);
                        switch (item.ItemNm)
                        {
                            case nameof(billPrint.StartRcpNum):
                            case nameof(billPrint.EndRcpNum):
                                propertyInfo.SetValue(billPrint, string.IsNullOrWhiteSpace(item.JoInput) ? (long?)null : Convert.ToInt64(item.JoInput), null);
                                break;
                            case nameof(billPrint.ClosingDate):
                                propertyInfo.SetValue(billPrint, string.IsNullOrWhiteSpace(item.JoInput) ? (int?)null : Convert.ToInt32(item.JoInput), null);
                                break;
                            case nameof(billPrint.StartRsrCatDropDown):
                            case nameof(billPrint.EndRsrCatDropDown):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                {
                                    var reservationClassification = ListReservationClass.Where(x => x.YoyaKbnSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                    propertyInfo.SetValue(billPrint, reservationClassification, null);
                                }
                                break;
                            case nameof(billPrint.BillingOfficeDropDown):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    billPrint.BillingOfficeDropDown = billingOfficeDatas.Where(x => x.Code == item.JoInput).FirstOrDefault();
                                break;
                            case nameof(billPrint.BillingAddressDropDown):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                    billPrint.BillingAddressDropDown = specifyBillingAddresses.Where(x => x.Code == item.JoInput).FirstOrDefault();
                                break;
                            case nameof(billPrint.startCustomerComponentGyosyaData):
                            case nameof(billPrint.endCustomerComponentGyosyaData):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                {
                                    var gyosya = ListGyosya.Where(x => x.GyosyaCdSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                    propertyInfo.SetValue(billPrint, gyosya, null);
                                    if (item.ItemNm == nameof(billPrint.startCustomerComponentGyosyaData))
                                        startCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                                    else
                                        endCustomerComponent.OnChangeDefaultGyosya(gyosya.GyosyaCdSeq);
                                }
                                break;
                            case nameof(billPrint.startCustomerComponentTokiskData):
                            case nameof(billPrint.endCustomerComponentTokiskData):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                {
                                    var tokisk = ListTokisk.Where(x => x.TokuiSeq == int.Parse(item.JoInput)).FirstOrDefault();
                                    propertyInfo.SetValue(billPrint, tokisk, null);
                                    if (item.ItemNm == nameof(billPrint.startCustomerComponentGyosyaData))
                                        startCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                                    else
                                        endCustomerComponent.OnChangeDefaultTokisk(tokisk.TokuiSeq);
                                }
                                break;
                            case nameof(billPrint.startCustomerComponentTokiStData):
                            case nameof(billPrint.endCustomerComponentTokiStData):
                                if (!string.IsNullOrWhiteSpace(item.JoInput))
                                {
                                    var tokist = ListTokiSt.Where(x => x.TokuiSeq.ToString() + x.SitenCdSeq.ToString() == item.JoInput).FirstOrDefault();
                                    propertyInfo.SetValue(billPrint, tokist, null);
                                    if (item.ItemNm == nameof(billPrint.startCustomerComponentGyosyaData))
                                        startCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                                    else
                                        endCustomerComponent.OnChangeDefaultTokiSt(tokist.SitenCdSeq);
                                }
                                break;
                            case nameof(billPrint.HandlingCharPrtDropDown):
                                if(!string.IsNullOrWhiteSpace(item.JoInput))
                                    billPrint.HandlingCharPrtDropDown = handlingCharPrts.Where(x => x.Code == item.JoInput).FirstOrDefault();
                                break;
                            case nameof(billPrint.FareBilTyp):
                            case nameof(billPrint.FutaiBilTyp):
                            case nameof(billPrint.TollFeeBilTyp):
                            case nameof(billPrint.ArrangementFeeBilTyp):
                            case nameof(billPrint.GuideFeeBilTyp):
                            case nameof(billPrint.LoadedItemBilTyp):
                            case nameof(billPrint.CancelFeeBilTyp):
                                propertyInfo.SetValue(billPrint, byte.Parse(item.JoInput), null);
                                break;
                            case nameof(billPrint.BillingType):
                                propertyInfo.SetValue(billPrint, item.JoInput, null);
                                billingTypes = string.IsNullOrWhiteSpace(item.JoInput) ? new List<int>() : item.JoInput.Split(",").Select(int.Parse).ToList();
                                foreach (var checkbox in billPrint.checkBoxFilters)
                                {
                                    if (billingTypes.Any(x => x == checkbox.Number))
                                    {
                                        checkbox.IsChecked = true;
                                    }
                                }
                                break;
                        }
                    }
                    }
                }
                //dateInputComponent.OnDateChanged(billPrint.IssueYmd);
                /*CHECK PARAM*/
                if (Option == PrintMode.Preview.ToString() || Option == PrintMode.SaveAsPDF.ToString())
                {
                    NavManager.NavigateTo("/billprint", false);
                    if (Option == PrintMode.Preview.ToString())
                        await Preview();
                    if (Option == PrintMode.SaveAsPDF.ToString())
                        await PrintPage();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task GetInitData()
        {
            try
            {
                GetCustomData();
                await GetDataFromParameter();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void GetCustomData()
        {
            try
            {
                specifyBillingAddresses.Add(new DropDown
                {
                    Code = "1",
                    Name = BillAddress,
                    CodeText = BillAddress
                });
                specifyBillingAddresses.Add(new DropDown
                {
                    Code = "2",
                    Name = CustomerAddress,
                    CodeText = CustomerAddress
                });
                handlingCharPrts.Add(new DropDown
                {
                    Code = "1",
                    Name = Exist,
                    CodeText = Exist
                });
                handlingCharPrts.Add(new DropDown
                {
                    Code = "2",
                    Name = None,
                    CodeText = None
                });
                billPrint.PrintMode = OutSiji;
                billPrint.IssueYmd = DateTime.UtcNow;
                billPrint.BillingAddress = specifyBillingAddresses.FirstOrDefault().Code;
                billPrint.BillingAddressDropDown = specifyBillingAddresses.FirstOrDefault();
                billPrint.OutDataTables = OutDataTable;
                billPrint.HandlingCharPrt = handlingCharPrts.FirstOrDefault().Code;
                billPrint.HandlingCharPrtDropDown = handlingCharPrts.FirstOrDefault();
                billPrint.BillingOffice = billingOfficeDatas.FirstOrDefault() == null ? 0 : int.Parse(billingOfficeDatas.FirstOrDefault().Code);
                billPrint.BillingOfficeDropDown = billingOfficeDatas.FirstOrDefault();
                //checkbox
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType1", Name = Lang["FareBilTyp"], Number = 1 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType2", Name = Lang["FutaiBilTyp"], Number = 2 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType3", Name = Lang["TollFeeBilTyp"], Number = 3 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType4", Name = Lang["ArrangementFeeBilTyp"], Number = 4 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType5", Name = Lang["GuideFeeBilTyp"], Number = 5 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType6", Name = Lang["LoadedItemBilTyp"], Number = 6 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType7", Name = Lang["CancelFeeBilTyp"], Number = 7 });
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task GetDataFromParameter()
        {
            try
            {
                outDataTableOutputs = await _billPrintService.GetOutDataTableAsync(OutDataTable.Any() ? OutDataTable.FirstOrDefault().UkeNo : string.Empty);
                if (!outDataTableOutputs.Any())
                    return;
                DateTime DateValue;
                if(OutSiji == (int)PaymentRequestPrintMode.Print)
                    billPrint.InvoiceYm = DateTime.TryParseExact(outDataTableOutputs.FirstOrDefault().SyoriYm, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateValue) ?
                    DateTime.ParseExact(outDataTableOutputs.FirstOrDefault().SyoriYm, "yyyyMM", null).AddMonths(1) : DateTime.Now;
                if (OutSiji == (int)PaymentRequestPrintMode.IndependPrint)
                {
                    billPrint.StartRcpNum = Convert.ToInt64(OutDataTable.FirstOrDefault().UkeNo.Substring(5));
                    billPrint.EndRcpNum = Convert.ToInt64(OutDataTable.FirstOrDefault().UkeNo.Substring(5));
                }
                billPrint.BillingOffice = outDataTableOutputs.FirstOrDefault().SeiEigyoCdSeq;
                billPrint.ZipCode = outDataTableOutputs.FirstOrDefault().SeikyZipCd;
                billPrint.Address1 = outDataTableOutputs.FirstOrDefault().SeikyJyus1;
                billPrint.Address2 = outDataTableOutputs.FirstOrDefault().SeikyJyus2;
                billPrint.CustomerNm = outDataTableOutputs.FirstOrDefault().SeikyTokuiNm;
                billPrint.CustomerBrchNm = outDataTableOutputs.FirstOrDefault().SeikySitenNm;
                billPrint.PrintMode = OutSiji;
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        public async Task UpdateFormValue(string propertyName, dynamic value)
        {
            try
            {
                noDataMessage = string.Empty;
                var propertyInfo = billPrint.GetType().GetProperty(propertyName);
                propertyInfo.SetValue(billPrint, value, null);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task OnDateInputChange(string propertyName, string value)
        {
            try
            {
                var date = CommonHelper.ConvertToDateTime(value);
                if (date == null)
                    return;
                await UpdateFormValue(propertyName, date);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task PrintPage()
        {
            try
            {
                isLoading = true;
                StateHasChanged();
                billPrint.IsPreview = false;
                printedFiles.Clear();

                if (billPrint.PrintMode == (int)PaymentRequestPrintMode.BillNumberChosenPrint)
                {
                    string seiFileId = await _billPrintService.GetSeiFileIdAsync(billPrint.InvoiceOutNum.GetValueOrDefault(), (short)billPrint.InvoiceSerNum.GetValueOrDefault());
                    if (string.IsNullOrWhiteSpace(seiFileId))
                        await PrintProcess();
                    else
                    {
                        var s3File = await _s3Service.GetSingleFileAsync(seiFileId);
                        printedFiles.Add(s3File);
                        iShowPrintedModal = true;
                    }
                }
                else
                    await PrintProcess();

                if (printedFiles.Any())
                {
                    keyValueFilterPairs = await GenerateFilterValueDictionaryService.GenerateForBillPrint(billPrint);
                    await FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.BillPrint, 0, syainCdSeq);
                }
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                errorModalService.HandleError(ex);
                StateHasChanged();
            }
        }

        public async Task PrintProcess()
        {
            try
            {
                List<PaymentRequestReport> paymentRequestReportPdf = await _billPrintService.GetPaymentRequestAsync(billPrint);
                if (!paymentRequestReportPdf.Any())
                {
                    noDataMessage = Lang["NoData"];
                    StateHasChanged();
                    return;
                }

                List<PaymentRequestGroup> paymentRequestGroups = paymentRequestReportPdf.Select(x => new PaymentRequestGroup()
                {
                    SeiOutSeq = x.MainInfoReport.SeiOutSeq,
                    SeiRen = x.MainInfoReport.SeiRen
                }).Distinct().ToList();

                var reportImage = await GetReportImage();
                var tenantInfo = await _billPrintService.GetTenantInfoAsync();
                var result = await Task.WhenAll(paymentRequestGroups.Select(g => UploadFile(g, paymentRequestReportPdf, tenantInfo, reportImage)));
                await _billPrintService.SaveFileIdToTKDSeiKyuAsync(result.Where(x => x != null).ToList());
                if(result.Any(x => x == null))
                    errorModalService.ShowErrorPopup("エラー", Lang["NoInvoiceOutput"]);

                iShowPrintedModal = result.All(x => x != null);
            } catch(Exception ex)
            {
                iShowPrintedModal = false;
                errorModalService.HandleError(ex);
            }
        }
        private async Task<PaymentRequestGroup> UploadFile(PaymentRequestGroup paymentRequestGroup, List<PaymentRequestReport> paymentRequestReportPdf, PaymentRequestTenantInfo tenantInfo, Image reportImage)
        {
            try
            {
                var reportPdfs = paymentRequestReportPdf.Where(x => x.MainInfoReport.SeiOutSeq == paymentRequestGroup.SeiOutSeq
    && x.MainInfoReport.SeiRen == paymentRequestGroup.SeiRen);
                var reportPdf = reportPdfs.FirstOrDefault();
                var report = await _reportLayoutSettingService.GetCurrentTemplate(ReportIdForSetting.Billprint, BaseNamespace.Billprint, new ClaimModel().TenantID, new ClaimModel().EigyoCdSeq, (byte)PaperSize.A4);
                report.DataSource = reportPdfs;
                report.BeforePrint += (sender, e) =>
                {
                    var temp = sender as XtraReport;
                    var lastCarryFowardAmountLabel = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LastCarryFowardAmountLabel")).FirstOrDefault();
                    var lastCarryFowardAmountValue = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LastCarryFowardAmountValue")).FirstOrDefault();
                    var lotsMaterialLabel = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LotsMaterialLabel")).FirstOrDefault();
                    var lotsMaterialValue = temp.AllControls<XRLabel>().Where(_ => _.Name.Contains("LotsMaterialValue")).FirstOrDefault();

                    lastCarryFowardAmountLabel.Visible = reportPdfs.Any() ? reportPdfs.FirstOrDefault().MainInfoReport.ZenKurG != 0 : true;
                    lastCarryFowardAmountValue.Visible = reportPdfs.Any() ? reportPdfs.FirstOrDefault().MainInfoReport.ZenKurG != 0 : true;
                    lotsMaterialLabel.Visible = reportPdfs.Any() ? reportPdfs.FirstOrDefault().MainInfoReport.KonTesG != 0 : true;
                    lotsMaterialValue.Visible = reportPdfs.Any() ? reportPdfs.FirstOrDefault().MainInfoReport.KonTesG != 0 : true;
                };

                if(reportImage != null)
                {
                    if(report.Report.ControlType.Contains("PaymentRequestPreviewReport1A4"))
                        (report as Reports.PaymentRequestPreviewReport1A4).SetImageSource(reportImage);
                    else
                        (report as Reports.PaymentRequestPreviewReport2A4).SetImageSource(reportImage);
                }

                report.CreateDocument();
                using (MemoryStream ms = new MemoryStream())
                {
                    report.ExportToPdf(ms);
                    byte[] exportedFileBytes = ms.ToArray();
                    var data = new SharedLibraries.Utility.Models.FileSendData()
                    {
                        File = exportedFileBytes,
                        FileName = $"{tenantInfo.TenantCdSeq.ToString("D5")}-{paymentRequestGroup.SeiOutSeq.ToString("D8")}-{paymentRequestGroup.SeiRen.ToString("D4")}_請求書.pdf",
                        FilePath = $"{tenantInfo.TenantCdSeq.ToString("D5")}-{tenantInfo.TenantCompanyName}" +
                        $"/{reportPdf.MainInfoReport.TokuiSeq.ToString("D8")}-{reportPdf.MainInfoReport.SeiTokuiNm}" +
                        $"/請求書/{reportPdf.MainInfoReport.SeiHatYmd}",
                        Password = string.Empty,
                        FileSize = ms.Capacity,
                        TenantId = claimModel.TenantID,
                        UpdSyainCd = claimModel.SyainCdSeq,
                        UpdPrgID = string.Empty
                    };
                    var f = await _s3Service.UploadFileAsync(data);
                    printedFiles.Add(f);
                    paymentRequestGroup.SeiFileId = f.EncryptedId;
                    return paymentRequestGroup;
                }
            }
            catch (Exception ex)
            {
                //errorModalService.ShowErrorPopup("エラー", Lang["NoInvoiceOutput"]);
                return null;
            }
        }

        private async Task<Image> GetReportImage()
        {
            try
            {
                var fileId = await _billPrintService.GetCompanyFileId();
                if (!string.IsNullOrWhiteSpace(fileId) && !string.IsNullOrEmpty(fileId))
                {
                    var file = await _s3Service.DownloadFileAsync(new SharedLibraries.Utility.Models.DownloadModel()
                    {
                        FileId = fileId,
                        Password = string.Empty,
                        UpdSyainCd = new ClaimModel().SyainCdSeq,
                        UpdPrgID = CommonConst.UpdPrgID
                    });
                    if (file != null)
                    {
                        var byteArr = Convert.FromBase64String(file.Content);
                        using (var mem = new MemoryStream(byteArr))
                        {
                            return Image.FromStream(mem);
                        }
                    }
                }
                return null;
            } catch(Exception ex)
            {
                return null;
            }
        }

        public async void FilterChanged(CheckBoxFilter checkBoxFilter)
        {
            try
            {
                checkBoxFilter.IsChecked = !checkBoxFilter.IsChecked;
                switch (checkBoxFilter.Number)
                {
                    case 1:
                        billPrint.FareBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 2:
                        billPrint.FutaiBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 3:
                        billPrint.TollFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 4:
                        billPrint.ArrangementFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 5:
                        billPrint.GuideFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 6:
                        billPrint.LoadedItemBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                    case 7:
                        billPrint.CancelFeeBilTyp = checkBoxFilter.IsChecked ? (byte)1 : (byte)2;
                        break;
                }
                if (billingTypes.Any(x => x == checkBoxFilter.Number))
                {
                    billingTypes.Remove(checkBoxFilter.Number);
                }
                else
                {
                    billingTypes.Add(checkBoxFilter.Number);
                }
                billPrint.BillingType = string.Join(",", billingTypes);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        //public async void DropDownChanged(DropDown dropDown, string name)
        //{
        //    try
        //    {
        //        if (name == HandlingCharPrt)
        //        {
        //            billPrint.HandlingCharPrt = dropDown.Code;
        //            billPrint.HandlingCharPrtDropDown = dropDown;
        //        }
        //        if (name == BillingOffice)
        //        {
        //            billPrint.BillingOffice = int.Parse(dropDown.Code);
        //            billPrint.BillingOfficeDropDown = dropDown;
        //        }
        //        if (name == BillingAddress)
        //        {
        //            billPrint.BillingAddress = dropDown.Code;
        //            billPrint.BillingAddressDropDown = dropDown;
        //        }
        //        if (name == StartRsrCat)
        //        {
        //            billPrint.StartRsrCat = dropDown == null ? 0 : Convert.ToInt32(dropDown.Code);
        //            billPrint.StartRsrCatDropDown = dropDown;
        //        }
        //        if (name == EndRsrCat)
        //        {
        //            billPrint.EndRsrCat = dropDown == null ? 0 : Convert.ToInt32(dropDown.Code);
        //            billPrint.EndRsrCatDropDown = dropDown;
        //        }
        //        if (name == StartBillAdd)
        //        {
        //            billPrint.StartBillAdd = dropDown == null ? (long)0 : long.Parse(dropDown.Code);
        //            billPrint.StaSeiCdSeq = dropDown == null ? 0 : dropDown.SeiCdSeq;
        //            billPrint.StaSeiSitCdSeq = dropDown == null ? 0 : dropDown.SeiSitCdSeq;
        //            billPrint.StartBillAddDropDown = dropDown;
        //        }
        //        if (name == EndBillAdd)
        //        {
        //            billPrint.EndBillAdd = dropDown == null ? (long)0 : long.Parse(dropDown.Code);
        //            billPrint.EndSeiCdSeq = dropDown == null ? 0 : dropDown.SeiCdSeq;
        //            billPrint.EndSeiSitCdSeq = dropDown == null ? 0 : dropDown.SeiSitCdSeq;
        //            billPrint.EndBillAddDropDown = dropDown;
        //        }
        //        StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        errorModalService.HandleError(ex);
        //    }
        //}

        public bool isDisabled(int[] value)
        {
            try
            {
                return value.Any(x => x == billPrint.PrintMode);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return false;
            }
        }

        public async Task RefreshBillPrint(int printMode)
        {
            try
            {
                if (printMode == (int)PaymentRequestPrintMode.BillNumberChosenPrint)
                {
                    OutSiji = (int)PaymentRequestPrintMode.BillNumberChosenPrint;
                    billPrint = new BillPrintInput()
                    {
                        PrintMode = printMode,
                        InvoiceYm = DateTime.UtcNow,
                        IssueYmd = DateTime.UtcNow,
                        OutDataTables = OutDataTable,
                        BillingOffice = 1,
                        BillingOfficeDropDown = billingOfficeDatas.FirstOrDefault(),
                        BillingAddress = "1",
                        BillingAddressDropDown = specifyBillingAddresses.FirstOrDefault(),
                        HandlingCharPrt = "1",
                        HandlingCharPrtDropDown = handlingCharPrts.FirstOrDefault()
                    };
                }
                else
                {
                    OutSiji = (int)PaymentRequestPrintMode.Print;
                    billPrint = new BillPrintInput()
                    {
                        PrintMode = printMode,
                        InvoiceYm = DateTime.UtcNow,
                        IssueYmd = DateTime.UtcNow,
                        BillingAddress = "1",
                        BillingAddressDropDown = specifyBillingAddresses.FirstOrDefault(),
                        OutDataTables = OutDataTable,
                        HandlingCharPrt = "1",
                        HandlingCharPrtDropDown = handlingCharPrts.FirstOrDefault(),
                        BillingOffice = 1,
                        BillingOfficeDropDown = billingOfficeDatas.FirstOrDefault()
                    };
                }
                ModeOutput = (int)OutputInstruction.Pdf;
                editFormContext = new EditContext(billPrint);
                //checkbox
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType1", Name = Lang["FareBilTyp"], Number = 1 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType2", Name = Lang["FutaiBilTyp"], Number = 2 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType3", Name = Lang["TollFeeBilTyp"], Number = 3 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType4", Name = Lang["ArrangementFeeBilTyp"], Number = 4 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType5", Name = Lang["GuideFeeBilTyp"], Number = 5 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType6", Name = Lang["LoadedItemBilTyp"], Number = 6 });
                billPrint.checkBoxFilters.Add(new CheckBoxFilter() { Id = "billingType7", Name = Lang["CancelFeeBilTyp"], Number = 7 });
                await FilterConditionService.DeleteCustomFilerCondition(syainCdSeq, 0, FormFilterName.BillPrint);
                await GetDataFromParameter();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task Preview()
        {
            try
            {
                if (!editFormContext.Validate())
                {
                    StateHasChanged();
                    return;
                }
                isLoading = true;
                StateHasChanged();
                billPrint.IsPreview = true;
                List<PaymentRequestReport> paymentRequestReportPdf = await _billPrintService.GetPaymentRequestAsync(billPrint);
                if (!paymentRequestReportPdf.Any())
                {
                    noDataMessage = Lang["NoData"];
                    isLoading = false;
                    StateHasChanged();
                    return;
                }
                var searchString = EncryptHelper.EncryptToUrl(billPrint);
                isLoading = false;
                StateHasChanged();
                await jSRuntime.InvokeVoidAsync("openNewUrlInNewTab", "paymentrequestpreview?searchString=" + searchString);
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                errorModalService.HandleError(ex);
            }
        }

        protected async Task BtnStart()
        {
            try
            {
                if (!editFormContext.Validate())
                {
                    StateHasChanged();
                    return;
                }
                if (ModeOutput == (int)OutputInstruction.Pdf)
                    await PrintPage();
                if (ModeOutput == (int)OutputInstruction.Preview)
                    await Preview();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void SetAction(string action, S3File file)
        {
            try
            {
                switch (action)
                {
                    case "Download":
                        if (file.SiyoKbn == CommonConst.ActiveSiyoKbn)
                        {
                            var url = GetDownloadFileUrl(file.EncryptedId);
                            jSRuntime.InvokeVoidAsync("open", url, "_blank");
                        }
                        break;
                    case "Copy":
                        jSRuntime.InvokeVoidAsync("copyText", $"copy-url-{file.Name}");
                        break;
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected string GetDownloadFileUrl(string encryptedFileId)
        {
            try
            {
                return _s3Service.BuildDownloadUrl(NavManager.BaseUri, encryptedFileId);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                return string.Empty;
            }
        }
    }
}

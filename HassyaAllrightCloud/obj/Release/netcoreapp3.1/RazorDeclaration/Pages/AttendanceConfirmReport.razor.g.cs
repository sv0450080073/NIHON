#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReport.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3eb9887aecb52ac2a0ee7ca876fdd73ac6338eef"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Infrastructure.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Domain.Dto;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Commons.Constants;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReport.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReport.razor"
using DevExpress.XtraPrinting;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/attendanceconfirmreport")]
    public partial class AttendanceConfirmReport : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 283 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReport.razor"
       
    [Parameter]
    public string Option { get; set; }
    [Parameter]
    public string Date { get; set; }
    int activeTabIndex = 0;
    int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
    AttendanceConfirmReportData reportCondition = new AttendanceConfirmReportData();
    EditContext formContext;
    DateTime operationDate = DateTime.Today;
    List<CompanyChartData> companychartlst = new List<CompanyChartData>();
    List<BranchChartData> branchchartlst = new List<BranchChartData>();
    List<DepartureOfficeData> vehicledispatchofficelst = new List<DepartureOfficeData>();
    List<ReservationData> reservationlst = new List<ReservationData>();
    List<string> undeliveredlst = new List<string>();
    List<OutputOrderData> outputorderlst = new List<OutputOrderData>();
    List<string> sizeofpaperlst = new List<string>();
    IEnumerable<TPM_CodeKbDataReport> keyObjectiveslst = new List<TPM_CodeKbDataReport>();
    IEnumerable<TPM_CodeKbDataReport> instructionslst = new List<TPM_CodeKbDataReport>();
    List<TPM_CodeKbDataReport> listTemp = new List<TPM_CodeKbDataReport>();
    IEnumerable<TPM_CodeKbDataReport> SelectedKeyObjectivesItems { get; set; } = new List<TPM_CodeKbDataReport>();
    List<string> SelectedKeyObjectivesItemstmp = new List<string>();
    IEnumerable<TPM_CodeKbDataReport> SelectedInstructionsItems { get; set; } = new List<TPM_CodeKbDataReport>();
    List<string> SelectedInstructionsItemstmp { get; set; } = new List<string>();
    List<string> selectedlstKeyObjectives = new List<string>();
    List<string> selectedlstKeyObjectivestmp = new List<string>();
    List<string> selectedlstInstructions = new List<string>();
    List<string> selectedlstInstructionstmp = new List<string>();
    VpmSyain SyainNmItem = new VpmSyain();
    IEnumerable<CompanyChartData> SelectedCompanyItems { get; set; } = new List<CompanyChartData>();
    List<CompanyChartData> tmpcompanychart = new List<CompanyChartData>();
    CompanyChartData allcompany = new CompanyChartData();
    DepartureOfficeData allbranch = new DepartureOfficeData();
    ReservationData allreservation = new ReservationData();
    bool PopupVisible { get; set; } = false;
    bool PopupCheckData { get; set; } = false;
    bool checkCompanyAll { get; set; } = true;
    string showSelectedCompany = "";
    bool disabledTxtKeyObjectives { get; set; } = true;
    bool disabledTxtInstructions { get; set; } = true;

    #region string LocalizationInit
    string PageTitleReport;
    string FormTitle;
    string OperationDate;
    string Company;
    string VehicleDispatchOffice;
    string ReservationCategory;
    string Undelivered;
    string OutputOrder;
    string KeyObjectives;
    string Instructions;
    string Insert;
    string Preview;
    string Export;
    string SizeOfPaper;
    string Message;
    string TitlePopupViewer;
    string TitlePopupConfirm;
    string TitlePopupWarning;
    string TitlePopupInfo;


    List<string> emptyItemMessage = new List<string>();
    string EmptyCompanyMessage = "";
    string EmptyBranchMessage = "";
    string EmptyBookingTypeMessage = "";
    string MessageCheckDataExist = "";
    bool CheckSelectedVehicleDispatchToAll = true;
    bool CheckSelectedReservationEndAll = true;
    bool OpenPopPreview = false;
    bool checkNullItem = false;

    /// <summary>
    /// Initialize localization strings
    /// </summary>
    private void LocalizationInit()
    {
        PageTitleReport = Lang["PageTitle"];
        FormTitle = Lang["FormTitle"];
        OperationDate = Lang["OperationDate"];
        Company = Lang["Company"];
        VehicleDispatchOffice = Lang["VehicleDispatchOffice"];
        ReservationCategory = Lang["ReservationCategory"];
        Undelivered = Lang["Undelivered"];
        OutputOrder = Lang["OutputOrder"];
        KeyObjectives = Lang["KeyObjectives"];
        Instructions = Lang["Instructions"];
        Insert = Lang["InsertButton"];
        Preview = Lang["PreviewButton"];
        Export = Lang["ExportButton"];
        SizeOfPaper = Lang["SizeOfPaper"];
        Message = Lang["Message"];

        EmptyCompanyMessage = Lang["BI_T001"];
        EmptyBranchMessage = Lang["BI_T005"];
        EmptyBookingTypeMessage = Lang["BI_T007"];
        MessageCheckDataExist = Lang["BI_T006"];

        TitlePopupViewer = Lang["TitlePopupViewer"];
        TitlePopupConfirm = Lang["TitlePopupConfirm"];
        TitlePopupWarning = Lang["TitlePopupWarning"];
        TitlePopupInfo = Lang["TitlePopupInfo"];
    }
    #endregion

    #region Component Lifecycle
    /// <summary>
    ///
    /// </summary>
    protected override void OnParametersSet()
    {
        JSRuntime.InvokeVoidAsync("loadPageScript", "attendanceConfirmReportPage");
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="firstRender"></param>
    protected override void OnAfterRender(bool firstRender)
    {
        JSRuntime.InvokeAsync<string>("addMaxLength", "length", 25);
        //JSRuntime.InvokeVoidAsync("loadPageScript", "attendanceConfirmReportPage", "ClickShowListbox");
        //JSRuntime.InvokeVoidAsync("loadPageScript", "attendanceConfirmReportPage", "ClickOutsideHideListbox");
    }


    protected override async Task OnInitializedAsync()
    {
        LocalizationInit();
        formContext = new EditContext(reportCondition);
        companychartlst = await TPM_CompnyDataService.GetCompany(Common.TenantID);
        if (companychartlst == null)
        {
            companychartlst = new List<CompanyChartData>();
        }
        else
        {
            companychartlst.Insert(0, allcompany);
            SelectedCompanyItems = companychartlst;
        }
        vehicledispatchofficelst = await TPM_EigyosDataService.GetAllBranchData(Common.TenantID);
        if (vehicledispatchofficelst == null)
        {
            vehicledispatchofficelst = new List<DepartureOfficeData>();
        }
        else
        {
            vehicledispatchofficelst.Insert(0, allbranch);
        }
        reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
        if (reservationlst == null)
        {
            reservationlst = new List<ReservationData>();
        }
        else
        {
            reservationlst.Insert(0, allreservation);
        }
        /*Load Undelivered Data*/
        undeliveredlst = new List<string>(){
            "?????????",
            "??????",
        };

        /*Load Output Data*/
        outputorderlst = new List<OutputOrderData>();
        outputorderlst = OutputOrderListData.OutputOrderlst;

        /*Load SizeOfPaper Data*/
        sizeofpaperlst = new List<string>(){
            "A3",
            "A4",
            "B4",
        };

        /*Load Key Objectives Data*/
        keyObjectiveslst = new List<TPM_CodeKbDataReport>();
        keyObjectiveslst = await TPM_CodeKbnService.GetdataTENKOBOMOKUHYO();
        /*Load Instructions Data*/
        instructionslst = new List<TPM_CodeKbDataReport>();
        instructionslst = await TPM_CodeKbnService.GetdataTENKOBOSHIJI();
        SyainNmItem = await TPM_CompnyDataService.GetSyainNm(Common.UpdSyainCd);
        CheckDataNullFromService();
        await Load();
        //formContext = new EditContext(reportCondition);
    }
    //check data
    void CheckDataNullFromService()
    {
        if (companychartlst == null || companychartlst.Count == 0)
        {
            emptyItemMessage.Add(EmptyCompanyMessage);
        }
        if (vehicledispatchofficelst.Count == 0 || vehicledispatchofficelst == null)
        {
            emptyItemMessage.Add(EmptyBranchMessage);
        }
        if (reservationlst.Count == 0 || reservationlst == null)
        {
            emptyItemMessage.Add(EmptyBookingTypeMessage);
        }
    }
    async Task Load()
    {
        if (emptyItemMessage.Count > 0) checkNullItem = true;
        reportCondition.OperationDate = DateTime.Today;
        if (companychartlst?.Count >= 1)
        {
            reportCondition.CompanyChartData = SelectedCompanyItems.ToList();
        }
        if (vehicledispatchofficelst.Count >= 1)
        {
            reportCondition.VehicleDispatchOffice1 = vehicledispatchofficelst.First();
            reportCondition.VehicleDispatchOffice2 = vehicledispatchofficelst.First();
        }
        if (reservationlst.Count >= 1)
        {
            reportCondition.ReservationStart = reservationlst.First();
            reportCondition.ReservationEnd = reservationlst.First();
        }
        reportCondition.Undelivered = undeliveredlst.First();
        reportCondition.OutputOrder = outputorderlst.First();
        reportCondition.SizeOfPaper = sizeofpaperlst.First();
        reportCondition.TenantCdSeq = Commons.Constants.Common.TenantID;
        reportCondition.SyainNm = SyainNmItem.SyainNm == null ? "" : SyainNmItem.SyainNm;


        if (!string.IsNullOrEmpty(Option) && !string.IsNullOrEmpty(Date) && (Option == OptionReport.Preview.ToString() || Option == OptionReport.Download.ToString()))
        {
            DateTime dateTimeConvert;
            try
            {
                dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
                reportCondition.OperationDate = dateTimeConvert;
                operationDate = dateTimeConvert;
                NavManager.NavigateTo("/attendanceconfirmreport",false);
                bool checkDataExist = false;
                checkDataExist = await checkDataExistInDb(reportCondition);
                if (checkDataExist)//c?? d??? li???u
                {
                    if (Option == OptionReport.Preview.ToString())//preview
                    {
                        OpenPopPreview = true;
                    }
                    else //download
                    {
                        await ExportReportAsPdf(1, $"{nameof(HassyaAllrightCloud.Reports.ReportFactory.ReportTenkokiroku)}?" + reportCondition.Uri);
                    }
                }
                else
                {
                    PopupCheckData = true;
                }
            }
            catch
            {
                PopupCheckData = true;
            }
        }
        else
        {

        }
    }
    void OnOperationDateChanged(DateTime newDate)
    {
        operationDate = newDate;
        reportCondition.OperationDate = newDate;
        InvokeAsync(StateHasChanged);
    }

    void OnSelectedCompanyItemsChanged(IEnumerable<CompanyChartData> selectedCompanyItems)
    {
        SelectedCompanyItems = selectedCompanyItems;
        if (SelectedCompanyItems.Count() == 0)
        {
            showSelectedCompany = "???????????????0";
        }
        else if (SelectedCompanyItems.Count() == 1)
        {
            foreach (var item in SelectedCompanyItems)
            {
                showSelectedCompany = item.Text;
            }
        }
        else if (SelectedCompanyItems.Count() > 1 && SelectedCompanyItems.Count() < companychartlst.Count())
        {
            showSelectedCompany = "???????????????" + SelectedCompanyItems.Count().ToString();
        }
        else if (SelectedCompanyItems.Count() == companychartlst.Count())
        {
            showSelectedCompany = "?????????";
        }

        if (checkCompanyAll == true && !SelectedCompanyItems.Contains(allcompany))
        {
            SelectedCompanyItems = SelectedCompanyItems.Take(0);
            checkCompanyAll = false;
        }
        if (checkCompanyAll == false && (SelectedCompanyItems.Contains(allcompany) || (!SelectedCompanyItems.Contains(allcompany)
            && SelectedCompanyItems.Count() == companychartlst.Count() - 1)))
        {
            SelectedCompanyItems = companychartlst;
            checkCompanyAll = true;
        }
        if (checkCompanyAll == true && SelectedCompanyItems.Contains(allcompany)
            && SelectedCompanyItems.Count() < companychartlst.Count())
        {
            SelectedCompanyItems = SelectedCompanyItems.Where(t => t.CompanyCdSeq != allcompany.CompanyCdSeq);
            checkCompanyAll = false;
        }
        reportCondition.CompanyChartData = SelectedCompanyItems.ToList();
        vehicledispatchofficelst.Insert(0, allbranch);
        reportCondition.VehicleDispatchOffice1 = vehicledispatchofficelst.First();
        reportCondition.VehicleDispatchOffice2 = vehicledispatchofficelst.First();
        formContext.Validate();
        vehicledispatchofficelst = TPM_EigyosDataService.GetBranchDataByIdCompany(reportCondition.CompanyChartData, Common.TenantID);
        vehicledispatchofficelst.Insert(0, allbranch);
        InvokeAsync(StateHasChanged);
    }
    void OnVehicleDispatchOffice1Changed(DepartureOfficeData departureOffice)
    {
        if (departureOffice.EigyoCdSeq == 0)
        {
            reportCondition.VehicleDispatchOffice1 = departureOffice ?? new DepartureOfficeData();
            reportCondition.VehicleDispatchOffice2 = departureOffice ?? new DepartureOfficeData();
            CheckSelectedVehicleDispatchToAll = true;
        }
        else
        {
            CheckSelectedVehicleDispatchToAll = false;
            reportCondition.VehicleDispatchOffice1 = departureOffice ?? new DepartureOfficeData();
        }
        formContext.Validate();
        InvokeAsync(StateHasChanged);
    }

    void OnVehicleDispatchOffice2Changed(DepartureOfficeData departureOffice)
    {
        if (departureOffice.EigyoCdSeq == 0)
        {
            reportCondition.VehicleDispatchOffice1 = departureOffice ?? new DepartureOfficeData();
            reportCondition.VehicleDispatchOffice2 = departureOffice ?? new DepartureOfficeData();
            CheckSelectedVehicleDispatchToAll = true;
        }
        else
        {
            reportCondition.VehicleDispatchOffice2 = departureOffice ?? new DepartureOfficeData();
        }
        formContext.Validate();
        InvokeAsync(StateHasChanged);
    }

    void OnReservationStartChanged(ReservationData reservation)
    {
        if (reservation.YoyaKbnSeq == 0)
        {
            reportCondition.ReservationStart = reservation ?? new ReservationData();
            reportCondition.ReservationEnd = reservation ?? new ReservationData();
            CheckSelectedReservationEndAll = true;
        }
        else
        {
            CheckSelectedReservationEndAll = false;
            reportCondition.ReservationStart = reservation ?? new ReservationData();
        }
        formContext.Validate();
        InvokeAsync(StateHasChanged);
    }

    void OnReservationEndChanged(ReservationData reservation)
    {
        if (reservation.YoyaKbnSeq == 0)
        {
            reportCondition.ReservationStart = reservation ?? new ReservationData();
            reportCondition.ReservationEnd = reservation ?? new ReservationData();
            CheckSelectedReservationEndAll = true;
        }
        else
        {
            CheckSelectedReservationEndAll = false;
            reportCondition.ReservationEnd = reservation ?? new ReservationData();
        }
        formContext.Validate();
        InvokeAsync(StateHasChanged);
    }

    void OnUndeliveredChanged(string newValue)
    {
        reportCondition.Undelivered = newValue;
        InvokeAsync(StateHasChanged);
    }

    void OnOutputOrderChanged(OutputOrderData order)
    {
        reportCondition.OutputOrder = order;
        InvokeAsync(StateHasChanged);
    }

    void OnSizeOfPaperChanged(string newValue)
    {
        reportCondition.SizeOfPaper = newValue;
        InvokeAsync(StateHasChanged);
    }

    void OnTxtKeyObjectivesChange(string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            disabledTxtKeyObjectives = true;
        }
        else
        {
            disabledTxtKeyObjectives = false;
        }
        reportCondition.TxtKeyObjectives = newValue;
        InvokeAsync(StateHasChanged);
    }

    void OnTxtInstructionsChange(string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            disabledTxtInstructions = true;
        }
        else
        {
            disabledTxtInstructions = false;
        }
        reportCondition.TxtInstructions = newValue;
        InvokeAsync(StateHasChanged);
    }
    #endregion

    /// <summary>
    ///
    /// </summary>
    private void HandleValidSubmit()
    {
        bool isValid = formContext.Validate();
        if (isValid)
        {
            try
            {
                var client = new HttpClient();

            }
            catch
            {
            }
        }
        // To do
    }

    /// <summary>
    /// add item KeyObjectives (????????????)
    /// </summary>
    private async Task OnAddTxtKeyObjectives()
    {
        if (selectedlstKeyObjectives.Count() >= 5)
        {
            PopupVisible = true;
        }
        else
        {
            if (!String.IsNullOrEmpty(reportCondition.TxtKeyObjectives) &&
                !selectedlstKeyObjectives.Contains(reportCondition.TxtKeyObjectives))
            {
                selectedlstKeyObjectives.Add(reportCondition.TxtKeyObjectives);
                selectedlstKeyObjectivestmp.Add(reportCondition.TxtKeyObjectives);
                reportCondition.TxtKeyObjectives = "";
                disabledTxtKeyObjectives = true;
            }
        }
        reportCondition.KeyObjectivesList = selectedlstKeyObjectives;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// add item Instructions (????????????)
    /// </summary>
    private async Task OnAddTxtInstructions()
    {
        if (selectedlstInstructions.Count() >= 5)
        {
            PopupVisible = true;
        }
        else
        {
            if (!String.IsNullOrEmpty(reportCondition.TxtInstructions) && !selectedlstInstructions.Contains(reportCondition.TxtInstructions))
            {
                selectedlstInstructions.Add(reportCondition.TxtInstructions);
                selectedlstInstructionstmp.Add(reportCondition.TxtInstructions);
                reportCondition.TxtInstructions = "";
                disabledTxtInstructions = true;
            }
        }
        reportCondition.InstructionsList = selectedlstInstructions;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// selected item KeyObjectives (????????????)
    /// </summary>
    /// <param name="selectedItems"></param>
    private void OnSelectedKeyObjectivesChanged(IEnumerable<TPM_CodeKbDataReport> selectedItems)
    {
        if (selectedlstKeyObjectives.Count >= 5 && selectedItems.Count() > SelectedKeyObjectivesItems.Count())
        {
            PopupVisible = true;
            SelectedKeyObjectivesItems = SelectedKeyObjectivesItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
            selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
        }
        else
        {
            // start check Item same
            List<TPM_CodeKbDataReport> Tmp = new List<TPM_CodeKbDataReport>();
            List<TPM_CodeKbDataReport> TmpTheSame = new List<TPM_CodeKbDataReport>();
            Tmp = selectedItems.ToList();
            if (selectedItems.Count() > 1)
            {
                int countTmp = selectedItems.Count();
                for (int i = 0; i < countTmp; i++)
                {
                    for (int j = i + 1; j < countTmp; j++)
                    {
                        if (Tmp[i].CodeKb_CodeKbnNm == Tmp[j].CodeKb_CodeKbnNm)
                        {
                            TmpTheSame.Add(Tmp[j]);
                        }
                    }
                }
                if (TmpTheSame.Count >= 1)
                {
                    for (int k = 0; k < TmpTheSame.Count; k++)
                    {
                        selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != TmpTheSame[k].CodeKb_CodeKbnSeq).ToList();
                    }
                }
            }
            //end  check Item same
            if (selectedlstKeyObjectives.Count() >= 5 && selectedItems.Count() > 5)
            {
                PopupVisible = true;
                SelectedKeyObjectivesItems = SelectedKeyObjectivesItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
                selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
            }
            else
            {
                SelectedKeyObjectivesItems = selectedItems;
                SelectedKeyObjectivesItemstmp = new List<string>();
                foreach (var item in SelectedKeyObjectivesItems)
                {
                    SelectedKeyObjectivesItemstmp.Add(item.CodeKb_CodeKbnNm);
                    if (!selectedlstKeyObjectives.Contains(item.CodeKb_CodeKbnNm))
                    {
                        selectedlstKeyObjectives.Add(item.CodeKb_CodeKbnNm);
                    }

                }
            }
            IEnumerable<string> tmp = SelectedKeyObjectivesItemstmp.Union(selectedlstKeyObjectivestmp);
            foreach (string item in selectedlstKeyObjectives)
            {
                if (!tmp.Contains(item))
                {
                    selectedlstKeyObjectives.Remove(item);
                    break;
                }
            }
        }
        reportCondition.KeyObjectivesList = selectedlstKeyObjectives;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// selected item Instructions (????????????)
    /// </summary>
    /// <param name="selectedItems"></param>
    private void OnSelectedInstructionsChanged(IEnumerable<TPM_CodeKbDataReport> selectedItems)
    {
        if (selectedlstInstructions.Count >= 5 && selectedItems.Count() > SelectedInstructionsItems.Count())
        {
            PopupVisible = true;
            SelectedInstructionsItems = SelectedInstructionsItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
            selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
        }
        else
        {
            List<TPM_CodeKbDataReport> Tmp = new List<TPM_CodeKbDataReport>();
            List<TPM_CodeKbDataReport> TmpTheSame = new List<TPM_CodeKbDataReport>();
            Tmp = selectedItems.ToList();
            if (selectedItems.Count() > 1)
            {
                int countTmp = selectedItems.Count();
                for (int i = 0; i < countTmp; i++)
                {
                    for (int j = i + 1; j < countTmp; j++)
                    {
                        if (Tmp[i].CodeKb_CodeKbnNm == Tmp[j].CodeKb_CodeKbnNm)
                        {
                            TmpTheSame.Add(Tmp[j]);
                        }
                    }
                }
                if (TmpTheSame.Count >= 1)
                {
                    for (int k = 0; k < TmpTheSame.Count; k++)
                    {
                        selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != TmpTheSame[k].CodeKb_CodeKbnSeq).ToList();
                    }
                }
            }

            if (selectedlstInstructions.Count() >= 5 && selectedItems.Count() > 5)
            {
                PopupVisible = true;

                SelectedInstructionsItems = SelectedInstructionsItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
                selectedItems = selectedItems.Where(x => x.CodeKb_CodeKbnSeq != selectedItems.Last().CodeKb_CodeKbnSeq).ToList();
            }
            else
            {
                SelectedInstructionsItems = selectedItems;
                SelectedInstructionsItemstmp = new List<string>();
                foreach (var item in SelectedInstructionsItems)
                {
                    SelectedInstructionsItemstmp.Add(item.CodeKb_CodeKbnNm);

                    if (!selectedlstInstructions.Contains(item.CodeKb_CodeKbnNm))
                    {
                        selectedlstInstructions.Add(item.CodeKb_CodeKbnNm);

                    }
                }
            }
            IEnumerable<string> tmp = SelectedInstructionsItemstmp.Union(selectedlstInstructionstmp);
            foreach (string item in selectedlstInstructions)
            {
                if (!tmp.Contains(item))
                {
                    selectedlstInstructions.Remove(item);
                    break;
                }
            }
        }
        reportCondition.InstructionsList = selectedlstInstructions;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// remove item KeyObjectives (????????????) both listbox and list report
    /// </summary>
    /// <param name="selectedItems"></param>
    private async Task OnRemoveSelectedKeyObjectives(string selectedItems)
    {
        if (selectedlstKeyObjectives.Contains(selectedItems))
        {
            // remove in list
            selectedlstKeyObjectives.Remove(selectedItems);
            selectedlstKeyObjectivestmp.Remove(selectedItems);
            // remove in listbox
            foreach (var item in SelectedKeyObjectivesItems)
            {
                if (item.CodeKb_CodeKbnNm == selectedItems)
                {
                    SelectedKeyObjectivesItems = SelectedKeyObjectivesItems.Where(t => t.CodeKb_CodeKbnNm != selectedItems);
                }
            }
        }

        reportCondition.KeyObjectivesList = selectedlstKeyObjectives;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Remove item Instructions (????????????)
    /// </summary>
    /// <param name="selectedItems"></param>
    private async Task OnRemoveSelectedInstructions(string selectedItems)
    {
        if (selectedlstInstructions.Contains(selectedItems))
        {
            // remove in list
            selectedlstInstructions.Remove(selectedItems);
            selectedlstInstructionstmp.Remove(selectedItems);
            // remove in listbox
            foreach (var item in SelectedInstructionsItems)
            {
                if (item.CodeKb_CodeKbnNm == selectedItems)
                {
                    SelectedInstructionsItems = SelectedInstructionsItems.Where(t => t.CodeKb_CodeKbnNm != selectedItems);
                }
            }
        }
        reportCondition.InstructionsList = selectedlstInstructions;
        await InvokeAsync(StateHasChanged);
    }
    async void OpenPopupPreview()
    {

        bool checkDataExist = false;
        checkDataExist = await checkDataExistInDb(reportCondition);
        if (checkDataExist)
        {
            OpenPopPreview = true;
        }
        else
        {
            PopupCheckData = true;
        }
    }

    async Task ExportReportAsPdf(int printMode, string uri)
    {

        var report = new HassyaAllrightCloud.Reports.ReportFactory.ReportTenkokirokuCreator("ReportTenkokiroku", "Report Tenkokiroku").GetReport().
            CreateByUrl(uri);
        await new System.Threading.Tasks.TaskFactory().StartNew(() =>
        {
            string fileType = "";
            report.CreateDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                switch (printMode)
                {
                    case 1:
                        fileType = "pdf";
                        report.ExportToPdf(ms);
                        break;
                    case (int)PrintMode.SaveAsExcel:
                        fileType = "xlsx";
                        report.ExportToXlsx(ms);
                        break;
                    case (int)PrintMode.Print:
                        PrintToolBase tool = new PrintToolBase(report.PrintingSystem);
                        tool.Print();
                        return;
                }

                byte[] exportedFileBytes = ms.ToArray();
                string myExportString = Convert.ToBase64String(exportedFileBytes);
                JSRuntime.InvokeVoidAsync("loadPageScript", "attendanceConfirmReportPage", "downloadFileAttendanceConfirmReport", myExportString, fileType);
            }

        });
    }

    /// <summary>
    /// remove disable button Insert TxtKeyObjectives when keypress textbox
    /// </summary>
    void enabledInsertTxtKeyObjectives()
    {
        disabledTxtKeyObjectives = false;
    }

    async void DownLoadReport()
    {
        bool checkDataExist = false;
        checkDataExist = await checkDataExistInDb(reportCondition);
        if (checkDataExist)
        {
            await ExportReportAsPdf(1, $"{nameof(HassyaAllrightCloud.Reports.ReportFactory.ReportTenkokiroku)}?" + reportCondition.Uri);
        }
        else
        {
            PopupCheckData = true;
        }
    }
    async Task<bool> checkDataExistInDb(AttendanceConfirmReportData attendanceConfirmReportData)
    {
        List<AttendanceConfirmReportData> attendanceConfirmDataList = new List<AttendanceConfirmReportData>();
        attendanceConfirmDataList = await TenkokirokuReportService.GetInfoMainReport(attendanceConfirmReportData);
        if (attendanceConfirmDataList.Count == 0 || attendanceConfirmDataList == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITenkokirokuReportService TenkokirokuReportService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CodeKbListService TPM_CodeKbnService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_EigyosDataListService TPM_EigyosDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CompnyDataListService TPM_CompnyDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<AttendanceConfirmReport> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591

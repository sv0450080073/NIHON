#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e6d57c665aae14df65dfc63571f365d358982822"
// <auto-generated/>
#pragma warning disable 1591
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
using HassyaAllrightCloud.Commons.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using BlazorContextMenu;

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Application.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Validation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 22 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 23 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Microsoft.Extensions.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.IService;

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 29 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using LexLibrary.Line.NotifyBot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Blazored.Modal.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using SharedLibraries.UI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen;

#line default
#line hidden
#nullable disable
#nullable restore
#line 38 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using Radzen.Blazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using HassyaAllrightCloud.Pages.Components.CommonComponents;

#line default
#line hidden
#nullable disable
    public partial class AttendanceConfirmReportPreview : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor"
 if (!string.IsNullOrEmpty(reportUrl) && !string.IsNullOrWhiteSpace(reportUrl))
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "    ");
            __builder.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewer>(1);
            __builder.AddAttribute(2, "ReportUrl", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor"
                                  reportUrl

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(3, "Height", "800px");
            __builder.AddAttribute(4, "Width", "100%");
            __builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewerTabPanelSettings>(7);
                __builder2.AddAttribute(8, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor"
                                                 180

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(9, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(10, "\r\n");
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 14 "E:\Project\HassyaAllrightCloud\Pages\AttendanceConfirmReportPreview.razor"
       
    [Parameter] public string IsLoadDefault { get; set; }
    [Parameter] public string Option { get; set; }
    [Parameter] public string Date { get; set; }
    [Parameter] public string SearchString { get; set; }
    public string reportUrl { get; set; }
    List<CompanyChartData> companychartlst = new List<CompanyChartData>();
    List<BranchChartData> branchchartlst = new List<BranchChartData>();
    List<DepartureOfficeData> vehicledispatchofficelst = new List<DepartureOfficeData>();
    List<ReservationData> reservationlst = new List<ReservationData>();
    IEnumerable<ReservationData> SelectedReservations = new List<ReservationData>();
    List<string> undeliveredlst = new List<string>();
    List<OutputOrderData> outputorderlst = new List<OutputOrderData>();
    List<string> sizeofpaperlst = new List<string>();
    IEnumerable<TPM_CodeKbDataReport> keyObjectiveslst = new List<TPM_CodeKbDataReport>();
    IEnumerable<TPM_CodeKbDataReport> instructionslst = new List<TPM_CodeKbDataReport>();
    VpmSyain SyainNmItem = new VpmSyain();
    AttendanceConfirmReportData reportCondition = new AttendanceConfirmReportData();
    AttendanceConfirmReportDataUri AttendanceConfirmReportDataUri = new AttendanceConfirmReportDataUri();
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await LoadCompanyData();
            reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
            if (reservationlst == null)
            {
                reservationlst = new List<ReservationData>();
            }
            else
            {
                reservationlst.Insert(0, new ReservationData());
            }
            if (IsLoadDefault == "1")
            {
                await LoadCompanyData();
                vehicledispatchofficelst = await TPM_EigyosDataService.GetAllBranchData(new ClaimModel().TenantID);
                if (vehicledispatchofficelst == null)
                {
                    vehicledispatchofficelst = new List<DepartureOfficeData>();
                }
                else
                {
                    vehicledispatchofficelst.Insert(0, new DepartureOfficeData());
                }
                /*Load Undelivered Data*/
                undeliveredlst = new List<string>(){
            "??????",
            "?????????",
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
                keyObjectiveslst = await TPM_CodeKbnService.GetdataTENKOBOMOKUHYO(new ClaimModel().TenantID);
                /*Load Instructions Data*/
                instructionslst = new List<TPM_CodeKbDataReport>();
                instructionslst = await TPM_CodeKbnService.GetdataTENKOBOSHIJI(new ClaimModel().TenantID);
                SyainNmItem = await TPM_CompnyDataService.GetSyainNm(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                reportCondition.CompanyChartData = companychartlst.ToList();
                reportCondition.VehicleDispatchOffice1 = vehicledispatchofficelst.First();
                reportCondition.VehicleDispatchOffice2 = vehicledispatchofficelst.First();
                reportCondition.ReservationList = reservationlst.ToList();
                reportCondition.Undelivered = undeliveredlst.First();
                reportCondition.OutputOrder = outputorderlst.First();
                reportCondition.SizeOfPaper = sizeofpaperlst.First();
                reportCondition.TenantCdSeq = new ClaimModel().TenantID;
                reportCondition.SyainNm = SyainNmItem.SyainNm == null ? "" : SyainNmItem.SyainNm;
                reportCondition.OperationDate = DateTime.Today;
                //Check param
                if (!string.IsNullOrEmpty(Date))
                {
                    DateTime dateTimeConvert;
                    try
                    {
                        dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
                        reportCondition.OperationDate = dateTimeConvert;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                SearchString = EncryptHelper.EncryptToUrl(reportCondition);
                reportUrl = $"{nameof(ITenkokirokuReportService)}?{SearchString}";
            }
            else
            {
                AttendanceConfirmReportDataUri = EncryptHelper.DecryptFromUrl<AttendanceConfirmReportDataUri>(SearchString);
                SearchString = EncryptHelper.EncryptToUrl(SetValueUri(AttendanceConfirmReportDataUri));
                reportUrl = $"{nameof(ITenkokirokuReportService)}?{SearchString}";
            }
            NavManager.NavigateTo("/attendanceconfirmreportPreview", false);
        }
        catch (Exception ex)
        {

        }


    }
    private async Task LoadCompanyData()
    {
        companychartlst = await TPM_CompnyDataService.GetCompanyListBox(new ClaimModel().TenantID);
        if (companychartlst == null)
        {
            companychartlst = new List<CompanyChartData>();
        }
        else
        {
            companychartlst.Insert(0, new CompanyChartData());
        }
    }
    private AttendanceConfirmReportData SetValueUri(AttendanceConfirmReportDataUri data)
    {
        var result = new AttendanceConfirmReportData();
        result.OperationDate = data.OperationDate;
        result.CompanyChartData = CutSpecialCharactersCompanyList(data.CompanyChartDataID);
        result.ReservationList = CutSpecialCharactersReservationList(data.ReservationListID);
        result.VehicleDispatchOffice1 = data.VehicleDispatchOffice1;
        result.VehicleDispatchOffice2 = data.VehicleDispatchOffice2;
        result.Undelivered = data.Undelivered;
        result.OutputOrder = data.OutputOrder;
        result.SizeOfPaper = data.SizeOfPaper;
        result.TxtInstructions = data.TxtInstructions;
        result.TxtKeyObjectives = data.TxtKeyObjectives;
        result.KeyObjectivesList = data.KeyObjectivesList;
        result.InstructionsList = data.InstructionsList;
        result.TenantCdSeq = data.TenantCdSeq;
        result.SyainNm = data.SyainNm;
        result.DateTimeFooter = data.DateTimeFooter;
        result.BookingTypeFrom = data.BookingTypeFrom;
        result.BookingTypeTo = data.BookingTypeTo;
        return result;
    }
    private List<CompanyChartData> CutSpecialCharactersCompanyList(string strValue)
    {
        string[] strValueArr = strValue.Split('-');
        var result = companychartlst.Where(x => strValueArr.Contains(x.CompanyCdSeq.ToString())).ToList();
        return result;
    }
    private List<ReservationData> CutSpecialCharactersReservationList(string strValue)
    {
        string[] strValueArr = strValue.Split('-');
        var result = reservationlst.Where(x => strValueArr.Contains(x.YoyaKbnSeq.ToString())).ToList();
        return result;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITenkokirokuReportService TenkokirokuReportService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CodeKbListService TPM_CodeKbnService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_EigyosDataListService TPM_EigyosDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CompnyDataListService TPM_CompnyDataService { get; set; }
    }
}
#pragma warning restore 1591

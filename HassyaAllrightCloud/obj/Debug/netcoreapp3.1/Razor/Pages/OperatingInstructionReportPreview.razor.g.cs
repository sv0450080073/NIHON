#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "15e13939df6db296b1294a5c31f9126cb526f9e5"
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
#nullable restore
#line 5 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
using MediatR;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
using HassyaAllrightCloud.Application.ReportLayout.Queries;

#line default
#line hidden
#nullable disable
    public partial class OperatingInstructionReportPreview : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
 if (!string.IsNullOrEmpty(strUri) && !string.IsNullOrWhiteSpace(strUri))
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
     if (IsOperationInstructions == true && IsCrewRecordBook == false)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, "        ");
            __builder.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewer>(1);
            __builder.AddAttribute(2, "ReportUrl", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 12 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                       $"{nameof(HassyaAllrightCloud.Reports.ReportFactory.ReportUnkoushijisho)}?"+strUri

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(3, "Height", "800px");
            __builder.AddAttribute(4, "Width", "100%");
            __builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(6, "\r\n            ");
                __builder2.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewerTabPanelSettings>(7);
                __builder2.AddAttribute(8, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 13 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                                     180

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(9, "\r\n        ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(10, "\r\n");
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
    }
    else if (IsOperationInstructions == false && IsCrewRecordBook == true)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(11, "        ");
            __builder.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewer>(12);
            __builder.AddAttribute(13, "ReportUrl", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 18 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                       $"{nameof(HassyaAllrightCloud.Reports.ReportFactory.ReportJomukirokubo)}?"+strUri

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(14, "Height", "800px");
            __builder.AddAttribute(15, "Width", "100%");
            __builder.AddAttribute(16, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(17, "\r\n            ");
                __builder2.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewerTabPanelSettings>(18);
                __builder2.AddAttribute(19, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 19 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                                     180

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(20, "\r\n        ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 21 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
    }
    else
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(22, "        ");
            __builder.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewer>(23);
            __builder.AddAttribute(24, "ReportUrl", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                       $"{nameof(HassyaAllrightCloud.Reports.ReportFactory.ReportUnkoushijishoBase)}?"+strUri

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(25, "Height", "800px");
            __builder.AddAttribute(26, "Width", "100%");
            __builder.AddAttribute(27, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(28, "\r\n            ");
                __builder2.OpenComponent<DevExpress.Blazor.Reporting.DxDocumentViewerTabPanelSettings>(29);
                __builder2.AddAttribute(30, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 25 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
                                                     180

#line default
#line hidden
#nullable disable
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(31, "\r\n        ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(32, "\r\n");
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
     
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 30 "E:\Project\HassyaAllrightCloud\Pages\OperatingInstructionReportPreview.razor"
       
    [Parameter] public string searchString { get; set; }
    [Parameter] public string IsLoadDefault { get; set; }
    [Parameter] public string Option { get; set; }
    [Parameter] public string Date { get; set; }
    [Parameter] public string Mode { get; set; }
    [Parameter] public string BookingID { get; set; }
    [Parameter] public string TeiDanNo { get; set; }
    [Parameter] public string UnkRen { get; set; }
    [Parameter] public string BunkRen { get; set; }
    [Parameter] public string UkenoList { get; set; }
    [Parameter] public string FormOutput { get; set; }
    [Inject] IMediator _mediator { get; set; }
    public OperatingInstructionReportData reportData = new OperatingInstructionReportData();
    List<DepartureOfficeData> departureofficelst;
    List<ReservationData> reservationlst;
    List<OutputOrderData> outputorderlst;
    bool IsOperationInstructions { get; set; }
    bool IsCrewRecordBook { get; set; }
    string strUri { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (IsLoadDefault == "1")
        {
            //reservationlst = new List<ReservationData>();
            //reservationlst = await TPM_YoyKbnDataService.GetYoyKbnbySiyoKbn();
            //reservationlst.Insert(0, new ReservationData());
            //reportData.ReservationList = reservationlst.ToList();
            reportData.BookingFrom = reservationlst.FirstOrDefault();
            reportData.BookingTo = reservationlst.FirstOrDefault();
            reportData.DeliveryDate = DateTime.Today;
            reportData.ReceiptNumberFrom = "";
            reportData.ReceiptNumberTo = "";
            /*Load Departure Office*/
            departureofficelst = new List<DepartureOfficeData>();
            departureofficelst = await TPM_EigyosDataService.GetAllBranchData(new ClaimModel().TenantID);
            departureofficelst.Insert(0, new DepartureOfficeData());
            reportData.DepartureOffice = departureofficelst.First();
            /*Load Output Data*/
            outputorderlst = new List<OutputOrderData>();
            outputorderlst = OutputOrderListData.OutputOrderlst;
            reportData.OutputOrder = OutputOrderListData.OutputOrderlst.First();
            //check Param
            if (!string.IsNullOrEmpty(Date))
            {
                DateTime dateTimeConvert;
                try
                {
                    dateTimeConvert = DateTime.ParseExact(Date, "yyyyMMdd", new CultureInfo("ja-JP"));
                    reportData.DeliveryDate = dateTimeConvert;
                    reportData.ReceiptNumberFrom = "0";
                    reportData.ReceiptNumberTo = "2147483647";
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                reportData.DeliveryDate = new DateTime();
            }
            if (!string.IsNullOrEmpty(BookingID) && !string.IsNullOrEmpty(TeiDanNo) && !string.IsNullOrEmpty(UnkRen) && !string.IsNullOrEmpty(BunkRen))
            {
                reportData.DeliveryDate = new DateTime();
                reportData.ReceiptNumberFrom = BookingID.Substring(5, 10);
                reportData.ReceiptNumberTo = BookingID.Substring(5, 10);
                reportData.TeiDanNo = int.Parse(TeiDanNo);
                reportData.UnkRen = int.Parse(UnkRen);
                reportData.BunkRen = int.Parse(BunkRen);
            }
            if (Mode == "1")
            {
                reportData.OperationInstructions = true;
                reportData.CrewRecordBook = false;
            }
            else if (Mode == "2")
            {
                reportData.OperationInstructions = false;
                reportData.CrewRecordBook = true;
            }
            else
            {
                reportData.OperationInstructions = true;
                reportData.CrewRecordBook = true;
            }
            reportData.UkenoList = UkenoList;
            reportData.FormOutput = int.Parse(FormOutput == null ? "0" : FormOutput);
            IsOperationInstructions = reportData.OperationInstructions;
            IsCrewRecordBook = reportData.CrewRecordBook;
            strUri = GetUri();
        }
        else
        {
            {
                var searchParams = EncryptHelper.Decrypt<OperatingInstructionReportData>(searchString);
                IsOperationInstructions = searchParams.OperationInstructions;
                IsCrewRecordBook = searchParams.CrewRecordBook;
                strUri = searchParams.Uri;
            }
        }
        NavManager.NavigateTo("/operatinginstructionreportpreview", false);
    }
    private string GetUri()
    {
        return String.Format("TenantCdSeq={0}&" +
                     "SyuKoYmd={1}&" +
                     "UkeCdFrom={2}&" +
                     "UkeCdTo={3}&" +
                     "YoyakuFrom={4}" +
                     "&SyuEigCdSeq={5}" +
                     "&TeiDanNo={6}&" +
                     "UnkRen={7}" +
                     "&BunkRen={8}" +
                     "&SortOrder={9}" +
                     "&UkenoList={10}" +
                     "&FormOutput={11}" +
                     "&YoyakuTo={12}"
                     , new ClaimModel().TenantID, reportData.DeliveryDate.ToString("yyyyMMdd"),
                     reportData.ReceiptNumberFrom == "" ? 0 : int.Parse(reportData.ReceiptNumberFrom),
                     reportData.ReceiptNumberTo == "" ? int.MaxValue : int.Parse(reportData.ReceiptNumberTo),
                     (reportData.YoyakuFrom == null) ? 0 : reportData.YoyakuFrom.YoyaKbnSeq,
                     reportData.DepartureOffice.EigyoCdSeq,
                     reportData.TeiDanNo,
                     reportData.UnkRen,
                     reportData.BunkRen,
                     reportData.OutputOrder.IdValue,
                     UkenoList != "" ? UkenoList : "",
                     FormOutput,
                     (reportData.YoyakuTo == null) ? 0 : reportData.YoyakuTo.YoyaKbnSeq
                      );
    }

    public void GetTemplate()
    {
        Task.Run(async () =>
        {
            var claimModel = new ClaimModel();
            int CurrentTemplateId = await _mediator.Send(new GetReportCurrentTemplateQuery { TenantCdSeq = claimModel.TenantID, ReportId = ReportIdForSetting.Operatinginstructionreport, EigyouCdSeq = claimModel.EigyoCdSeq });
            string ReportClassName = string.Empty;
            if (IsOperationInstructions == true && IsCrewRecordBook == false)
            {
                ReportClassName = BaseNamespace.Report + BaseNamespace.UnkoushijishoReport + CurrentTemplateId + PaperSize.A4;
            }
            else if (IsOperationInstructions == false && IsCrewRecordBook == true)
            {
                ReportClassName = BaseNamespace.Report + BaseNamespace.JomukirokuboReport + CurrentTemplateId + PaperSize.A4;
            }
            else
            {
                ReportClassName = BaseNamespace.Report + BaseNamespace.UnkoushijishoBaseReport + CurrentTemplateId + PaperSize.A4;
            }
            strUri += "&ReportTemplate=" + ReportClassName;
        }).Wait();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_EigyosDataListService TPM_EigyosDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_CompnyDataListService TPM_CompnyDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITPM_YoyKbnDataListService TPM_YoyKbnDataService { get; set; }
    }
}
#pragma warning restore 1591

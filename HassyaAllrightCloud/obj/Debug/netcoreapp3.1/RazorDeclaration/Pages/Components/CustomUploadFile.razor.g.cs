#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\CustomUploadFile.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0b43ac32ccdae9775dc909b1b6bc768a2443dddd"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Pages.Components
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
#line 14 "E:\Project\HassyaAllrightCloud\Pages\Components\CustomUploadFile.razor"
using HassyaAllrightCloud.Pages.Components.Popup;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "E:\Project\HassyaAllrightCloud\Pages\Components\CustomUploadFile.razor"
using HassyaAllrightCloud.Domain.Dto.BillPrint;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "E:\Project\HassyaAllrightCloud\Pages\Components\CustomUploadFile.razor"
using SharedLibraries.Utility.Models;

#line default
#line hidden
#nullable disable
    public partial class CustomUploadFile : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 87 "E:\Project\HassyaAllrightCloud\Pages\Components\CustomUploadFile.razor"
       
    [Parameter] public string UkeNo { get; set; }
    [Parameter] public short UnkRen { get; set; }
    [Parameter] public BookingFormData BookingData { get; set; }
    [Parameter] public EventCallback<List<FileInfoData>> UpdateFileupload { get; set; }
    [Parameter] public EventCallback OnSubmit { get; set; }
    [CascadingParameter] public EditContext FormContext { get; set; }
    List<Unkobidatafile> lstOperatingSchedule = new List<Unkobidatafile>();
    List<FileInfoData> listfile = new List<FileInfoData>();
    List<SharedLibraries.Utility.Models.FileSendData> ListFile = new List<SharedLibraries.Utility.Models.FileSendData>();
    Unkobidatafile selectOperatingSchedule;
    bool UploadVisible { get; set; } = false;
    int MaxFileSize = 10000000;
    int MaxNumberOfFile = 50;
    private string url = string.Empty;
    List<string> listtypefile = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".xlsx", ".xls", ".csv", ".docx", ".doc" };
    PaymentRequestTenantInfo TenantInfo = new PaymentRequestTenantInfo();

    protected override async Task OnInitializedAsync()
    {
        listfile = new List<FileInfoData>();
        url = MyNavigationManager.BaseUri;
        TenantInfo = await billPrintService.GetTenantInfoAsync();
        if (UkeNo != "0" && UkeNo != null)
        {
            listfile = await TKD_UnkobiFileService.getDataFileInfo(new ClaimModel().TenantID, UkeNo, UnkRen);
            listfile = listfile.Where(file => file.SiyoKbn == 1).ToList();
        }
        if (BookingData != null && BookingData.UkeNo != null && BookingData.YykshoUpdYmdTime == null)
        {
            return;
        }
        lstOperatingSchedule = TKD_UnkobiFileService.getInfoUnkobi(new ClaimModel().TenantID, UkeNo);
        if (lstOperatingSchedule.Count() > 0)
        {
            selectOperatingSchedule = lstOperatingSchedule.Where(t => t.UkeNo == UkeNo && t.UnkRen == UnkRen).FirstOrDefault();
        }
        else
        {
            Unkobidatafile unkobifile = new Unkobidatafile();
            unkobifile.SyukoYmd = BookingData.ReservationTabData.GarageLeaveDate.ToString("yyyyMMdd");
            unkobifile.KikYmd = BookingData.ReservationTabData.GarageReturnDate.ToString("yyyyMMdd");
            unkobifile.UnkRen = 1;
            lstOperatingSchedule.Add(unkobifile);
            selectOperatingSchedule = lstOperatingSchedule.First();
        }
    }

    void OnSelectOperatingScheduleItemChanged(Unkobidatafile item)
    {
        selectOperatingSchedule = item;
        StateHasChanged();
    }

    async void DeleteFile(FileInfoData File)
    {
        try
        {
            await s3Service.RemoveFile(File.FileId);
        }
        catch
        {
            return;
        }
        finally
        {
            TKD_UnkobiFileService.DeleteFile(File.TenantCdSeq, File.UkeNo, File.UnkRen, File.FileNo);
            listfile = new List<FileInfoData>();
            listfile = await TKD_UnkobiFileService.getDataFileInfo(new ClaimModel().TenantID, UkeNo, UnkRen);
            BookingData.UnkobiFileUpdYmdTime = listfile.Select(x => x.UpdYmd + x.UpdTime).Max();
            listfile = listfile.Where(file => file.SiyoKbn == 1).ToList();
            StateHasChanged();
        }
    }

    async Task DownloadFile(FileInfoData File)
    {
        var model = new DownloadModel()
        {
            FileId = File.FileId,
            Password = string.Empty,
            UpdSyainCd = new ClaimModel().SyainCdSeq,
            UpdPrgID = Common.UpdPrgId
        };

        var result = await s3Service.DownloadFileAsync(model);
        if (result != null)
        {
            var extension = Path.GetExtension(File.FileNm);
            string myExportString = result.Content;
            await JSRuntime.InvokeVoidAsync("downloadFileClientSide", myExportString, extension.Replace(".", string.Empty).ToLower(), File.FileNm.Replace(extension, string.Empty) + "_");
        }
    }

    async void DownloadAll()
    {
        foreach (FileInfoData file in listfile)
        {
            await DownloadFile(file);
        }
    }

    protected async void Save()
    {
        List<HaitaModelCheck> HaitaModelsToCheck = new List<HaitaModelCheck>();
        HaitaModelsToCheck.Add(new HaitaModelCheck()
        {
            TableName = "TKD_UnkobiFile",
            CurrentUpdYmdTime = BookingData.UnkobiFileUpdYmdTime,
            PrimaryKeys = new List<PrimaryKeyToCheck>(new PrimaryKeyToCheck[] { new PrimaryKeyToCheck() { PrimaryKey = "UkeNo =", Value = "'" + UkeNo + "'" } })
        });
        bool isHaita = await HaitaCheckService.GetHaitaCheck(HaitaModelsToCheck);
        if (!isHaita)
        {
            await OnSubmit.InvokeAsync(null).ContinueWith(t =>
            {
                if (FormContext != null)
                {
                    FormContext.MarkAsUnmodified();
                }
            });
            return;
        }
        var result = await Task.WhenAll(ListFile.Select(f => ProcessFiles(f)));
        BookingData.UkeNo = UkeNo;
        if (result.Any(e => e))
        {
            TKD_UnkobiDataListService.InsertUnkobiFile(BookingData, listfile);
        }
        listfile = new List<FileInfoData>();
        listfile = await TKD_UnkobiFileService.getDataFileInfo(new ClaimModel().TenantID, UkeNo, UnkRen);
        BookingData.UnkobiFileUpdYmdTime = listfile.Select(x => x.UpdYmd + x.UpdTime).Max();
        listfile = listfile.Where(file => file.SiyoKbn == 1).ToList();
        UploadVisible = false;
        StateHasChanged();
    }

    protected string GetUploadUrl(string url)
    {
        return NavigationManager.ToAbsoluteUri(url).AbsoluteUri;
    }

    protected void SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
    {
        UploadVisible = files.ToList().Count > 0;
        if (files.ToList().Count < listfile.Count)
        {
            listfile.RemoveAll(item => item.FileId == null && !files.Any(f => f.Name == item.FileNm));
            UpdateFileupload.InvokeAsync(listfile);
        }
        StateHasChanged();
    }
    protected async void OnFileUploaded(FileUploadEventArgs args)
    {
        string url = NavigationManager.ToAbsoluteUri("api/Upload/FileInformation/?fileName=").AbsoluteUri + args.FileInfo.Name;
        var response = await Http.GetJsonAsync<SharedLibraries.Utility.Models.FileSendData>(url);
        if (response != null && listfile.Count < MaxNumberOfFile)
        {
            ListFile.Add(response);
            FileInfoData fileInfoData = new FileInfoData();
            fileInfoData.FileNm = response.FileName;
            fileInfoData.File = response.File;
            fileInfoData.UkeNo = UkeNo;
            fileInfoData.FileSize = (int)response.FileSize;
            fileInfoData.UnkRen = UnkRen;
            fileInfoData.TenantCdSeq = new ClaimModel().TenantID;
            listfile.Add(fileInfoData);
        }
        await UpdateFileupload.InvokeAsync(listfile);
    }

    protected async Task<bool> ProcessFiles(SharedLibraries.Utility.Models.FileSendData myFile)
    {
        try
        {
            myFile.FilePath = string.Format("{0:D5}", new ClaimModel().TenantID) + "-" + TenantInfo.TenantCompanyName + "/" + Constants.SubStoredFolderForBookingInput + "/" + UkeNo.Substring(5);
            var f = await s3Service.UploadFileAsync(myFile);
            FileInfoData file = listfile.Find(item => item.FileNm == f.Name);
            if (file != null)
            {
                file.FolderId = f.EncryptedFolderId;
                file.FileId = f.EncryptedId;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBillPrintService billPrintService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private SharedLibraries.UI.Services.ISharedLibrariesApi s3Service { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHaitaCheckService HaitaCheckService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_UnkobiDataListService TKD_UnkobiDataListService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation MyNavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_UnkobiFileService TKD_UnkobiFileService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFileHandler _fileHandler { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Index> IndexLang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<BookingInputTab> Lang { get; set; }
    }
}
#pragma warning restore 1591
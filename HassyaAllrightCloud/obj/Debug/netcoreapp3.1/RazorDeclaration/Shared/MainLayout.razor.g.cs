#pragma checksum "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ced6d7bd42885ffe9a1f2aafb1c6fabb193c897"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace HassyaAllrightCloud.Shared
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
#line 7 "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor"
using Microsoft.AspNetCore.Hosting;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor"
using Google.Cloud.Speech.V1;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor"
using System.Text.RegularExpressions;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 70 "E:\Project\HassyaAllrightCloud\Shared\MainLayout.razor"
       
    [CascadingParameter]
    private Task<AuthenticationState> authState { get; set; }
    [Inject] private IStringLocalizer<CommonResource> _lang { get; set; }
    protected ClaimModel ClaimModel;

    private bool collapseNavMenu = true;
    private bool isMinimize = false;
    private string NavMenuCssClass => collapseNavMenu ? "active" : null;
    private string ContentCssClass => collapseNavMenu ? "expand" : "collapse";

    public HyperFormData hyperData = new HyperFormData();
    public string InputText { get; set; } = string.Empty;
    public bool IsNoData { get; set; } = false;
    public bool IsValid { get; set; } = false;
    public bool IsFocus { get; set; } = false;

    public string strSpeaking { get; set; }
    public string strStreamText { get; set; }
    public bool isSpeaking { get; set; } = false;
    public bool recognitionEnded { get; set; } = false;
    public bool isProcessing { get; set; } = false;
    int notification { get; set; } = 0;


    IDisposable thisReference;

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

    protected override async Task OnInitializedAsync()
    {
        if (authState != null)
        {
            var principal = (await authState).User;
            ClaimModel = new ClaimModel();
            if (principal != null)
            {
                var tenantId = principal.Claims.FirstOrDefault(x => x.Type.ToLower() == "tenantcdseq")?.Value;
                if (tenantId != null) ClaimModel.TenantID = int.Parse(tenantId);
                var SyainCdSeq = principal.Claims.FirstOrDefault(x => x.Type.ToLower() == "syaincdseq")?.Value;
                if (SyainCdSeq != null) ClaimModel.SyainCdSeq = int.Parse(SyainCdSeq);
                var CompanyId = principal.Claims.FirstOrDefault(x => x.Type.ToLower() == "companyid")?.Value;
                if (CompanyId != null) ClaimModel.CompanyID = int.Parse(CompanyId);
                var EigyoCdSeq = principal.Claims.FirstOrDefault(x => x.Type.ToLower() == "eigyocdseq")?.Value;
                if (EigyoCdSeq != null) ClaimModel.EigyoCdSeq = int.Parse(EigyoCdSeq);
            }
        }
        try
        {
            await Task.Run(() =>
            {
                Loadpage().Wait();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //TODO use logger to log down exception ex.message
        }
    }
    public async Task Loadpage()
    {
        try
        {
            if (new ClaimModel().TenantID != 0)
            {
                notification = await TenantGroupService.GetUnreadNotifications(new ClaimModel().TenantID);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //TODO use logger to log down exception ex.message
        }
    }
    private async Task reloadcout(string a)
    {
        await Task.Run(() =>
        {
            Loadpage().Wait();
        });
        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("removeTooltip");
        thisReference = DotNetObjectReference.Create(this);
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("fadeToggle");
            await JSRuntime.InvokeVoidAsync("showNofitications");
            await JSRuntime.InvokeVoidAsync("speechRecognition", "ja-JP", thisReference);
        }
    }

    private void RedirectToSuperMenu(SuperMenyTypeDisplay type)
    {
        if (IsValid)
        {
            Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
            keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForHyperFormData(hyperData).Result;
            FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.SuperMenuReservation, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
            string QueryParam = hyperData.ToQueryString();
            //NavigationManager.NavigateTo(string.Format("/supermenu?{0}&type={1}", QueryParam, (int)type), true);
            NavigationManager.NavigateTo(string.Format("/supermenu?type={0}", (int)type), true);
        }
    }

    protected void OnChangedInput(ChangeEventArgs e)
    {
        InputText = e.Value.ToString().Trim();
        CheckInput(InputText);
        StateHasChanged();
    }

    private bool ValidateDate(string date, bool isNotValidateDate = false)
    {
        DateTime tempDate = DateTime.MinValue;
        List<string> listFormat = new List<string>()
        {
            "yyyy", "yyyyMMdd", "yyyy/MM/dd", "yyyyMM", "yyyy/MM", "yyyy年MM月", "yyyy年M月", "yyyy年M月d日", "yyyy年MM月dd日", "yyyy年MM月d日", "yyyy年M月dd日"
        };
        bool isValidDate = true;
        string format = string.Empty;
        if (!string.IsNullOrEmpty(date))
        {
            for (int i = 0; i < listFormat.Count; i++)
            {
                isValidDate = DateTime.TryParseExact(date, listFormat[i], CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate);
                if (isValidDate)
                {
                    format = listFormat[i];
                    break;
                }
            }
        }

        if (isNotValidateDate)
        {
            if (!isValidDate)
            {
                hyperData.DantaiNm = date;
            }
            isValidDate = true;
        }

        if (isValidDate)
        {
            if (format.Contains("d"))
            {
                hyperData.HaishaBiFrom = tempDate;
                hyperData.HaishaBiTo = tempDate;
            }
            else if (format.Equals("yyyy"))
            {
                hyperData.HaishaBiFrom = new DateTime(int.Parse(date), 1, 1);
                hyperData.HaishaBiTo = new DateTime(int.Parse(date), 12, 31);
            }
            else if (string.IsNullOrEmpty(format))
            {
                hyperData.HaishaBiTo = new DateTime(DateTime.Now.Year, 12, 31);
            }
            else
            {
                hyperData.HaishaBiFrom = new DateTime(tempDate.Year, tempDate.Month, 1);
                hyperData.HaishaBiTo = hyperData.HaishaBiFrom.Value.AddMonths(1).AddDays(-1);
            }
        }
        return isValidDate;
    }

    protected void InputSpeechFake()
    {
        isSpeaking = true;
        isProcessing = true;
        StateHasChanged();
    }

    [JSInvokable]
    public void InputSpeech(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            data = data.Trim();
            CheckInput(data, true);
            RedirectToSuperMenu(SuperMenyTypeDisplay.Reservation);
            if (!IsValid)
            {
                InputText = data;
                InvokeAsync(StateHasChanged);
            }
        }
    }

    //protected void InputSpeech()
    //{
    //    if (!isSpeaking)
    //    {
    //        string lang = "ja-JP";
    //        string path = Path.Combine(hostingEnvironment.ContentRootPath, "GoogleSpeechToText-31eba7ef6c30.json");
    //        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

    //        if (NAudio.Wave.WaveIn.DeviceCount < 1)
    //        {
    //            strSpeaking = "No microphone!";
    //        }
    //        else
    //        {
    //            isSpeaking = true;
    //            InvokeAsync(StateHasChanged);
    //            int seconds = 1;
    //            var speech = SpeechClient.Create();
    //            var streamingCall = speech.StreamingRecognize();
    //            // Write the initial request with the config.
    //            streamingCall.WriteAsync(
    //                new StreamingRecognizeRequest()
    //                {
    //                    StreamingConfig = new StreamingRecognitionConfig()
    //                    {
    //                        Config = new RecognitionConfig()
    //                        {
    //                            Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
    //                            SampleRateHertz = 16000,
    //                            LanguageCode = LanguageCodes.Vietnamese.Vietnam
    //                        },
    //                        InterimResults = true,
    //                        SingleUtterance = true
    //                    }
    //                }).Wait();
    //            // Print responses as they arrive.
    //            recognitionEnded = false;
    //            Task printResponses = Task.Run(async () =>
    //            {
    //                var responseStream = streamingCall.GetResponseStream();
    //                while (await responseStream.MoveNextAsync())
    //                {
    //                    StreamingRecognizeResponse response = responseStream.Current;
    //                    // Z Add to check end response: có điều kiện này thì ko cần đợi hết giây
    //                    if (response.SpeechEventType == StreamingRecognizeResponse.Types.SpeechEventType.EndOfSingleUtterance)
    //                    {
    //                        recognitionEnded = true;
    //                    }
    //                    foreach (StreamingRecognitionResult result in response.Results)
    //                    {
    //                        foreach (SpeechRecognitionAlternative alternative in result.Alternatives)
    //                        {
    //                            if (result.IsFinal)
    //                            {
    //                                strStreamText = alternative.Transcript;
    //                            }
    //                        }
    //                    }
    //                }
    //            });

    //        //Read from the microphone and stream to API.
    //        object writeLock = new object();
    //            bool writeMore = true;
    //            var waveIn = new NAudio.Wave.WaveInEvent();
    //            waveIn.DeviceNumber = 0;
    //            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
    //            waveIn.DataAvailable +=
    //                (object sender, NAudio.Wave.WaveInEventArgs args) =>
    //                {
    //                    lock (writeLock)
    //                    {
    //                        if (!writeMore)
    //                        {
    //                            return;
    //                        }

    //                        streamingCall.WriteAsync(
    //                            new StreamingRecognizeRequest()
    //                            {
    //                                AudioContent = Google.Protobuf.ByteString
    //                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
    //                            }).Wait();
    //                    }
    //                };
    //            waveIn.StartRecording();
    //            while (!recognitionEnded) {; }
    //            // Stop recording and shut down.
    //            waveIn.StopRecording();
    //            isSpeaking = false;
    //            InvokeAsync(StateHasChanged);
    //            bool isEndRecording = true;
    //            while (!isEndRecording) {; }
    //            Task.Delay(TimeSpan.FromSeconds(seconds)).Wait();
    //            if (lang == "ja-JP")
    //            {
    //                if (!string.IsNullOrEmpty(strStreamText))
    //                {
    //                    strStreamText = strStreamText.Trim();
    //                    CheckInput(strStreamText, true);
    //                    RedirectToSuperMenu(SuperMenyTypeDisplay.Reservation);
    //                    if (!IsValid)
    //                    {
    //                        InputText = strStreamText;
    //                        InvokeAsync(StateHasChanged);
    //                    }
    //                }
    //            }
    //            lock (writeLock)
    //            {
    //                writeMore = false;
    //            }

    //            streamingCall.WriteCompleteAsync().Wait();
    //            printResponses.Wait();
    //        }
    //    }
    //    else
    //    {
    //        recognitionEnded = true;
    //        InvokeAsync(StateHasChanged);
    //    }
    //}

    private void CheckInput(string inputText, bool isValidateSpeech = false)
    {
        if (!string.IsNullOrEmpty(inputText))
        {
            inputText = inputText.Replace('　', ' ');
            if (isValidateSpeech)
            {
                inputText = inputText.Replace(" ", string.Empty);
            }
            bool isValid = false;
            if (inputText.Contains(' '))
            {
                string date = inputText.Substring(inputText.LastIndexOf(" ") + 1);
                string dantaNm = inputText.Substring(0, inputText.LastIndexOf(" "));
                isValid = ValidateDate(date);
                if (isValid)
                {
                    hyperData.DantaiNm = dantaNm;
                    IsValid = true;
                    StateHasChanged();
                    return;
                }
            }
            else
            {
                isValid = isValidateSpeech ? ValidateDateSpeech(inputText) : ValidateDate(inputText, true);
                if (isValid)
                {
                    IsValid = true;
                    StateHasChanged();
                    return;
                }
            }
        }
        IsValid = false;
        hyperData.DantaiNm = string.Empty;
        hyperData.HaishaBiFrom = null;
        hyperData.HaishaBiTo = null;
    }

    private bool ValidateDateSpeech(string inputString)
    {
        Regex regex = new Regex("([0-9]{0,4}[年日]{0,1}[0-9]{1,2}[月]{0,1}[0-9]{0,4}[年日]{0,1}|一昨日|昨日|今日|明日|明後日|((次の|先週の|今週の|来週の){0,1}(月曜日|火曜日|水曜日|木曜日|金曜日|土曜日|日曜日))|先週|来週|先月|今月|来月|今年|本年|来年|翌年|昨年|去年)");
        Match match = regex.Match(inputString);
        if (!string.IsNullOrEmpty(match.Value))
        {
            hyperData.DantaiNm = inputString.Replace(match.Value, string.Empty).Trim();
            inputString = inputString.Replace(hyperData.DantaiNm, string.Empty);

            Regex regexNumber = new Regex("\\d");
            string formatDate = string.Empty;
            DateTime day = DateTime.MinValue;
            switch (inputString)
            {
                case string number when regexNumber.IsMatch(number):
                    var result = CalculateDateNumber(inputString); day = result.Item1; formatDate = result.Item2; break;
                case string next when next.Contains("次の") || next.Contains("来週の"):
                    day = CalculateDateString(inputString.Replace("次の", string.Empty).Replace("来週の", string.Empty), 7); formatDate = "d"; break;
                case string prev when prev.Contains("先週の"):
                    day = CalculateDateString(inputString.Replace("先週の", string.Empty), -7); formatDate = "d"; break;
                case "先月":
                    day = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
                    formatDate = string.Empty;
                    break;
                case "今月":
                    day = DateTime.Today;
                    formatDate = string.Empty;
                    break;
                case "来月":
                    day = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
                    formatDate = string.Empty;
                    break;
                case "今年":
                case "本年":
                    formatDate = "y";
                    break;
                case "来年":
                case "翌年":
                    formatDate = "y + 1";
                    break;
                case "昨年":
                case "去年":
                    formatDate = "y - 1";
                    break;
                default:
                    day = CalculateDateString(inputString.Replace("今週の", string.Empty), 0); formatDate = "d"; break;
            }

            if (day == DateTime.MinValue)
            {
                return false;
            }

            if (formatDate.Contains("d"))
            {
                hyperData.HaishaBiFrom = day;
                hyperData.HaishaBiTo = day;
            }
            else if (formatDate.Equals("y"))
            {
                hyperData.HaishaBiFrom = new DateTime(DateTime.Now.Year, 1, 1);
                hyperData.HaishaBiTo = new DateTime(DateTime.Now.Year, 12, 31);
            }
            else if (formatDate.Equals("y - 1"))
            {
                hyperData.HaishaBiFrom = new DateTime(DateTime.Now.Year - 1, 1, 1);
                hyperData.HaishaBiTo = new DateTime(DateTime.Now.Year - 1, 12, 31);
            }
            else if (formatDate.Equals("y + 1"))
            {
                hyperData.HaishaBiFrom = new DateTime(DateTime.Now.Year + 1, 1, 1);
                hyperData.HaishaBiTo = new DateTime(DateTime.Now.Year + 1, 12, 31);
            }
            else
            {
                hyperData.HaishaBiFrom = new DateTime(day.Year, day.Month, 1);
                hyperData.HaishaBiTo = hyperData.HaishaBiFrom.Value.AddMonths(1).AddDays(-1);
            }
        }
        else
        {
            hyperData.DantaiNm = inputString;
            hyperData.HaishaBiTo = new DateTime(DateTime.Now.Year, 12, 31);
        }
        return true;
    }

    private static (DateTime, string) CalculateDateNumber(string date)
    {
        DateTime returnDate = DateTime.MinValue;
        string formatDate = string.Empty;
        List<string> listFormat = new List<string>()
        {
            "yyyyMMdd", "yyyyMM", "yyyy年MM月", "yyyy年M月", "yyyy年M月d日", "yyyy年MM月dd日", "yyyy年MM月d日", "yyyy年M月dd日"
        };
        foreach (var item in listFormat)
        {
            if (DateTime.TryParseExact(date, item, CultureInfo.InvariantCulture, DateTimeStyles.None, out returnDate))
            {
                formatDate = item;
                break;
            }
        }
        return (returnDate, formatDate);
    }

    private static DateTime CalculateDateString(string date, int daysAdd)
    {
        DateTime today = DateTime.Today;
        var dayOfWeek = -1;
        switch (date)
        {
            case "月曜日":
                dayOfWeek = (int)DayOfWeek.Monday; break;
            case "火曜日":
                dayOfWeek = (int)DayOfWeek.Tuesday; break;
            case "水曜日":
                dayOfWeek = (int)DayOfWeek.Wednesday; break;
            case "木曜日":
                dayOfWeek = (int)DayOfWeek.Thursday; break;
            case "金曜日":
                dayOfWeek = (int)DayOfWeek.Friday; break;
            case "土曜日":
                dayOfWeek = (int)DayOfWeek.Saturday; break;
            case "日曜日":
                dayOfWeek = (int)DayOfWeek.Sunday; break;
            case "明日":
                today = today.AddDays(1); break;
            case "明後日":
                today = today.AddDays(2); break;
            case "昨日":
                today = today.AddDays(-1); break;
            case "一昨日":
                today = today.AddDays(-2); break;
            case "先週":
                today = today.AddDays(-7); break;
            case "来週":
                today = today.AddDays(7); break;
                //case "今日":
                //    today = DateTime.Today; break;
        }

        if (dayOfWeek > -1)
        {
            today = today.AddDays(-(int)today.DayOfWeek);
            if (dayOfWeek != (int)today.DayOfWeek)
            {
                today = today.AddDays(dayOfWeek);
            }
            today = today.AddDays(daysAdd);
        }

        return today;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITenantGroupServiceService TenantGroupService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Pages.Home> lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IWebHostEnvironment hostingEnvironment { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFilterCondition FilterConditionService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHyperDataService HyperDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591

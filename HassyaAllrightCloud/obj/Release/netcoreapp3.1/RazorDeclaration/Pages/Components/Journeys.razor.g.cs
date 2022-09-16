#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Journeys.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c4f4ee9ce0b598577b5fd49dddaa2363478c74fd"
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
#line 28 "E:\Project\HassyaAllrightCloud\_Imports.razor"
using DevExpress.Blazor.Reporting;

#line default
#line hidden
#nullable disable
    public partial class Journeys : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 110 "E:\Project\HassyaAllrightCloud\Pages\Components\Journeys.razor"
       
    [Parameter] public string ukeNo { get; set; }
    [Parameter] public short bunkRen { get; set; }
    List<JourneysData> CarListData = new List<JourneysData>();
    JourneysData CarListItem = new JourneysData();
    List<JourneyDataDate> datelst = new List<JourneyDataDate>();
    List<TKD_KoteiData> textlst = new List<TKD_KoteiData>();
    JourneyDataDate dateItem = new JourneyDataDate();
    JourneyDataDate dateItemisnert = new JourneyDataDate();
    TKD_KoteikData koteikData = new TKD_KoteikData();
    string tempstr;
    JourneysData CarList;
    DateTime fromdate;
    DateTime todate;
    DateTime fromdatemin;
    DateTime todatemax;
    bool isZenHaFlg = false;
    bool isKhakFlg = false;
    bool isCommon = true;
    bool popupsave = false;
    bool popupclear = false;
    string ukeno;
    int ukeCD;
    short unkRen;
    string text;
    string baseUrl;
    string message;
    TimeSpan SyuPaTime;
    TimeSpan TouChTime;
    string JisaIPKm = "0";
    string JisaKSKm = "0";
    string KisoIPkm = "0";
    string KisoKOKm = "0";
    short bunkRenMin = 1;
    short bunkRenMax = 1;
    string TimesEndKha = "";
    string TimesStartZen = "";
    protected override async Task OnInitializedAsync()
    {
        isZenHaFlg = false;
        isKhakFlg = false;
        isCommon = true;
        CarListData = await BusBookingDataService.GetJourneysData(ukeNo);
        /*New M*/
        bunkRenMin = CarListData.OrderBy(x => x.Haisha_BunkRen).ToList().First().Haisha_BunkRen;
        bunkRenMax = CarListData.OrderBy(x => x.Haisha_BunkRen).ToList().Last().Haisha_BunkRen;


        ukeCD = CarListData.First().Yyksho_UkeCd;
        CarListData.Insert(0, new JourneysData());
        CarListItem = CarListData.First();
        CarListData.First().Unkobi_TouChTime = CarListData.Skip(1).Take(1).First().Unkobi_TouChTime;
        CarListData.First().Unkobi_SyuPaTime = CarListData.Skip(1).Take(1).First().Unkobi_SyuPaTime;
        baseUrl = AppSettingsService.GetBaseUrl();
        DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_HaiSYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out fromdatemin);
        DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_HaiSYmd,
                              "yyyyMMdd",
                              CultureInfo.CurrentCulture,
                              DateTimeStyles.None,
                              out fromdate);
        DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_TouYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out todate);
        DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_TouYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out todatemax);
        if (CarListData.Skip(1).Take(1).First().Unkobi_ZenHaFlg == 1)
        {
            isZenHaFlg = true;
        }
        if (CarListData.Skip(1).Take(1).First().Unkobi_KhakFlg == 1)
        {
            isKhakFlg = true;
        }
        ukeno = CarListData.Skip(1).Take(1).First().Haisha_UkeNo;
        unkRen = CarListData.Skip(1).Take(1).First().Haisha_UnkRen;
        datelst = getDateData(fromdate, todate, fromdatemin, todatemax, true, isZenHaFlg, isKhakFlg, ukeno, CarListData.First().Haisha_BunkRen, CarListData.First().Haisha_TeiDanNo, unkRen, CarListData.First(),0);
        dateItem = datelst.First();
        dateItemisnert = dateItem;
        SyuPaTime = dateItem.form;
        TouChTime = dateItem.to;
        TimesStartZen = dateItem.timeStartZenHaFlg;
        TimesEndKha = dateItem.timeEndKhakFlg;
        text = "";
        textlst = new List<TKD_KoteiData>();
        textlst = await TKD_KoteiDataService.GetDataKotei(dateItem.isCommom, ukeno, dateItem.isZenHaFlg, dateItem.isKhakFlg, dateItem.teiDanNo, unkRen, dateItem.bunkRen, dateItem.nittei);
        foreach (var items in textlst)
        {
            text += items.Kotei_Koutei + "\n";
            JisaIPKm = items.Koteik_JisaIPKm.ToString();
            JisaKSKm = items.Koteik_JisaKSKm.ToString();
            KisoIPkm = items.Koteik_KisoIPkm.ToString();
            KisoKOKm = items.Koteik_KisoKOKm.ToString();
            if (items.Koteik_SyuPaTime != "")
            {
                SyuPaTime = BusScheduleHelper.ConvertTime(items.Koteik_SyuPaTime);
            }
            if (items.Koteik_TouChTime != "")
            {
                TouChTime = BusScheduleHelper.ConvertTime(items.Koteik_TouChTime);
            }
        }
        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await JSRuntime.InvokeAsync<string>("settextinlinetextarea", 37);
        }

        await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 7);
        await JSRuntime.InvokeAsync<string>("addMaxLength", "length", 4);
        await JSRuntime.InvokeVoidAsync("setEventforCurrencyField");

    }

    #region InputTime changed event

    private void OnStartTimeTextChanged(BookingInputHelper.MyTime newTime)
    {
        string timeValue = CommonUtil.MyTimeFormat(newTime.Str, Formats.HHmm);

        SyuPaTime = BusScheduleHelper.ConvertTime(timeValue);
        StateHasChanged();
    }

    private void OnArrivedTimeTextChanged(BookingInputHelper.MyTime newTime)
    {
        string timeValue = CommonUtil.MyTimeFormat(newTime.Str, Formats.HHmm);

        TouChTime = BusScheduleHelper.ConvertTime(timeValue);
        StateHasChanged();
    }

    #endregion

    private async Task DateItemChanged(JourneyDataDate dataDateItem)
    {
        text = "";
        dateItem = dataDateItem;
        dateItemisnert = dataDateItem;
        SyuPaTime = dateItem.form;
        TouChTime = dateItem.to;
        TimesStartZen = dateItem.timeStartZenHaFlg;
        TimesEndKha = dateItem.timeEndKhakFlg;
        textlst = new List<TKD_KoteiData>();
        textlst = await TKD_KoteiDataService.GetDataKotei(dataDateItem.isCommom, dataDateItem.ukeNo, dataDateItem.isZenHaFlg, dataDateItem.isKhakFlg, dataDateItem.teiDanNo, dataDateItem.unkRen, dataDateItem.bunkRen, dataDateItem.nittei);
        foreach (var item in textlst)
        {
            text += item.Kotei_Koutei + "\n";
            JisaIPKm = item.Koteik_JisaIPKm.ToString();
            JisaKSKm = item.Koteik_JisaKSKm.ToString();
            KisoIPkm = item.Koteik_KisoIPkm.ToString();
            KisoKOKm = item.Koteik_KisoKOKm.ToString();
        }
        if(textlst.Count >0)
        {
            SyuPaTime = BusScheduleHelper.ConvertTime(textlst[0].Koteik_SyuPaTime);
            TimesStartZen = "";

            TouChTime = BusScheduleHelper.ConvertTime(textlst[0].Koteik_TouChTime);
            TimesEndKha = "";
        }
        StateHasChanged();
    }
    private async Task DateItemInsertChanged(JourneyDataDate dataDateItem)
    {
        dateItemisnert = dataDateItem;
        StateHasChanged();
    }
    private async Task CarListDataChanged(JourneysData journeysItem)
    {
        bool checkAllCarList = true;
        CarListItem = new JourneysData();
        CarListItem = journeysItem;
        if (journeysItem.Haisha_UkeNo == "0")
        {
            isCommon = true;
            DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_HaiSYmd,
                                   "yyyyMMdd",
                                   CultureInfo.CurrentCulture,
                                   DateTimeStyles.None,
                                   out fromdatemin);
            DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_HaiSYmd,
                                  "yyyyMMdd",
                                  CultureInfo.CurrentCulture,
                                  DateTimeStyles.None,
                                  out fromdate);
            DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_TouYmd,
                                   "yyyyMMdd",
                                   CultureInfo.CurrentCulture,
                                   DateTimeStyles.None,
                                   out todate);
            DateTime.TryParseExact(CarListData.Skip(1).Take(1).First().Unkobi_TouYmd,
                                   "yyyyMMdd",
                                   CultureInfo.CurrentCulture,
                                   DateTimeStyles.None,
                                   out todatemax);
            if (CarListData.Skip(1).Take(1).First().Unkobi_ZenHaFlg == 1)
            {
                isZenHaFlg = true;
            }
            if (CarListData.Skip(1).Take(1).First().Unkobi_KhakFlg == 1)
            {
                isKhakFlg = true;
            }
            ukeno = CarListData.Skip(1).Take(1).First().Haisha_UkeNo;
            unkRen = CarListData.Skip(1).Take(1).First().Haisha_UnkRen;
        }
        else
        {
            isCommon = false;
            DateTime.TryParseExact(journeysItem.Unkobi_HaiSYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out fromdatemin);
            DateTime.TryParseExact(journeysItem.Haisha_HaiSYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out fromdate);
            DateTime.TryParseExact(journeysItem.Haisha_TouYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out todate);
            DateTime.TryParseExact(journeysItem.Unkobi_TouYmd,
                               "yyyyMMdd",
                               CultureInfo.CurrentCulture,
                               DateTimeStyles.None,
                               out todatemax);
            if (journeysItem.Unkobi_ZenHaFlg == 1)
            {
                isZenHaFlg = true;
            }
            if (journeysItem.Unkobi_KhakFlg == 1)
            {
                isKhakFlg = true;
            }
            ukeno = journeysItem.Haisha_UkeNo;
            unkRen = journeysItem.Haisha_UnkRen;
        }

        datelst = getDateData(fromdate, todate, fromdatemin, todatemax, isCommon, isZenHaFlg, isKhakFlg, ukeno, journeysItem.Haisha_BunkRen, journeysItem.Haisha_TeiDanNo, unkRen, journeysItem,1);
        dateItem = datelst.First();
        dateItemisnert = dateItem;
        SyuPaTime = dateItem.form;
        TouChTime = dateItem.to;
        TimesStartZen = dateItem.timeStartZenHaFlg;
        TimesEndKha = dateItem.timeEndKhakFlg;
        text = "";
        textlst = new List<TKD_KoteiData>();
        textlst = await TKD_KoteiDataService.GetDataKotei(dateItem.isCommom, ukeno, dateItem.isZenHaFlg, dateItem.isKhakFlg, dateItem.teiDanNo, unkRen, dateItem.bunkRen, dateItem.nittei);
        foreach (var items in textlst)
        {
            text += items.Kotei_Koutei + "\n";
            JisaIPKm = items.Koteik_JisaIPKm.ToString();
            JisaKSKm = items.Koteik_JisaKSKm.ToString();
            KisoIPkm = items.Koteik_KisoIPkm.ToString();
            KisoKOKm = items.Koteik_KisoKOKm.ToString();
            if (items.Koteik_SyuPaTime != "")
            {
                SyuPaTime = BusScheduleHelper.ConvertTime(items.Koteik_SyuPaTime);
            }
            if (items.Koteik_TouChTime != "")
            {
                TouChTime = BusScheduleHelper.ConvertTime(items.Koteik_TouChTime);
            }
        }
        InvokeAsync(StateHasChanged);
    }
    public List<JourneyDataDate> getDateData(DateTime fromdate, DateTime todate, DateTime fromdatemin, DateTime todatemax, bool isCommom, bool isZenHaFlg, bool isKhakFlg, string ukeNo, short bunkRen, short teiDanNo, short unkRen, JourneysData journeyitem,int checkLoadFirst)
    {
        datelst = new List<JourneyDataDate>();
        int i = 0;
        for (DateTime date = fromdate; date <= todate; date = date.AddDays(1))
        {
            JourneyDataDate itemdate = new JourneyDataDate();
            itemdate.isKhakFlg = isKhakFlg;
            if (isKhakFlg == true)
            {
                itemdate.isKhakFlg = !isKhakFlg;
            }
            itemdate.isZenHaFlg = isZenHaFlg;
            if (isZenHaFlg == true)
            {
                itemdate.isZenHaFlg = !isZenHaFlg;
            }
            itemdate.tomKbn = 1;
            i++;
            itemdate.nittei = (byte)i;
            if (!isCommom)
            {
                itemdate.isCommom = isCommom;
            }
            itemdate.date = date;
            itemdate.ukeNo = ukeno;
            itemdate.bunkRen = bunkRen;
            itemdate.unkRen = unkRen;
            itemdate.teiDanNo = teiDanNo;

            if(isCommom)
            {
                itemdate.form = BusScheduleHelper.ConvertTime(journeyitem.Unkobi_SyuPaTime);
                itemdate.to = BusScheduleHelper.ConvertTime(journeyitem.Unkobi_TouChTime);
            }
            else
            {
                itemdate.form = BusScheduleHelper.ConvertTime(journeyitem.Haisha_SyuPaTime);
                itemdate.to = BusScheduleHelper.ConvertTime(journeyitem.Haisha_TouChTime);
            }
            datelst.Add(itemdate);
        }
        if (isCommom == true)
        {
            if (isZenHaFlg && fromdate == fromdatemin)
            {
                JourneyDataDate itemdate = new JourneyDataDate();
                if(isKhakFlg==false)
                {
                    itemdate.isKhakFlg = isKhakFlg;
                }
                else
                {
                    itemdate.isKhakFlg = !isKhakFlg;
                }
                itemdate.isZenHaFlg = isZenHaFlg;
                itemdate.nittei = 1;
                itemdate.tomKbn = 2;
                if (!isCommom)
                {
                    itemdate.isCommom = isCommom;
                }
                itemdate.date = fromdate.AddDays(-1);
                itemdate.ukeNo = ukeno;
                itemdate.bunkRen = bunkRen;
                itemdate.unkRen = unkRen;
                itemdate.teiDanNo = teiDanNo;
                itemdate.form = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeStartZenHaFlg = "1";
                itemdate.to = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeEndKhakFlg = "1";
                datelst.Add(itemdate);
            }
            if (isKhakFlg && todate == todatemax)
            {
                JourneyDataDate itemdate = new JourneyDataDate();
                if(isZenHaFlg==false)
                {
                    itemdate.isZenHaFlg = isZenHaFlg;
                }
                else
                {
                    itemdate.isZenHaFlg = !isZenHaFlg;
                }
                itemdate.isKhakFlg = isKhakFlg;
                itemdate.nittei = 1;
                itemdate.tomKbn = 3;
                if (!isCommom)
                {
                    itemdate.isCommom = isCommom;
                }
                itemdate.date = todate.AddDays(1);
                itemdate.ukeNo = ukeno;
                itemdate.bunkRen = bunkRen;
                itemdate.unkRen = unkRen;
                itemdate.teiDanNo = teiDanNo;
                itemdate.form = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeStartZenHaFlg = "1";
                itemdate.to = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeEndKhakFlg = "1";

                datelst.Add(itemdate);
            }

        }
        else
        {
            if (isZenHaFlg && bunkRen ==bunkRenMin)
            {
                JourneyDataDate itemdate = new JourneyDataDate();
                if(isKhakFlg==false)
                {
                    itemdate.isKhakFlg = isKhakFlg;
                }
                else
                {
                    itemdate.isKhakFlg = !isKhakFlg;
                }
                itemdate.isZenHaFlg = isZenHaFlg;
                itemdate.nittei = 1;
                itemdate.tomKbn = 2;
                if (!isCommom)
                {
                    itemdate.isCommom = isCommom;
                }
                itemdate.date = fromdate.AddDays(-1);
                itemdate.ukeNo = ukeno;
                itemdate.bunkRen = bunkRen;
                itemdate.unkRen = unkRen;
                itemdate.teiDanNo = teiDanNo;
                itemdate.form = BusScheduleHelper.ConvertTime("0000");
                itemdate.to = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeEndKhakFlg = "1";
                itemdate.timeStartZenHaFlg = "1";
                datelst.Add(itemdate);
            }
            if (isKhakFlg && bunkRen == bunkRenMax)
            {
                JourneyDataDate itemdate = new JourneyDataDate();
                if(isZenHaFlg==false)
                {
                    itemdate.isZenHaFlg = isZenHaFlg;
                }
                else
                {
                    itemdate.isZenHaFlg = !isZenHaFlg;
                }
                itemdate.isKhakFlg = isKhakFlg;
                itemdate.nittei = 1;
                itemdate.tomKbn = 3;
                if (!isCommom)
                {
                    itemdate.isCommom = isCommom;
                }
                itemdate.date = todate.AddDays(1);
                itemdate.ukeNo = ukeno;
                itemdate.bunkRen = bunkRen;
                itemdate.unkRen = unkRen;
                itemdate.teiDanNo = teiDanNo;
                itemdate.form = BusScheduleHelper.ConvertTime("0000");
                itemdate.to = BusScheduleHelper.ConvertTime("0000");
                itemdate.timeEndKhakFlg = "1";
                itemdate.timeStartZenHaFlg = "1";
                datelst.Add(itemdate);
            }
        }
        if (isCommom)
        {
            datelst.OrderBy(t => t.date).First().form = BusScheduleHelper.ConvertTime("0000");
            datelst.OrderBy(t => t.date).Last().to = BusScheduleHelper.ConvertTime("0000");

        }
        else
        {
            if (isZenHaFlg)
            {
                datelst.OrderBy(t => t.date).First().form = BusScheduleHelper.ConvertTime(journeyitem.Unkobi_SyuPaTime);
            }
            if (isKhakFlg)
            {
                datelst.OrderBy(t => t.date).Last().to = BusScheduleHelper.ConvertTime(journeyitem.Unkobi_TouChTime);
            }
        }
        /* Handle DataDate again ->Load data follow table Koteik if tb_Koteik has record of ukeno */
        datelst = datelst.OrderBy(t => t.date).ToList();
        int teiDanTomKbn = 0;
        TKD_KoteikData KoteilData = new TKD_KoteikData();
        for (int k = 0; k < datelst.Count; k++)
        {
            if (checkLoadFirst == 0 || journeyitem.Haisha_UkeNo == "0") //load first-->load chung
            {
                teiDanTomKbn = 9;
                bunkRen = 0;
            }
            else if (checkLoadFirst == 1) //change xe
            {
                teiDanTomKbn = datelst[k].tomKbn;
                bunkRen = datelst[k].bunkRen;
            }
            KoteilData = TKD_KoteikDataListService.GetDataKoteik(ukeNo, unkRen, teiDanNo, bunkRen, teiDanTomKbn, datelst[k].tomKbn, datelst[k].nittei);
            if (KoteilData != null)
            {
                datelst[k].timeEndKhakFlg = "";
                datelst[k].timeStartZenHaFlg = "";
                datelst[k].form = BusScheduleHelper.ConvertTime(KoteilData.SyuPaTime);
                datelst[k].to = BusScheduleHelper.ConvertTime(KoteilData.TouChTime);
            }
        }

        datelst =datelst.OrderBy(t => t.date).ToList();

        return datelst.OrderBy(t => t.date).ToList();
    }
    private async Task UpdateClear(MouseEventArgs e)
    {
        popupclear = false;
        datelst = getDateData(fromdate, todate, fromdatemin, todatemax, true, isZenHaFlg, isKhakFlg, ukeno, CarListData.First().Haisha_BunkRen, CarListData.First().Haisha_TeiDanNo, unkRen, CarListData.First(),1);
        dateItem = datelst.First();
        dateItemisnert = dateItem;
        SyuPaTime = dateItem.form;
        TouChTime = dateItem.to;
        JisaIPKm = "0";
        JisaKSKm = "0";
        KisoIPkm = "0";
        KisoKOKm = "0";
        text = "";
        dateItemisnert = dateItem;
        StateHasChanged();
    }
    private async Task CancelClear(MouseEventArgs e)
    {
        popupclear = false;
        StateHasChanged();
    }
    private async void ClearKotei(MouseEventArgs e)
    {
        popupclear = true;
        StateHasChanged();
    }
    private async Task CancelSave(MouseEventArgs e)
    {
        popupsave = false;
        message = "";
        StateHasChanged();
    }
    private async void UpdateKotei(MouseEventArgs e)
    {
        var client = new HttpClient();
        koteikData = new TKD_KoteikData();
        koteikData.UkeNo = dateItemisnert.ukeNo;
        koteikData.UnkRen = dateItemisnert.unkRen;
        koteikData.BunkRen = dateItemisnert.bunkRen;
        koteikData.isCommom = dateItemisnert.isCommom;
        koteikData.isKhakFlg = dateItemisnert.isKhakFlg;
        koteikData.isZenHaFlg = dateItemisnert.isZenHaFlg;
        koteikData.Nittei = dateItemisnert.nittei;
        koteikData.TeiDanNo = dateItemisnert.teiDanNo;
        koteikData.TomKbn = dateItemisnert.tomKbn;
        koteikData.SyuPaTime = SyuPaTime.ToString("hhmm");
        koteikData.TouChTime = TouChTime.ToString("hhmm");
        decimal JisaIPKmParse;
        decimal JisaKSKmParse;
        decimal KisoIPkmParse;
        decimal KisoKOKmParse;

        if (decimal.TryParse(JisaIPKm, out JisaIPKmParse))
        {
            koteikData.JisaIPKm = JisaIPKmParse;
        }
        else
        {
            koteikData.JisaIPKm = 0;
            JisaIPKm = "0";
        }

        if (decimal.TryParse(JisaKSKm, out JisaKSKmParse))
        {
            koteikData.JisaKSKm = JisaKSKmParse;
        }
        else
        {
            koteikData.JisaKSKm = 0;
            JisaKSKm = "0";
        }
        if (decimal.TryParse(KisoIPkm, out KisoIPkmParse))
        {
            koteikData.KisoIPkm = KisoIPkmParse;
        }
        else
        {
            koteikData.KisoIPkm = 0;
            KisoIPkm = "0";
        }
        if (decimal.TryParse(KisoKOKm, out KisoKOKmParse))
        {
            koteikData.KisoKOKm = KisoKOKmParse;
        }
        else
        {
            koteikData.KisoKOKm = 0;
            KisoKOKm = "0";
        }
        List<string> listTpm = new List<string>(
                       text.Split(new string[] { "\r\n", "\n" },
                       StringSplitOptions.None));
        List<string> list = new List<string>();
        for(int f=0;f< listTpm.Count;f++)
        {
            if(f==listTpm.Count-1) //end
            {
                if(listTpm[f].ToString()!="")
                {
                    list.Add(listTpm[f]);
                }
            }
            else
            {
                list.Add(listTpm[f]);
            }
        }



        /* if (String.IsNullOrEmpty(list.Last().ToString())|| String.IsNullOrWhiteSpace(list.Last().ToString()))
         {
             list.Remove(list.Last());
         }*/
        List<TKD_KoteiDataInsert> JourneyLst = new List<TKD_KoteiDataInsert>();
        int i = 1;
        foreach (var item in list)
        {

            if(item.Length >37)
            {
                int textLen = item.Length;
                int textRemain = textLen % 37;
                int loopNumber = textLen / 37;
                if (textRemain > 0) loopNumber += 1;
                string[] textArr = new string[loopNumber];
                int start = 0;
                int end = 37;
                for (int k=0;k< loopNumber;k++)
                {
                    if (k+1 == loopNumber)
                    {
                        textArr[k] = item.Substring(start, textRemain);
                    }
                    else
                    {
                        textArr[k] = item.Substring(start, end);
                    }
                    start += end;
                }
                for(int j=0;j< textArr.Length; j++)
                {
                    TKD_KoteiDataInsert JourneyItem = new TKD_KoteiDataInsert();
                    JourneyItem.KouRen = (byte)i;
                    JourneyItem.Koutei = textArr[j];
                    JourneyLst.Add(JourneyItem);
                    i++;
                }
            }

            else
            {
                TKD_KoteiDataInsert JourneyItem = new TKD_KoteiDataInsert();
                JourneyItem.KouRen = (byte)i;
                JourneyItem.Koutei = item;
                JourneyLst.Add(JourneyItem);
                i++;
            }
        }
        koteikData.JourneyLst = JourneyLst;
        HttpResponseMessage response = await client.PostAsync($"{baseUrl}/api/TkdKoteik/", Http.getStringContentFromObject(koteikData));
        if((int)response.StatusCode==200)
        {
            message =Lang["Message_save"];
            popupsave = true;
        }
        else
        {
            message = Lang["Message_unsave"];
            popupsave = true;
        }
        StateHasChanged();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Journeys> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private BusScheduleHelper BusScheduleHelper { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private AppSettingsService AppSettingsService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomHttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_KoteikDataListService TKD_KoteikDataListService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ITKD_KoteiDataListService TKD_KoteiDataService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBusBookingDataListService BusBookingDataService { get; set; }
    }
}
#pragma warning restore 1591

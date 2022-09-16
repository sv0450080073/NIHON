#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\Tehai.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6cfb61897bb7e206520f6a7ea0139074d6724f05"
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
    public partial class Tehai : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 247 "E:\Project\HassyaAllrightCloud\Pages\Components\Tehai.razor"
       
    List<TehaiData> data = new List<TehaiData>();
    List<string> carlst;
    string carItem;

    List<string> schedulelst = new List<string>();
    string scheduleItem;

    List<string> tehaicodelst = new List<string>();
    string tehaicodeItem;

    List<string> tehaiclassificationlst = new List<string>();
    string tehaiclassificationItem;

    List<string> tehaiplacecodelst = new List<string>();
    string tehaiplacecodeItem;

    List<string> tehaitypecodelst = new List<string>();
    string tehaitypecodeItem;

    bool collapseRow = false;
    string showRowCssClass => collapseRow ? null : "d-none";
    string tempstr;

    protected override void OnInitialized()
    {
        for (var i = 0; i < 5; i++)
        {
            data.Add(new TehaiData
            {
                ID = i,
            });
        }

        carlst = new List<string>()
        {
            "共通",
            "01　号車",
            "02　号車",
            "03　号車",
            "04　号車",
        };
        schedulelst = new List<string>()
        {
            "通常1日目 20/09/18 (金)",
            "通常1日目 20/09/19 (土)",
            "通常2日目 20/09/19 (土)",
            "通常3日目 20/09/20 (月)",
            "後迫1日目 20/09/21 (火)",
        };
        tehaicodelst = new List<string>()
        {
            "東京都・特別区",
            "東京・市部別",
            "東京・群部別",
            "神奈川県",
            "神奈川県・箱根",
            "千葉県・成田",
            "千葉県",
            "埼玉県",
            "茨城県",
            "栃木県・日光",
            "群馬県",
            "北海道",
            "青森県",
            "岩手県",
            "秋田県",
            "山形県",
            "宮城県",
            "福島県",
            "静岡県・熱海温泉",
        };
        tehaiclassificationlst = new List<string>()
        {
            "未指定",
            "01：空港",
            "02：駅",
            "03：港",
        };
        tehaiplacecodelst = new List<string>()
        {
            "10005：東京サマーランド",
            "10010：東京セサミプレイス",
            "30001：東京ディズニーランド"
        };
        tehaitypecodelst = new List<string>()
        {
            "宿泊",
            "見学",
            "食事",
            "その他",
        };

        carItem = carlst.First();
        scheduleItem = schedulelst.First();
        tehaicodeItem = tehaicodelst.First();
        tehaiclassificationItem = tehaiclassificationlst.First();
        tehaiplacecodeItem = tehaiplacecodelst.First();
        tehaitypecodeItem = tehaitypecodelst.First();
    }

    void SelectedCarItemChanged(string selectedItem) {
        carItem = selectedItem;
    }

    void SelectedScheduleItemChanged(string str)
    {
        scheduleItem = str;
    }

    void SelectedTehaiCodeItemChanged(string str)
    {
        tehaicodeItem = str;
    }

    void SelectedTehaiClassificationItemChanged(string str)
    {
        tehaiclassificationItem = str;
    }

    void SelectedTehaiPlaceCodeItemChanged(string str) {
        tehaiplacecodeItem = str;
    }

    void SelectedTehaiTypeCodeItemChanged(string str) {
        tehaitypecodeItem = str;
    }

    private void AddNewRow()
    {
        collapseRow = true;
        foreach (var i in data)
        {
            i.IsEdit = false;
        }
    }

    private void EditRow(TehaiData seletedItem)
    {
        collapseRow = false;
        foreach (var i in data)
        {
            if (i.ID == seletedItem.ID)
            {
                i.IsEdit = true;
            }
            else {
                i.IsEdit = false;
            }
        }
    }

    private void DeleteRow(TehaiData seletedItem)
    {
        // Todo
    }

    private void AddRow()
    {
        // Todo
    }

    private void CancelRow()
    {
        collapseRow = false;
    }

    private void UpdateRow(TehaiData seletedItem)
    {
        // Todo
    }

    private void CancelRowEdit(TehaiData seletedItem)
    {
        foreach(var i in data)
        {
            if (i.ID == seletedItem.ID) {
                i.IsEdit = false;
            }
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<Tehai> Lang { get; set; }
    }
}
#pragma warning restore 1591
#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\CancelListGridComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "01e5c190d61e34192161496e9114e12d6f38d0b8"
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
#line 36 "E:\Project\HassyaAllrightCloud\_Imports.razor"
[Authorize]

#line default
#line hidden
#nullable disable
    public partial class CancelListGridComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 24 "E:\Project\HassyaAllrightCloud\Pages\Components\CancelListGridComponent.razor"
       
    #region Parameter
    [Parameter] public List<CancelListSearchData> Data { get; set; } = new List<CancelListSearchData>();
    [Parameter] public string CssClass { get; set; }
    #endregion

    #region Variable
    protected HeaderTemplate Header { get; set; }
    protected BodyTemplate Body { get; set; }
    protected List<CancelListSearchData> CheckedItems { get; set; } = new List<CancelListSearchData>();
    [Parameter] public EventCallback<string> ValueCheckedChanged { get; set; }
    protected Pagination pagination { get; set; }
    protected int totalPage { get; set; }
    public string Value { get; set; }
    public bool isClickRow { get; set; } = false;
    public int LastXClicked { get; set; }
    public int LastYClicked { get; set; }
    public int? CurrentClick { get; set; } = null;
    public int? CurrentScroll { get; set; }
    protected ShowCheckboxArgs<CancelListSearchData> ShowCheckboxOptions { get; set; } = new ShowCheckboxArgs<CancelListSearchData>()
    {
        RowIdentifier = (checkedItem, item) => checkedItem.RowID == item.RowID,
        Disable = (item) => false
    };
    #endregion

    #region Function
    protected override async Task OnInitializedAsync()
    {      
        (Header, Body) = await IKoboSimpleGridService.InitGrid(JSRuntime, "teatest", InitHeader(), InitBody());
        await base.OnInitializedAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            JSRuntime.InvokeVoidAsync("handleSelectByKeyUp", DotNetObjectReference.Create(this));
            StateHasChanged();
        }
    }

    private HeaderTemplate InitHeader()
    {
        return new HeaderTemplate()
        {
            Rows = new List<RowHeaderTemplate>()
{
                new RowHeaderTemplate()
                {
                    Columns = new List<ColumnHeaderTemplate>()
{
                        new ColumnHeaderTemplate() { ColName = Lang["no_col"], Width = 50, RowSpan= 3 },
                        new ColumnHeaderTemplate() { ColName = Lang["customer_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["cancellation_date"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["booking_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName =Lang["processing_division"], RowSpan = 3 , Width = 150 },

                        new ColumnHeaderTemplate() { ColName = Lang["fare"]+ "・" + Lang["sales"], Width = 150, ColSpan=2 },

                        new ColumnHeaderTemplate() { ColName = Lang["CancelCharge"], Width = 150, ColSpan=2 },
                        new ColumnHeaderTemplate() { ColName = Lang["vehicle_dispatch"]+"・"+Lang["arrival"], Width = 200 ,RowSpan = 3 },
                        new ColumnHeaderTemplate() { ColName = Lang["organization_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["vehicle_type"], Width = 150, RowSpan = 3 },
                        new ColumnHeaderTemplate() { ColName = Lang["number_of_units"], Width = 50 ,RowSpan= 3},
                        new ColumnHeaderTemplate() { ColName = Lang["boarding_personnel"], Width = 100 },
                        new ColumnHeaderTemplate() { ColName = Lang["invoice"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["reception_committee_sales_office"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["BookingType"]+"・"+Lang["type"], Width = 150 },
                    }
                },
                  new RowHeaderTemplate()
                {
                    Columns = new List<ColumnHeaderTemplate>()
{
                        new ColumnHeaderTemplate() { ColName = Lang["customer_contact"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["sales_office"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["reason_cancel"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["consumption_tax"], Width = 150, ColSpan=2  },

                        new ColumnHeaderTemplate() { ColName = Lang["sales"], Width = 150, ColSpan=2},

                        new ColumnHeaderTemplate() { ColName = Lang["secretary_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["plus_people"], Width = 100 },
                        new ColumnHeaderTemplate() { ColName = Lang["issue_date"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["Staff"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["UkeYmd"], Width = 150 },
                    }
                },
                  new RowHeaderTemplate()
                {
                    Columns = new List<ColumnHeaderTemplate>()
{
                        new ColumnHeaderTemplate() { ColName = Lang["supplier_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["person_in_charge"], Width = 50 },
                        new ColumnHeaderTemplate() { ColName = Lang["strempty"], Width = 150 },

                        new ColumnHeaderTemplate() { ColName = Lang["fee"], Width = 150, ColSpan=2  },

                        new ColumnHeaderTemplate() { ColName = Lang["consumption_tax"], Width = 150, ColSpan=2 },
                        new ColumnHeaderTemplate() { ColName = Lang["destination_name"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["strempty"], Width = 100 },
                        new ColumnHeaderTemplate() { ColName = Lang["strempty"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["input_person"], Width = 150 },
                        new ColumnHeaderTemplate() { ColName = Lang["UkeCd"], Width = 150 },
                    }
                },
            }
        };
    }

    private BodyTemplate InitBody()
    {
        return new BodyTemplate()
        {
            CustomCssDelegate = CustomRowCss,
            Rows = new List<RowBodyTemplate>()
{
                new RowBodyTemplate()
                {
                    Columns = new List<ColumnBodyTemplate>()
{
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.RowID), RowSpan = 3, AlignCol = AlignColEnum.Center },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.TokuiSaki), CustomTextFormatDelegate = KoboGridHelper.ToFormatString, AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelYmdText), AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BookingName),AlignCol = AlignColEnum.Left},

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.FixedDateText),AlignCol = AlignColEnum.Left },


                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BookingAmountText),ColSpan=2,AlignCol = AlignColEnum.Right  },



                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelRateText),ColSpan=1,AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelFeeText),ColSpan=1,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.HaiSTimeTextGrid),AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.DanTaNm),AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BusType01) ,AlignCol = AlignColEnum.Right},

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Daisu01),AlignCol = AlignColEnum.Center },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.PassengerText), CustomTextFormatDelegate = KoboGridHelper.ToFormatString, AlignCol = AlignColEnum.Right },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.EmptyValue) },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.ReceivedBranch), CustomTextFormatDelegate = KoboGridHelper.ToFormatString, AlignCol = AlignColEnum.Left },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BookingStatusText), CustomTextFormatDelegate = KoboGridHelper.ToFormatString,  AlignCol = AlignColEnum.Left },
                    }
    },
                new RowBodyTemplate()
    {
        Columns = new List<ColumnBodyTemplate>()
{
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.TokuiTanNm), AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Eigos), AlignCol = AlignColEnum.Left  },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelReason),AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Status),AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },


                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.TaxRateText),ColSpan=1,AlignCol = AlignColEnum.Left},
                         new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.TaxAmountText),ColSpan=1,AlignCol = AlignColEnum.Right},

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelTaxRateText), ColSpan=1,AlignCol = AlignColEnum.Left} ,
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.CancelTaxFeeText),ColSpan=1,AlignCol = AlignColEnum.Right} ,

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.TouChTimeTextGrid),AlignCol = AlignColEnum.Left },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.KanJNm),AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BusType02), AlignCol = AlignColEnum.Right},
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Daisu02),AlignCol = AlignColEnum.Center },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.PlusJinText), CustomTextFormatDelegate = KoboGridHelper.ToFormatString, AlignCol = AlignColEnum.Right },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.InvoiceIssueDateText) ,CustomTextFormatDelegate = KoboGridHelper.ToFormatString, AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.InChargeStaff2), AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.UkeYmdText), AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },
                    }
                },
                    new RowBodyTemplate()
    {
        Columns = new List<ColumnBodyTemplate>()
{
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.ShiireSaki), AlignCol = AlignColEnum.Left  },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.InChargeStaff), AlignCol = AlignColEnum.Left  },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.EmptyValue) ,AlignCol = AlignColEnum.Left},

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Cancel),AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },


                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.ChargeRateText),ColSpan=1,AlignCol = AlignColEnum.Left, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.ChargeAmountText),ColSpan=1,AlignCol = AlignColEnum.Right, CustomTextFormatDelegate = KoboGridHelper.ToFormatString },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.EmptyValue), ColSpan=2 },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.DGInfo),AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.IkNm),AlignCol = AlignColEnum.Left },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.BusType03),AlignCol = AlignColEnum.Right },
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.Daisu03),AlignCol = AlignColEnum.Center },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.EmptyValue) },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.EmptyValue) },

                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.InputBy) ,AlignCol = AlignColEnum.Left},
                        new ColumnBodyTemplate() { DisplayFieldName = nameof(CancelListSearchData.UkeCdText),AlignCol = AlignColEnum.Left , CustomTextFormatDelegate = KoboGridHelper.ToFormatString },
                    }
                }
            }
        };
    }

    public static Func<object, string> CustomRowCss = (item) =>
    {
        var cssClass = "";
        var model = item as CancelListSearchData;

        if (model != null)
        {
            //cssClass = model.ColorClass;
        }

        return cssClass;
    };
    protected async Task RowClick(RowClickEventArgs<CancelListSearchData> args)
    {

        Value = "";
        CancelListSearchData SelectedItem = args.SelectedItem;
        LastXClicked = Convert.ToInt32(args.Event.ClientX);
        LastYClicked = Convert.ToInt32(args.Event.ClientY);
        CurrentScroll = null;
        if (!args.Event.ShiftKey && !args.Event.CtrlKey)
        {
            CheckedItems = new List<CancelListSearchData>() { SelectedItem };
            // await blazorContextMenuService.ShowMenu("gridRowClickMenu", LastXClicked, LastYClicked);
        }
        else
        {
            isClickRow = true;
            //await blazorContextMenuService.HideMenu("gridRowClickMenu");
            if (args.Event.CtrlKey)
            {
                if (!CheckedItems.Any(item => item.RowID == SelectedItem.RowID))
                {
                    CheckedItems.Add(Data.FirstOrDefault(item => item.RowID == SelectedItem.RowID));
                }
                else
                {
                    CheckedItems.RemoveAll(item => item.RowID == SelectedItem.RowID);
                    if (CheckedItems.Count == 0)
                    {
                        CurrentClick = null;
                    }
                    else
                    {
                        CurrentClick = CheckedItems.Max(item => item.RowID);
                    }
                }
            }
            else
            {
                int BeginIndex = Math.Min(SelectedItem.RowID, (int)CurrentClick);
                int EndIndex = Math.Max(SelectedItem.RowID, (int)CurrentClick);
                for (int IndexToBeAdd = BeginIndex; IndexToBeAdd <= EndIndex; IndexToBeAdd++)
                {
                    if (!CheckedItems.Any(item => item.RowID == IndexToBeAdd))
                    {
                        CheckedItems.Add(Data.FirstOrDefault(item => item.RowID == IndexToBeAdd));
                    }
                }
            }
        }
        if (CheckedItems.Any())
        {
            Value = string.Join('-', CheckedItems.Select(_ => _.RowID));
        }
        CurrentClick = SelectedItem.RowID;
        await ValueCheckedChanged.InvokeAsync(Value);
        StateHasChanged();
    }

    protected void CheckedChange(CheckedChangeEventArgs<CancelListSearchData> args)
    {
        Value = "";
        var tmpListRowId = new List<CancelListSearchData>();
        if (args.IsCheckAll == true && args.CheckedItems.Any())
        {
            Value = "0";
        }
        else if (args.IsCheckAll == true && !args.CheckedItems.Any())
        {
            Value = "";
        }
        else
        {
            if (args.IsChecked == true)
            {
                tmpListRowId.Add(args.CheckedItem);
            }
            else
            {
                tmpListRowId.Remove(args.CheckedItem);
            }
            Value = string.Join('-', tmpListRowId.Select(_ => _.RowID));
        }
        ValueCheckedChanged.InvokeAsync(Value);
        StateHasChanged();
    }
    async Task PageChanged(int pageNum)
    {
        // to do
    }
    #endregion

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IKoboSimpleGridService IKoboSimpleGridService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<CancelList> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBlazorContextMenuService blazorContextMenuService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
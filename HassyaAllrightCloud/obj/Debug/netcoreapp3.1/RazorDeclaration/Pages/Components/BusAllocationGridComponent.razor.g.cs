#pragma checksum "E:\Project\HassyaAllrightCloud\Pages\Components\BusAllocationGridComponent.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "35568c83cfeca7d78e1cda386d6a1f73b435cfdc"
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
    public partial class BusAllocationGridComponent : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 26 "E:\Project\HassyaAllrightCloud\Pages\Components\BusAllocationGridComponent.razor"
       
    #region Parameter
    [Parameter] public List<BusAllocationDataGrid> Data { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public EventCallback<BusAllocationDataGrid> OnShowPopup { get; set; }
    [Parameter] public EventCallback<List<BusAllocationDataGrid>> OnEditAll { get; set; }
    #endregion

    #region Variable
    protected HeaderTemplate Header { get; set; }
    protected BodyTemplate Body { get; set; }
    protected List<BusAllocationDataGrid> CheckedItems { get; set; } = new List<BusAllocationDataGrid>();
    protected Pagination pagination { get; set; }
    protected int totalPage { get; set; }
    private string FormName = "KU0600";
    private string FilterName = "KU0600" + " default";
    public bool isClickRow { get; set; } = false;
    public int LastXClicked { get; set; }
    public int LastYClicked { get; set; }
    public int? CurrentClick { get; set; } = null;
    public int? CurrentScroll { get; set; }
    #endregion

    #region Function
    protected override async Task OnInitializedAsync()
    {

        GenrateGrid();

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


    public static Func<object, string> CustomRowCss = (item) =>
    {
        var cssClass = "";
        var model = item as BusAllocationDataGrid;

        if (model != null)
        {
            cssClass = model.ColorClass;
        }

        return cssClass;
    };

    protected ShowCheckboxArgs<BusAllocationDataGrid> ShowCheckboxOptions { get; set; } = new ShowCheckboxArgs<BusAllocationDataGrid>()
    {
        RowIdentifier = (checkedItem, item) => checkedItem.RowID == item.RowID
    };

    protected void RowClick(RowClickEventArgs<BusAllocationDataGrid> args)
    {
        BusAllocationDataGrid SelectedItem = args.SelectedItem;
        LastXClicked = Convert.ToInt32(args.Event.ClientX);
        LastYClicked = Convert.ToInt32(args.Event.ClientY);
        CurrentScroll = null;
        if (!args.Event.ShiftKey && !args.Event.CtrlKey)
        {
            CheckedItems = new List<BusAllocationDataGrid>() { SelectedItem };
        }
        else
        {
            isClickRow = true;
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

        CurrentClick = SelectedItem.RowID;
        StateHasChanged();
    }

    protected void RowDbClick(BusAllocationDataGrid item)
    {
        OnShowPopup.InvokeAsync(item);
        StateHasChanged();
    }

    protected void CheckedChange(CheckedChangeEventArgs<BusAllocationDataGrid> args)
    {
        if (args.CheckedItems != null && args.CheckedItems.Any())
        {
            OnEditAll.InvokeAsync(args.CheckedItems);
        }
        StateHasChanged();
    }

    async Task PageChanged(int pageNum)
    {
        // to do
    }
    public async void SaveGridLayout()
    {
        await _loadingService.ShowAsync();
        await Task.Run(() =>
        {
            var headerColumns = Header.Rows.Count > 0 ? Header.Rows[0].Columns : new List<ColumnHeaderTemplate>();
            List<TkdGridLy> gridLayouts = new List<TkdGridLy>();
            if (headerColumns.Count > 0)
            {
                for (int i = 0; i < headerColumns.Count; i++)
                {
                    string itemName = headerColumns[i].CodeName;
                    gridLayouts.Add(new TkdGridLy()
                    {
                        DspNo = i,
                        FormNm = FormName,
                        FrozenCol = 0,
                        GridNm = BusAllocationGridGridHeaderNameConstants.GridName,
                        ItemNm = itemName,
                        SyainCdSeq = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdPrgId = Common.UpdPrgId,
                        UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                        UpdYmd = DateTime.Now.ToString().Substring(0, 10).Replace("/", string.Empty),
                        UpdTime = DateTime.Now.ToString().Substring(11).Replace(":", string.Empty),
                        Width = headerColumns[i].Width
                    });
                }
            }

            var isSaved = GridLayoutService.SaveGridLayout(gridLayouts).Result;
            InvokeAsync(StateHasChanged).Wait();
        });
        await _loadingService.HideAsync();
    }
    public async void InitGridLayout()
    {

        await _loadingService.ShowAsync();
        await Task.Run(() =>
        {
            List<TkdGridLy> tkdGridLies = new List<TkdGridLy>();
            RenderGridBySavedLayout(tkdGridLies);
            DeleteSavedGridLayout();
            InvokeAsync(StateHasChanged).Wait();
        });
        await _loadingService.HideAsync();
    }
    private void DeleteSavedGridLayout()
    {
        var isDeleted = GridLayoutService.DeleteSavedGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormName, BusAllocationGridGridHeaderNameConstants.GridName).Result;
    }

    private void GenrateGrid()
    {

        List<TkdGridLy> tkdGridLies = GridLayoutService.GetGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormName, BusAllocationGridGridHeaderNameConstants.GridName).Result;
        RenderGridBySavedLayout(tkdGridLies);
    }
    private void RenderGridBySavedLayout(List<TkdGridLy> gridlayouts)
    {
        HeaderTemplate headerTemplate = new HeaderTemplate();
        headerTemplate.Rows = new List<RowHeaderTemplate>();
        headerTemplate.StickyCount = 1;
        BodyTemplate bodyTemplate = new BodyTemplate();
        bodyTemplate.Rows = new List<RowBodyTemplate>();
        bodyTemplate.CustomCssDelegate = CustomRowCss;
        for (int i = 0; i < 3; i++)
        {
            headerTemplate.Rows.Add(new RowHeaderTemplate());
            bodyTemplate.Rows.Add(new RowBodyTemplate());
        }
        for (int i = 0; i < 3; i++)
        {
            headerTemplate.Rows[i].Columns = new List<ColumnHeaderTemplate>();
            bodyTemplate.Rows[i].Columns = new List<ColumnBodyTemplate>();
        }
        int CurrentIndex = 0;
        while (true)
        {
            // No
            if ((gridlayouts.Count == 0 && CurrentIndex == 0) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.no_colItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["no_col"], CodeName = BusAllocationGridGridHeaderNameConstants.no_colItemNm, Width = gridlayouts.Count == 0 ? 50 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.RowID), RowSpan = 2, AlignCol = AlignColEnum.Center });
            }
            //受付番号
            if ((gridlayouts.Count == 0 && CurrentIndex == 1) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.receipt_numberItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["receipt_number"], CodeName = BusAllocationGridGridHeaderNameConstants.receipt_numberItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.YYKSHO_UkeCd), CustomTextFormatDelegate = KoboGridHelper.ToFormatString, RowSpan = 2, AlignCol = AlignColEnum.Center });
            }
            //号車
            if ((gridlayouts.Count == 0 && CurrentIndex == 2) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.carItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["car"], CodeName = BusAllocationGridGridHeaderNameConstants.carItemNm, Width = gridlayouts.Count == 0 ? 50 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_GoSya), RowSpan = 2, AlignCol = AlignColEnum.Center });
            }
            //営業所
            if ((gridlayouts.Count == 0 && CurrentIndex == 3) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.sales_officeItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["sales_office"], CodeName = BusAllocationGridGridHeaderNameConstants.sales_officeItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.EIGYOS_RyakuNm), RowSpan = 2 });
            }
            //車号／車種
            if ((gridlayouts.Count == 0 && CurrentIndex == 4) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.car_number_modelItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["car_number_model"], CodeName = BusAllocationGridGridHeaderNameConstants.car_number_modelItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.SYARYO_SyaRyoNm) });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.SYASYU_SyaSyuNm) });
            }
            //団体名／団体名2
            if ((gridlayouts.Count == 0 && CurrentIndex == 5) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.group_name_2ItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["group_name_2"], CodeName = BusAllocationGridGridHeaderNameConstants.group_name_2ItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.UNKOBI_DanTaNm) });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_DanTaNm2) });
            }
            //出庫日時／帰庫日時
            if ((gridlayouts.Count == 0 && CurrentIndex == 6) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.departure_return_datetimeItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["departure_return_datetime"], CodeName = BusAllocationGridGridHeaderNameConstants.departure_return_datetimeItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.CheckSyuKoHaiSha), CustomTextFormatDelegate = KoboGridHelper.ToFormatDateTime });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.CheckKikHaiSha), CustomTextFormatDelegate = KoboGridHelper.ToFormatDateTime });
            }
            //配車日時／到着日時
            if ((gridlayouts.Count == 0 && CurrentIndex == 7) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.delivery_arrival_timeItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["delivery_arrival_time"], CodeName = BusAllocationGridGridHeaderNameConstants.delivery_arrival_timeItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.CheckHaiSHaiSha), CustomTextFormatDelegate = KoboGridHelper.ToFormatDateTime });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.CheckTouHaiSha), CustomTextFormatDelegate = KoboGridHelper.ToFormatDateTime });
            }
            //配車地／到着地
            if ((gridlayouts.Count == 0 && CurrentIndex == 8) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.depot_arrivalItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["depot/arrival"], CodeName = BusAllocationGridGridHeaderNameConstants.depot_arrivalItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_HaiSNm) });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_TouNm) });
            }
            //接続　配車／到着
            if ((gridlayouts.Count == 0 && CurrentIndex == 9) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.connection_dispatch_arrivalItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["connection_dispatch/arrival"], CodeName = BusAllocationGridGridHeaderNameConstants.connection_dispatch_arrivalItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_HaiSBinNm) });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_TouBinNm) });
            }
            //税込運賃
            if ((gridlayouts.Count == 0 && CurrentIndex == 10) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.tax_included_fareItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["tax-included_fare"], CodeName = BusAllocationGridGridHeaderNameConstants.tax_included_fareItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_SyaRyoUncSyo), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right });
            }
            //運賃
            if ((gridlayouts.Count == 0 && CurrentIndex == 11) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.fareItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["fare"], CodeName = BusAllocationGridGridHeaderNameConstants.fareItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_SyaRyoUnc), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right });
            }
            //消費税
            if ((gridlayouts.Count == 0 && CurrentIndex == 12) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.sale_taxItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["sale_tax"], CodeName = BusAllocationGridGridHeaderNameConstants.sale_taxItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_SyaRyoSyo), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right });
            }
            //手数料
            if ((gridlayouts.Count == 0 && CurrentIndex == 13) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.feeItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["fee"], CodeName = BusAllocationGridGridHeaderNameConstants.feeItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_SyaRyoTes), CustomTextFormatDelegate = KoboGridHelper.AddCommasForNumber, RowSpan = 2, AlignCol = AlignColEnum.Right });
            }
            //配車地住所／到着地住所
            if ((gridlayouts.Count == 0 && CurrentIndex == 14) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.depot_arrival_addressItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["depot_arrival_address"], CodeName = BusAllocationGridGridHeaderNameConstants.depot_arrival_addressItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_HaiSJyus1) });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_TouJyusyo1) });
            }
            //乗車人員/プラス人員
            if ((gridlayouts.Count == 0 && CurrentIndex == 15) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.riding_plus_personnelItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["riding_plus_personnel"], CodeName = BusAllocationGridGridHeaderNameConstants.riding_plus_personnelItemNm, Width = gridlayouts.Count == 0 ? 200 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_JyoSyaJin), CustomTextFormatDelegate = KoboGridHelper.ToFormatPerson });
                bodyTemplate.Rows[1].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_PlusJin) , CustomTextFormatDelegate = KoboGridHelper.ToFormatPerson});
            }
            //その他人員
            if ((gridlayouts.Count == 0 && CurrentIndex == 16) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.other_personnelItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["other_personnel"], CodeName = BusAllocationGridGridHeaderNameConstants.other_personnelItemNm, Width = gridlayouts.Count == 0 ? 150 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.StringEmpty), RowSpan = 2 });
            }
            //乗務員1
            if ((gridlayouts.Count == 0 && CurrentIndex == 17) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.crew1ItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["crew1"], CodeName = BusAllocationGridGridHeaderNameConstants.crew1ItemNm, Width = gridlayouts.Count == 0 ? 100 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.StringEmpty), RowSpan = 2 });
            }
            //乗務員2
            if ((gridlayouts.Count == 0 && CurrentIndex == 18) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.crew2ItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["crew2"], CodeName = BusAllocationGridGridHeaderNameConstants.crew2ItemNm, Width = gridlayouts.Count == 0 ? 100 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.StringEmpty), RowSpan = 2 });
            }
            //乗務員2
            if ((gridlayouts.Count == 0 && CurrentIndex == 19) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.crew3ItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["crew3"], CodeName = BusAllocationGridGridHeaderNameConstants.crew3ItemNm, Width = gridlayouts.Count == 0 ? 100 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.StringEmpty), RowSpan = 2 });
            }
            //行き先名
            if ((gridlayouts.Count == 0 && CurrentIndex == 20) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.destination_nameItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["destination_name"], CodeName = BusAllocationGridGridHeaderNameConstants.destination_nameItemNm, Width = gridlayouts.Count == 0 ? 100 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_IkNm), RowSpan = 2 });
            }
            //プレートNo
            if ((gridlayouts.Count == 0 && CurrentIndex == 21) || (gridlayouts.Count != 0 && gridlayouts[CurrentIndex].ItemNm == BusAllocationGridGridHeaderNameConstants.plate_noItemNm))
            {
                headerTemplate.Rows[0].Columns.Add(new ColumnHeaderTemplate() { ColName = Lang["plate_no"], CodeName = BusAllocationGridGridHeaderNameConstants.plate_noItemNm, Width = gridlayouts.Count == 0 ? 100 : gridlayouts[CurrentIndex].Width });

                bodyTemplate.Rows[0].Columns.Add(new ColumnBodyTemplate() { DisplayFieldName = nameof(BusAllocationDataGrid.HAISHA_PlatNo), RowSpan = 2 });
                if (gridlayouts.Count == 0)
                {
                    break;
                }
            }
            CurrentIndex++;
            if (gridlayouts.Count != 0 && CurrentIndex >= gridlayouts.Count)
            {
                break;
            }
        }
        Header = headerTemplate;
        Body = bodyTemplate;
    }
    #endregion

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGridLayoutService GridLayoutService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ILoadingService _loadingService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IStringLocalizer<BusAllocation> Lang { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomNavigation NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IBlazorContextMenuService blazorContextMenuService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
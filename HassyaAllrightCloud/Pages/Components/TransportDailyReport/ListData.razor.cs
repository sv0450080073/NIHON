using DevExpress.Compatibility.System.Web;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SharedLibraries.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.TransportDailyReport
{
    public class ListDataBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<ListData> _lang { get; set; }
        [Inject]
        protected ITransportDailyReportService transportDailyReportService { get; set; }

        [Parameter]
        public TransportDailyReportSearchParams searchParams { get; set; }
        [Parameter]
        public byte fontSize { get; set; }
        [Parameter]
        public EventCallback<bool> DataNotFound { get; set; }

        public List<TransportDailyReportData> listData = new List<TransportDailyReportData>();
        public List<TransportDailyReportData> listDataDisplay = new List<TransportDailyReportData>();
        public List<TotalTransportDailyReportData> listTotalData { get; set; } = new List<TotalTransportDailyReportData>();
        public List<TotalTransportDailyReportData> totalData { get; set; } = new List<TotalTransportDailyReportData>();
        public TotalTransportDailyReportData total { get; set; }
        public List<EigyoSearchData> listEigyo { get; set; } = new List<EigyoSearchData>();
        protected Pagination paging = new Pagination();
        public byte itemPerPage { get; set; } = 25;
        public int currentPage { get; set; } = 0;
        public int totalCount { get; set; } = 0;

        protected HeaderTemplate Header { get; set; }
        protected BodyTemplate Body { get; set; }
        protected HeaderTemplate HeaderTotal1 { get; set; }
        protected BodyTemplate BodyTotal1 { get; set; }
        protected HeaderTemplate HeaderTotal2 { get; set; }
        protected BodyTemplate BodyTotal2 { get; set; }

        protected List<TkdGridLy> gridlayouts { get; set; } = new List<TkdGridLy>();
        protected List<TkdGridLy> gridlayoutstotal { get; set; } = new List<TkdGridLy>();
        protected List<TkdGridLy> gridlayoutstotal1 { get; set; } = new List<TkdGridLy>();

        [Inject] 
        protected IGridLayoutService GridLayoutService { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InitGrid();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            //await OnSearch(false, false);
        }

        public async Task OnSearch(bool isChangePage, bool isChangeEigyo)
        {
            try
            {
                if (!isChangePage)
                {
                    if (!isChangeEigyo)
                    {
                        var taskData = transportDailyReportService.GetListTransportDailyReport(searchParams);
                        var taskTotalData = transportDailyReportService.GetTotalTransportDailyReport(searchParams);
                        await Task.WhenAll(taskData, taskTotalData);
                        listData = taskData.Result;
                        listTotalData = taskTotalData.Result;
                        listEigyo = (from t in listData.Where(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) != 0)
                                     group t by t.EigyoCd into temp
                                     select new EigyoSearchData()
                                     {
                                         EigyoCd = temp.Select(_ => int.Parse(_.EigyoCd)).FirstOrDefault(),
                                         RyakuNm = temp.Select(_ => _.EigyoRyakuNm).FirstOrDefault()
                                     }).ToList();
                        if (listEigyo.Count > 0)
                            searchParams.selectedEigyo = listEigyo[0];

                        paging.currentPage = 0;
                        currentPage = 0;
                    }

                    paging.currentPage = 0;
                    currentPage = 0;
                }

                if (listData.Count > 0)
                {
                    totalCount = listData.Count(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) == searchParams.selectedEigyo.EigyoCd);
                    listDataDisplay = listData.Where(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) == searchParams.selectedEigyo.EigyoCd).Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();

                    int count = 1;
                    foreach (var item in listDataDisplay)
                    {
                        item.No = (count++).ToString();
                    }

                    if (listDataDisplay.Count > 0)
                    {
                        totalData.Clear();
                        var tempTotal = listTotalData.FirstOrDefault(_ => _.EigyoCd == searchParams.selectedEigyo.EigyoCd);
                        tempTotal.Text1 = _lang["total"];
                        tempTotal.Text2 = _lang["currentmonth"];
                        totalData.Add(tempTotal);
                        total = tempTotal;
                        await DataNotFound.InvokeAsync(false);
                    }
                    else
                    {
                        totalData.Clear();
                        total = null;
                        await DataNotFound.InvokeAsync(true);
                    }
                }
                else
                {
                    await ClearData();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }

            StateHasChanged();
        }

        public async Task ClearData()
        {
            totalData.Clear();
            total = null;
            listData = new List<TransportDailyReportData>();
            listDataDisplay = new List<TransportDailyReportData>();
            searchParams.selectedEigyo = null;
            paging.currentPage = 0;
            currentPage = 0;
            totalCount = 0;
            await DataNotFound.InvokeAsync(true);
        }

        protected async Task OnEigyoChanged(EigyoSearchData item)
        {
            try
            {
                searchParams.selectedEigyo = item;
                paging.currentPage = 0;
                currentPage = 0;
                await OnSearch(true, true);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnChangedItem(int type)
        {
            try
            {
                if (listEigyo.Count > 0)
                {
                    var index = listEigyo.IndexOf(searchParams.selectedEigyo);
                    if (type == 0)
                    {
                        if (index - 1 > -1)
                        {
                            await OnEigyoChanged(listEigyo[index - 1]);
                        }
                        else
                        {
                            await OnEigyoChanged(listEigyo[listEigyo.Count - 1]);
                        }
                    }
                    else
                    {
                        if (index + 1 < listEigyo.Count)
                        {
                            await OnEigyoChanged(listEigyo[index + 1]);
                        }
                        else
                        {
                            await OnEigyoChanged(listEigyo[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected bool OnCheckDisable(byte type)
        {
            if(type == 0)
            {
                if (listEigyo.Count < 1 || listEigyo.IndexOf(searchParams.selectedEigyo) == 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (listEigyo.Count < 1 || listEigyo.IndexOf(searchParams.selectedEigyo) == listEigyo.Count - 1)
                {
                    return true;
                }
                return false;
            }
        }

		protected void OnChangePage(int page)
        {
            try
            {
                currentPage = page;
                listDataDisplay = listData.Where(_ => !string.IsNullOrEmpty(_.EigyoCd) && int.Parse(_.EigyoCd) == searchParams.selectedEigyo.EigyoCd).Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            try
            {
                itemPerPage = _itemPerPage;
                OnChangePage(currentPage);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task InitGrid()
        {
            var taskgridlayouts = GridLayoutService.GetGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridName);
            var taskgridlayoutstotal = GridLayoutService.GetGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridNameTotal);
            var taskgridlayoutstotal1 = GridLayoutService.GetGridLayout(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridNameTotal1);
            await Task.WhenAll(taskgridlayouts, taskgridlayoutstotal, taskgridlayoutstotal1);
            gridlayouts = taskgridlayouts.Result;
            gridlayoutstotal = taskgridlayoutstotal.Result;
            gridlayoutstotal1 = taskgridlayoutstotal1.Result;
            InitTable();
            InitTotal1();
            InitTotal2();
        }

        protected void InitTable()
        {
            var DefaultHeader = new HeaderTemplate()
            {
                StickyCount = 1,
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["no_list"], Width = 50, CodeName = TransportDailyGridConstants.No },
                            new ColumnHeaderTemplate() { ColName = _lang["carnumber_list"], Width = 150, CodeName = TransportDailyGridConstants.CarNum },
                            new ColumnHeaderTemplate() { ColName = _lang["capacity_list"], Width = 50, CodeName = TransportDailyGridConstants.Capacity },
                            new ColumnHeaderTemplate() { ColName = _lang["customer_list"], Width = 150, CodeName = TransportDailyGridConstants.Customer },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["organizationname_list"], _lang["destinationname_list"]), Width = 400, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Name },
                            new ColumnHeaderTemplate() { ColName = _lang["dayandnight_list"], Width = 50, CodeName = TransportDailyGridConstants.DayAndNight },
                            new ColumnHeaderTemplate() { ColName = _lang["issue_list"], Width = 120, CodeName = TransportDailyGridConstants.Issue },
                            new ColumnHeaderTemplate() { ColName = _lang["return_list"], Width = 120, CodeName = TransportDailyGridConstants.Return },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["fare_list"], _lang["fee_list"]), Width = 120, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.FareFee },
                            new ColumnHeaderTemplate() { ColName = _lang["netincome_list"], Width = 120, CodeName = TransportDailyGridConstants.NetIncome },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["boardingpersonnel_list"], _lang["pluspersonnel_list"]), Width = 120, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Personel },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["actualkm_list"], _lang["general_highspeed_list"]), Width = 120, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Actual },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["forwardingkm_list"], _lang["general_highspeed_list"]), Width = 120, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Forwarding },
                            new ColumnHeaderTemplate() { ColName = _lang["otherkm_list"], Width = 120, CodeName = TransportDailyGridConstants.Other },
                            new ColumnHeaderTemplate() { ColName = _lang["totalkm_list"], Width = 120, CodeName = TransportDailyGridConstants.Total },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel1_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel1 },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel2_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel2 },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel3_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel3 },
                            new ColumnHeaderTemplate() { ColName = _lang["crewname_list"], Width = 200, CodeName = TransportDailyGridConstants.Crew }
                        }
                    }
                }
            };

            var DefaultBody = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.No), AlignCol = AlignColEnum.Center, RowSpan = 2, CodeName = TransportDailyGridConstants.No },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SyaRyoNm), RowSpan = 2, CodeName = TransportDailyGridConstants.CarNum },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.TeiCnt), AlignCol = AlignColEnum.Right, RowSpan = 2, CodeName = TransportDailyGridConstants.Capacity },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Tokui_RyakuNm), CodeName = TransportDailyGridConstants.Customer },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.DisplayDantaNm), CodeName = TransportDailyGridConstants.Name },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Hihaku), AlignCol = AlignColEnum.Center, RowSpan = 2, CodeName = TransportDailyGridConstants.DayAndNight },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.DisplaySyuko), CodeName = TransportDailyGridConstants.Issue },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.DisplayKik), CodeName = TransportDailyGridConstants.Return },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SyaRyoUnc), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.FareFee },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SumSyaRyo), AlignCol = AlignColEnum.Right, RowSpan = 2, CodeName = TransportDailyGridConstants.NetIncome },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.JyoSyaJin), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Personel },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_JisaIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Actual },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_KisoIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Forwarding },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_OthKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, RowSpan = 2, CodeName = TransportDailyGridConstants.Other },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_TotalKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, RowSpan = 2, CodeName = TransportDailyGridConstants.Total },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo1RyakuNm), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel1 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo2RyakuNm), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo3RyakuNm), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel3 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SyainNm), RowSpan = 2, CodeName = TransportDailyGridConstants.Crew }
                        }
                    },
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Siten_RyakuNm), CodeName = TransportDailyGridConstants.Customer },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.IkNm), CodeName = TransportDailyGridConstants.Name },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SyuKoTime), CustomTextFormatDelegate = e => e.ToString().Insert(2, ":"), CodeName = TransportDailyGridConstants.Issue },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.KikTime), CustomTextFormatDelegate = e => e.ToString().Insert(2, ":"), CodeName = TransportDailyGridConstants.Return },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.SyaRyoTes), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.FareFee },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.PlusJin), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Personel },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_JisaKSKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Actual },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Total_KisoKSKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Forwarding },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo1), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel1 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo2), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel2 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TransportDailyReportData.Nenryo3), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Fuel3 },
                        }
                    }
                }
            };

            var result = LoadGridKobo.Load(DefaultHeader, DefaultBody, gridlayouts, 1);
            
            Header = result.Item1;
            Body = result.Item2;
        }

        protected void InitTotal1()
        {
            var DefaultHeader = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["total_childlist_1"], Width = 70, CodeName = TransportDailyGridConstants.TotalList },
                            new ColumnHeaderTemplate() { ColName = _lang["fare_childlist_1"], Width = 150, CodeName = TransportDailyGridConstants.FareTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["fee_list"], Width = 120, CodeName = TransportDailyGridConstants.FeeTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["netincome_list"], Width = 120, CodeName = TransportDailyGridConstants.NetIncomeTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["boardingpersonnel_list"], Width = 120, CodeName = TransportDailyGridConstants.PersonelTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["actualkm_list"], Width = 120, CodeName = TransportDailyGridConstants.ActualTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["forwardingkm_list"], Width = 120, CodeName = TransportDailyGridConstants.ForwardingTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["otherkm_list"], Width = 120, CodeName = TransportDailyGridConstants.OtherKmTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["totalkm_list"], Width = 120, CodeName = TransportDailyGridConstants.TotalKmTotal },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel1_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel1Total },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel2_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel2Total },
                            new ColumnHeaderTemplate() { ColName = _lang["fuel3_list"], Width = 120, CodeName = TransportDailyGridConstants.Fuel3Total },
                        }
                    }
                }
            };

            var DefaultBody = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.Text1), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.TotalList },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumSyaRyoUnc), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.FareTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumSyaRyoTes), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), RowSpan = 2, AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.FeeTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumSyaRyo), RowSpan = 2, AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.NetIncomeTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumJyoSyaJin), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.PersonelTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumJisaIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ActualTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumKisoIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ForwardingTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumOthKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.OtherKmTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumTotalKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.TotalKmTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumNenryo1), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel1Total },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumNenryo2), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel2Total },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumNenryo3), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel3Total },
                        }
                    },
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumPlusJin), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.PersonelTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumJisaKSKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ActualTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.SumKisoKOKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ForwardingTotal },
                        }
                    },
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.Text2), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.TotalList },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthSyaRyoUnc), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.FareTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthSyaRyoTes), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), RowSpan = 2, AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.FeeTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthSumSyaRyo), RowSpan = 2, AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.NetIncomeTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthJyoSyaJin), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.PersonelTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthJisaIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ActualTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthKisoIPKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ForwardingTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthOthKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.OtherKmTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthTotalKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.TotalKmTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthNenryo1), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel1Total },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthNenryo2), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel2Total },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthNenryo3), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), RowSpan = 2, AlignCol = AlignColEnum.Center, CodeName = TransportDailyGridConstants.Fuel3Total },
                        }
                    },
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthPlusJin), CustomTextFormatDelegate = e => int.Parse(e.ToString()).ToString("N0"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.PersonelTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthJisaKSKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ActualTotal },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.CurMonthKisoKOKm), CustomTextFormatDelegate = e => decimal.Parse(e.ToString()).ToString("N2"), AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.ForwardingTotal },
                        }
                    }
                }
            };

            var result = LoadGridKobo.Load(DefaultHeader, DefaultBody, gridlayoutstotal, 0);

            HeaderTotal1 = result.Item1;
            BodyTotal1 = result.Item2;
        }

        public void InitTotal2()
        {
            var DefaultHeader = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = _lang["numberofvehicle_childlist_2"], ColSpan = 5, Width = 630, CodeName = TransportDailyGridConstants.NumberOfVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["groupbreakdown_childlist_2"], ColSpan = 3, Width = 360, CodeName = TransportDailyGridConstants.GroupBreakDown },
                            new ColumnHeaderTemplate() { ColName = _lang["numberofservice_childlist_2"], ColSpan = 3, Width = 360, CodeName = TransportDailyGridConstants.NumberOfService },
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["reality_childlist_2"], _lang["numberofvehicle_childlist_2"]), Width = 150, RowSpan = 2, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Reality, ParentCodeName = TransportDailyGridConstants.NumberOfVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["numberofactualvehicle_childlist_2"], Width = 120, ColSpan = 2, CodeName = TransportDailyGridConstants.NumberOfActualVehicle, ParentCodeName = TransportDailyGridConstants.NumberOfVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["tempincrease_childlist_2"], Width = 120, RowSpan = 2, CodeName = TransportDailyGridConstants.TempIncrease, ParentCodeName = TransportDailyGridConstants.NumberOfVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["vehicle_childlist_2"], Width = 120, RowSpan = 2, CodeName = TransportDailyGridConstants.Vehicle, ParentCodeName = TransportDailyGridConstants.NumberOfVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["numberofgroup_childlist_2"], Width = 120, RowSpan = 2, CodeName = TransportDailyGridConstants.NumberOfGroup, ParentCodeName = TransportDailyGridConstants.GroupBreakDown },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["home_childlist_2"], _lang["headoffice_childlist_2"]), Width = 120, RowSpan = 2, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.HeadOffice, ParentCodeName = TransportDailyGridConstants.GroupBreakDown },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["home_childlist_2"], _lang["mediator_childlist_2"]), Width = 120, RowSpan = 2, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Mediator, ParentCodeName = TransportDailyGridConstants.GroupBreakDown },
                            new ColumnHeaderTemplate() { ColName = _lang["numberoftrip_childlist_2"], Width = 120, RowSpan = 2, CodeName = TransportDailyGridConstants.NumberOfTrip, ParentCodeName = TransportDailyGridConstants.NumberOfService },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["home_childlist_2"], _lang["headoffice_childlist_2"]), Width = 120, RowSpan = 2, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.HeadOffice1, ParentCodeName = TransportDailyGridConstants.NumberOfService },
                            new ColumnHeaderTemplate() { ColName = string.Format("{0} \n{1}", _lang["home_childlist_2"], _lang["mediator_childlist_2"]), Width = 120, RowSpan = 2, CssClass = "header-multiline", CodeName = TransportDailyGridConstants.Mediator1, ParentCodeName = TransportDailyGridConstants.NumberOfService },
                        }
                    },
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { ColName = searchParams.OutputCategory == 1 ? _lang["outgoing_childlist_2"] : _lang["returnamount_childlist_2"], Width = 120, CodeName = TransportDailyGridConstants.OutGoing, ParentCodeName = TransportDailyGridConstants.NumberOfActualVehicle },
                            new ColumnHeaderTemplate() { ColName = _lang["night_childlist_2"], Width = 120, CodeName = TransportDailyGridConstants.Night, ParentCodeName = TransportDailyGridConstants.NumberOfActualVehicle },
                        }
                    }
                }
            };

            var DefaultBody = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalActualSyaryo), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Reality },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalWorkStock), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.OutGoing },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalWorkNight), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Night },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TempIncrease), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.TempIncrease },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.Total), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Vehicle },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalDantai), CustomTextFormatDelegate = e => e.ToString() + _lang["both_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.NumberOfGroup },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalDantaiHeadOffice), CustomTextFormatDelegate = e => e.ToString() + _lang["case_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.HeadOffice },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalDantaiMediator), CustomTextFormatDelegate = e => e.ToString() + _lang["case_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Mediator },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalUnko), CustomTextFormatDelegate = e => e.ToString() + _lang["time_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.NumberOfTrip },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalUnkoHeadOffice), CustomTextFormatDelegate = e => e.ToString() + _lang["time_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.HeadOffice1 },
                            new ColumnBodyTemplate() { DisplayFieldName = nameof(TotalTransportDailyReportData.TotalUnkoMediator), CustomTextFormatDelegate = e => e.ToString() + _lang["time_label"], AlignCol = AlignColEnum.Right, CodeName = TransportDailyGridConstants.Mediator1 },
                        }
                    }
                }
            };

            var result = LoadGridKobo.Load(DefaultHeader, DefaultBody, gridlayoutstotal1, 0);

            HeaderTotal2 = result.Item1;
            BodyTotal2 = result.Item2;
        }

        public async Task OnSave()
        {
            var task = SaveGridLayout(Header, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridName);
            var taskTotal1 = SaveGridLayout(HeaderTotal1, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridNameTotal);
            var taskTotal2 = SaveGridLayout(HeaderTotal2, FormFilterName.TransportDailyReport, TransportDailyGridConstants.GridNameTotal1);
            await Task.WhenAll(task, taskTotal1, taskTotal2);
        }

        private async Task SaveGridLayout(HeaderTemplate Header, string FormNm, string GridNm)
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
                        FormNm = FormNm,
                        FrozenCol = 0,
                        GridNm = GridNm,
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

            await GridLayoutService.SaveGridLayout(gridLayouts);
        }
    }
}

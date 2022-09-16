using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.Pages.Components.ETC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class ETCBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<ETC> _lang { get; set; }
        [Inject] protected IETCService etcService { get; set; }
        [Inject] private ETCDataTranferService eTCDataTranferService { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private IFilterCondition _filterService { get; set; }
        [Inject] private ILoadingService _loading { get; set; }
        public EditContext searchForm { get; set; }
        public ETCSearchParam searchParam { get; set; } = new ETCSearchParam();
        public List<ETCCompany> listCompany { get; set; } = new List<ETCCompany>();
        public List<ETCEigyo> listEigyo { get; set; } = new List<ETCEigyo>();
        public List<ETCEigyo> listEigyoDisplay { get; set; } = new List<ETCEigyo>();
        public List<ETCSyaRyo> listSyaRyo { get; set; } = new List<ETCSyaRyo>();
        public List<ETCSyaRyo> listSyaRyoDisplay { get; set; } = new List<ETCSyaRyo>();
        public List<ETCFutai> listFutai { get; set; } = new List<ETCFutai>();
        public List<ETCSeisan> listSeisan { get; set; } = new List<ETCSeisan>();
        public List<ETCDropDown> listSortOrder { get; set; } = new List<ETCDropDown>();
        public List<ETCDropDown> listTesuKbn { get; set; } = new List<ETCDropDown>();
        public List<ETCDropDown> listTensoKbn { get; set; } = new List<ETCDropDown>();
        public List<ETCDropDown> listPageSize { get; set; } = new List<ETCDropDown>();
        public bool isDataNotFound { get; set; } = false;
        public byte fontSize { get; set; }
        public ETCList list { get; set; }
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await _loading.ShowAsync();
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                await OnInitData();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            finally
            {
                await _loading.HideAsync();
            }
        }

        private async Task OnInitData()
        {
            var taskCompany = etcService.GetListETCCompany(searchParam.TenantCdSeq);
            var taskFutai = etcService.GetListETCFutai(searchParam.TenantCdSeq);
            var taskSeisan = etcService.GetListETCSeisan();
            var taskEigyo = etcService.GetListETCEigyo(searchParam.TenantCdSeq);
            var taskSyaRyo = etcService.GetListETCSyaRyo(searchParam.TenantCdSeq);
            await Task.WhenAll(taskCompany, taskFutai, taskSeisan, taskEigyo, taskSyaRyo);

            listCompany = taskCompany.Result;
            if (listCompany.Any()) searchParam.SelectedCompany = listCompany.First();
            listFutai = taskFutai.Result;
            listSeisan = taskSeisan.Result;
            listEigyo = taskEigyo.Result;
            listEigyoDisplay = listEigyo;
            listSyaRyo = taskSyaRyo.Result;
            listSyaRyoDisplay = listSyaRyo;

            listSortOrder.AddRange
            (
                new List<ETCDropDown>()
                {
                    new ETCDropDown() { Text = _lang["vehicle_code_sortorder"], Value = 0 },
                    new ETCDropDown() { Text = _lang["receipt_number_sortorder"], Value = 1 }
                }
            );

            listTesuKbn.AddRange
            (
                new List<ETCDropDown>()
                {
                    new ETCDropDown(){ Text = _lang["fee_tesukbn"], Value = 0 },
                    new ETCDropDown(){ Text = _lang["nofee_tesukbn"], Value = 1 }
                }
            );

            listTensoKbn.AddRange
            (
                new List<ETCDropDown>()
                {
                    new ETCDropDown(){ Text = _lang["all_tensokbn"], Value = 2 },
                    new ETCDropDown(){ Text = _lang["not_transferred_tensokbn"], Value = 0 },
                    new ETCDropDown(){ Text = _lang["transferred_tensokbn"], Value = 1 }
                }
            );

            listPageSize.AddRange
            (
                new List<ETCDropDown>()
                {
                    new ETCDropDown(){ Text = "A4", Value = 0 },
                    new ETCDropDown(){ Text = "A3", Value = 1 },
                    new ETCDropDown(){ Text = "B4", Value = 2 }
                }
            );
            await Clear(false);

            fontSize = (byte)ViewMode.Medium;
            await _loading.HideAsync();
        }
        
        protected async Task Clear(bool isReset)
        {
            try
            {
                if (isReset)
                {
                    searchParam.SelectedCompany = listCompany.FirstOrDefault(c => c.CompanyCdSeq == new ClaimModel().CompanyID);
                    searchParam.SelectedEigyo = null;
                    searchParam.SelectedSyaRyoFrom = null;
                    searchParam.SelectedSyaRyoTo = null;
                    searchParam.SelectedFutai = listFutai.FirstOrDefault();
                    searchParam.SelectedSeisan = listSeisan.FirstOrDefault();
                    searchParam.ListFileName = null;
                    searchParam.ETCDateFrom = DateTime.Now;
                    searchParam.ETCDateTo = DateTime.Now;
                    list.listETC = new List<ETCData>();
                    await _filterService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.EtcList);
                }

                if (eTCDataTranferService.Model != null)
                {
                    searchParam.ETCDateFrom = eTCDataTranferService.Model.ETCDateFrom;
                    searchParam.ETCDateTo = eTCDataTranferService.Model.ETCDateTo;
                    searchParam.SelectedTensoKbn = listTensoKbn.FirstOrDefault(e => e.Value == eTCDataTranferService.Model.SelectedTensoKbn.Value);
                    searchParam.TenantCdSeq = eTCDataTranferService.Model.TenantCdSeq;
                    searchParam.ListFileName = eTCDataTranferService.Model.ListFileName;
                    searchParam.ScreenType = 0;
                    searchParam.AcquisitionRange = 30;
                }
                else
                {
                    searchParam.AcquisitionRange = 30;
                    searchParam.ETCDateFrom = DateTime.Now;
                    searchParam.ETCDateTo = DateTime.Now;
                    searchParam.ReturnDateFrom = null;
                    searchParam.ReturnDateTo = null;
                    searchParam.ScreenType = 1;
                    searchParam.SelectedTensoKbn = listTensoKbn[0];
                    searchParam.SelectedFutai = listFutai.FirstOrDefault();
                    searchParam.SelectedSeisan = listSeisan.FirstOrDefault();
                }

                searchParam.SelectedSortOrder = listSortOrder[0];
                searchParam.SelectedTesuKbn = listTesuKbn[0];
                searchForm = new EditContext(searchParam);

                if (eTCDataTranferService.Model == null)
                {
                    searchParam = await BuildSearchModel(searchParam);
                }else
                    eTCDataTranferService.Model = null;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
      
        protected void OnTabChanged()
        {
            try
            {
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnSetFontSize(byte value)
        {
            try
            {
                fontSize = value;
                if(searchForm.Validate())
                    await SaveSearchModel(searchParam);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnSetOutputSetting(byte value)
        {
            try
            {
                searchParam.OutputSetting = value;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void DataNotFound(bool value)
        {
            try
            {
                isDataNotFound = value;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnHandleChanged(dynamic value, string propName)
        {
            try
            {
                var classType = searchParam.GetType();
                var prop = classType.GetProperty(propName);
                
                switch (propName)
                {
                    case "SelectedCompany":
                        prop.SetValue(searchParam, value as ETCCompany, null);
                        if (searchParam.SelectedCompany != null)
                        {
                            listEigyoDisplay = listEigyo.Where(_ => _.CompanyCdSeq == searchParam.SelectedCompany.CompanyCdSeq).ToList();
                        }
                        else
                        {
                            listEigyoDisplay = new List<ETCEigyo>();
                        }
                        break;
                    case "SelectedEigyo":
                        prop.SetValue(searchParam, value as ETCEigyo, null);
                        if (searchParam.SelectedCompany != null)
                        {
                            listSyaRyoDisplay = listSyaRyo.Where(_ => _.CompanyCdSeq == searchParam.SelectedCompany.CompanyCdSeq).ToList();
                            if (searchParam.SelectedEigyo != null)
                            {
                                listSyaRyoDisplay = listSyaRyoDisplay.Where(_ => _.EigyoCdSeq == searchParam.SelectedEigyo.EigyoCdSeq).ToList();
                            }
                        }
                        else
                        {
                            listSyaRyoDisplay = new List<ETCSyaRyo>();
                        }
                        break;
                    case "SelectedSyaRyoFrom":
                    case "SelectedSyaRyoTo":
                        prop.SetValue(searchParam, value as ETCSyaRyo, null);
                        break;
                    case "ETCDateFrom":
                    case "ETCDateTo":
                    case "ReturnDateFrom":
                    case "ReturnDateTo":
                        prop.SetValue(searchParam, value as DateTime?, null);
                        break;
                    case "AcquisitionRange":
                        prop.SetValue(searchParam, value);
                        break;
                    case "SelectedFutai":
                        prop.SetValue(searchParam, value as ETCFutai, null);
                        break;
                    case "SelectedSeisan":
                        prop.SetValue(searchParam, value as ETCSeisan, null);
                        break;
                    case "SelectedTesuKbn":
                    case "SelectedTensoKbn":
                        prop.SetValue(searchParam, value as ETCDropDown, null);
                        break;
                }

                if (searchForm.Validate())
                {
                    await _loading.ShowAsync();
                    await SaveSearchModel(searchParam);
                    await list.OnSearch();
                    await _loading.HideAsync();
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task OnHandleSortOrderChanged(ETCDropDown value)
        {
            try
            {
                searchParam.SelectedSortOrder = value;
                if (searchForm.Validate())
                    await SaveSearchModel(searchParam);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task SaveSearchModel(ETCSearchParam model)
        {
            var filers = GetSearchFormModel(model);
            await _filterService.SaveFilterCondtion(filers, FormFilterName.EtcList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
        }
        
        private Dictionary<string, string> GetSearchFormModel(ETCSearchParam model)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (model == null) return result;

            result.Add(nameof(ETCSearchParam.SelectedCompany), model.SelectedCompany?.CompanyCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedEigyo), model.SelectedEigyo?.EigyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedSyaRyoFrom), model.SelectedSyaRyoFrom?.SyaRyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedSyaRyoTo), model.SelectedSyaRyoTo?.SyaRyoCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.ETCDateFrom), model.ETCDateFrom?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(ETCSearchParam.ETCDateTo), model.ETCDateTo?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(ETCSearchParam.ReturnDateFrom), model.ReturnDateFrom?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(ETCSearchParam.ReturnDateTo), model.ReturnDateTo?.ToString(DateTimeFormat.yyyyMMdd) ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedSortOrder), model.SelectedSortOrder?.Value.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedTesuKbn), model.SelectedTesuKbn?.Value.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.AcquisitionRange), model.AcquisitionRange.ToString());
            result.Add(nameof(ETCSearchParam.SelectedFutai), model.SelectedFutai?.FutaiCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedSeisan), model.SelectedSeisan?.SeisanCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(ETCSearchParam.SelectedTensoKbn), model.SelectedTensoKbn?.Value.ToString() ?? string.Empty);
            result.Add(nameof(fontSize), fontSize.ToString());

            return result;
        }

        private async Task<ETCSearchParam> BuildSearchModel(ETCSearchParam model)
        {
            var filters = await _filterService.GetFilterCondition(FormFilterName.EtcList, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);

            foreach (var item in filters)
            {
                if (item.ItemNm == nameof(fontSize))
                {
                    fontSize = byte.Parse(item.JoInput);
                    continue;
                }
                var propertyInfo = model.GetType().GetProperty(item.ItemNm);
                if (propertyInfo != null && !string.IsNullOrEmpty(item.JoInput))
                {
                    switch (item.ItemNm)
                    {
                        case nameof(ETCSearchParam.SelectedCompany):
                            var selectedCdSeq = int.Parse(item.JoInput);
                            var selected = listCompany.FirstOrDefault(e => e.CompanyCdSeq == selectedCdSeq);
                            propertyInfo.SetValue(model, selected);
                            break;
                        case nameof(ETCSearchParam.SelectedEigyo):
                            var selectedEigyoCdSeq = int.Parse(item.JoInput);
                            var selectedEigyo = listEigyoDisplay.FirstOrDefault(e => e.CompanyCdSeq == selectedEigyoCdSeq);
                            propertyInfo.SetValue(model, selectedEigyo);
                            break;
                        case nameof(ETCSearchParam.SelectedSyaRyoFrom):
                        case nameof(ETCSearchParam.SelectedSyaRyoTo):
                            var selectedSyaRyoCdSeq = int.Parse(item.JoInput);
                            var selectedSyaRyo = listSyaRyoDisplay.FirstOrDefault(e => e.SyaRyoCdSeq == selectedSyaRyoCdSeq);
                            propertyInfo.SetValue(model, selectedSyaRyo);
                            break;
                        case nameof(ETCSearchParam.ETCDateFrom):
                        case nameof(ETCSearchParam.ETCDateTo):
                        case nameof(ETCSearchParam.ReturnDateFrom):
                        case nameof(ETCSearchParam.ReturnDateTo):
                            var selectedDate = DateTime.ParseExact(item.JoInput, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                            propertyInfo.SetValue(model, selectedDate);
                            break;
                        case nameof(ETCSearchParam.SelectedSortOrder):
                            var selectedSortOrderVal = byte.Parse(item.JoInput);
                            var selectedSortOrder = listSortOrder.FirstOrDefault(e => e.Value == selectedSortOrderVal);
                            propertyInfo.SetValue(model, selectedSortOrder);
                            break;
                        case nameof(ETCSearchParam.SelectedTesuKbn):
                            var selectedTesuKbnVal = byte.Parse(item.JoInput);
                            var selectedTesuKbn = listTesuKbn.FirstOrDefault(e => e.Value == selectedTesuKbnVal);
                            propertyInfo.SetValue(model, selectedTesuKbn);
                            break;
                        case nameof(ETCSearchParam.AcquisitionRange):
                            propertyInfo.SetValue(model, int.Parse(item.JoInput));
                            break;
                        case nameof(ETCSearchParam.SelectedFutai):
                            var selectedFutaiCdSeq = int.Parse(item.JoInput);
                            var selectedFutai = listFutai.FirstOrDefault(e => e.FutaiCdSeq == selectedFutaiCdSeq);
                            propertyInfo.SetValue(model, selectedFutai);
                            break;
                        case nameof(ETCSearchParam.SelectedSeisan):
                            var selectedSeisanCdSeq = int.Parse(item.JoInput);
                            var selectedSeisan = listSeisan.FirstOrDefault(e => e.SeisanCdSeq == selectedSeisanCdSeq);
                            propertyInfo.SetValue(model, selectedSeisan);
                            break;
                        case nameof(ETCSearchParam.SelectedTensoKbn):
                            var selectedTensoKbnVal = byte.Parse(item.JoInput);
                            var selectedTensoKbn = listTensoKbn.FirstOrDefault(e => e.Value == selectedTensoKbnVal);
                            propertyInfo.SetValue(model, selectedTensoKbn);
                            break;
                    }
                }
            }
            return model;
        }
    }
}

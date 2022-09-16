using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.Home
{
    public class ShowAlertSettingPopUpBase : ComponentBase
    {
        [CascadingParameter(Name = "ClaimModel")]
        protected ClaimModel ClaimModel { get; set; }
        [Inject]
        public IStringLocalizer<HassyaAllrightCloud.Pages.Home> Lang { get; set; }

        [Inject]
        public IAlertSettingService alertSettingService { get; set; }

        [Inject]
        public ITPM_CodeKbListService codeKbService { get; set; }

        [Inject]
        public ILoadingService loadingService { get; set; }

        [Inject]
        public IJSRuntime jSRuntime { get; set; }
        [Parameter]
        public bool isOpeningShowAlertSettingPopUp { get; set; }
        [Parameter]
        public EventCallback<bool> isOpeningShowAlertSettingPopUpChanged { get; set; }
        [Parameter]
        public List<AlertSetting> alertSettings { get; set; } = new List<AlertSetting>();
        [Parameter]
        public EventCallback<List<AlertSetting>> alertSettingsChanged { get; set; }

        public List<CodeDataModel> alertTypes { get; set; }
        public List<ShowAlertSettingGrid> showAlertSettingGrids { get; set; } = new List<ShowAlertSettingGrid>();
        public List<ShowAlertSettingGridDisplay> showAlertSettingGridDisplays { get; set; } = new List<ShowAlertSettingGridDisplay>();
        public int syainCdSeq;
        public int tenantCdSeq;
        public int companyCdSeq;
        public string errorMessage { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await loadingService.HideAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            if (ClaimModel != null)
            {
                tenantCdSeq = ClaimModel.TenantID;
                companyCdSeq = ClaimModel.CompanyID;
                syainCdSeq = ClaimModel.SyainCdSeq;
            }
            await GetData();
        }

        public async Task GetData()
        {
            alertTypes = (await codeKbService.GetDataByNameAsync("ALERTKBN")).ToList();
            var result = await alertSettingService.GetShowAlertSettingAsync(tenantCdSeq, syainCdSeq);
            foreach (var item in result)
            {
                var alertType = alertTypes.Where(x => short.Parse(x.CodeKbn) == item.AlertKbn).FirstOrDefault();
                var data = showAlertSettingGridDisplays.Where(x => x.AlertTypeName == alertType.CodeKbnNm).FirstOrDefault();
                if (data == null)
                {
                    var showAlertSettingGridDisplay = new ShowAlertSettingGridDisplay()
                    {
                        AlertTypeName = alertType.CodeKbnNm,
                        AlertTypeColor = int.Parse(alertType.CodeKbn) == 1 ? "#fff176" : int.Parse(alertType.CodeKbn) == 2 ? "#ffb74e"
                        : int.Parse(alertType.CodeKbn) == 3 ? "#aed581" : int.Parse(alertType.CodeKbn) == 4 ? "#64b5f7" : string.Empty,
                        ShowAlertSettingGrids = new List<ShowAlertSettingGrid>() { item }
                    };
                    showAlertSettingGridDisplays.Add(showAlertSettingGridDisplay);
                }
                else
                {
                    data.ShowAlertSettingGrids.Add(item);
                }
            }
        }

        public async void UpdateValue(int i, int j)
        {
            errorMessage = string.Empty;
            var item = showAlertSettingGridDisplays[i].ShowAlertSettingGrids[j];
            var data = showAlertSettingGrids.Where(x => x.AlertCd == item.AlertCd && x.TenantCdSeq == item.TenantCdSeq).FirstOrDefault();
            if (data == null)
            {
                item.SyainCdSeq = syainCdSeq;
                showAlertSettingGrids.Add(item);
            }
            else
            {
                data.Checked = item.Checked;
            }
            StateHasChanged();
        }

        public async Task Save()
        {
            await loadingService.ShowAsync();
            errorMessage = await alertSettingService.SaveShowAlertSettingAsync(showAlertSettingGrids);
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                List<int> alertCds = (await alertSettingService.GetShowAlertSettingAsync(tenantCdSeq, syainCdSeq)).Where(x => x.Checked).Select(x => x.AlertCd).ToList();
                alertSettings = await alertSettingService.GetAlertSettingAsync(alertCds, tenantCdSeq, syainCdSeq, companyCdSeq);
                await alertSettingsChanged.InvokeAsync(alertSettings);
                CloseModal();
            }
            await loadingService.HideAsync();
            StateHasChanged();
        }

        public void CloseModal()
        {
            isOpeningShowAlertSettingPopUp = false;
            isOpeningShowAlertSettingPopUpChanged.InvokeAsync(isOpeningShowAlertSettingPopUp);
            StateHasChanged();
        }
    }
}

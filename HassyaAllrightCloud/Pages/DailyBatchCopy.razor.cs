using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class DailyBatchCopyBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<DailyBatchCopy> _lang { get; set; }
        [Inject]
        protected IDailyBatchCopyService _dailyBatchCopyService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Parameter]
        public string searchString { get; set; }

        public DailyBatchCopySearchModel searchModel { get; set; }
        public EditContext searchForm { get; set; }
        public List<DailyBatchCopyData> listData { get; set; } = new List<DailyBatchCopyData>();
        public List<string> listDate { get; set; } = new List<string>();
        public Dictionary<string, string> dict { get; set; } = new Dictionary<string, string>();
        public bool isShow { get; set; } = false;
        public MessageBoxType type { get; set; }
        public string message { get; set; } = string.Empty;
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();
        [Inject] protected IFilterCondition FilterConditionService { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await OnInitData(0);
        }

        protected async Task SaveConditions()
        {
            if (searchForm.Validate())
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(nameof(DailyBatchCopySearchModel.Vehicle), searchModel.Vehicle.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.StartDate), searchModel.StartDate.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsProcess), searchModel.IsProcess.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsArrangement), searchModel.IsArrangement.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsLoadedGoods), searchModel.IsLoadedGoods.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsIncidental), searchModel.IsIncidental.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsMonday), searchModel.IsMonday.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsWebnesday), searchModel.IsWebnesday.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsThursday), searchModel.IsThursday.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsFriday), searchModel.IsFriday.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsSaturday), searchModel.IsSaturday.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.RepeatEnd), searchModel.RepeatEnd.ToString());
                dict.Add(nameof(DailyBatchCopySearchModel.IsSunday), searchModel.IsSunday.ToString());

                await FilterConditionService.SaveFilterCondtion(dict, FormFilterName.DailyBatchCopy, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
            }
        }

        protected async Task OnInitData(byte type)
        {
            try
            {
                dict = EncryptHelper.DecryptFromUrl<Dictionary<string, string>>(searchString);
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                searchModel = new DailyBatchCopySearchModel();
                if (type == 0)
                {
                    var conditions = await FilterConditionService.GetFilterCondition(FormFilterName.DailyBatchCopy, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    if (conditions.Any())
                    {
                        searchModel.Vehicle = byte.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.Vehicle))?.JoInput ?? "1");
                        var startDate = conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.StartDate));
                        searchModel.StartDate = startDate == null ? DateTime.Now : DateTime.Parse(startDate.JoInput);
                        searchModel.IsProcess = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsProcess))?.JoInput ?? "true");
                        searchModel.IsArrangement = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsArrangement))?.JoInput ?? "true");
                        searchModel.IsLoadedGoods = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsLoadedGoods))?.JoInput ?? "false");
                        searchModel.IsIncidental = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsIncidental))?.JoInput ?? "false");
                        searchModel.IsMonday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsMonday))?.JoInput ?? "false");
                        searchModel.IsTuesday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsTuesday))?.JoInput ?? "false");
                        searchModel.IsWebnesday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsWebnesday))?.JoInput ?? "false");
                        searchModel.IsThursday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsThursday))?.JoInput ?? "false");
                        searchModel.IsFriday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsFriday))?.JoInput ?? "false");
                        searchModel.IsSaturday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsSaturday))?.JoInput ?? "false");
                        searchModel.IsSunday = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsSunday))?.JoInput ?? "false");
                        var repeatEnd = conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.RepeatEnd));
                        searchModel.RepeatEnd = repeatEnd == null ? DateTime.Now : DateTime.Parse(repeatEnd.JoInput);
                        searchModel.IsDayOfWeek = bool.Parse(conditions.FirstOrDefault(_ => _.ItemNm == nameof(DailyBatchCopySearchModel.IsDayOfWeek))?.JoInput ?? "false");
                    }
                }
                else
                {
                    await FilterConditionService.DeleteCustomFilerCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.DailyBatchCopy);
                }
                searchForm = new EditContext(searchModel);
                listDate = new List<string>();
                if (dict != null)
                {
                    listData = await _dailyBatchCopyService.GetListDailyBatchCopy(new ClaimModel().TenantID, dict["listUkeNo"]);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnRaidoButtonChanged(ChangeEventArgs args, string propName)
        {
            try
            {
                var classType = searchModel.GetType();
                var prop = classType.GetProperty(propName);

                prop.SetValue(searchModel, byte.Parse(args.Value.ToString()), null);
                await SaveConditions();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnHandleChanged(dynamic value, string propName)
        {
            try
            {
                var classType = searchModel.GetType();
                var prop = classType.GetProperty(propName);

                switch (propName)
                {
                    case "StartDate":
                        prop.SetValue(searchModel, value, null);
                        var date = searchModel.StartDate.ToString(DateTimeFormat.yyyyMMddSlash);
                        if (!searchModel.IsDayOfWeek && !listDate.Contains(date))
                        {
                            listDate.Add(date);
                        }
                        break;
                    case "IsProcess":
                    case "IsArrangement":
                    case "IsLoadedGoods":
                    case "IsIncidental":
                    case "IsMonday":
                    case "IsTuesday":
                    case "IsWebnesday":
                    case "IsThursday":
                    case "IsFriday":
                    case "IsSaturday":
                    case "IsSunday":
                        prop.SetValue(searchModel, value, null);
                        break;
                    case "RepeatEnd":
                        prop.SetValue(searchModel, value, null);
                        break;
                }
                await SaveConditions();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnRemoveDate(string date)
        {
            try
            {
                listDate.Remove(date);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnRemoveItem(DailyBatchCopyData item)
        {
            try
            {
                listData.Remove(item);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnCopy()
        {
            try
            {
                if (searchForm.Validate() && listData.Count > 0)
                {
                    var listDateExecute = new List<string>();
                    if (searchModel.IsDayOfWeek)
                    {
                        GetDaysInRange(listDateExecute);
                    }
                    else
                    {
                        listDateExecute = listDate.ToList();
                    }

                    var result = await _dailyBatchCopyService.ExecuteCopy(listData, listDateExecute, searchModel);
                    if (result)
                    {
                        type = MessageBoxType.Info;
                        message = _lang["SuccessMessage"];
                        isShow = true;
                    }
                    else
                    {
                        type = MessageBoxType.Error;
                        message = _lang["ErrorMessage"];
                        isShow = true;
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private void GetDaysInRange(List<string> listDate)
        {
            if (searchModel.IsMonday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Monday);
            }
            if (searchModel.IsTuesday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Tuesday);
            }
            if (searchModel.IsWebnesday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Wednesday);
            }
            if (searchModel.IsThursday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Thursday);
            }
            if (searchModel.IsFriday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Friday);
            }
            if (searchModel.IsSaturday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Saturday);
            }
            if (searchModel.IsSunday)
            {
                GetDaysInRangeWithDOW(listDate, DayOfWeek.Sunday);
            }
        }

        private void GetDaysInRangeWithDOW(List<string> listDate, DayOfWeek dayOfWeek)
        {
            var nextDate = searchModel.StartDate;
            if (nextDate.DayOfWeek == dayOfWeek)
            {
                listDate.Add(nextDate.ToString(CommonConstants.FormatYMD));
            }
            else
            {
                nextDate = nextDate.AddDays(-(int)nextDate.DayOfWeek).AddDays((int)dayOfWeek);
                if(nextDate <= searchModel.RepeatEnd && nextDate >= searchModel.StartDate)
                {
                    listDate.Add(nextDate.ToString(CommonConstants.FormatYMD));
                }
            }
            nextDate = nextDate.AddDays(7);

            while (nextDate <= searchModel.RepeatEnd)
            {
                listDate.Add(nextDate.ToString(CommonConstants.FormatYMD));
                nextDate = nextDate.AddDays(7);
            }
        }

        protected void OnCloseMessageBox(bool value)
        {
            try
            {
                isShow = value;
                message = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            //if(type == MessageBoxType.Info)
            //{
            //    navigationManager.NavigateTo("/supermenu?&type=1");
            //}
        }
    }
}

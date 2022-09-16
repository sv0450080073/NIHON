using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace HassyaAllrightCloud.Pages
{
    public class ReservationMobileBase : ComponentBase
    {
        [Parameter] public string Date { get; set; }
        [Parameter] public string SyaRyoCdSeq { get; set; }
        [Parameter] public string UkeCd { get; set; }

        public List<ReservationTokiskData> listTokisk { get; set; } = new List<ReservationTokiskData>();
        public List<ReservationCodeKbnData> listCodeKb { get; set; } = new List<ReservationCodeKbnData>();
        public ReservationMobileData Data { get; set; } = new ReservationMobileData();
        public ReservationMobileData DataClone { get; set; } = new ReservationMobileData();
        public List<ReservationSyaSyuData> listSyaSyu { get; set; } = new List<ReservationSyaSyuData>();
        public List<ReservationMobileChildItemData> listDelete { get; set; } = new List<ReservationMobileChildItemData>();
        public List<EditContext> listContext = new List<EditContext>();
        public EditContext EditForm { get; set; }

        [Inject] public IStringLocalizer<ReservationMobile> Lang { get; set; }
        [Inject] public IEditReservationMobileService _service { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }
        public Dictionary<string, string> LangDic = new Dictionary<string, string>();

        public bool IsShow { get; set; }
        public MessageBoxType Type { get; set; } = MessageBoxType.Info;
        public string Message { get; set; }

        public bool IsLoading { get; set; }
        public bool IsInsert { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                IsShow = false;
                IsLoading = true;
                await Task.Delay(100);
                await InvokeAsync(StateHasChanged);
                await OnLoadData();
                IsLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected override Task OnInitializedAsync()
        {
            try
            {
                var dataLang = Lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            return base.OnInitializedAsync();
        }

        private async Task OnLoadData()
        {
            bool isAddNew = false;
            var taskTokisk = _service.GetListTokisk(new ClaimModel().TenantID);
            var taskCodeKb = _service.GetListCodeKb();
            var taskSyaSyu = _service.GetListSyaSyu(new ClaimModel().TenantID);
            if (!string.IsNullOrEmpty(UkeCd))
            {
                var taskReservation = _service.GetReservationData(int.Parse(UkeCd), new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID);
                var taskListChildItem = _service.GetListChildItem(int.Parse(UkeCd), new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID);
                await Task.WhenAll(taskTokisk, taskCodeKb, taskSyaSyu, taskReservation, taskListChildItem);
                Data = taskReservation.Result;
                Data.ListItems = taskListChildItem.Result;
                if (Data == null) Data = new ReservationMobileData();
            }
            else
            {
                isAddNew = true;
                var childItem = new ReservationMobileChildItemData();
                childItem.IsCanDelete = false;

                if (int.TryParse(SyaRyoCdSeq, out int v))
                {
                    var taskSyaSyuCdSeq = _service.GetSyaSyuCdSeq(v);
                    await Task.WhenAll(taskTokisk, taskCodeKb, taskSyaSyu, taskSyaSyuCdSeq);
                    int syaSyuCdSeq = taskSyaSyuCdSeq.Result;
                    listSyaSyu = taskSyaSyu.Result;
                    InsertSyaSyu();
                    var syasyu = listSyaSyu.FirstOrDefault(_ => _.SyaSyuCdSeq == syaSyuCdSeq);
                    if (syasyu != null)
                    {
                        childItem.SyaSyu = syasyu;
                        childItem.IsDisable = true;
                    }
                }
                else
                {
                    await Task.WhenAll(taskTokisk, taskCodeKb, taskSyaSyu);
                }

                if (!string.IsNullOrEmpty(Date))
                {
                    if (DateTime.TryParseExact(Date, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                    {
                        Data.DispatchDate = result;
                        Data.ArrivalDate = result;
                    }
                }
                Data.ListItems.Add(childItem);
                listContext.Add(new EditContext(childItem));
            }

            listTokisk = taskTokisk.Result;
            listCodeKb = taskCodeKb.Result;
            if (listSyaSyu.Count == 0)
            {
                listSyaSyu = taskSyaSyu.Result;
                InsertSyaSyu();
            }

            Data.Tokisk = listTokisk.FirstOrDefault(_ => _.TokuiSeq == Data.TokuiSeq && _.SitenCdSeq == Data.SitenCdSeq);
            Data.CodeKb = listCodeKb.FirstOrDefault(_ => _.CodeKbnSeq == Data.DantaiCdSeq && _.JyoKyakuCdSeq == Data.JyoKyakuCdSeq);

            if (!isAddNew)
            {
                foreach (var item in Data.ListItems)
                {
                    listContext.Add(new EditContext(item));
                    item.SyaSyu = listSyaSyu.FirstOrDefault(_ => _.SyaSyuCdSeq == item.SyaSyuCdSeq && _.KataKbn == item.KataKbn);
                }

                if (Data.ListItems.Count > 0)
                {
                    Data.ListItems[0].IsCanDelete = false;
                }
            }

            DataClone = (ReservationMobileData)(Data.Clone());

            EditForm = new EditContext(Data);
        }

        private void InsertSyaSyu()
        {
            var listKataKbn = listSyaSyu.Select(_ => _.KataKbn).Distinct().ToList();
            foreach (var kataKbn in listKataKbn)
            {
                var syasyu = listSyaSyu.FirstOrDefault(_ => _.KataKbn == kataKbn);
                listSyaSyu.Insert(listSyaSyu.IndexOf(syasyu), new ReservationSyaSyuData()
                {
                    SyaSyuCdSeq = 0,
                    KataKbn = kataKbn,
                    KataNm = syasyu.KataNm,
                    SyaSyuCd = 0,
                    SyaSyuNm = Lang["SyaSyuNotSpecify"]
                });
            }
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                JSRuntime.InvokeVoidAsync("formatTime");
                JSRuntime.InvokeVoidAsync("inputNumber");
            }
            catch(Exception ex)
            {
                errorModalService.HandleError(ex);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected void OnAddRow()
        {
            try
            {
                if (Data.ListItems.Count < 200)
                {
                    var model = new ReservationMobileChildItemData();
                    Data.ListItems.Add(model);
                    listContext.Add(new EditContext(model));
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnRemoveRow(ReservationMobileChildItemData item)
        {
            try
            {
                if (!item.IsAddNew) listDelete.Add(item);
                var index = Data.ListItems.IndexOf(item);
                listContext.RemoveAt(index);
                Data.ListItems.Remove(item);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnClear()
        {
            //if (!string.IsNullOrEmpty(UkeNo))
            //{
            //    Data = (ReservationMobileData)(DataClone.Clone());
            //    StateHasChanged();
            //}
            try
            {
                Data = (ReservationMobileData)(DataClone.Clone());
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task OnSave()
        {
            try
            {
                bool isValid = true;
                foreach (var item in listContext)
                {
                    if (!item.Validate())
                    {
                        isValid = false;
                    }
                }

                if (EditForm.Validate() && isValid)
                {
                    IsLoading = true;
                    await Task.Delay(100);
                    await InvokeAsync(StateHasChanged);

                    if (string.IsNullOrEmpty(UkeCd))
                    {
                        IsInsert = true;
                        var result = await _service.InsertReservation(Data, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, int.Parse(SyaRyoCdSeq), new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, new HassyaAllrightCloud.Domain.Dto.ClaimModel().EigyoCdSeq);
                        Type = MessageBoxType.Info;
                        Message = string.Format(Lang["BI_T005"], result.Substring(5), "reservationmobile?UkeCd=" + result.Substring(5));
                        IsShow = true;
                        StateHasChanged();
                    }
                    else if (Data != null)
                    {
                        if (string.IsNullOrEmpty(Data.KaktYmd))
                        {
                            await _service.UpdateReservation(Data, listDelete, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                            Type = MessageBoxType.Info;
                            Message = Lang["BI_T006"];
                            IsShow = true;
                            StateHasChanged();
                        }
                        else
                        {
                            Type = MessageBoxType.Confirm;
                            Message = Lang["ConfirmMessage"];
                            IsShow = true;
                            StateHasChanged();
                        }
                    }

                    IsLoading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task OnClosePopup(bool value)
        {
            try
            {
                IsShow = false;
                if (value)
                {
                    await _service.UpdateReservation(Data, listDelete, new HassyaAllrightCloud.Domain.Dto.ClaimModel().CompanyID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().TenantID, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq);
                    Type = MessageBoxType.Info;
                    Message = Lang["BI_T006"];
                    IsShow = true;
                }
                if (IsInsert)
                {
                    IsInsert = false;
                    if (!string.IsNullOrEmpty(Date))
                    {
                        NavigationManager.NavigateTo(string.Format("vehicleavailabilityconfirmationmobile?startDateParam={0}", Date));
                    }
                    else if (!string.IsNullOrEmpty(SyaRyoCdSeq))
                    {
                        NavigationManager.NavigateTo(string.Format("vehicleschedulermobile?SyaRyoCdSeq={0}", SyaRyoCdSeq));
                    }
                }
                DataClone = (ReservationMobileData)(Data.Clone());
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnHandleChangeFormValue(string propName, dynamic value)
        {
            try
            {
                switch (propName)
                {
                    case nameof(Data.Organization):
                        string v = Convert.ToString(value);
                        if (v.Length > 100)
                            Data.Organization = v.Substring(0, 100);
                        else
                            Data.Organization = v;
                        break;
                    case nameof(Data.Tokisk):
                        Data.Tokisk = value as ReservationTokiskData;
                        break;
                    case nameof(Data.CodeKb):
                        Data.CodeKb = value as ReservationCodeKbnData;
                        break;
                    case nameof(Data.DispatchDate):
                        Data.DispatchDate = value;
                        break;
                    case nameof(Data.DispatchTime):
                        var dTime = Convert.ToString(value);
                        if (CheckValidTime(dTime)) Data.DispatchTime = dTime.Insert(2, ":");
                        break;
                    case nameof(Data.ArrivalDate):
                        Data.ArrivalDate = value;
                        break;
                    case nameof(Data.ArrivalTime):
                        var aTime = Convert.ToString(value);
                        if (CheckValidTime(aTime)) Data.ArrivalTime = aTime.Insert(2, ":");
                        break;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private bool CheckValidTime(string time)
        {
            if(time.Length == 4)
            {
                var hour = int.Parse(time.Substring(0, 2));
                var minute = int.Parse(time.Substring(2));
                return hour >= 0 && hour < 24 && minute < 60 && minute >= 0;
            }
            return false;
        }

        protected void OnHandleChangeChildFormValue(string propName, dynamic value, ReservationMobileChildItemData item)
        {
            try
            {
                switch (propName)
                {
                    case nameof(ReservationMobileChildItemData.SyaSyu):
                        item.SyaSyu = value as ReservationSyaSyuData;
                        item.SyaSyuCd = item.SyaSyu.SyaSyuCd;
                        break;
                    case nameof(ReservationMobileChildItemData.BusCount):
                        var bcount = Convert.ToString(value);
                        if (int.TryParse(bcount, out int b) && b > 0 && b <= 99)
                        {
                            item.BusCount = b.ToString();
                        }
                        break;
                    case nameof(ReservationMobileChildItemData.DriverCount):
                        var dcount = Convert.ToString(value);
                        if (int.TryParse(dcount, out int d) && d > 0 && d <= 99)
                        {
                            item.DriverCount = d.ToString();
                        }
                        break;
                    case nameof(ReservationMobileChildItemData.GuiderCount):
                        var gcount = Convert.ToString(value);
                        if (int.TryParse(gcount, out int g) && g >= 0 && g <= 99)
                        {
                            item.GuiderCount = g.ToString();
                        }
                        break;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}

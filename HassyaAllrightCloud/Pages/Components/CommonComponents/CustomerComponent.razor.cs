using DevExpress.Blazor;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.CommonComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.CommonComponents
{
    public class CustomerComponentBase : ComponentBase
    {
        [Parameter] public int VehicleClassification { get; set; }
        [Parameter] public string SiyoDateStr { get; set; } = DateTime.Now.ToString(CommonConstants.FormatYMD);
        [Parameter] public string SiyoDateEnd { get; set; } = DateTime.Now.ToString(CommonConstants.FormatYMD);
        [Parameter] public bool IsGyoSyaNotNull { get; set; }
        [Parameter] public string GyosyaTextError { get; set; }
        [Parameter] public bool ReadOnlyGyosya { get; set; } = false;
        [Parameter] public int? DefaultGyosya { get; set; }
        [Parameter] public CustomerComponentGyosyaData SelectedGyosya { get; set; }
        [Parameter] public EventCallback<CustomerComponentGyosyaData> SelectedGyosyaChanged { get; set; } = new EventCallback<CustomerComponentGyosyaData>();
        [Parameter] public Expression<Func<Object>> GyosyaExpression { get; set; }
        [Parameter] public string GyosyaCssClass { get; set; } = "w-100";
        [Parameter] public bool isGyosyaAddNull { get; set; }
        [Parameter] public string DefaultTokiskNullText { get; set; }


        [Parameter] public string TokiskTextError { get; set; }
        [Parameter] public bool ReadOnlyTokisk { get; set; } = false;
        [Parameter] public int DefaultTokisk { get; set; }
        [Parameter] public CustomerComponentTokiskData SelectedTokisk { get; set; }
        [Parameter] public EventCallback<CustomerComponentTokiskData> SelectedTokiskChanged { get; set; } = new EventCallback<CustomerComponentTokiskData>();
        [Parameter] public Expression<Func<Object>> TokiskExpression { get; set; }
        [Parameter] public string DefaultGyosyaNullText { get; set; }
        [Parameter] public string TokiskCssClass { get; set; } = string.Empty;
        [Parameter] public bool isTokiskAddNull { get; set; }


        [Parameter] public string TokiStTextError { get; set; }
        [Parameter] public bool ReadOnlyTokiSt { get; set; } = false;
        [Parameter] public int DefaultTokiSt { get; set; }
        [Parameter] public CustomerComponentTokiStData SelectedTokiSt { get; set; }
        [Parameter] public EventCallback<CustomerComponentTokiStData> SelectedTokiStChanged { get; set; } = new EventCallback<CustomerComponentTokiStData>();
        [Required] [Parameter] public Expression<Func<Object>> TokiStExpression { get; set; }
        [Parameter] public string DefaultTokiStNullText { get; set; }
        [Parameter] public string TokiStCssClass { get; set; } = string.Empty;
        [Parameter] public bool isTokiStAddNull { get; set; }

        [Parameter] public Action FirstLoaded { get; set; }


        [Parameter] public Dictionary<string, string> LangDic { get; set; }
        [Parameter] public DropDownDirection DropDownDirection { get; set; } = DropDownDirection.Down;
        [Parameter] public DataEditorClearButtonDisplayMode ClearButtonDisplayMode { get; set; } = DataEditorClearButtonDisplayMode.Never;
        [Parameter] public ListRenderMode RenderMode { get; set; } = ListRenderMode.Entire;

        [Inject] protected IStringLocalizer<Commons.CommonComponents> Lang { get; set; }
        [Inject] protected IErrorHandlerService errorModalService { get; set; }
        [Inject] protected ICustomerComponentService _service { get; set; }

        public const string Gyosya = "Gyosya";
        public const string Tokisk = "Tokisk";
        public const string Tokist = "Tokist";

        public List<CustomerComponentGyosyaData> ListGyosya { get; set; } = new List<CustomerComponentGyosyaData>();
        public List<CustomerComponentTokiskData> ListTokisk { get; set; } = new List<CustomerComponentTokiskData>();
        public List<CustomerComponentTokiStData> ListTokiSt { get; set; } = new List<CustomerComponentTokiStData>();

        public List<CustomerComponentTokiskData> TokiskData { get; set; } = new List<CustomerComponentTokiskData>();
        public List<CustomerComponentTokiStData> TokiStData { get; set; } = new List<CustomerComponentTokiStData>();

        public string gyosyaSearchString { get; set; }
        public string tokiskSearchString { get; set; }
        public string tokistSearchString { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (DefaultGyosyaNullText == null)
                    DefaultGyosyaNullText = Lang["gyosya_nulltext"];
                if (DefaultTokiskNullText == null)
                    DefaultTokiskNullText = Lang["tokisk_nulltext"];
                if (DefaultTokiStNullText == null)
                    DefaultTokiStNullText = Lang["tokist_nulltext"];

                var taskGyosya = _service.GetListGyosya();
                // N.T.L.Anh Change str 2021/06/09
                var taskTokisk = _service.GetListTokisk(SiyoDateStr, SiyoDateEnd);
                var taskTokiSt = _service.GetListTokiSt(SiyoDateStr, SiyoDateEnd);
                // N.T.L.Anh Change end 2021/06/09


                await Task.WhenAll(taskGyosya, taskTokisk, taskTokiSt);

                ListGyosya = taskGyosya.Result;
                TokiskData = taskTokisk.Result;
                TokiStData = taskTokiSt.Result;
                // N.T.L.Anh Add str 2021/06/01
                if (isGyosyaAddNull)
                    ListGyosya.Insert(0, null);
                // N.T.L.Anh Add end 2021/06/01
                if (DefaultGyosya == null)
                    await RenderByTokiskTokist();
                else
                    await RenderDefault();

                if (FirstLoaded != null)
                    FirstLoaded.Invoke();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async Task RenderByTokiskTokist()
        {
            if (!TokiskData.Any() && _service != null)
            {
                var taskGyosya = _service.GetListGyosya();
                var taskTokisk = _service.GetListTokisk(SiyoDateStr, SiyoDateEnd);
                var taskTokiSt = _service.GetListTokiSt(SiyoDateStr, SiyoDateEnd);

                await Task.WhenAll(taskGyosya, taskTokisk, taskTokiSt);

                ListGyosya = taskGyosya.Result;
                TokiskData = taskTokisk.Result;
                TokiStData = taskTokiSt.Result;
            }
            var tokiskData = TokiskData.Where(_ => _ != null && _.TokuiSeq == DefaultTokisk).FirstOrDefault();
            if (tokiskData == null)
            {
                await RenderDefault();
                return;
            }
            ListTokisk = TokiskData.Where(x => x.GyosyaCdSeq == tokiskData.GyosyaCdSeq).ToList();
            if (ListTokisk.Any() && isTokiskAddNull)
                ListTokisk.Insert(0, null);
            SelectedTokisk = ListTokisk.FirstOrDefault(_ => _ != null && _.TokuiSeq == DefaultTokisk);
            await SelectedTokiskChanged.InvokeAsync(SelectedTokisk);

            SelectedGyosya = ListGyosya.FirstOrDefault(_ => _ != null && _.GyosyaCdSeq == SelectedTokisk?.GyosyaCdSeq);
            await SelectedGyosyaChanged.InvokeAsync(SelectedGyosya);

            ListTokiSt = TokiStData.Where(_ => _ != null && _.TokuiSeq == tokiskData.TokuiSeq).ToList();
            if (ListTokiSt.Any() && isTokiStAddNull)
                ListTokiSt.Insert(0, null);
            SelectedTokiSt = ListTokiSt.FirstOrDefault(_ => _ != null && _.SitenCdSeq == DefaultTokiSt);
            await SelectedTokiStChanged.InvokeAsync(SelectedTokiSt);
        }

        public async Task SetNullComponent(bool isSelected)
        {
            if (isSelected)
            {
                ListGyosya = await _service.GetListGyosya();
                ListGyosya.Insert(0, null);
            }
            else
                ListGyosya = new List<CustomerComponentGyosyaData>();
            ListTokisk = new List<CustomerComponentTokiskData>();
            ListTokiSt = new List<CustomerComponentTokiStData>();
            await SelectedGyosyaChanged.InvokeAsync(null);
            await SelectedTokiskChanged.InvokeAsync(null);
            await SelectedTokiStChanged.InvokeAsync(null);
        }

        public async Task RenderDefault()
        {
            if (DefaultGyosya == 0)
                SelectedGyosya = ListGyosya.Where(_ => _ != null).ToList().FirstOrDefault(_ => VehicleClassification != 1 || _.GyosyaKbn == 2);
            else
                SelectedGyosya = ListGyosya.Where(_ => _ != null).ToList().FirstOrDefault(_ => _.GyosyaCdSeq == DefaultGyosya);

            // N.T.L.Anh Add str 2021/06/01
            if (isGyosyaAddNull && DefaultGyosya == 0)
                SelectedGyosya = ListGyosya.FirstOrDefault();
            // N.T.L.Anh Add end 2021/06/01
            await SelectedGyosyaChanged.InvokeAsync(SelectedGyosya);

            ListTokisk = TokiskData.Where(_ => _ != null && _.GyosyaCdSeq == (SelectedGyosya?.GyosyaCdSeq ?? -1)).ToList();
            if (ListTokisk.Any() && isTokiskAddNull)
                ListTokisk.Insert(0, null);

            if (DefaultTokisk == 0)
                return;

            SelectedTokisk = ListTokisk.FirstOrDefault(_ => _ != null && _.TokuiSeq == DefaultTokisk);
            await SelectedTokiskChanged.InvokeAsync(SelectedTokisk);
            if (SelectedTokisk != null)
                SelectedGyosya = ListGyosya.FirstOrDefault(_ => _ != null && _.GyosyaCdSeq == SelectedTokisk.GyosyaCdSeq);
            await SelectedGyosyaChanged.InvokeAsync(SelectedGyosya);

            ListTokiSt = TokiStData.Where(_ => _ != null && _.TokuiSeq == (SelectedTokisk?.TokuiSeq ?? -1)).ToList();
            if (ListTokiSt.Any() && isTokiStAddNull)
                ListTokiSt.Insert(0, null);
            if (DefaultTokiSt != 0)
            {
                SelectedTokiSt = ListTokiSt.FirstOrDefault(_ => _ != null && _.SitenCdSeq == DefaultTokiSt);
                await SelectedTokiStChanged.InvokeAsync(SelectedTokiSt);
            }
        }

        public void UpdateSelectedItems()
        {
            BindSelectedItems();
        }

        private void BindSelectedItems()
        {
            if (DefaultGyosya == null)
                SelectedGyosya = null;
            else if (DefaultGyosya == 0)
                SelectedGyosya = ListGyosya.FirstOrDefault(_ => VehicleClassification != 1 || _.GyosyaKbn == 2);
            else
                SelectedGyosya = ListGyosya.FirstOrDefault(_ => _?.GyosyaCdSeq == DefaultGyosya);
            ListTokisk = TokiskData.Where(_ => _ != null && _.GyosyaCdSeq == (SelectedGyosya?.GyosyaCdSeq ?? -1)).ToList();
            if (ListTokisk.Any() && isTokiskAddNull)
                ListTokisk.Insert(0, null);
            ListTokiSt = TokiStData.Where(_ => _ != null && _.TokuiSeq == (SelectedTokisk?.TokuiSeq ?? -1)).ToList();
            if (ListTokiSt.Any() && isTokiStAddNull)
                ListTokiSt.Insert(0, null);
        }

        protected void OnChangeGyosya(object gyosya)
        {
            try
            {
                SelectedGyosya = gyosya as CustomerComponentGyosyaData;
                if (SelectedGyosya != null)
                    SelectedGyosya.IsSelect = true;
                SelectedGyosyaChanged.InvokeAsync(SelectedGyosya);
                ListTokisk = TokiskData.Where(_ => _ != null && _.GyosyaCdSeq == (SelectedGyosya?.GyosyaCdSeq ?? -1)).ToList();
                if (ListTokisk.Any() && isTokiskAddNull)
                    ListTokisk.Insert(0, null);
                OnChangeTokisk(null);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnChangeTokisk(object tokisk)
        {
            try
            {
                SelectedTokisk = tokisk as CustomerComponentTokiskData;
                if(SelectedTokisk != null)
                  SelectedTokisk.IsSelect = true;
                SelectedTokiskChanged.InvokeAsync(SelectedTokisk);
                ListTokiSt = TokiStData.Where(_ => _ != null && _.TokuiSeq == (SelectedTokisk?.TokuiSeq ?? -1)).ToList();
                if (ListTokiSt.Any() && isTokiStAddNull)
                    ListTokiSt.Insert(0, null);
                OnChangeTokiSt(null);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void OnChangeTokiSt(object tokiSt)
        {
            try
            {
                SelectedTokiSt = tokiSt as CustomerComponentTokiStData;
                if (SelectedTokiSt != null)
                    SelectedTokiSt.IsSelect = true;
                SelectedTokiStChanged.InvokeAsync(SelectedTokiSt);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OnChangeDefaultGyosya(int? gyosya)
        {
            try
            {
                DefaultGyosya = gyosya;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OnChangeDefaultTokisk(int tokisk)
        {
            try
            {
                DefaultTokisk = tokisk;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public void OnChangeDefaultTokiSt(int tokist)
        {
            try
            {
                DefaultTokiSt = tokist;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
        public void OnchangeCustomerDataByTime(List<CustomerComponentTokiskData> _TokiskData, List<CustomerComponentTokiStData> _TokiStData)
        {
            TokiskData = _TokiskData;
            TokiStData = _TokiStData;
            OnChangeGyosya(null);
        }

        public void Enter(KeyboardEventArgs e, string ValueName)
        {
            try
            {
                if (e.Code != "Enter")
                    return;
                switch (ValueName)
                {
                    case Gyosya:
                        if (string.IsNullOrWhiteSpace(gyosyaSearchString))
                        {
                            OnChangeGyosya(null);
                            return;
                        }
                        var gyosyaFirstItem = ListGyosya.FirstOrDefault(x => x != null && x.Text.Contains(gyosyaSearchString));
                        if (gyosyaFirstItem == null)
                            return;
                        OnChangeGyosya(gyosyaFirstItem);
                        break;
                    case Tokisk:
                        if (string.IsNullOrWhiteSpace(tokiskSearchString))
                        {
                            OnChangeTokisk(null);
                            return;
                        }
                        var tokiskFirstItem = ListTokisk.FirstOrDefault(x => x != null && x.Text.Contains(tokiskSearchString));
                        if (tokiskFirstItem == null)
                            return;
                        OnChangeTokisk(tokiskFirstItem);
                        break;
                    case Tokist:
                        if (string.IsNullOrWhiteSpace(tokistSearchString))
                        {
                            OnChangeTokiSt(null);
                            return;
                        }
                        var firstItem = ListTokiSt.FirstOrDefault(x => x != null && x.Text.Contains(tokistSearchString));
                        if (firstItem == null)
                            return;
                        OnChangeTokiSt(firstItem);
                        break;
                }
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }
    }
}

using HassyaAllrightCloud.Commons;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages.Components.ETC
{
    public class ETCFormBase : ComponentBase
    {
        [Inject] protected IStringLocalizer<ETCForm> _lang { get; set; }
        [Inject] protected IStringLocalizer<ETCList> _langEtcList { get; set; }
        [Inject] protected IETCService etcService { get; set; }
        [Inject] protected IETCFormService etcFormService { get; set; }
        [Inject] protected IETCImportConditionSettingService eTCImportConditionSettingService { get; set; }
        [Inject] protected IJSRuntime jsRuntime { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Parameter] public bool FormValid { get; set; }
        [Parameter] public EventCallback<bool> FormValidChanged { get; set; } = new EventCallback<bool>();
        [Parameter] public EventCallback<FormVisibleChangedEvent> FormVisibleChanged { get; set; } = new EventCallback<FormVisibleChangedEvent>();
        [Parameter] public ETCData ETCData { get; set; }
        protected ETCFormModel Model { get; set; } = new ETCFormModel();
        protected List<ETCSyaRyo> listSyaRyo { get; set; } = new List<ETCSyaRyo>();
        protected List<ETCFutai> listFutai { get; set; } = new List<ETCFutai>();
        protected List<ETCSeisan> listSeisan { get; set; } = new List<ETCSeisan>();
        protected List<RyokinDataItem> listRyokin { get; set; } = new List<RyokinDataItem>();
        protected List<ETCYoyakuData> listYoyaku { get; set; } = new List<ETCYoyakuData>();
        private List<ETCKyoSet> listKyoSet { get; set; } = new List<ETCKyoSet>();
        protected ETCFormTypeEnum formType;
        protected string errorContent = string.Empty;
        protected bool isError = false;
        protected bool isCreateValid = false;
        protected bool? isSyaRyoError = null;
        protected bool? isUnkYmdError = null;
        protected bool? isFutaiError = null;
        protected bool? isSeisanError = null;
        private const string UpdPrgID = "KO0300P";
        private const string DefaultTime = "235900";
        private const string DefaultChiCd = "0";
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var syaRyoTask = etcService.GetListETCSyaRyo(new ClaimModel().TenantID);
                var futaiTask = etcService.GetListETCFutai(new ClaimModel().TenantID);
                var seisanTask = etcService.GetListETCSeisan();
                var ryokinTask = etcFormService.GetRyoKin();
                var kyoSetTask = etcService.GetListKyoSet();

                await Task.WhenAll(syaRyoTask, futaiTask, seisanTask, ryokinTask, kyoSetTask);
                listSyaRyo = syaRyoTask.Result;
                listFutai = futaiTask.Result;
                listSeisan = seisanTask.Result;
                listRyokin = ryokinTask.Result;
                listKyoSet = kyoSetTask.Result;

                Model = await InitDefaultData();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private async Task<List<ETCYoyakuData>> LoadListYoyaku(ETCFormModel model) => await etcService.GetListYoyaku(new ETCSearchParam()
        {
            ETCDateFrom = model.UnkYmd,
            ETCDateTo = model.UnkYmd,
            SyaryoCd = model.SyaRyo?.SyaRyoCd ?? 0,
            TenantCdSeq = new ClaimModel().TenantID
        });

        private async Task<ETCFormModel> InitDefaultData()
        {
            var selectedRow = ETCData;
            ETCFormModel formModel;
            if (selectedRow == null)
            {
                formType = ETCFormTypeEnum.Create;
                formModel = new ETCFormModel();
                formModel.SeisanWay = 2;
                formModel.ZeiKbnNm = _lang["zeiKbnNmDefaultNm"];
            }
            else
            {
                if (selectedRow.TensoKbn == 0)
                    formType = ETCFormTypeEnum.Update;
                else
                    formType = ETCFormTypeEnum.View;

                formModel = new ETCFormModel();
                formModel.SyaRyo = listSyaRyo.Find(e => e.SyaRyoCd == selectedRow.SyaRyoCd) ?? new ETCSyaRyo() { SyaRyoCd = selectedRow.SyaRyoCd };
                formModel.UnkYmd = DateTime.ParseExact(selectedRow.UnkYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                formModel.UnkTime = selectedRow.UnkTime?.AddColon2HHMMSS() ?? string.Empty;
                formModel.Futai = listFutai.Find(e => e.FutaiCd == selectedRow.selectedFutai?.FutaiCd) ?? listFutai.FirstOrDefault();
                formModel.IriRyaku = listRyokin.Find(e => e.RyokinCd.Trim() == selectedRow.IriRyoCd.Trim() && e.RyokinTikuCd.Trim() == selectedRow.IriRyoChiCd.ToString().Trim());
                formModel.DeRyaku = listRyokin.Find(e => e.RyokinCd.Trim() == selectedRow.DeRyoCd.Trim() && e.RyokinTikuCd.Trim() == selectedRow.DeRyoChiCd.ToString().Trim());
                formModel.Seisan = listSeisan.Find(e => e.SeisanCd == selectedRow.selectedSeisan?.SeisanCd) ?? listSeisan.FirstOrDefault();
                formModel.SeisanWay = (byte)(selectedRow.SeisanKbn == 0 ? 2 : selectedRow.SeisanKbn);
                formModel.Suryo = selectedRow.Suryo;
                formModel.Tanka = selectedRow.TanKa;
                formModel.UriGakKin = selectedRow.UriGakKin;
                formModel.ZeiKbnNm = selectedRow.ZeiKbnNm;
                formModel.ZeiRitu = selectedRow.ZeiRitu;
                formModel.SyaRyoSyo = selectedRow.SyaRyoSyo;
                formModel.TesuRitu = selectedRow.TesuRitu;
                formModel.SyaRyoTes = selectedRow.SyaRyoTes;
                formModel.BikoNm = selectedRow.BikoNm;
                formModel.DanNm = selectedRow.DantaiNm;
                formModel.UkeNo = selectedRow.UkeNo;

                await UpdateDataForETCFormModel(formModel);
                InitCalculate(formModel);
            }
            return formModel;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await jsRuntime.InvokeVoidAsync("formatHHmmss");
                    await jsRuntime.InvokeVoidAsync("inputNumber");
                    await jsRuntime.InvokeVoidAsync("formatDecimalField");
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task UpdateFormModel(string propertyName, dynamic value)
        {
            try
            {
                if (propertyName == nameof(ETCFormModel.UnkTime))
                    OnTimeChanged((string)value, propertyName);
                else
                {
                    var propertyInfo = Model.GetType().GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        switch (propertyName)
                        {
                            case nameof(ETCFormModel.Suryo):
                                propertyInfo.SetValue(Model, Convert.ToInt16(value));
                                break;
                            case nameof(ETCFormModel.Tanka):
                            case nameof(ETCFormModel.UriGakKin):
                            case nameof(ETCFormModel.SyaRyoTes):
                                propertyInfo.SetValue(Model, Convert.ToInt32(value));
                                break;
                            case nameof(ETCFormModel.TesuRitu):
                                propertyInfo.SetValue(Model, Convert.ToDecimal(value));
                                break;
                            case nameof(ETCFormModel.SeisanWay):
                                propertyInfo.SetValue(Model, Convert.ToByte(value.ToString()));
                                break;
                            default:
                                if (propertyName == nameof(ETCFormModel.DanNm))
                                    value = value == null ? string.Empty : (value as string).Substring(0, 50);
                                propertyInfo.SetValue(Model, value);
                                break;
                        }
                    }

                    if ((propertyName == nameof(ETCFormModel.UnkYmd) ||
                        propertyName == nameof(ETCFormModel.SyaRyo)) &&
                        Model.UnkYmd != null &&
                        Model.SyaRyo != null)
                        await UpdateDataForETCFormModel(Model);
                }

                FormValid = ValidateField(propertyName, value) && Model.UnkYmd != null && Model.SyaRyo != null;
                await FormValidChanged.InvokeAsync(FormValid);
                if (FormValid)
                    InitCalculate(Model);

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private bool ValidateForm()
        {
            isSyaRyoError = Model.SyaRyo == null;
            isUnkYmdError = Model.UnkYmd == null;
            isFutaiError = Model.Futai == null;
            isSeisanError = Model.Seisan == null;

            return !(isSyaRyoError == null || isSyaRyoError.Value ||
                            isUnkYmdError == null || isUnkYmdError.Value ||
                            isFutaiError == null || isFutaiError.Value ||
                            isSeisanError == null || isSeisanError.Value);
        }

        private bool ValidateField(string propertyName, dynamic value)
        {
            switch (propertyName)
            {
                case nameof(ETCFormModel.SyaRyo):
                    isSyaRyoError = value == null;
                    break;
                case nameof(ETCFormModel.UnkYmd):
                    isUnkYmdError = value == null;
                    break;
                case nameof(ETCFormModel.Futai):
                    isFutaiError = value == null;
                    break;
                case nameof(ETCFormModel.Seisan):
                    isSeisanError = value == null;
                    break;
            }
            return !((isSyaRyoError != null && isSyaRyoError.Value) ||
                                (isUnkYmdError != null && isUnkYmdError.Value) ||
                                (isFutaiError != null && isFutaiError.Value) ||
                                (isSeisanError != null && isSeisanError.Value));
        }

        protected string GetValidateClass(bool? isError)
        {
            try
            {
                return isError != null ? isError.Value ? "border-invalid" : "border-valid" : string.Empty;
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
                return string.Empty;
            }
        }

        private async Task UpdateDataForETCFormModel(ETCFormModel model)
        {
            listYoyaku = await LoadListYoyaku(model);
            if (listYoyaku.Count == 0)
            {
                errorContent = _lang["BI_T010"];
                isError = true;
                isCreateValid = false;
            }
            else
            {
                isError = false;
                isCreateValid = true;
                if(ETCData != null)
                {
                    var yoyaku = listYoyaku.FirstOrDefault(e => e.UkeNo == ETCData.UkeNo && e.UnkRen == ETCData.UnkRen &&
                       e.TeiDanNo == ETCData.TeiDanNo && e.BunkRen == ETCData.BunkRen && e.UnkYmd == ETCData.UnkYmd);
                    if (yoyaku == null)
                    {
                        var selectedYoyaku = GetSelectedYoyaku(ETCData);
                        if (selectedYoyaku != null)
                        {
                                yoyaku = listYoyaku.FirstOrDefault(e => e.UkeNo == selectedYoyaku.UkeNo && e.UnkRen == selectedYoyaku.UnkRen &&
                            e.TeiDanNo == selectedYoyaku.TeiDanNo && e.BunkRen == selectedYoyaku.BunkRen && e.UnkYmd == selectedYoyaku.UnkYmd);
                        }
                    }

                    if (yoyaku != null)
                        model.ETCYoyaku = yoyaku;
                    else
                        model.ETCYoyaku = listYoyaku.FirstOrDefault();
                }
                else
                    model.ETCYoyaku = listYoyaku.FirstOrDefault();

                model.UkeNo = model.ETCYoyaku.UkeNo;
                model.DanNm = model.ETCYoyaku.DanTaNm;
            }
        }

        private ETCYoyakuData GetSelectedYoyaku(ETCData item)
        {
            ETCYoyakuData? yoyaku = null;
            // get processing yoyaku
            if (item.listYoyaku.Count == 1)
            {
                yoyaku = item.listYoyaku[0];
            }
            else if (item.listYoyaku.Count > 1)
            {
                yoyaku = item.listYoyaku.FirstOrDefault(_ => _.UkeNo == item.UkeNo && _.UnkRen == item.UnkRen);
            }
            return yoyaku;
        }


        private void InitCalculate(ETCFormModel model)
        {
            // set data for ZeiRitu
            if (listKyoSet.Count == 1 && model.UnkYmd != null)
            {
                var kyoSet = listKyoSet.First();
                var unkYmd = model.UnkYmd.Value.ToString(DateTimeFormat.yyyyMMdd);
                if (kyoSet.Zei1StaYmd.CompareTo(unkYmd) < 0 && kyoSet.Zei1EndYmd.CompareTo(unkYmd) > -1)
                {
                    model.ZeiRitu = kyoSet.Zeiritsu1;
                }
                else if (kyoSet.Zei2StaYmd.CompareTo(unkYmd) < 0 && kyoSet.Zei2EndYmd.CompareTo(unkYmd) > -1)
                {
                    model.ZeiRitu = kyoSet.Zeiritsu2;
                }
                else if (kyoSet.Zei3StaYmd.CompareTo(unkYmd) < 0 && kyoSet.Zei3EndYmd.CompareTo(unkYmd) > -1)
                {
                    model.ZeiRitu = kyoSet.Zeiritsu3;
                }
            }

            OnCalculate(model);
        }

        private void OnCalculate(ETCFormModel model)
        {
            model.UriGakKin = model.Tanka * model.Suryo;
            if (model.Futai != null && model.Futai.ZeiHyoKbn == 1)
            {
                model.SyaRyoSyo = (int)Math.Round(model.UriGakKin * (model.ZeiRitu / 100));
                model.ZeiKbnNm = _langEtcList["tax_exemption"];
            }
            else if (model.Futai != null && model.Futai.ZeiHyoKbn == 2)
            {
                model.SyaRyoSyo = (int)Math.Round(model.UriGakKin * (model.ZeiRitu / 100) / (1 + (model.ZeiRitu / 100)));
                model.ZeiKbnNm = _langEtcList["tax_included"];
            }
            else
            {
                model.ZeiKbnNm = _langEtcList["tax_exempt"];
                model.ZeiRitu = 0;
                model.SyaRyoSyo = 0;
            }

            Model.SyaRyoTes = (int)(Model.TesuRitu * Model.UriGakKin / 100);
        }

        protected void OnTimeChanged(string value, string propName)
        {
            try
            {
                var prop = Model.GetType().GetProperty(propName);
                if (prop == null) return;
                value = value.Replace(":", "");

                if (string.IsNullOrEmpty(value))
                    value = CommonConstants.DefaultHHmmss;
                else
                {
                    if (value.Length < 6)
                        value = value.PadLeft(6, '0');
                    else if (value.Length > 6)
                        value = value.Substring(0, 6);

                    if (int.Parse(value.Substring(0, 2)) > 23 || int.Parse(value.Substring(2, 2)) > 59 || int.Parse(value.Substring(4)) > 59)
                        value = CommonConstants.DefaultHHmmss;
                }
                prop.SetValue(Model, value.Insert(2, ":").Insert(5, ":"), null);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        private TkdEtcImport BuildNewTkdEtcImportEntity(ETCFormModel model)
        {
            if (model.UnkYmd == null || model.SyaRyo == null || model.ETCYoyaku == null || model.Futai == null || model.ETCYoyaku == null || model.Seisan == null) return null;
            return new TkdEtcImport()
            {
                TenantCdSeq = new ClaimModel().TenantID,
                FileName = $"{CommonUtil.CurrentYYYYMMDD}{CommonUtil.CurrentHHMMSS}",
                CardNo = "0000",
                EtcRen = 1,
                UnkYmd = model.UnkYmd.Value.ToString(DateTimeFormat.yyyyMMdd),
                SyaRyoCd = model.SyaRyo.SyaRyoCd,
                UkeNo = model.UkeNo,
                UnkRen = model.ETCYoyaku.UnkRen,
                TeiDanNo = model.ETCYoyaku.TeiDanNo,
                BunkRen = model.ETCYoyaku.BunkRen,
                UnkTime = model.UnkTime?.Replace(":", "") ?? DefaultTime,
                HenKai = 0,
                FutTumCdSeq = model.Futai.FutaiCdSeq,
                FutTumNm = model.Futai.FutaiNm,
                IriRyoChiCd = byte.Parse(model.IriRyaku?.RyokinTikuCd ?? DefaultChiCd),
                IriRyoCd = model.IriRyaku?.RyokinCd ?? string.Empty,
                DeRyoChiCd = byte.Parse(model.DeRyaku?.RyokinTikuCd ?? DefaultChiCd),
                DeRyoCd = model.DeRyaku?.RyokinCd ?? string.Empty,
                SeisanCdSeq = model.Seisan.SeisanCdSeq,
                SeisanNm = model.Seisan.SeisanNm,
                SeisanKbn = model.SeisanWay,
                Suryo = model.Suryo,
                TanKa = model.Tanka,
                TesuRitu = model.TesuRitu,
                SyaRyoTes = model.SyaRyoTes,
                TensoKbn = 0,
                ImportTanka = 0,
                BikoNm = model.BikoNm ?? string.Empty,
                ExpItem = "1",
                SiyoKbn = 1,
                UpdYmd = CommonUtil.CurrentYYYYMMDD,
                UpdTime = CommonUtil.CurrentHHMMSS,
                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                UpdPrgId = UpdPrgID
            };
        }

        private TkdEtcImport BuildUpdateTkdEtcImportEntity(ETCFormModel model)
        {
            if (model.ETCYoyaku == null || ETCData == null || model.Futai == null || model.Seisan == null) return null;
            return new TkdEtcImport()
            {
                TenantCdSeq = new ClaimModel().TenantID,
                FileName = ETCData.FileName,
                CardNo = ETCData.CardNo,
                EtcRen = ETCData.EtcRen,
                UnkYmd = ETCData.UnkYmd,
                SyaRyoCd = ETCData.SyaRyoCd,
                UkeNo = model.ETCYoyaku.UkeNo,
                UnkRen = model.ETCYoyaku.UnkRen,
                TeiDanNo = model.ETCYoyaku.TeiDanNo,
                BunkRen = model.ETCYoyaku.BunkRen,
                UnkTime = model.UnkTime?.Replace(":", "") ?? DefaultTime,
                HenKai = 0,
                FutTumCdSeq = model.Futai.FutaiCdSeq,
                FutTumNm = model.Futai.FutaiNm,
                IriRyoChiCd = byte.Parse(model.IriRyaku?.RyokinTikuCd ?? DefaultChiCd),
                IriRyoCd = model.IriRyaku?.RyokinCd ?? string.Empty,
                DeRyoChiCd = byte.Parse(model.DeRyaku?.RyokinTikuCd ?? DefaultChiCd),
                DeRyoCd = model.DeRyaku?.RyokinCd ?? string.Empty,
                SeisanCdSeq = model.Seisan.SeisanCdSeq,
                SeisanNm = model.Seisan.SeisanNm,
                SeisanKbn = model.SeisanWay,
                Suryo = model.Suryo,
                TanKa = model.Tanka,
                TesuRitu = model.TesuRitu,
                SyaRyoTes = model.SyaRyoTes,
                TensoKbn = 0,
                ImportTanka = 0,
                BikoNm = model.BikoNm ?? string.Empty,
                ExpItem = "1",
                SiyoKbn = 1,
                UpdYmd = CommonUtil.CurrentYYYYMMDD,
                UpdTime = CommonUtil.CurrentHHMMSS,
                UpdSyainCd = new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq,
                UpdPrgId = UpdPrgID
            };
        }

        public async Task Save()
        {
            try
            {
                FormValid = ValidateForm();
                if (FormValid)
                {
                    TkdEtcImport? entity = null;
                    bool? isInsert = null;
                    if (formType == ETCFormTypeEnum.Create)
                    {
                        entity = BuildNewTkdEtcImportEntity(Model);
                        isInsert = true;
                    }
                    else if (formType == ETCFormTypeEnum.Update)
                    {
                        entity = BuildUpdateTkdEtcImportEntity(Model);
                        isInsert = false;
                    }

                    if (entity == null || isInsert == null)
                        await FormValidChanged.InvokeAsync(false);
                    else
                    {
                        await eTCImportConditionSettingService.InsertOrUpdateEtcImport(entity, isInsert.Value);
                        await FormVisibleChanged.InvokeAsync(new FormVisibleChangedEvent() { IsVisible = false, IsReload = !isInsert.Value });
                    }
                }
                else
                {
                    await FormValidChanged.InvokeAsync(false);
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task Clear()
        {
            try
            {
                if(formType == ETCFormTypeEnum.Create || formType == ETCFormTypeEnum.Update)
                {
                    var selectedRow = ETCData;
                    ETCFormModel formModel;
                    if (formType == ETCFormTypeEnum.Create)
                    {
                        formModel = new ETCFormModel();
                        formModel.SeisanWay = 2;
                        formModel.ZeiKbnNm = _lang["zeiKbnNmDefaultNm"];
                        listYoyaku = new List<ETCYoyakuData>();
                        isSyaRyoError = isUnkYmdError = isFutaiError = isSeisanError = false;
                        await FormValidChanged.InvokeAsync(false);
                    }
                    else
                    {
                        formModel = new ETCFormModel();
                        formModel.SyaRyo = listSyaRyo.Find(e => e.SyaRyoCd == selectedRow.SyaRyoCd) ?? new ETCSyaRyo() { SyaRyoCd = selectedRow.SyaRyoCd };
                        formModel.UnkYmd = DateTime.ParseExact(selectedRow.UnkYmd, DateTimeFormat.yyyyMMdd, CultureInfo.InvariantCulture);
                        formModel.SeisanWay = 2;
                        await UpdateDataForETCFormModel(formModel);
                        await FormValidChanged.InvokeAsync(ValidateForm());
                    }
                    
                    Model = formModel;
                    isCreateValid = false;
                    isError = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task ChangeSelectedRow(ETCYoyakuData item)
        {
            try
            {
                if (formType != ETCFormTypeEnum.View)
                {
                    Model.ETCYoyaku = item;
                    Model.UkeNo = Model.ETCYoyaku.UkeNo;
                    Model.DanNm = Model.ETCYoyaku.DanTaNm;
                    Model.TesuRitu = Model.ETCYoyaku.TesuRituFut;
                }
                await FormValidChanged.InvokeAsync(ValidateForm());
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }
    }
}

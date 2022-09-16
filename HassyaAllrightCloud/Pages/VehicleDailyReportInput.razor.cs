using DevExpress.Entity.Model.Metadata;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class VehicleDailyReportInputBase : ComponentBase
    {
        [Inject]
        protected IStringLocalizer<VehicleDailyReportInput> _lang { get; set; }
        [Inject]
        protected IJSRuntime jsRuntime { get; set; }
        [Inject]
        protected IVehicleDailyReportService vehicleDailyReportService { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Parameter]
        public VehicleDailyReportModel selectedShabni { get; set; }
        [Parameter]
        public bool IsShowPopup { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> TogglePopup { get; set; } 

        public EditContext vehicleDailyInputForm { get; set; }
        public bool isShowPopup { get; set; } = false;
        public List<VehicleDailyReportData> listDto { get; set; } = new List<VehicleDailyReportData>();
        public VehicleDailyReportData dto { get; set; } = new VehicleDailyReportData();
        public List<string> listUnkYmd { get; set; } = new List<string>();
        public string selectedUnkYmd { get; set; }
        public List<string> listVehicle { get; set; } = new List<string>();
        public string selectedVehicle { get; set; }

        public VehicleDailyReportHaisha vehicleHaisha { get; set; }
        public string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //var selectedShabni = EncryptHelper.Decrypt<VehicleDailyReportModel>(searchString);
            //listUnkYmd = await vehicleDailyReportService.GetListUnkYmdForModify(selectedShabni.UkeNo);
            var start = DateTime.ParseExact(selectedShabni.SyukoYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(selectedShabni.KikYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture);
            listUnkYmd = Enumerable.Range(0, 1 + end.Subtract(start).Days).Select(offset => start.AddDays(offset).ToString(Formats.SlashyyyyMMdd)).ToList();
            selectedUnkYmd = selectedShabni.UnkYmd.Insert(4, "/").Insert(7, "/");
            listDto = await vehicleDailyReportService.GetListVehicleDailyReportForUpdate(selectedShabni);
            dto = listDto.FirstOrDefault(_ => _.UnkYmd == selectedUnkYmd.Replace("/", ""));
            if(dto == null)
            {
                dto = new VehicleDailyReportData()
                {
                    UkeNo = selectedShabni.UkeNo,
                    UnkRen = selectedShabni.UnkRen,
                    TeiDanNo = selectedShabni.TeiDanNo,
                    BunkRen = selectedShabni.BunkRen,
                    HaiSYmd = DateTime.ParseExact(selectedShabni.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture),
                    TouYmd = DateTime.ParseExact(selectedShabni.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture),
                    UnkYmd = selectedUnkYmd,
                    NenryoCd1Seq = selectedShabni.NenryoCd1Seq,
                    NenryoCd2Seq = selectedShabni.NenryoCd2Seq,
                    NenryoCd3Seq = selectedShabni.NenryoCd3Seq,
                    SyaRyoCd = selectedShabni.SyaRyoCd.ToString(),
                    SyaRyoNm = selectedShabni.SyaryoNm,
                    isUpdate = false
                };
            }
            dto.totalDays = (byte)listUnkYmd.Count;
            selectedVehicle = $"{dto.SyaRyoCd.PadLeft(5, '0')} : {dto.SyaRyoNm}";
            listVehicle.Add(selectedVehicle);
            OnFormatDto();

            var listHaisha = await vehicleDailyReportService.GetHaisha(selectedShabni);
            vehicleHaisha = listHaisha.FirstOrDefault();

            CalculateAmount();
            vehicleDailyInputForm = new EditContext(dto);
        }

        private void CalculateAmount()
        {
            if (vehicleHaisha != null)
            {
                dto.KoskuTime = dto.KoskuTime.Replace("-", "");
                var hour = int.Parse(dto.KoskuTime.Substring(0, 2)) + int.Parse(dto.InspectionTime.Substring(0, 2));
                var minute = int.Parse(dto.KoskuTime.Substring(3)) + int.Parse(dto.InspectionTime.Substring(3));
                MinMaxGridData minMaxGridData = new MinMaxGridData();
                minMaxGridData.KmRunning = (int)decimal.Parse(dto.TotalMile);
                minMaxGridData.TimeRunning = new BookingInputHelper.MyTime(hour, minute);
                MinMaxSettingFormData minMaxSettingFormData = new MinMaxSettingFormData();
                minMaxSettingFormData.minMaxGridData.Add(minMaxGridData);
                minMaxSettingFormData.typeBus = vehicleHaisha.KataKbn;
                VpmTransportationFeeRule vpmTransportationFeeRule = new VpmTransportationFeeRule();
                vpmTransportationFeeRule.BigVehicalMaxUnitPriceforHour = vehicleHaisha.BigVehicalMaxUnitPriceforHour;
                vpmTransportationFeeRule.BigVehicalMaxUnitPriceforKm = vehicleHaisha.BigVehicalMaxUnitPriceforKm;
                vpmTransportationFeeRule.BigVehicalMinUnitPriceforHour = vehicleHaisha.BigVehicalMinUnitPriceforHour;
                vpmTransportationFeeRule.BigVehicalMinUnitPriceforKm = vehicleHaisha.BigVehicalMinUnitPriceforKm;
                vpmTransportationFeeRule.MedVehicalMaxUnitPriceforHour = vehicleHaisha.MedVehicalMaxUnitPriceforHour;
                vpmTransportationFeeRule.MedVehicalMaxUnitPriceforKm = vehicleHaisha.MedVehicalMaxUnitPriceforKm;
                vpmTransportationFeeRule.MedVehicalMinUnitPriceforHour = vehicleHaisha.MedVehicalMinUnitPriceforHour;
                vpmTransportationFeeRule.MedVehicalMinUnitPriceforKm = vehicleHaisha.MedVehicalMinUnitPriceforKm;
                vpmTransportationFeeRule.SmallVehicalMaxUnitPriceforHour = vehicleHaisha.SmallVehicalMaxUnitPriceforHour;
                vpmTransportationFeeRule.SmallVehicalMaxUnitPriceforKm = vehicleHaisha.SmallVehicalMaxUnitPriceforKm;
                vpmTransportationFeeRule.SmallVehicalMinUnitPriceforHour = vehicleHaisha.SmallVehicalMinUnitPriceforHour;
                vpmTransportationFeeRule.SmallVehicalMinUnitPriceforKm = vehicleHaisha.SmallVehicalMinUnitPriceforKm;
                minMaxSettingFormData.SelectedTranSportationOfficePlace = vpmTransportationFeeRule;

                dto.FeeMaxAmount = vehicleHaisha.FeeMaxAmount;
                dto.FeeMinAmount = vehicleHaisha.FeeMinAmount;
                dto.FareMaxAmount = minMaxSettingFormData.getMaxUnitPriceForHour() + minMaxSettingFormData.getMaxUnitPriceForkm();
                dto.FareMinAmount = minMaxSettingFormData.getMinUnitPriceForHour() + minMaxSettingFormData.getMinUnitPriceForkm();
                
                dto.FareFeeMaxAmount = dto.FareMaxAmount + dto.FeeMaxAmount;
                dto.FareFeeMinAmount = dto.FareMinAmount + dto.FeeMinAmount;
            }
        }

        private void OnFormatDto()
        {
            dto.SyuPaTime = dto.SyuPaTime.Replace(":", "").Insert(2, ":");
            dto.SyuKoTime = dto.SyuKoTime.Replace(":", "").Insert(2, ":");
            dto.KoskuTime = dto.KoskuTime.Replace(":", "").Insert(2, ":");
            dto.TouChTime = dto.TouChTime.Replace(":", "").Insert(2, ":");
            dto.KikTime = dto.KikTime.Replace(":", "").Insert(2, ":");
            dto.JisTime = dto.JisTime.Replace(":", "").Insert(2, ":");
            dto.InspectionTime = dto.InspectionTime.Replace(":", "").Insert(2, ":");
            dto.TotalMile = (decimal.Parse(dto.EndMeter) - decimal.Parse(dto.StMeter)).ToString("F2");
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jsRuntime.InvokeVoidAsync("focusFirstItem", ".focus-popup");
            }
            jsRuntime.InvokeVoidAsync("formatTime");
            jsRuntime.InvokeVoidAsync("setEventforDecimalField", ".two-decimals-3", 3);
            jsRuntime.InvokeVoidAsync("setEventforDecimalField", ".two-decimals-5", 5);
            jsRuntime.InvokeVoidAsync("setEventforDecimalField", ".two-decimals-7", 7);
            jsRuntime.InvokeVoidAsync("EnterTab", ".focus-popup", false, false);
            jsRuntime.InvokeVoidAsync("inputNumber");
            return base.OnAfterRenderAsync(firstRender);
        }

        protected async Task OnTogglePopup(bool isDelete)
        {
            if (isDelete)
            {
                errorMessage = string.Empty;
                await Task.Delay(100);
                await InvokeAsync(StateHasChanged);

                bool isValid = await haitaCheck();

                if (isValid)
                {
                    var result = await vehicleDailyReportService.DeleteVehicleDailyReport(dto);
                    await TogglePopup.InvokeAsync(new MouseEventArgs() { Type = "search" });
                }
                else
                {
                    errorMessage = "BI_T006";
                }
            }
            isShowPopup = !isShowPopup;
        }

        protected void OnTimeChanged(string value, string propName)
        {
            var classType = dto.GetType();
            var prop = classType.GetProperty(propName);
            value = value.Normalize(NormalizationForm.FormKC);

            //if (!string.IsNullOrEmpty(value))
            //    value = ((string)value).TruncateWithMaxLength(maxLength);
            value = value.Replace(":", "");

            if (string.IsNullOrEmpty(value))
            {
                value = CommonConstants.DefaultTime;
            }
            else
            {
                if(value.Length < 4)
                {
                    value = value.PadLeft(4, '0');
                }
                if(int.Parse(value.Substring(0, 2)) > 23 || int.Parse(value.Substring(2)) > 59)
                {
                    value = CommonConstants.DefaultTime;
                }
            }
            prop.SetValue(dto, value.Insert(2, ":"), null);

            switch (propName)
            {
                case "SyuKoTime":
                case "KikTime":
                    dto.KoskuTime = CalculateTime(dto.SyuKoTime, dto.KikTime);
                    CalculateAmount();
                    break;
                case "SyuPaTime":
                case "TouChTime":
                    dto.JisTime = CalculateTime(dto.SyuPaTime, dto.TouChTime);
                    break;
                case "InspectionTime":
                    CalculateAmount();
                    break;
            }
            StateHasChanged();
        }

        private string CalculateTime(string data1, string data2)
        {
            var currentDateTime = DateTime.Now;
            var date1 = currentDateTime;
            if (!string.IsNullOrEmpty(data1))
            {
                date1 = date1.AddHours(Convert.ToInt32(data1.Substring(0, data1.Length < 2 ? data1.Length : 2)));
                if (data1.Length > 2)
                {
                    if (data1.Contains(":"))
                    {
                        date1 = date1.AddMinutes(Convert.ToInt32(data1.Substring(3)));
                    }
                    else
                    {
                        date1 = date1.AddMinutes(Convert.ToInt32(data1.Substring(2)));
                    }
                }
            }
            
            var date2 = currentDateTime;
            if (!string.IsNullOrEmpty(data2))
            {
                date2 = date2.AddHours(Convert.ToInt32(data2.Substring(0, data2.Length < 2 ? data2.Length : 2)));
                if (data2.Length > 2)
                {
                    if (data2.Contains(":"))
                    {
                        date2 = date2.AddMinutes(Convert.ToInt32(data2.Substring(3)));
                    }
                    else
                    {
                        date2 = date2.AddMinutes(Convert.ToInt32(data2.Substring(2)));
                    }
                }
            }
            var hour = (date2 - date1).TotalHours;
            var minute = Math.Round((hour % 1) * 60);
            var displayHour = hour < 0 ? "-" + Math.Floor(Math.Abs(hour)).ToString().PadLeft(2, '0') : Math.Floor(hour).ToString().PadLeft(2, '0');
            return string.Format("{0}:{1}", displayHour, Math.Abs(minute).ToString().PadLeft(2, '0'));
        }

        protected void OnDecimalChanged(string value, string propName, byte firstPart)
        {
            var classType = dto.GetType();
            var prop = classType.GetProperty(propName);
            value = value.Normalize(NormalizationForm.FormKC);
            if (decimal.TryParse(value, out decimal v) && v >= 0)
            {
                var temp = value;
                if (value.Contains("."))
                {
                    temp = value.Split('.')[0];
                }

                if (temp.Length > firstPart)
                {
                    temp = temp.Substring(0, firstPart);
                    value = temp + value.Split('.')[1];
                }

                //if (!string.IsNullOrEmpty(value))
                //    value = ((string)value).TruncateWithMaxLength(maxLength);

                if (!string.IsNullOrEmpty(value))
                {
                    prop.SetValue(dto, decimal.Parse(value).ToString("F2"), null);
                }
                else
                {
                    prop.SetValue(dto, "0.00", null);
                }

                switch (propName)
                {
                    case "StMeter":
                    case "EndMeter":
                        var totalMile = decimal.Parse(dto.EndMeter) - decimal.Parse(dto.StMeter);
                        dto.TotalMile = totalMile.ToString("F2");
                        var sum = decimal.Parse(dto.JisaIPKm) + decimal.Parse(dto.JisaKSKm) + decimal.Parse(dto.KisoIPKm) + decimal.Parse(dto.KisoKOKm);
                        dto.OthKm = (totalMile - sum).ToString("F2");
                        CalculateAmount();
                        break;
                    case "JisaIPKm":
                    case "JisaKSKm":
                    case "KisoIPkm":
                    case "KisoKOKm":
                        var total = decimal.Parse(dto.TotalMile);
                        var sumKm = decimal.Parse(dto.JisaIPKm) + decimal.Parse(dto.JisaKSKm) + decimal.Parse(dto.KisoIPKm) + decimal.Parse(dto.KisoKOKm);
                        dto.OthKm = (total - sumKm).ToString("F2");
                        break;
                }
                StateHasChanged();
            }
            else
            {
                var val = prop.GetValue(dto);
                prop.SetValue(dto, val.ToString(), null);
            }

            switch (propName)
            {
                case "StMeter":
                case "EndMeter":
                    var totalMile = decimal.Parse(dto.EndMeter) - decimal.Parse(dto.StMeter);
                    dto.TotalMile = totalMile.ToString("F");
                    var sum = decimal.Parse(dto.JisaIPKm) + decimal.Parse(dto.JisaKSKm) + decimal.Parse(dto.KisoIPKm) + decimal.Parse(dto.KisoKOKm);
                    dto.OthKm = (totalMile - sum).ToString("F");
                    CalculateAmount();
                    break;
                case "JisaIPKm":
                case "JisaKSKm":
                case "KisoIPKm":
                case "KisoKOKm":
                    var total = decimal.Parse(dto.TotalMile);
                    var sumKm = decimal.Parse(dto.JisaIPKm) + decimal.Parse(dto.JisaKSKm) + decimal.Parse(dto.KisoIPKm) + decimal.Parse(dto.KisoKOKm);
                    dto.OthKm = (total - sumKm).ToString("F");
                    break;
            }
            StateHasChanged();
        }

        protected void OnDateTimeChanged(DateTime? value, string propName)
        {
            var classType = dto.GetType();
            var prop = classType.GetProperty(propName);
            prop.SetValue(dto, value, null);
            StateHasChanged();
        }

        protected async Task OnChangeUnkYmd(string value)
        {
            selectedUnkYmd = value;
            var temp = selectedShabni.UnkYmd;
            selectedShabni.UnkYmd = DateTime.ParseExact(selectedUnkYmd, Formats.SlashyyyyMMdd, CultureInfo.InvariantCulture).ToString(CommonConstants.FormatYMD);
            listDto = await vehicleDailyReportService.GetListVehicleDailyReportForUpdate(selectedShabni);
            dto = listDto.FirstOrDefault();
            selectedShabni.UnkYmd = temp;
            if (dto == null)
            {
                dto = new VehicleDailyReportData()
                {
                    UkeNo = selectedShabni.UkeNo,
                    UnkRen = selectedShabni.UnkRen,
                    TeiDanNo = selectedShabni.TeiDanNo,
                    BunkRen = selectedShabni.BunkRen,
                    HaiSYmd = DateTime.ParseExact(selectedShabni.HaiSYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture),
                    TouYmd = DateTime.ParseExact(selectedShabni.TouYmd, CommonConstants.FormatYMD, CultureInfo.InvariantCulture),
                    UnkYmd = selectedUnkYmd,
                    NenryoCd1Seq = selectedShabni.NenryoCd1Seq,
                    NenryoCd2Seq = selectedShabni.NenryoCd2Seq,
                    NenryoCd3Seq = selectedShabni.NenryoCd3Seq,
                    isUpdate = false
                };
            }
            dto.totalDays = (byte)listUnkYmd.Count;
            OnFormatDto();
            CalculateAmount();
            vehicleDailyInputForm = new EditContext(dto);
            StateHasChanged();
        }

        protected void OnChangeNumber(string value, string propName)
        {
            var classType = dto.GetType();
            var prop = classType.GetProperty(propName);
            value = value.Normalize(NormalizationForm.FormKC);
            if (int.TryParse(value, out int v) && v >= 0 && v <= 9999)
            {
                prop.SetValue(dto, value, null);
            }
            else
            {
                var val = prop.GetValue(dto);
                prop.SetValue(dto, val.ToString(), null);
            }
            StateHasChanged();
        }

        protected async Task OnSave()
        {
            errorMessage = string.Empty;
            await Task.Delay(100);
            await InvokeAsync(StateHasChanged);

            bool isValid = await haitaCheck();

            if (vehicleDailyInputForm.Validate() && isValid)
            {
                var result = await vehicleDailyReportService.SaveVehicleDailyReport(dto);
                if (result)
                {
                    await TogglePopup.InvokeAsync(new MouseEventArgs() { Type = "search" });
                }
            }
            else if (!isValid)
            {
                errorMessage = "BI_T006";
            }
        }

        private async Task<bool> haitaCheck()
        {
            var haita = await vehicleDailyReportService.GetHaitaCheck(dto.UkeNo, dto.UnkRen, dto.TeiDanNo, dto.BunkRen, dto.UnkYmd);
            if (!string.IsNullOrEmpty(haita.UpdYmd) && !string.IsNullOrEmpty(haita.UpdTime) && (haita.UpdYmd != dto.UpdYmd || haita.UpdTime != dto.UpdTime))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(haita.HaishaUpdYmd) && !string.IsNullOrEmpty(haita.HaishaUpdTime) && (haita.HaishaUpdYmd != vehicleHaisha.UpdYmd || haita.HaishaUpdTime != vehicleHaisha.UpdTime))
            {
                return false;
            }

            return true;
        }
    }
}

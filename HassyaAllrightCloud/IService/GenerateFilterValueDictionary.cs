using HassyaAllrightCloud.Application.HyperData.Queries;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.DepositCoupon;
using HassyaAllrightCloud.Domain.Dto.InvoiceIssueRelease;
using HassyaAllrightCloud.Domain.Dto.BillPrint;
using HassyaAllrightCloud.Domain.Dto.BillingList;
using HassyaAllrightCloud.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.API.Native;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;

namespace HassyaAllrightCloud.IService
{
    public interface IGenerateFilterValueDictionary
    {
        Task<Dictionary<string, string>> GenerateForHyperFormData(HyperFormData hyperFormData);
        Task<Dictionary<string, string>> GenerateInvoiceIssueFilter(InvoiceIssueFilter invoiceIssueFilter);
        Task<Dictionary<string, string>> GenerateForBillCheckListFormData(BillsCheckListFormData billFormData);
        Task<Dictionary<string, string>> GenerateForAdvancedPayment(AdvancePaymentDetailsSearchParam searchParams);
        Task<Dictionary<string, string>> GenerateForMonthlyTransportationResult(MonthlyTransportationFormSearch formSearch);
        Task<Dictionary<string, string>> GenerateForReservableList(ReceivableFilterModel receivableFilterModel, List<CheckBoxFilter> billingTypes);
        Task<Dictionary<string, string>> GenerateForDepositList(DepositListSearchModel depositListSearchModel, List<CheckBoxFilter> paymentMethods, List<CheckBoxFilter> billingTypes);
        Task<Dictionary<string, string>> GenerateForLeaveManagement(ScheduleManageForm scheduleManageForm);
        Task<Dictionary<string, string>> GenerateForStaffSchedule(List<GroupScheduleInfo> groupScheduleInfos);
        Task<Dictionary<string, string>> GenerateForStaffScheduleCompanyId(List<GroupScheduleInfo> groupScheduleInfos);
        Task<Dictionary<string, string>> GenerateForStaffScheduleCalendar(List<CalendarValueModel> calendarValueModels);
        Task<Dictionary<string, string>> GenerateForStaffScheduleDateComment(List<DateCommentCheckValueModel> dateCommentCheckValueModels);
        Task<Dictionary<string, string>> GenerateForBatchKobanInput(BatchKobanInputFilterModel batchKobanInputFilterModel);

        Task<Dictionary<string, string>> GenerateForBillPrint(BillPrintInput billPrint);
        Task<Dictionary<string, string>> GenerateForTransportationContract(TransportationContractFormData transportationContractFormData);
        Task<Dictionary<string, string>> GenerateForAnnualTransportationRecord(AnnualTransportationRecordFormSearch formSearch);
        Task<Dictionary<string, string>> GenerateForReceiptOutput(ReceiptOutputFormSeachModel formSearch);
        Task<Dictionary<string, string>> GenerateForFaresUpperAndLowerLimits(FaresUpperAndLowerLimitsFormSearch formSearch);
        Task<Dictionary<string, string>> GenerateForVehicleAvailabilityConfirmationMobile(FormSearch formSearch);
        Task<Dictionary<string, string>> GenerateForFareFeeCorrection(FareFeeCorrectionModel formSearch);
        Dictionary<string, string> GenerateForStaffSearchData(ConfigStaff staff);
        Dictionary<string, string> GenerateForDepositCoupon(DepositCouponFilter depositCouponFilter);

        Task<Dictionary<string, string>> GenerateBillingListFilter(BillingListFilter billingListFilter);

        Task<Dictionary<string, string>> GenerateForKashikiriSetting(RegulationSettingModel regulationSettingModel);
        Task<Dictionary<string, string>> GenerateForVehicleDailyReport(VehicleDailyReportSearchParam vehicleDailyReportSearchParam);

    }
    public class GenerateFilterValueDictionary : IGenerateFilterValueDictionary
    {
        public Task<Dictionary<string, string>> GenerateForBillCheckListFormData(BillsCheckListFormData billFormData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(BillsCheckListFormData);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(billFormData, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;
                switch (fieldName)
                {
                    case nameof(billFormData.BillPeriodFrom):
                    case nameof(billFormData.BillPeriodTo):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.BillOffice):
                        if (value != null)
                        {
                            BillOfficeData data = (BillOfficeData)value;
                            JoInput = data.EigyoCd;
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.GyosyaTokuiSakiFrom):
                    case nameof(billFormData.GyosyaTokuiSakiTo):
                        if (value != null)
                        {
                            CustomerComponentGyosyaData data = (CustomerComponentGyosyaData)value;
                            JoInput = data.GyosyaCdSeq.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.TokiskTokuiSakiFrom):
                    case nameof(billFormData.TokiskTokuiSakiTo):
                        if (value != null)
                        {
                            CustomerComponentTokiskData data = (CustomerComponentTokiskData)value;
                            JoInput = data.TokuiSeq.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.TokiStTokuiSakiFrom):
                    case nameof(billFormData.TokiStTokuiSakiTo):
                        if (value != null)
                        {
                            CustomerComponentTokiStData data = (CustomerComponentTokiStData)value;
                            JoInput = data.SitenCdSeq.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.BillAdress):
                        if (value != null)
                        {
                            BillAddress data = (BillAddress)value;
                            JoInput = $"{data.GyoSyaCd:000}{data.TokuiCd:0000}{data.SitenCd:0000}";
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.StartReceiptNumber):
                    case nameof(billFormData.EndReceiptNumber):

                        JoInput = value != null ? $"{long.Parse(value.ToString()):0000000000}" : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.YoyakuFrom):
                    case nameof(billFormData.YoyakuTo):
                        if (value != null)
                        {
                            ReservationClassComponentData data = (ReservationClassComponentData)value;
                            JoInput = data.YoyaKbnSeq.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.StartBillClassification):
                    case nameof(billFormData.EndBillClassification):
                        if (value != null)
                        {
                            InvoiceType data = (InvoiceType)value;
                            JoInput = data.CodeKbnSeq.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.BillIssuedClassification):
                        if (value != null)
                        {
                            ComboboxFixField data = (ComboboxFixField)value;
                            JoInput = data.IdValue.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.BillTypeOrder):
                        if (value != null)
                        {
                            ComboboxFixField data = (ComboboxFixField)value;
                            JoInput = data.IdValue.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.itemFare):
                    case nameof(billFormData.itemIncidental):
                    case nameof(billFormData.itemTollFee):
                    case nameof(billFormData.itemArrangementFee):
                    case nameof(billFormData.itemGuideFee):
                    case nameof(billFormData.itemShippingCharge):
                    case nameof(billFormData.itemCancellationCharge):
                        if (value != null)
                        {
                            bool data = (bool)value;
                            JoInput = data ? "1" : "0";
                        }
                        else JoInput = "0";
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.OutputType):
                        if (value != null)
                        {
                            OutputReportType data = (OutputReportType)value;
                            JoInput = data.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.PageSize):
                    case nameof(billFormData.GroupType):
                    case nameof(billFormData.ActiveHeaderOption):
                    case nameof(billFormData.DelimiterType):
                        if (value != null)
                        {
                            ComboboxFixField data = (ComboboxFixField)value;
                            JoInput = data.IdValue.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.ActiveV):
                        if (value != null)
                        {
                            JoInput = value.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.lstActiveTypeTotal):
                        if (value != null)
                        {
                            List<int> data = (List<int>)value;
                            JoInput = String.Join(",", data.ToArray());
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(billFormData.typePrint):
                        if (value != null)
                        {
                            OutputReportType data = (OutputReportType)value;
                            JoInput = data.ToString();
                        }
                        else JoInput = string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    default:
                        break;
                }
            }
            return Task.FromResult(result);
        }

        public async Task<Dictionary<string, string>> GenerateForHyperFormData(HyperFormData hyperFormData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(HyperFormData);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(hyperFormData, null);

                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;
                if (fieldName == nameof(HyperFormData.HaishaBiFrom) || fieldName == nameof(HyperFormData.HaishaBiTo) || fieldName == nameof(HyperFormData.TochakuBiFrom) || fieldName == nameof(HyperFormData.TochakuBiTo) || fieldName == nameof(HyperFormData.YoyakuBiFrom) || fieldName == nameof(HyperFormData.YoyakuBiTo))
                {
                    JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                }
                if (fieldName == nameof(HyperFormData.UketsukeBangoFrom) || fieldName == nameof(HyperFormData.UketsukeBangoTo))
                {
                    JoInput = value != null ? (string)value : string.Empty;
                }
                if (fieldName == nameof(HyperFormData.YoyakuFrom) || fieldName == nameof(HyperFormData.YoyakuTo))
                {
                    if (value != null)
                    {
                        ReservationClassComponentData data = (ReservationClassComponentData)value;
                        JoInput = data.YoyaKbnSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.EigyoTantoShaFrom) || fieldName == nameof(HyperFormData.EigyoTantoShaTo) || fieldName == nameof(HyperFormData.NyuryokuTantoShaFrom) || fieldName == nameof(HyperFormData.NyuryokuTantoShaTo))
                {
                    if (value != null)
                    {
                        StaffsData data = (StaffsData)value;
                        JoInput = $"{data.SyainCd:0000000000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.UketsukeEigyoJoFrom) || fieldName == nameof(HyperFormData.UketsukeEigyoJoTo))
                {
                    if (value != null)
                    {
                        SaleBranchData data = (SaleBranchData)value;
                        JoInput = data.EigyoCd.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.GyosyaTokuiSakiFrom) || fieldName == nameof(HyperFormData.GyosyaTokuiSakiTo) || fieldName == nameof(HyperFormData.GyosyaShiireSakiFrom) || fieldName == nameof(HyperFormData.GyosyaShiireSakiTo))
                {
                    if (value != null)
                    {
                        CustomerComponentGyosyaData data = (CustomerComponentGyosyaData)value;
                        JoInput = data.GyosyaCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.TokiskTokuiSakiFrom) || fieldName == nameof(HyperFormData.TokiskTokuiSakiTo) || fieldName == nameof(HyperFormData.TokiskShiireSakiFrom) || fieldName == nameof(HyperFormData.TokiskShiireSakiTo))
                {
                    if (value != null)
                    {
                        CustomerComponentTokiskData data = (CustomerComponentTokiskData)value;
                        JoInput = data.TokuiSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.TokiStShiireSakiFrom) || fieldName == nameof(HyperFormData.TokiStShiireSakiTo) || fieldName == nameof(HyperFormData.TokiStTokuiSakiTo) || fieldName == nameof(HyperFormData.TokiStTokuiSakiFrom))
                {
                    if (value != null)
                    {
                        CustomerComponentTokiStData data = (CustomerComponentTokiStData)value;
                        JoInput = data.SitenCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.DantaiNm) || fieldName == nameof(HyperFormData.ShashuTankaFrom) || fieldName == nameof(HyperFormData.ShashuTankaTo) || fieldName == nameof(HyperFormData.ActiveV) || fieldName == nameof(HyperFormData.dateType))
                {
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(HyperFormData.DantaiKbnFrom) || fieldName == nameof(HyperFormData.DantaiKbnTo))
                {
                    if (value != null)
                    {
                        CodeTypeData data = (CodeTypeData)value;
                        JoInput = data.CodeKbn;
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.KyakuDaneKbnFrom) || fieldName == nameof(HyperFormData.KyakuDaneKbnTo))
                {
                    if (value != null)
                    {
                        CustomerClassification data = (CustomerClassification)value;
                        JoInput = data.JyoKyakuCd.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.YukiSakiFrom) || fieldName == nameof(HyperFormData.YukiSakiTo) || fieldName == nameof(HyperFormData.HasseiChiFrom) || fieldName == nameof(HyperFormData.HasseiChiTo) || fieldName == nameof(HyperFormData.AreaFrom) || fieldName == nameof(HyperFormData.AreaTo))
                {
                    if (value != null)
                    {
                        LoadLocation data = (LoadLocation)value;
                        JoInput = $"{data.BasyoKenCdSeq:00}-{data.BasyoMapCdSeq:0000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.HaishaChiFrom) || fieldName == nameof(HyperFormData.HaishaChiTo))
                {
                    if (value != null)
                    {
                        LoadDispatchArea data = (LoadDispatchArea)value;
                        JoInput = $"{data.BunruiCdSeq:00}-{data.HaiScdSeq:0000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.ShashuFrom) || fieldName == nameof(HyperFormData.ShashuTo))
                {
                    if (value != null)
                    {
                        BusTypesData data = (BusTypesData)value;
                        JoInput = $"{data.SyaSyuCdSeq:000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.UketsukeJokenFrom) || fieldName == nameof(HyperFormData.UketsukeJokenTo))
                {
                    if (value != null)
                    {
                        VpmCodeKb data = (VpmCodeKb)value;
                        JoInput = data.CodeKbn;
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.OutputType))
                {
                    if (value != null)
                    {
                        OutputReportType data = (OutputReportType)value;
                        JoInput = data.ToString("D");
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(HyperFormData.PageSize) || fieldName == nameof(HyperFormData.ActiveHeaderOption) || fieldName == nameof(HyperFormData.GroupType) || fieldName == nameof(HyperFormData.DelimiterType)
                    || fieldName == nameof(HyperFormData.MaxMinSetting) || fieldName == nameof(HyperFormData.ReservationStatus))
                {
                    if (value != null)
                    {
                        ComboboxFixField data = (ComboboxFixField)value;
                        JoInput = data.IdValue.ToString();
                    }
                    else JoInput = string.Empty;
                }

                result.Add(fieldName, JoInput);
            }

            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForMonthlyTransportationResult(MonthlyTransportationFormSearch formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(MonthlyTransportationFormSearch);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                if (fieldName == nameof(MonthlyTransportationFormSearch.EigyoFrom) || fieldName == nameof(MonthlyTransportationFormSearch.EigyoTo))
                {
                    EigyoItem data = (EigyoItem)value;
                    jsInput = (value != null) ? data.EigyoCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(MonthlyTransportationFormSearch.ProcessingDate) && !string.IsNullOrEmpty(value.ToString()))
                {
                    jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(MonthlyTransportationFormSearch.ShippingFrom) || fieldName == nameof(MonthlyTransportationFormSearch.ShippingTo))
                {
                    ShippingItem data = (ShippingItem)value;
                    jsInput = (value != null) ? data.CodeKbnSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(MonthlyTransportationFormSearch.Company) && value != null)
                {
                    CompanyItem data = (CompanyItem)value;
                    jsInput = (value != null) ? data.CompanyCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(MonthlyTransportationFormSearch.OutputInstructionMode) && value != null)
                {
                    jsInput = value.ToString();
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateBillingListFilter(BillingListFilter billingListFilter)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(BillingListFilter).GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                object propValue = prop.GetValue(billingListFilter, null);
                switch (propName)
                {
                    case nameof(billingListFilter.CloseDate):
                    case nameof(billingListFilter.ActiveV):
                    case nameof(billingListFilter.StartReceiptNumber):
                    case nameof(billingListFilter.EndReceiptNumber):
                        result.Add(prop.Name, propValue == null ? string.Empty : propValue.ToString());
                        break;
                    case nameof(billingListFilter.BillTypes):
                        var billTypes = propValue == null ? new List<int>() : (List<int>)propValue;
                        result.Add(prop.Name, string.Join(",", billTypes));
                        break;
                    case nameof(billingListFilter.BillOffice):
                        var billOffice = (BillOfficeData)propValue;
                        result.Add(prop.Name, billOffice == null ? string.Empty : billOffice.EigyoCd);
                        break;
                    case nameof(billingListFilter.startCustomerComponentGyosyaData):
                    case nameof(billingListFilter.endCustomerComponentGyosyaData):
                        var gyosya = (CustomerComponentGyosyaData)propValue;
                        result.Add(prop.Name, gyosya == null ? string.Empty : gyosya.GyosyaCdSeq.ToString());
                        break;
                    case nameof(billingListFilter.startCustomerComponentTokiskData):
                    case nameof(billingListFilter.endCustomerComponentTokiskData):
                        var tokisk = (CustomerComponentTokiskData)propValue;
                        result.Add(prop.Name, tokisk == null ? string.Empty : tokisk.TokuiSeq.ToString());
                        break;
                    case nameof(billingListFilter.startCustomerComponentTokiStData):
                    case nameof(billingListFilter.endCustomerComponentTokiStData):
                        var tokist = (CustomerComponentTokiStData)propValue;
                        result.Add(prop.Name, tokist == null ? string.Empty : (tokist.TokuiSeq.ToString() + tokist.SitenCdSeq.ToString()));
                        break;
                    case nameof(billingListFilter.StartReservationClassification):
                    case nameof(billingListFilter.EndReservationClassification):
                        var reservationClassification = (ReservationClassComponentData)propValue;
                        result.Add(prop.Name, reservationClassification == null ? string.Empty : reservationClassification.YoyaKbnSeq.ToString());
                        break;
                    case nameof(billingListFilter.BillDate):
                        var time = (DateTime?)propValue;
                        result.Add(prop.Name, time == null ? string.Empty : ((DateTime)time).ToString("yyyyMMdd"));
                        break;
                    case nameof(billingListFilter.StartBillClassification):
                    case nameof(billingListFilter.EndBillClassification):
                        var invoiceType = (InvoiceType)propValue;
                        result.Add(prop.Name, invoiceType == null ? string.Empty : invoiceType.CodeKbn);
                        break;
                    case nameof(billingListFilter.BillIssuedClassification):
                        var billIssuedClassification = (ComboboxFixField)propValue;
                        result.Add(prop.Name, billIssuedClassification == null ? string.Empty : billIssuedClassification.IdValue.ToString());
                        break;
                    case nameof(billingListFilter.BillTypeOrder):
                    case nameof(billingListFilter.PageSize):
                    case nameof(billingListFilter.ActiveHeaderOption):
                    case nameof(billingListFilter.GroupType):
                    case nameof(billingListFilter.DelimiterType):
                        var billTypeOrder = (ComboboxFixField)propValue;
                        result.Add(prop.Name, billTypeOrder == null ? string.Empty : billTypeOrder.StringValue);
                        break;
                    case nameof(billingListFilter.TransferAmountOutputClassification):
                        var transferAmountOutputClassification = (TransferAmountOutputClassification)propValue;
                        result.Add(prop.Name, transferAmountOutputClassification == null ? string.Empty : transferAmountOutputClassification.Code.ToString());
                        break;
                    case nameof(billingListFilter.itemFare):
                    case nameof(billingListFilter.itemIncidental):
                    case nameof(billingListFilter.itemTollFee):
                    case nameof(billingListFilter.itemArrangementFee):
                    case nameof(billingListFilter.itemGuideFee):
                    case nameof(billingListFilter.itemLoaded):
                    case nameof(billingListFilter.itemCancellationCharge):
                    case nameof(billingListFilter.isListMode):
                        result.Add(prop.Name, (bool)propValue ? "1" : "0");
                        break;
                    case nameof(billingListFilter.OutputType):
                        var outputType = (OutputReportType)propValue;
                        result.Add(prop.Name, outputType.ToString("D"));
                        break;
                }
            }

            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForTransportationContract(TransportationContractFormData transportationContractFormData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(TransportationContractFormData);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(transportationContractFormData, null);

                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;
                if (fieldName == nameof(TransportationContractFormData.PrintMode)
                    || fieldName == nameof(TransportationContractFormData.OutputUnit)
                    || fieldName == nameof(TransportationContractFormData.DateTypeContract)
                    || fieldName == nameof(TransportationContractFormData.OutputSelection)
                    || fieldName == nameof(TransportationContractFormData.YearlyContract))
                {
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.DateFrom) || fieldName == nameof(TransportationContractFormData.DateTo))
                {
                    JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.UketsukeEigyoJo))
                {
                    if (value != null)
                    {
                        SaleBranchData data = (SaleBranchData)value;
                        JoInput = data.EigyoCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.EigyoTantoSha)
                    || fieldName == nameof(TransportationContractFormData.InpSyainCd))
                {
                    if (value != null)
                    {
                        StaffsData data = (StaffsData)value;
                        JoInput = data.SyainCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.UkeNumber))
                {
                    if (value != null && value != string.Empty)
                    {
                        JoInput = $"{Int32.Parse(value.ToString()):0000000000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.Gyosya))
                {
                    if (value != null)
                    {
                        CustomerComponentGyosyaData data = (CustomerComponentGyosyaData)value;
                        JoInput = data.GyosyaCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.TokuiSaki))
                {
                    if (value != null)
                    {
                        CustomerComponentTokiskData data = (CustomerComponentTokiskData)value;
                        JoInput = data.TokuiSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.TokuiSiten))
                {
                    if (value != null)
                    {
                        CustomerComponentTokiStData data = (CustomerComponentTokiStData)value;
                        JoInput = data.SitenCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(TransportationContractFormData.YoyakuKbnList))
                {
                    if (value != null)
                    {
                        List<BookingTypeData> datas = (List<BookingTypeData>)value;
                        JoInput = string.Empty;
                        foreach (BookingTypeData data in datas)
                        {
                            JoInput += data.YoyaKbnSeq + ",";
                        }
                    }
                    else
                    {
                        JoInput = string.Empty;
                    }
                }
                if (fieldName == nameof(TransportationContractFormData.IsUpdateExportDate))
                {
                    JoInput = value != null ? value.ToString().ToLower() : string.Empty;
                }
                result.Add(fieldName, JoInput);
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForAdvancedPayment(AdvancePaymentDetailsSearchParam searchParams)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(AdvancePaymentDetailsSearchParam);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(searchParams, null);

                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;
                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.OutputSetting))
                {
                    JoInput = value != null ? value.ToString() : string.Empty;
                }

                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.PaperSize))
                {
                    if (value != null)
                    {
                        PaperSizeDropdown data = (PaperSizeDropdown)value;
                        JoInput = data.Value.ToString();
                    }
                    else JoInput = string.Empty;
                }

                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.ReceptionNumber))
                {
                    JoInput = (value != null && value != string.Empty) ? $"{Int32.Parse(value.ToString()):0000000000}" : string.Empty;
                }

                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.ScheduleYmdStart) || fieldName == nameof(AdvancePaymentDetailsSearchParam.ScheduleYmdEnd))
                {
                    JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                }

                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.AddressSpectify))
                {
                    if (value != null)
                    {
                        PaymentSearchDropdown data = (PaymentSearchDropdown)value;
                        JoInput = data.Value.ToString();
                    }
                    else JoInput = string.Empty;
                }

                if (fieldName == nameof(AdvancePaymentDetailsSearchParam.CustomerModelFrom) || fieldName == nameof(AdvancePaymentDetailsSearchParam.CustomerModelTo))
                {
                    CustomerModel data = (CustomerModel)value;
                    JoInput = (value != null) ? $"{data?.SelectedGyosya?.GyosyaCdSeq.ToString() ?? "-1"},{data?.SelectedTokisk?.TokuiSeq.ToString() ?? "-1"},{data?.SelectedTokiSt?.SitenCdSeq.ToString() ?? "-1"}" : "-1,-1,-1";
                }
                result.Add(fieldName, JoInput);
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForBillPrint(BillPrintInput billPrint)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(BillPrintInput).GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                object propValue = prop.GetValue(billPrint, null);
                switch (propName)
                {
                    case nameof(billPrint.StartRsrCatDropDown):
                    case nameof(billPrint.EndRsrCatDropDown):
                        result.Add(prop.Name, (ReservationClassComponentData)propValue == null ? string.Empty : ((ReservationClassComponentData)propValue).YoyaKbnSeq.ToString());
                        break;
                    case nameof(billPrint.startCustomerComponentGyosyaData):
                    case nameof(billPrint.endCustomerComponentGyosyaData):
                        var gyosya = (CustomerComponentGyosyaData)propValue;
                        result.Add(prop.Name, gyosya == null ? string.Empty : gyosya.GyosyaCdSeq.ToString());
                        break;
                    case nameof(billPrint.startCustomerComponentTokiskData):
                    case nameof(billPrint.endCustomerComponentTokiskData):
                        var tokisk = (CustomerComponentTokiskData)propValue;
                        result.Add(prop.Name, tokisk == null ? string.Empty : tokisk.TokuiSeq.ToString());
                        break;
                    case nameof(billPrint.startCustomerComponentTokiStData):
                    case nameof(billPrint.endCustomerComponentTokiStData):
                        var tokist = (CustomerComponentTokiStData)propValue;
                        result.Add(prop.Name, tokist == null ? string.Empty : (tokist.TokuiSeq.ToString() + tokist.SitenCdSeq.ToString()));
                        break;
                    case nameof(billPrint.BillingOfficeDropDown):
                    case nameof(billPrint.BillingAddressDropDown):
                    case nameof(billPrint.HandlingCharPrtDropDown):
                        result.Add(prop.Name, (DropDown)propValue == null ? string.Empty : ((DropDown)propValue).Code.ToString());
                        break;
                    case nameof(billPrint.StartRcpNum):
                    case nameof(billPrint.EndRcpNum):
                    case nameof(billPrint.ClosingDate):
                    case nameof(billPrint.BillingType):
                        result.Add(prop.Name, propValue == null ? string.Empty : propValue.ToString());
                        break;
                    case nameof(billPrint.FareBilTyp):
                    case nameof(billPrint.FutaiBilTyp):
                    case nameof(billPrint.TollFeeBilTyp):
                    case nameof(billPrint.ArrangementFeeBilTyp):
                    case nameof(billPrint.GuideFeeBilTyp):
                    case nameof(billPrint.LoadedItemBilTyp):
                    case nameof(billPrint.CancelFeeBilTyp):
                        result.Add(prop.Name, (byte)propValue == (byte)2 ? "2" : "1");
                        break;
                }
            }

            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForAnnualTransportationRecord(AnnualTransportationRecordFormSearch formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(AnnualTransportationRecordFormSearch);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                if (fieldName == nameof(AnnualTransportationRecordFormSearch.EigyoFrom) || fieldName == nameof(AnnualTransportationRecordFormSearch.EigyoTo))
                {
                    EigyoItem data = (EigyoItem)value;
                    jsInput = (value != null) ? data.EigyoCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if ((fieldName == nameof(AnnualTransportationRecordFormSearch.ProcessingDateFrom) || fieldName == nameof(AnnualTransportationRecordFormSearch.ProcessingDateTo)) && !string.IsNullOrEmpty(value.ToString()))
                {
                    jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(AnnualTransportationRecordFormSearch.ShippingFrom) || fieldName == nameof(AnnualTransportationRecordFormSearch.ShippingTo))
                {
                    ShippingItem data = (ShippingItem)value;
                    jsInput = (value != null) ? data.CodeKbnSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(AnnualTransportationRecordFormSearch.Company) && value != null)
                {
                    CompanyItem data = (CompanyItem)value;
                    jsInput = data.CompanyCdSeq.ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(AnnualTransportationRecordFormSearch.OutputInstructionMode) && value != null)
                {
                    jsInput = value.ToString();
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateInvoiceIssueFilter(InvoiceIssueFilter invoiceIssueFilter)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(InvoiceIssueFilter).GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                object propValue = prop.GetValue(invoiceIssueFilter, null);
                switch (propName)
                {
                    case nameof(invoiceIssueFilter.BillOutputSeq):
                    case nameof(invoiceIssueFilter.BillSerialNumber):
                    case nameof(invoiceIssueFilter.ActiveV):
                        result.Add(prop.Name, propValue == null ? string.Empty : propValue.ToString());
                        break;
                    //case nameof(invoiceIssueFilter.StartBillAddress):
                    //case nameof(invoiceIssueFilter.EndBillAddress):
                    //    var billAddress = (LoadCustomerList)propValue;
                    //    result.Add(prop.Name, propValue == null ? string.Empty : billAddress.GyoSyaCd.ToString("D3") + billAddress.TokuiCd.ToString("D4") + billAddress.SitenCd.ToString("D4"));
                    //    break;
                    case nameof(invoiceIssueFilter.StartBillIssuedDate):
                    case nameof(invoiceIssueFilter.EndBillIssuedDate):
                        var time = (DateTime?)propValue;
                        result.Add(prop.Name, propValue == null ? string.Empty : ((DateTime)time).ToString("yyyyMMdd"));
                        break;
                    case nameof(invoiceIssueFilter.StartBillAddressString):
                    case nameof(invoiceIssueFilter.EndBillAddressString):
                    case nameof(invoiceIssueFilter.StartBillIssuedDateString):
                    case nameof(invoiceIssueFilter.EndBillIssuedDateString):
                        break;
                }
            }

            result.Add(nameof(invoiceIssueFilter.SelectedGyosyaFrom), invoiceIssueFilter.SelectedGyosyaFrom?.GyosyaCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(invoiceIssueFilter.SelectedTokiskFrom), invoiceIssueFilter.SelectedTokiskFrom?.TokuiSeq.ToString() ?? string.Empty);
            result.Add(nameof(invoiceIssueFilter.SelectedTokiStFrom), invoiceIssueFilter.SelectedTokiStFrom?.SitenCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(invoiceIssueFilter.SelectedGyosyaTo), invoiceIssueFilter.SelectedGyosyaTo?.GyosyaCdSeq.ToString() ?? string.Empty);
            result.Add(nameof(invoiceIssueFilter.SelectedTokiskTo), invoiceIssueFilter.SelectedTokiskTo?.TokuiSeq.ToString() ?? string.Empty);
            result.Add(nameof(invoiceIssueFilter.SelectedTokiStTo), invoiceIssueFilter.SelectedTokiStTo?.SitenCdSeq.ToString() ?? string.Empty);

            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForReceiptOutput(ReceiptOutputFormSeachModel formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(ReceiptOutputFormSeachModel);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                if (fieldName == nameof(ReceiptOutputFormSeachModel.BillOffice))
                {
                    BillOfficeReceipt data = (BillOfficeReceipt)value;
                    jsInput = (value != null) ? data.EigyoCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if ((fieldName == nameof(ReceiptOutputFormSeachModel.StaInvoicingDate) || fieldName == nameof(ReceiptOutputFormSeachModel.EndInvoicingDate) || fieldName == nameof(ReceiptOutputFormSeachModel.InvoiceYearMonth)) && !string.IsNullOrEmpty(value?.ToString()))
                {
                    jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                    result.Add(fieldName, jsInput);
                }
                //else if (fieldName == nameof(ReceiptOutputFormSeachModel.BillAddressFrom) || fieldName == nameof(ReceiptOutputFormSeachModel.BillAddressTo))
                //{
                //    BillAddressReceiptFromTo data = (BillAddressReceiptFromTo)value;
                //    jsInput = (value != null) ? $"{data.TokuiCdSeq.ToString() ?? "0"},{data.SitenCdSeq.ToString() ?? "0"}" : "0,0";
                //    result.Add(fieldName, jsInput);
                //}
                else if (fieldName == nameof(ReceiptOutputFormSeachModel.CustomerModelFrom) || fieldName == nameof(ReceiptOutputFormSeachModel.CustomerModelTo))
                {
                    CustomerModel data = (CustomerModel)value;
                    jsInput = (value != null) ? $"{data?.SelectedGyosya?.GyosyaCdSeq.ToString() ?? "-1"},{data?.SelectedTokisk?.TokuiSeq.ToString() ?? "-1"},{data?.SelectedTokiSt?.SitenCdSeq.ToString() ?? "-1"}" : "-1,-1,-1";
                    result.Add(fieldName, jsInput);
                }
                else if ((fieldName == nameof(ReceiptOutputFormSeachModel.StaInvoiceOutNum) || fieldName == nameof(ReceiptOutputFormSeachModel.StaInvoiceSerNum)
                        || fieldName == nameof(ReceiptOutputFormSeachModel.EndInvoiceOutNum) || fieldName == nameof(ReceiptOutputFormSeachModel.EndInvoiceSerNum)) && value != null)
                    result.Add(fieldName, value?.ToString());
                else if (fieldName == nameof(ReceiptOutputFormSeachModel.ActiveV) && value != null)
                {
                    jsInput = value.ToString();
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForFaresUpperAndLowerLimits(FaresUpperAndLowerLimitsFormSearch formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(FaresUpperAndLowerLimitsFormSearch);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;

                if ((fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.OutputStartDate) || fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.OutputEndDate)) && !string.IsNullOrEmpty(value?.ToString()))
                {
                    jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.SaleOffice))
                {
                    SaleOffice data = (SaleOffice)value;
                    jsInput = (value != null) ? data?.EigyoCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.OutputStartDate) || fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.OutputEndDate))
                {
                    if (!string.IsNullOrEmpty(value?.ToString()))
                    {
                        jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                        result.Add(fieldName, jsInput);
                    }
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.SalePersonInCharge))
                {
                    SalePersonInCharge data = (SalePersonInCharge)value;
                    jsInput = (value != null) ? $"{data?.SyainCdSeq.ToString() ?? "0"},{data?.EigyoCdSeq.ToString() ?? "0"}" : "0,0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.DateClassification))
                {
                    var data = (DateClassification)Enum.Parse(typeof(DateClassification), value.ToString());
                    jsInput = ((int)data).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.ChooseCause))
                {
                    var data = (ChooseCause)Enum.Parse(typeof(ChooseCause), value.ToString());
                    jsInput = ((int)data).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.OutputSetting))
                {
                    var data = (OutputInstruction)Enum.Parse(typeof(OutputInstruction), value.ToString());
                    jsInput = ((int)data).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.ItemOutOfRange))
                {
                    var data = (ItemOutOfRange)Enum.Parse(typeof(ItemOutOfRange), value.ToString());
                    jsInput = ((int)data).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveV))
                {
                    jsInput = int.Parse(value.ToString()) != 0 ? value.ToString() : ((int)ViewMode.Medium).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FaresUpperAndLowerLimitsFormSearch.ActiveL))
                {
                    jsInput = int.Parse(value.ToString()) != 0 ? value.ToString() : ((int)LayoutMode.Save).ToString();
                    result.Add(fieldName, jsInput);
                }
                else
                {
                    jsInput = value != null ? value?.ToString() : string.Empty;
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForVehicleAvailabilityConfirmationMobile(FormSearch formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(FormSearch);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                if (fieldName == nameof(FormSearch.BusType))
                {
                    BusType data = (BusType)value;
                    jsInput = (value != null) ? data.SyaSyuCdSeq.ToString() : "0";
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FormSearch.SelectedDate) && !string.IsNullOrEmpty(value?.ToString()))
                {
                    jsInput = value.ToString().Substring(0, 10).Replace("/", string.Empty);
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForFareFeeCorrection(FareFeeCorrectionModel formSearch)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(FareFeeCorrectionModel);
            var props = new List<PropertyInfo>(type.GetProperties());
            var jsInput = string.Empty;

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(formSearch, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;

                if (fieldName == nameof(FareFeeCorrectionModel.ActiveV))
                {
                    jsInput = int.Parse(value.ToString()) != 0 ? value.ToString() : ((int)ViewMode.Medium).ToString();
                    result.Add(fieldName, jsInput);
                }
                else if (fieldName == nameof(FareFeeCorrectionModel.ActiveL))
                {
                    jsInput = int.Parse(value.ToString()) != 0 ? value.ToString() : ((int)LayoutMode.Save).ToString();
                    result.Add(fieldName, jsInput);
                }
            }
            return result;
        }
        public Dictionary<string, string> GenerateForStaffSearchData(ConfigStaff staff)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            result.Add(nameof(ConfigStaff.Group), staff.Group.ToString());
            result.Add(nameof(ConfigStaff.Time), staff.Time.ToString());
            result.Add(nameof(ConfigStaff.View), staff.View.ToString());
            result.Add(nameof(ConfigStaff.Sort), staff.Sort.ToString());
            result.Add(nameof(ConfigStaff.Display), staff.Display.ToString());
            result.Add(nameof(ConfigStaff.CrewSort), staff.CrewSort.ToString());
            result.Add(nameof(ConfigStaff.WorkSort), staff.WorkSort.ToString());
            result.Add(nameof(ConfigStaff.Display2), staff.Display2.ToString());

            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForReservableList(ReceivableFilterModel receivableFilterModel, List<CheckBoxFilter> billingTypes)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(ReceivableFilterModel);
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(receivableFilterModel, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                switch (fieldName)
                {
                    case nameof(ReceivableFilterModel.CompanyData):
                        CompanyData companyData = (CompanyData)value;
                        JoInput = value != null ? companyData.CompanyCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.StartSaleBranchList):
                        LoadSaleBranchList startSaleBranchList = (LoadSaleBranchList)value;
                        JoInput = value != null ? startSaleBranchList.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.EndSaleBranchList):
                        LoadSaleBranchList endSaleBranchList = (LoadSaleBranchList)value;
                        JoInput = value != null ? endSaleBranchList.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.SaleOfficeType):
                        SaleOfficeModel saleOfficeType = (SaleOfficeModel)value;
                        JoInput = value != null ? saleOfficeType.SaleOfficeKbn.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.StartPaymentDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.EndPaymentDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentGyosyaData):
                    case nameof(ReceivableFilterModel.endCustomerComponentGyosyaData):
                        var gyosya = (CustomerComponentGyosyaData)value;
                        result.Add(prop.Name, gyosya == null ? string.Empty : gyosya.GyosyaCdSeq.ToString());
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentTokiskData):
                    case nameof(ReceivableFilterModel.endCustomerComponentTokiskData):
                        var tokisk = (CustomerComponentTokiskData)value;
                        result.Add(prop.Name, tokisk == null ? string.Empty : tokisk.TokuiSeq.ToString());
                        break;
                    case nameof(ReceivableFilterModel.startCustomerComponentTokiStData):
                    case nameof(ReceivableFilterModel.endCustomerComponentTokiStData):
                        var tokist = (CustomerComponentTokiStData)value;
                        result.Add(prop.Name, tokist == null ? string.Empty : (tokist.TokuiSeq.ToString() + tokist.SitenCdSeq.ToString()));
                        break;
                    case nameof(ReceivableFilterModel.StartReceiptNumber):
                        JoInput = value != null ? value.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.EndReceiptNumber):
                        JoInput = value != null ? value.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.StartReservationClassification):
                    case nameof(ReceivableFilterModel.EndReservationClassification):
                        var reservationClassification = (ReservationClassComponentData)value;
                        result.Add(prop.Name, reservationClassification == null ? string.Empty : reservationClassification.YoyaKbnSeq.ToString());
                        break;
                    case nameof(ReceivableFilterModel.BillingType):
                        if (value == null)
                        {
                            JoInput = string.Empty;
                        }
                        else
                        {
                            JoInput = string.Join(',', billingTypes.Where(x => x.IsChecked).Select(x => x.Id).ToList());
                        }
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.UnpaidSelection):
                        ComboboxFixField unpaidSelection = (ComboboxFixField)value;
                        JoInput = value != null ? unpaidSelection.IdValue.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.PaymentDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ReceivableFilterModel.OutputType):
                        var outputType = (OutputReportType)value;
                        result.Add(prop.Name, outputType.ToString("D"));
                        break;
                    case nameof(ReceivableFilterModel.ReportPageSize):
                    case nameof(ReceivableFilterModel.ActiveHeaderOption):
                    case nameof(ReceivableFilterModel.GroupType):
                    case nameof(ReceivableFilterModel.DelimiterType):
                        var billTypeOrder = (ComboboxFixField)value;
                        result.Add(prop.Name, billTypeOrder == null ? string.Empty : billTypeOrder.StringValue);
                        break;
                    case nameof(ReceivableFilterModel.ReceivableReport):
                        result.Add(prop.Name, value.ToString());
                        break;
                }
            }
            return result;
        }
        public async Task<Dictionary<string, string>> GenerateForDepositList(DepositListSearchModel depositListSearchModel, List<CheckBoxFilter> paymentMethods, List<CheckBoxFilter> billingTypes)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(DepositListSearchModel);
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(depositListSearchModel, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                switch (fieldName)
                {
                    case nameof(DepositListSearchModel.CompanyData):
                        CompanyData companyData = (CompanyData)value;
                        JoInput = value != null ? companyData.CompanyCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.StartSaleBranchList):
                        LoadSaleBranchList startSaleBranchList = (LoadSaleBranchList)value;
                        JoInput = value != null ? startSaleBranchList.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.EndSaleBranchList):
                        LoadSaleBranchList endSaleBranchList = (LoadSaleBranchList)value;
                        JoInput = value != null ? endSaleBranchList.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.SaleOfficeType):
                        SaleOfficeModel saleOfficeType = (SaleOfficeModel)value;
                        JoInput = value != null ? saleOfficeType.SaleOfficeKbn.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.StartPaymentDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.EndPaymentDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.startCustomerComponentGyosyaData):
                    case nameof(DepositListSearchModel.endCustomerComponentGyosyaData):
                        var gyosya = (CustomerComponentGyosyaData)value;
                        result.Add(prop.Name, gyosya == null ? string.Empty : gyosya.GyosyaCdSeq.ToString());
                        break;
                    case nameof(DepositListSearchModel.startCustomerComponentTokiskData):
                    case nameof(DepositListSearchModel.endCustomerComponentTokiskData):
                        var tokisk = (CustomerComponentTokiskData)value;
                        result.Add(prop.Name, tokisk == null ? string.Empty : tokisk.TokuiSeq.ToString());
                        break;
                    case nameof(DepositListSearchModel.startCustomerComponentTokiStData):
                    case nameof(DepositListSearchModel.endCustomerComponentTokiStData):
                        var tokist = (CustomerComponentTokiStData)value;
                        result.Add(prop.Name, tokist == null ? string.Empty : (tokist.TokuiSeq.ToString() + tokist.SitenCdSeq.ToString()));
                        break;
                    case nameof(DepositListSearchModel.StartReceiptNumber):
                        JoInput = value != null ? value.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.EndReceiptNumber):
                        JoInput = value != null ? value.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.StartReservationClassification):
                    case nameof(DepositListSearchModel.EndReservationClassification):
                        var reservationClassification = (ReservationClassComponentData)value;
                        result.Add(prop.Name, reservationClassification == null ? string.Empty : reservationClassification.YoyaKbnSeq.ToString());
                        break;
                    case nameof(DepositListSearchModel.BillingType):
                        if (value == null)
                        {
                            JoInput = string.Empty;
                        }
                        else
                        {
                            JoInput = string.Join(',', billingTypes.Where(x => x.IsChecked).Select(x => x.Id).ToList());
                        }
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.StartTransferBank):
                        TransferBankModel startTransferBank = (TransferBankModel)value;
                        JoInput = value != null ? startTransferBank.Code : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.EndTransferBank):
                        TransferBankModel endTransferBank = (TransferBankModel)value;
                        JoInput = value != null ? endTransferBank.Code : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.PaymentMethod):
                        if (value == null)
                        {
                            JoInput = string.Empty;
                        }
                        else
                        {
                            JoInput = string.Join(',', paymentMethods.Where(x => x.IsChecked).Select(x => x.Id).ToList());
                        }
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(DepositListSearchModel.OutputType):
                        var outputType = (OutputReportType)value;
                        result.Add(prop.Name, outputType.ToString("D"));
                        break;
                    case nameof(DepositListSearchModel.PageSizeReport):
                    case nameof(DepositListSearchModel.ActiveHeaderOption):
                    case nameof(DepositListSearchModel.GroupType):
                    case nameof(DepositListSearchModel.DelimiterType):
                        var billTypeOrder = (ComboboxFixField)value;
                        result.Add(prop.Name, billTypeOrder == null ? string.Empty : billTypeOrder.StringValue);
                        break;
                    case nameof(DepositListSearchModel.DepositOutputTemplate):
                        var data = (DepositOutputClass)(value);
                        result.Add(prop.Name, data.Id.ToString());
                        break;
                }
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForLeaveManagement(ScheduleManageForm scheduleManageForm)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(ScheduleManageForm);
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(scheduleManageForm, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                switch (fieldName)
                {
                    case nameof(ScheduleManageForm.StartDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ScheduleManageForm.EndDate):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ScheduleManageForm.ApprovalStatus):
                        Status status = (Status)value;
                        JoInput = value != null ? status.status : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ScheduleManageForm.Staff):
                        Staffs staffs = (Staffs)value;
                        JoInput = value != null ? staffs.Seg.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ScheduleManageForm.Branch):
                        Branch branch = (Branch)value;
                        JoInput = value != null ? branch.Seg.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(ScheduleManageForm.CustomGroup):
                        CustomGroup customGroup = (CustomGroup)value;
                        JoInput = value != null ? customGroup.CusGrpSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                }
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForStaffSchedule(List<GroupScheduleInfo> groupScheduleInfos)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in groupScheduleInfos)
            {
                if (item.GroupName != new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name)
                {
                    result.Add(item.GroupId.ToString(), item.GroupName);
                }
            }

            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForStaffScheduleCompanyId(List<GroupScheduleInfo> groupScheduleInfos)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in groupScheduleInfos)
            {
                if (item.GroupName != new HassyaAllrightCloud.Domain.Dto.ClaimModel().Name)
                {
                    result.Add(item.GroupId.ToString(), item.CompanyId.ToString());
                }
            }

            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForBatchKobanInput(BatchKobanInputFilterModel batchKobanInputFilterModel)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(BatchKobanInputFilterModel);
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(batchKobanInputFilterModel, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                switch (fieldName)
                {
                    case nameof(BatchKobanInputFilterModel.KinmuYmd):
                        JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.Company):
                        CompanyData companay = (CompanyData)value;
                        JoInput = value != null ? companay.CompanyCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.EigyoStart):
                        LoadSaleBranchList eStart = (LoadSaleBranchList)value;
                        JoInput = value != null ? eStart.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.EigyoEnd):
                        LoadSaleBranchList eEnd = (LoadSaleBranchList)value;
                        JoInput = value != null ? eEnd.EigyoCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.SyainStart):
                        Staffs sStart = (Staffs)value;
                        JoInput = value != null ? sStart.SyainCd.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.SyainEnd):
                        Staffs sEnd = (Staffs)value;
                        JoInput = value != null ? sEnd.SyainCd.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.SyokumuStart):
                        TaskModel tStart = (TaskModel)value;
                        JoInput = value != null ? tStart.SyokumuCd.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.SyokumuEnd):
                        TaskModel tEnd = (TaskModel)value;
                        JoInput = value != null ? tEnd.SyokumuCd.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.SyuJun):
                        ComboboxFixField syujun = (ComboboxFixField)value;
                        JoInput = value != null ? syujun.IdValue.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(BatchKobanInputFilterModel.DisplayKbn):
                        ComboboxFixField display = (ComboboxFixField)value;
                        JoInput = value != null ? display.IdValue.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                }
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForStaffScheduleCalendar(List<CalendarValueModel> calendarValueModels)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in calendarValueModels)
            {
                result.Add(item.CalendarSeq.ToString(), item.Value ? "1" : "0");
            }

            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForStaffScheduleDateComment(List<DateCommentCheckValueModel> dateCommentCheckValueModels)
        {
            var result = new Dictionary<string, string>();

            foreach (var item in dateCommentCheckValueModels)
            {
                result.Add(item.Id.ToString(), item.Value ? "1" : "0");
            }

            return result;
        }
        public Dictionary<string, string> GenerateForDepositCoupon(DepositCouponFilter depositCouponFilter)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            IList<PropertyInfo> props = new List<PropertyInfo>(typeof(DepositCouponFilter).GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var propName = prop.Name;
                object propValue = prop.GetValue(depositCouponFilter, null);
                switch (propName)
                {
                    case nameof(depositCouponFilter.BillPeriodFrom):
                    case nameof(depositCouponFilter.BillPeriodTo):
                        var time = (DateTime?)propValue;
                        result.Add(prop.Name, propValue == null ? string.Empty : ((DateTime)time).ToString("yyyyMMdd"));
                        break;
                    case nameof(depositCouponFilter.ActiveV):
                    case nameof(depositCouponFilter.DepositOutputClassification):
                        result.Add(prop.Name, propValue == null ? string.Empty : propValue.ToString());
                        break;
                    case nameof(depositCouponFilter.BillTypes):
                        var billTypes = propValue == null ? new List<int>() : (List<int>)propValue;
                        result.Add(prop.Name, string.Join(",", billTypes));
                        break;
                    case nameof(depositCouponFilter.BillOffice):
                        var billOffice = (BillOfficeData)propValue;
                        result.Add(prop.Name, billOffice == null ? string.Empty : billOffice.EigyoCd);
                        break;
                    case nameof(depositCouponFilter.startCustomerComponentGyosyaData):
                    case nameof(depositCouponFilter.endCustomerComponentGyosyaData):
                        var gyosya = (CustomerComponentGyosyaData)propValue;
                        result.Add(prop.Name, gyosya == null ? string.Empty : gyosya.GyosyaCdSeq.ToString());
                        break;
                    case nameof(depositCouponFilter.startCustomerComponentTokiskData):
                    case nameof(depositCouponFilter.endCustomerComponentTokiskData):
                        var tokisk = (CustomerComponentTokiskData)propValue;
                        result.Add(prop.Name, tokisk == null ? string.Empty : tokisk.TokuiSeq.ToString());
                        break;
                    case nameof(depositCouponFilter.startCustomerComponentTokiStData):
                    case nameof(depositCouponFilter.endCustomerComponentTokiStData):
                        var tokist = (CustomerComponentTokiStData)propValue;
                        result.Add(prop.Name, tokist == null ? string.Empty : (tokist.TokuiSeq.ToString() + tokist.SitenCdSeq.ToString()));
                        break;
                    case nameof(depositCouponFilter.StartReservationClassification):
                    case nameof(depositCouponFilter.EndReservationClassification):
                        var reservationClassification = (ReservationClassComponentData)propValue;
                        result.Add(prop.Name, reservationClassification == null ? string.Empty : reservationClassification.YoyaKbnSeq.ToString());
                        break;
                    case nameof(depositCouponFilter.itemFare):
                    case nameof(depositCouponFilter.itemIncidental):
                    case nameof(depositCouponFilter.itemTollFee):
                    case nameof(depositCouponFilter.itemArrangementFee):
                    case nameof(depositCouponFilter.itemGuideFee):
                    case nameof(depositCouponFilter.itemLoaded):
                    case nameof(depositCouponFilter.itemCancellationCharge):
                        result.Add(prop.Name, (bool)propValue ? "1" : "0");
                        break;
                }
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForKashikiriSetting(RegulationSettingModel regulationSettingModel)
        {
            var result = new Dictionary<string, string>();
            var type = typeof(RegulationSettingModel);
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                var value = prop.GetValue(regulationSettingModel, null);
                var fieldName = prop.Name;
                var JoInput = string.Empty;
                switch (fieldName)
                {
                    case nameof(RegulationSettingModel.CompanyFrom):
                        CompanyListItem companyFrom = (CompanyListItem)value;
                        JoInput = value != null ? companyFrom.CompanyCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                    case nameof(RegulationSettingModel.CompanyTo):
                        CompanyListItem companyTo = (CompanyListItem)value;
                        JoInput = value != null ? companyTo.CompanyCdSeq.ToString() : string.Empty;
                        result.Add(fieldName, JoInput);
                        break;
                }
            }
            return result;
        }

        public async Task<Dictionary<string, string>> GenerateForVehicleDailyReport(VehicleDailyReportSearchParam vehicleDailyReportSearchParam)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var type = typeof(VehicleDailyReportSearchParam);
            IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(vehicleDailyReportSearchParam, null);

                var fieldName = prop.Name;
                var JoInput = string.Empty;
                var value = propValue;
                if (fieldName == nameof(VehicleDailyReportSearchParam.OutputSetting))
                {
                    JoInput = value != null ? value.ToString() : string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.OutputWithHeader)
                    || fieldName == nameof(VehicleDailyReportSearchParam.KukuriKbn)
                    || fieldName == nameof(VehicleDailyReportSearchParam.KugiriCharType)
                    || fieldName == nameof(VehicleDailyReportSearchParam.OutputKbn))
                {
                    if (value != null)
                    {
                        VehicleSearchDropdown data = (VehicleSearchDropdown)value;
                        JoInput = data.Value.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.ScheduleYmdStart) || fieldName == nameof(VehicleDailyReportSearchParam.ScheduleYmdEnd))
                {
                    JoInput = value != null ? value.ToString().Substring(0, 10).Replace("/", string.Empty) : string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.selectedBusSaleStart) || fieldName == nameof(VehicleDailyReportSearchParam.selectedBusSaleEnd))
                {
                    if (value != null)
                    {
                        BusSaleBranchSearch data = (BusSaleBranchSearch)value;
                        JoInput = data.EigyoCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.selectedBusCodeStart) || fieldName == nameof(VehicleDailyReportSearchParam.selectedBusCodeEnd))
                {
                    if (value != null)
                    {
                        BusCodeSearch data = (BusCodeSearch)value;
                        JoInput = data.SyaRyoCdSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.ReceptionStart) || fieldName == nameof(VehicleDailyReportSearchParam.ReceptionEnd))
                {
                    if (value != null && value != string.Empty)
                    {
                        JoInput = $"{Int32.Parse(value.ToString()):0000000000}";
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.selectedReservationStart) || fieldName == nameof(VehicleDailyReportSearchParam.selectedReservationEnd))
                {
                    if (value != null)
                    {
                        ReservationClassComponentData data = (ReservationClassComponentData)value;
                        JoInput = data.YoyaKbnSeq.ToString();
                    }
                    else JoInput = string.Empty;
                }
                if (fieldName == nameof(VehicleDailyReportSearchParam.fontSize))
                {
                    JoInput = value.ToString();
                }
                result.Add(fieldName, JoInput);
            }
            return result;
        }
    }
}

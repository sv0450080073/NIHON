using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.CommonComponents;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.Pages;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.IService
{
    public interface IGetFilterDataService
    {
        HyperFormData GetHyperFormData(List<TkdInpCon> formFilterValues, List<StaffsData> StaffList, List<SaleBranchData> SaleBranchList,
            List<CodeTypeData> CodeKbList, List<CustomerClassification> CustomerClassificationList, List<LoadLocation> DestinationList,
            List<LoadDispatchArea> DispatchList, List<LoadLocation> OriginList, List<LoadLocation> AreaList, List<BusTypesData> BusTypeList,
            List<VpmCodeKb> ConditionList, List<ComboboxFixField> PagePrintData, List<ComboboxFixField> ShowHeaderOptionData, List<ComboboxFixField> GroupTypeData, List<ComboboxFixField> DelimiterTypeData,
            List<ComboboxFixField> MaxMinSettingList, List<ComboboxFixField> ReservationStatusList, List<ReservationClassComponentData> ListReservationClass, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData);
        BillsCheckListFormData GetBillCheckListFormData(List<TkdInpCon> formFilterValues, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData, List<ReservationClassComponentData> ListReservationClass, List<InvoiceType> billClassificationList, List<BillOfficeData> billOfficeList, List<ComboboxFixField> lstBillIssuedClassification, List<ComboboxFixField> lstBillTypeSortGridData, List<ComboboxFixField> lstPageSize, List<ComboboxFixField> lstOutType, List<ComboboxFixField> lstGroupType, List<ComboboxFixField> lstDelimiterType);
        AdvancePaymentDetailsSearchParam GetAdvancePaymentFormData(List<TkdInpCon> formFilterValues, List<PaperSizeDropdown> listPaperSize, List<PaymentSearchDropdown> listAddressSpectify, List<SeikyuSakiSearch> listAddress);
    }
    public class GetFilterDataService : IGetFilterDataService
    {
        public HyperFormData GetHyperFormData(List<TkdInpCon> formFilterValues, List<StaffsData> StaffList, List<SaleBranchData> SaleBranchList,
            List<CodeTypeData> CodeKbList, List<CustomerClassification> CustomerClassificationList, List<LoadLocation> DestinationList,
            List<LoadDispatchArea> DispatchList, List<LoadLocation> OriginList, List<LoadLocation> AreaList, List<BusTypesData> BusTypeList,
            List<VpmCodeKb> ConditionList, List<ComboboxFixField> PagePrintData, List<ComboboxFixField> ShowHeaderOptionData, List<ComboboxFixField> GroupTypeData, List<ComboboxFixField> DelimiterTypeData,
            List<ComboboxFixField> MaxMinSettingList, List<ComboboxFixField> ReservationStatusList, List<ReservationClassComponentData> ListReservationClass, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData)
        {
            var result = new HyperFormData();
            foreach (var item in formFilterValues)
            {
                if (item.ItemNm == nameof(HyperFormData.HaishaBiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.HaishaBiFrom = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.HaishaBiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.HaishaBiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.HaishaBiTo = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.HaishaBiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TochakuBiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.TochakuBiFrom = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.TochakuBiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TochakuBiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.TochakuBiTo = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.TochakuBiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YoyakuBiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.YoyakuBiFrom = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.YoyakuBiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YoyakuBiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.YoyakuBiTo = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                    }
                    else
                    {
                        result.YoyakuBiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeBangoFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeBangoFrom = item.JoInput;
                    }
                    else
                    {
                        result.UketsukeBangoFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeBangoTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeBangoTo = item.JoInput;
                    }
                    else
                    {
                        result.UketsukeBangoTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YoyakuFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.YoyakuFrom = ListReservationClass.FirstOrDefault(_ => _.YoyaKbnSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.YoyakuFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YoyakuTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.YoyakuTo = ListReservationClass.FirstOrDefault(_ => _.YoyaKbnSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.YoyakuTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.EigyoTantoShaFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.EigyoTantoShaFrom = StaffList.Find(x => x != null && x.SyainCd == item.JoInput);
                    }
                    else
                    {
                        result.EigyoTantoShaFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.EigyoTantoShaTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.EigyoTantoShaTo = StaffList.Find(x => x != null && x.SyainCd == item.JoInput);
                    }
                    else
                    {
                        result.EigyoTantoShaTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.NyuryokuTantoShaFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.NyuryokuTantoShaFrom = StaffList.Find(x => x != null && x.SyainCd == item.JoInput);
                    }
                    else
                    {
                        result.NyuryokuTantoShaFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.NyuryokuTantoShaTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.NyuryokuTantoShaTo = StaffList.Find(x => x != null && x.SyainCd == item.JoInput);
                    }
                    else
                    {
                        result.NyuryokuTantoShaTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeEigyoJoFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeEigyoJoFrom = SaleBranchList.Find(x => x != null && x.EigyoCd == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.UketsukeEigyoJoFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeEigyoJoTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeEigyoJoTo = SaleBranchList.Find(x => x != null && x.EigyoCd == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.UketsukeEigyoJoTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.GyosyaTokuiSakiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.GyosyaTokuiSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.GyosyaTokuiSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.GyosyaTokuiSakiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.GyosyaTokuiSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiskTokuiSakiFrom))
                {
                    if (item.JoInput != string.Empty && result.GyosyaTokuiSakiFrom != null)
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (result.GyosyaTokuiSakiFrom?.GyosyaCdSeq ?? -1)).ToList();
                        result.TokiskTokuiSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiskTokuiSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiskTokuiSakiTo))
                {
                    if (item.JoInput != string.Empty && result.GyosyaTokuiSakiTo != null)
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (result.GyosyaTokuiSakiTo?.GyosyaCdSeq ?? -1)).ToList();
                        result.TokiskTokuiSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiskTokuiSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiStTokuiSakiFrom))
                {
                    if (item.JoInput != string.Empty && result.TokiskTokuiSakiFrom != null)
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (result.TokiskTokuiSakiFrom?.TokuiSeq ?? -1)).ToList();
                        result.TokiStTokuiSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiStTokuiSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiStTokuiSakiTo))
                {
                    if (item.JoInput != string.Empty && result.TokiskTokuiSakiTo != null)
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (result.TokiskTokuiSakiTo?.TokuiSeq ?? -1)).ToList();
                        result.TokiStTokuiSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiStTokuiSakiTo = null;
                    }
                }

                //
                if (item.ItemNm == nameof(HyperFormData.GyosyaShiireSakiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.GyosyaShiireSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.GyosyaShiireSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.GyosyaShiireSakiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.GyosyaShiireSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.GyosyaShiireSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiskShiireSakiFrom))
                {
                    if (item.JoInput != string.Empty && result.GyosyaShiireSakiFrom != null)
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (result.GyosyaShiireSakiFrom?.GyosyaCdSeq ?? -1)).ToList();
                        result.TokiskShiireSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiskShiireSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiskShiireSakiTo))
                {
                    if (item.JoInput != string.Empty && result.GyosyaShiireSakiTo != null)
                    {
                        List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                        LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (result.GyosyaShiireSakiTo?.GyosyaCdSeq ?? -1)).ToList();
                        result.TokiskShiireSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiskShiireSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiStShiireSakiFrom))
                {
                    if (item.JoInput != string.Empty && result.TokiskShiireSakiFrom != null)
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (result.TokiskShiireSakiFrom?.TokuiSeq ?? -1)).ToList();
                        result.TokiStShiireSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiStShiireSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.TokiStShiireSakiTo))
                {
                    if (item.JoInput != string.Empty && result.TokiskShiireSakiTo != null)
                    {
                        List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                        LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (result.TokiskShiireSakiTo?.TokuiSeq ?? -1)).ToList();
                        result.TokiStShiireSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == (int.TryParse(item.JoInput, out int temp) ? temp : 0));
                    }
                    else
                    {
                        result.TokiStShiireSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.DantaiNm))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.DantaiNm = item.JoInput;
                    }
                    else
                    {
                        result.DantaiNm = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.ShashuTankaFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.ShashuTankaFrom = item.JoInput;
                    }
                    else
                    {
                        result.ShashuTankaFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.ShashuTankaTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.ShashuTankaTo = item.JoInput;
                    }
                    else
                    {
                        result.ShashuTankaTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.DantaiKbnFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.DantaiKbnFrom = CodeKbList.Find(x => x != null && x.CodeKbn == item.JoInput);
                    }
                    else
                    {
                        result.DantaiKbnFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.DantaiKbnTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.DantaiKbnTo = CodeKbList.Find(x => x != null && x.CodeKbn == item.JoInput);
                    }
                    else
                    {
                        result.DantaiKbnTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.KyakuDaneKbnFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.KyakuDaneKbnFrom = CustomerClassificationList.Find(x => x != null && x.JyoKyakuCd == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.KyakuDaneKbnFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.KyakuDaneKbnTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.KyakuDaneKbnTo = CustomerClassificationList.Find(x => x != null && x.JyoKyakuCd == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.KyakuDaneKbnTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YukiSakiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.YukiSakiFrom = DestinationList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.YukiSakiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.YukiSakiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.YukiSakiTo = DestinationList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.YukiSakiTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.HasseiChiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.HasseiChiFrom = OriginList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.HasseiChiFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.HasseiChiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.HasseiChiTo = OriginList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.HasseiChiTo = null;
                    }

                }
                if (item.ItemNm == nameof(HyperFormData.AreaFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.AreaFrom = AreaList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.AreaFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.AreaTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.AreaTo = AreaList.Find(x => x != null && x.BasyoMapCdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.AreaTo = null;
                    }

                }
                if (item.ItemNm == nameof(HyperFormData.HaishaChiFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.HaishaChiFrom = DispatchList.Find(x => x != null && x.HaiScdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.HaishaChiFrom = null;
                    }

                }
                if (item.ItemNm == nameof(HyperFormData.HaishaChiTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        var datas = item.JoInput.Split("-");
                        result.HaishaChiTo = DispatchList.Find(x => x != null && x.HaiScdSeq == int.Parse(datas[1]));
                    }
                    else
                    {
                        result.HaishaChiTo = null;
                    }

                }
                if (item.ItemNm == nameof(HyperFormData.ShashuFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.ShashuFrom = BusTypeList.Find(x => x != null && x.SyaSyuCdSeq == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.ShashuFrom = null;
                    }

                }
                if (item.ItemNm == nameof(HyperFormData.ShashuTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.ShashuTo = BusTypeList.Find(x => x != null && x.SyaSyuCdSeq == int.Parse(item.JoInput));
                    }
                    else
                    {
                        result.ShashuTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeJokenFrom))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeJokenFrom = ConditionList.Find(x => x != null && x.CodeKbn == item.JoInput);
                    }
                    else
                    {
                        result.UketsukeJokenFrom = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.UketsukeJokenTo))
                {
                    if (item.JoInput != string.Empty)
                    {
                        result.UketsukeJokenTo = ConditionList.Find(x => x != null && x.CodeKbn == item.JoInput);
                    }
                    else
                    {
                        result.UketsukeJokenTo = null;
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.OutputType))
                {
                    result.OutputType = (OutputReportType)int.Parse(item.JoInput);
                }
                if (item.ItemNm == nameof(HyperFormData.PageSize))
                {
                    if (PagePrintData.Count == 0)
                    {
                        result.PageSize = new ComboboxFixField { IdValue = 1, StringValue = "A4" };
                    }
                    else
                    {
                        result.PageSize = PagePrintData.Find(x => x.IdValue == int.Parse(item.JoInput));
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.ActiveHeaderOption))
                {
                    if (ShowHeaderOptionData.Count == 0)
                    {
                        result.ActiveHeaderOption = ShowHeaderOptions.ShowHeaderOptionData[0];
                    }
                    else
                    {
                        result.ActiveHeaderOption = ShowHeaderOptionData.Find(x => x.IdValue == int.Parse(item.JoInput));
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.GroupType))
                {
                    if (GroupTypeData.Count == 0)
                    {
                        result.GroupType = GroupTypes.GroupTypeData[0];
                    }
                    else
                    {
                        result.GroupType = GroupTypeData.Find(x => x.IdValue == int.Parse(item.JoInput));
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.DelimiterType))
                {
                    if (DelimiterTypeData.Count == 0)
                    {
                        result.DelimiterType = DelimiterTypes.DelimiterTypeData[2];
                    }
                    else
                    {
                        result.DelimiterType = DelimiterTypeData.Find(x => x.IdValue == int.Parse(item.JoInput));
                    }
                }
                if (item.ItemNm == nameof(HyperFormData.ActiveV))
                {
                    result.ActiveV = int.Parse(item.JoInput);
                }
                if (item.ItemNm == nameof(HyperFormData.dateType))
                {
                    result.dateType = int.Parse(item.JoInput);
                }
                if (item.ItemNm == nameof(HyperFormData.MaxMinSetting))
                {
                    result.MaxMinSetting = MaxMinSettingList.Find(x => int.TryParse(item.JoInput, out _) && x != null && x.IdValue == int.Parse(item.JoInput));
                }
                if (item.ItemNm == nameof(HyperFormData.ReservationStatus))
                {
                    result.ReservationStatus = ReservationStatusList.Find(x => int.TryParse(item.JoInput, out _) && x != null && x.IdValue == int.Parse(item.JoInput));
                }
            }
            return result;
        }

        public BillsCheckListFormData GetBillCheckListFormData(List<TkdInpCon> formFilterValues, List<CustomerComponentGyosyaData> ListGyosya, List<CustomerComponentTokiskData> TokiskData, List<CustomerComponentTokiStData> TokiStData, List<ReservationClassComponentData> ListReservationClass, List<InvoiceType> billClassificationList, List<BillOfficeData> billOfficeList, List<ComboboxFixField> lstBillIssuedClassification, List<ComboboxFixField> lstBillTypeSortGridData, List<ComboboxFixField> lstPageSize, List<ComboboxFixField> lstOutType, List<ComboboxFixField> lstGroupType, List<ComboboxFixField> lstDelimiterType)
        {
            CustomerComponentGyosyaData GyosyaTokuiFrom = new CustomerComponentGyosyaData();
            CustomerComponentGyosyaData GyosyaTokuiTo = new CustomerComponentGyosyaData();
            CustomerComponentTokiskData TokiskTokuiFrom = new CustomerComponentTokiskData();
            CustomerComponentTokiskData TokiskTokuiTo = new CustomerComponentTokiskData();

            var result = new BillsCheckListFormData();
            result.lstActiveTypeTotal = new List<int> { 1, 2, 3 };
            result.ActiveV = (int)ViewMode.Medium;
            result.OutputType = OutputReportType.Preview;
            result.DelimiterType = lstDelimiterType.FirstOrDefault();
            result.GroupType = lstGroupType.FirstOrDefault();
            result.ActiveHeaderOption = lstOutType.FirstOrDefault();
            result.PageSize = lstPageSize.FirstOrDefault();
            result.BillOffice = billOfficeList.FirstOrDefault();
            foreach (var item in formFilterValues)
            {
                switch (item.ItemNm)
                {
                    case nameof(BillsCheckListFormData.BillPeriodFrom):
                        if (item.JoInput != string.Empty)
                        {
                            result.BillPeriodFrom = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        }
                        else
                        {
                            result.BillPeriodFrom = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.BillPeriodTo):
                        if (item.JoInput != string.Empty)
                        {
                            result.BillPeriodTo = DateTime.ParseExact(item.JoInput, "yyyyMMdd", null);
                        }
                        else
                        {
                            result.BillPeriodTo = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.BillOffice):
                        if (item.JoInput != string.Empty)
                        {
                            result.BillOffice = billOfficeList.Find(x => x != null && x.EigyoCd == item.JoInput);
                        }
                        else
                        {
                            result.BillOffice = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.GyosyaTokuiSakiFrom):
                        if (int.TryParse(item.JoInput, out int outValue))
                        {
                            result.GyosyaTokuiSakiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                            GyosyaTokuiFrom = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue);
                        }
                        else
                        {
                            result.GyosyaTokuiSakiFrom = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.GyosyaTokuiSakiTo):
                        if (int.TryParse(item.JoInput, out int outValue1))
                        {
                            result.GyosyaTokuiSakiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue1);
                            GyosyaTokuiTo = ListGyosya.FirstOrDefault(_ => _.GyosyaCdSeq == outValue1);
                        }
                        else
                        {
                            result.GyosyaTokuiSakiTo = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.TokiskTokuiSakiFrom):
                        if (result.GyosyaTokuiSakiFrom != null && int.TryParse(item.JoInput, out int outValue2))
                        {
                            List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                            LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiFrom?.GyosyaCdSeq ?? -1)).ToList();
                            result.TokiskTokuiSakiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue2);
                            TokiskTokuiFrom = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue2);
                        }
                        else
                        {
                            result.TokiskTokuiSakiFrom = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.TokiskTokuiSakiTo):
                        if (result.GyosyaTokuiSakiTo != null && int.TryParse(item.JoInput, out int outValue3))
                        {
                            List<CustomerComponentTokiskData> LstTokisk = new List<CustomerComponentTokiskData>();
                            LstTokisk = TokiskData.Where(_ => _.GyosyaCdSeq == (GyosyaTokuiTo?.GyosyaCdSeq ?? -1)).ToList();
                            result.TokiskTokuiSakiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue3);
                            TokiskTokuiTo = LstTokisk.FirstOrDefault(_ => _.TokuiSeq == outValue3);
                        }
                        else
                        {
                            result.TokiskTokuiSakiTo = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.TokiStTokuiSakiFrom):
                        if (result.TokiskTokuiSakiFrom != null && int.TryParse(item.JoInput, out int outValue4))
                        {
                            List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                            LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiFrom?.TokuiSeq ?? -1)).ToList();
                            result.TokiStTokuiSakiFrom = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue4);
                        }
                        else
                        {
                            result.TokiStTokuiSakiFrom = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.TokiStTokuiSakiTo):
                        if (result.TokiskTokuiSakiTo != null && int.TryParse(item.JoInput, out int outValue5))
                        {
                            List<CustomerComponentTokiStData> LstTokiSt = new List<CustomerComponentTokiStData>();
                            LstTokiSt = TokiStData.Where(_ => _.TokuiSeq == (TokiskTokuiTo?.TokuiSeq ?? -1)).ToList();
                            result.TokiStTokuiSakiTo = LstTokiSt.FirstOrDefault(_ => _.SitenCdSeq == outValue5);
                        }
                        else
                        {
                            result.TokiStTokuiSakiTo = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.StartReceiptNumber):
                        if (item.JoInput != string.Empty)
                        {
                            result.StartReceiptNumber = item.JoInput;
                        }
                        else
                        {
                            result.StartReceiptNumber = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.EndReceiptNumber):
                        if (item.JoInput != string.Empty)
                        {
                            result.EndReceiptNumber = item.JoInput;
                        }
                        else
                        {
                            result.EndReceiptNumber = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.YoyakuFrom):
                        if (item.JoInput != string.Empty && int.TryParse(item.JoInput, out int outValue6))
                        {
                            result.YoyakuFrom = ListReservationClass.Where(x => x.YoyaKbnSeq == outValue6).FirstOrDefault();
                        }
                        else
                        {
                            result.YoyakuFrom = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.YoyakuTo):
                        if (item.JoInput != string.Empty && int.TryParse(item.JoInput, out int outValue7))
                        {
                            result.YoyakuTo = ListReservationClass.Where(x => x.YoyaKbnSeq == outValue7).FirstOrDefault();
                        }
                        else
                        {
                            result.YoyakuTo = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.StartBillClassification):
                        if (item.JoInput != string.Empty)
                        {
                            result.StartBillClassification = billClassificationList.Find(x => x != null && x.CodeKbnSeq.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.StartBillClassification = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.EndBillClassification):
                        if (item.JoInput != string.Empty)
                        {
                            result.EndBillClassification = billClassificationList.Find(x => x != null && x.CodeKbnSeq.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.EndBillClassification = null;
                        }
                        break;
                    case nameof(BillsCheckListFormData.BillIssuedClassification):
                        if (item.JoInput != string.Empty)
                        {
                            result.BillIssuedClassification = lstBillIssuedClassification.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.BillIssuedClassification = lstBillIssuedClassification.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.BillTypeOrder):
                        if (item.JoInput != string.Empty)
                        {
                            result.BillTypeOrder = lstBillTypeSortGridData.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.BillTypeOrder = lstBillTypeSortGridData.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemFare):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemFare = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemFare = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemIncidental):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemIncidental = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemIncidental = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemTollFee):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemTollFee = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemTollFee = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemArrangementFee):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemArrangementFee = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemArrangementFee = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemGuideFee):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemGuideFee = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemGuideFee = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemShippingCharge):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemShippingCharge = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemShippingCharge = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.itemCancellationCharge):
                        if (item.JoInput != string.Empty)
                        {
                            result.itemCancellationCharge = item.JoInput == "0" ? false : true;
                        }
                        else
                        {
                            result.itemCancellationCharge = true;
                        }
                        break;
                    case nameof(BillsCheckListFormData.PageSize):
                        if (item.JoInput != string.Empty)
                        {
                            result.PageSize = lstPageSize.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.PageSize = lstPageSize.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.ActiveHeaderOption):
                        if (item.JoInput != string.Empty)
                        {
                            result.ActiveHeaderOption = lstOutType.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.ActiveHeaderOption = lstOutType.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.GroupType):
                        if (item.JoInput != string.Empty)
                        {
                            result.GroupType = lstGroupType.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.GroupType = lstGroupType.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.DelimiterType):
                        if (item.JoInput != string.Empty)
                        {
                            result.DelimiterType = lstDelimiterType.Find(x => x != null && x.IdValue.ToString() == item.JoInput);
                        }
                        else
                        {
                            result.DelimiterType = lstDelimiterType.FirstOrDefault();
                        }
                        break;
                    case nameof(BillsCheckListFormData.ActiveV):
                        if (item.JoInput != string.Empty)
                        {
                            int temp = 0;
                            int.TryParse(item.JoInput, out temp);
                            result.ActiveV = temp;
                        }
                        else
                        {
                            result.ActiveV = (int)ViewMode.Medium;
                        }
                        break;
                    case nameof(BillsCheckListFormData.lstActiveTypeTotal):
                        List<int> _lstActiveTypeTotal = new List<int>();
                        if (item.JoInput != string.Empty)
                        {
                            string[] totals = item.JoInput.Split(",");
                            foreach (string itemTotal in totals)
                            {
                                _lstActiveTypeTotal.Add(int.Parse(itemTotal.Trim()));
                            }
                            result.lstActiveTypeTotal = _lstActiveTypeTotal;
                        }
                        else
                        {
                            result.lstActiveTypeTotal = new List<int> { 1, 2, 3 };
                        }
                        break;
                    case nameof(BillsCheckListFormData.OutputType):
                        if (item.JoInput != string.Empty)
                        {
                            switch (item.JoInput)
                            {
                                case "Preview":
                                    result.OutputType = OutputReportType.Preview;
                                    break;
                                case "CSV":
                                    result.OutputType = OutputReportType.CSV;
                                    break;
                                default:
                                    result.OutputType = OutputReportType.ExportPdf;
                                    break;
                            }
                        }
                        else
                        {
                            result.OutputType = OutputReportType.Preview;
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public AdvancePaymentDetailsSearchParam GetAdvancePaymentFormData(List<TkdInpCon> formFilterValues, List<PaperSizeDropdown> listPaperSize, List<PaymentSearchDropdown> listAddressSpectify, List<SeikyuSakiSearch> listAddress)
        {
            var result = new AdvancePaymentDetailsSearchParam();
            foreach (var item in formFilterValues)
            {
                switch (item.ItemNm)
                {
                    case nameof(AdvancePaymentDetailsSearchParam.OutputSetting):
                        result.OutputSetting = item.JoInput != string.Empty ? (byte)int.Parse(item.JoInput) : (byte)PrintMode.Preview;
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.PaperSize):
                        if (item.JoInput != string.Empty)
                        {
                            result.PaperSize = listPaperSize.Find(x => x != null && x.Value == (byte)int.Parse(item.JoInput));
                        }
                        else
                        {
                            result.PaperSize = new PaperSizeDropdown() { Value = (byte)PaperSize.A4, Text = "A4" };
                        }
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.AddressSpectify):
                        result.AddressSpectify = (item.JoInput != string.Empty) ? listAddressSpectify.Find(x => x != null && x.Value == int.Parse(item.JoInput)) : null;
                        break;

                    //case nameof(AdvancePaymentDetailsSearchParam.StartAddress):
                    //    if (item.JoInput != string.Empty)
                    //    {
                    //        var datas = item.JoInput.Split("-");
                    //        result.StartAddress = listAddress.Find(x => x != null && x.TokuiCd == int.Parse(datas[0]) && x.SitenCd == int.Parse(datas[1]));
                    //    }
                    //    else
                    //    {
                    //        result.StartAddress = null;
                    //    }
                    //break;

                    //case nameof(AdvancePaymentDetailsSearchParam.EndAddress):
                    //    if (item.JoInput != string.Empty)
                    //    {
                    //        var datas = item.JoInput.Split("-");
                    //        result.EndAddress = listAddress.Find(x => x != null && x.TokuiCd == int.Parse(datas[0]) && x.SitenCd == int.Parse(datas[1]));
                    //    }
                    //    else
                    //    {
                    //        result.EndAddress = null;
                    //    }
                    //break;

                    case nameof(AdvancePaymentDetailsSearchParam.StartAddress):
                        if (item.JoInput != string.Empty)
                        {
                            var datas = item.JoInput.Split("-");
                            result.StartAddress = listAddress.Find(x => x != null && x.TokuiCd == int.Parse(datas[0]) && x.SitenCd == int.Parse(datas[1]));
                        }
                        else
                        {
                            result.StartAddress = null;
                        }
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.EndAddress):
                        if (item.JoInput != string.Empty)
                        {
                            var datas = item.JoInput.Split("-");
                            result.EndAddress = listAddress.Find(x => x != null && x.TokuiCd == int.Parse(datas[0]) && x.SitenCd == int.Parse(datas[1]));
                        }
                        else
                        {
                            result.EndAddress = null;
                        }
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.ScheduleYmdStart):
                        result.ScheduleYmdStart = (item.JoInput != string.Empty) ? DateTime.ParseExact(item.JoInput, "yyyyMMdd", new CultureInfo("ja-JP")) : (DateTime?)null;
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.ScheduleYmdEnd):
                        result.ScheduleYmdEnd = (item.JoInput != string.Empty) ? DateTime.ParseExact(item.JoInput, "yyyyMMdd", new CultureInfo("ja-JP")) : (DateTime?)null;
                        break;

                    case nameof(AdvancePaymentDetailsSearchParam.ReceptionNumber):
                        result.ReceptionNumber = (item.JoInput != string.Empty) ? item.JoInput : null;
                        break;
                }
            }
            return result;
        }
    }
}

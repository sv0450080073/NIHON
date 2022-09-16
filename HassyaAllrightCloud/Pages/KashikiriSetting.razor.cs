using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.Domain.Dto.RegulationSetting;
using HassyaAllrightCloud.Domain.Entities;
using HassyaAllrightCloud.IService;
using HassyaAllrightCloud.IService.RegulationSetting;
using HassyaAllrightCloud.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using SharedLibraries.UI.Models;

namespace HassyaAllrightCloud.Pages
{
    public class RegulationSettingBase : ComponentBase, IDisposable
    {
        [Inject] protected IStringLocalizer<KashikiriSetting> _lang { get; set; }
        [Inject] private IErrorHandlerService _errService { get; set; }
        [Inject] private ILoadingService _loading { get; set; }
        [Inject] private IRegulationSettingService _service { get; set; }
        [Inject] private ITransportationSummaryService _transportationSummaryService { get; set; }
        [Inject]
        protected IFilterCondition FilterConditionService { get; set; }
        [Inject]
        protected IGenerateFilterValueDictionary GenerateFilterValueDictionaryService { get; set; }
        public string FormName = FormFilterName.KashikiriSetting;
        Dictionary<string, string> keyValueFilterPairs = new Dictionary<string, string>();
        public List<RegulationSettingItem> displayItems { get; set; } = new List<RegulationSettingItem>();
        public List<RegulationSettingItem> items { get; set; } = new List<RegulationSettingItem>();
        private CancellationTokenSource source { get; set; } = new CancellationTokenSource();
        protected IEnumerable<CompanyListItem> companyList = new List<CompanyListItem>();
        public RegulationSettingForm RegulationSettingForm;
        protected Dictionary<string, string> LangDic = new Dictionary<string, string>();
        protected byte itemPerPage { get; set; } = Common.DefaultPageSize;
        protected int gridSize { get; set; } = (int)ViewMode.Medium;
        protected int currentPage { get; set; } = DefaultPage;
        protected RegulationSettingModel model { get; set; }
        public KashikiriSetting RegulationSetting { get; set; }
        protected EditContext formContext { get; set; }
        public HeaderTemplate Header { get; set; }
        private const string colorFormat = "#{0}";
        public BodyTemplate Body { get; set; }
        private const int DefaultPage = 0;
        protected Pagination paging;
        protected bool showPopup { get; set; }
        protected bool isCreate { get; set; } = true;
        protected RegulationSettingItem selectedItem { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                RegulationSetting = new KashikiriSetting();
                var dataLang = _lang.GetAllStrings();
                LangDic = dataLang.ToDictionary(l => l.Name, l => l.Value);
                InitTable();
                model = new RegulationSettingModel();
                companyList = await _transportationSummaryService.GetCompanyListItems(0);
                List<TkdInpCon> filterValues = FilterConditionService.GetFilterCondition(FormFilterName.KashikiriSetting, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Result;
                if (filterValues.Count > 0)
                {
                    model = GenerateRegulationSettingModel(filterValues);
                }
                await LoadData();
                displayItems = items.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
                formContext = new EditContext(model);
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

        private RegulationSettingModel GenerateRegulationSettingModel(List<TkdInpCon> filterValues)
        {            
            RegulationSettingModel result = new RegulationSettingModel();
            foreach (var item in filterValues)
            {
                switch (item.ItemNm)
                {
                    case nameof(RegulationSettingModel.CompanyFrom):
                        result.CompanyFrom = !string.IsNullOrWhiteSpace(item.JoInput) ? companyList.Where(x => x != null && x.CompanyCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault() : null;
                        break;
                    case nameof(RegulationSettingModel.CompanyTo):
                        result.CompanyTo = !string.IsNullOrWhiteSpace(item.JoInput) ? companyList.Where(x => x != null && x.CompanyCdSeq == Int32.Parse(item.JoInput)).FirstOrDefault() : null;
                        break;
                }
            }
            return result;
        }
        public void ResetForm()
        {
            try
            {
                model = new RegulationSettingModel();
                FilterConditionService.DeleteFilterCondition(new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq, 0, FormFilterName.KashikiriSetting).Wait();
                LoadData();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            
        }

        public void OnReset(bool e)
        {
            try
            {
                ResetForm();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            
        }

        public void OnTogglePopup(bool e)
        {
            try
            {
                //RegulationSettingForm.ClosePopup();
                showPopup = false;
                LoadData();
                
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            
        }

        public async Task LoadData()
        {
            try
            {
                await _loading.ShowAsync();
                var dataItems = await _service.GetRegulationSettingsAsync(model, source.Token);
                if (dataItems.Count > 0)
                {
                    keyValueFilterPairs = GenerateFilterValueDictionaryService.GenerateForKashikiriSetting(model).Result;
                    FilterConditionService.SaveFilterCondtion(keyValueFilterPairs, FormFilterName.KashikiriSetting, 0, new HassyaAllrightCloud.Domain.Dto.ClaimModel().SyainCdSeq).Wait();
                }
                items.Clear();
                items = dataItems.Select((e, i) => new RegulationSettingItem()
                {
                    AutKarJyun = e.AutKarJyun,
                    AutKouKbn = e.AutKouKbn,
                    CanEKan1 = e.CanEKan1,
                    CanEKan2 = e.CanEKan2,
                    CanEKan3 = e.CanEKan3,
                    CanEKan4 = e.CanEKan4,
                    CanEKan5 = e.CanEKan5,
                    CanEKan6 = e.CanEKan6,
                    CanJidoKbn = e.CanJidoKbn,
                    CanKbn = e.CanKbn,
                    CanKikan = e.CanKikan,
                    CanMDKbn = e.CanMDKbn,
                    CanRit1 = e.CanRit1,
                    CanRit2 = e.CanRit2,
                    CanRit3 = e.CanRit3,
                    CanRit4 = e.CanRit4,
                    CanRit5 = e.CanRit5,
                    CanRit6 = e.CanRit6,
                    CanSKan1 = e.CanSKan1,
                    CanSKan2 = e.CanSKan2,
                    CanSKan3 = e.CanSKan3,
                    CanSKan4 = e.CanSKan4,
                    CanSKan5 = e.CanSKan5,
                    CanSKan6 = e.CanSKan6,
                    CanWaitKbn = e.CanWaitKbn,
                    ColHai = e.ColHai,
                    ColHaiin = e.ColHaiin,
                    ColIcHai = e.ColIcHai,
                    ColIcHaiin = e.ColIcHaiin,
                    ColIcKari = e.ColIcKari,
                    ColIcKariH = e.ColIcKariH,
                    ColIcNCou = e.ColIcNCou,
                    ColIcNip = e.ColIcNip,
                    ColIcNyu = e.ColIcNyu,
                    ColIcShiha = e.ColIcShiha,
                    ColIcSCou = e.ColIcSCou,
                    ColIcWari = e.ColIcWari,
                    ColIcYou = e.ColIcYou,
                    ColKahar = e.ColKahar,
                    ColKaku = e.ColKaku,
                    ColKanyu = e.ColKanyu,
                    ColKari = e.ColKari,
                    ColKariH = e.ColKariH,
                    ColKYoy = e.ColKYoy,
                    ColMiKari = e.ColMiKari,
                    ColNCou = e.ColNCou,
                    ColNin = e.ColNin,
                    ColNip = e.ColNip,
                    ColNyu = e.ColNyu,
                    ColSCou = e.ColSCou,
                    ColSelect = e.ColSelect,
                    ColShiha = e.ColShiha,
                    ColWari = e.ColWari,
                    ColYou = e.ColYou,
                    CompanyCdSeq = e.CompanyCdSeq,
                    DaySyoKbn = e.DaySyoKbn,
                    DrvAutoSet = e.DrvAutoSet,
                    ETCKinKbn = e.ETCKinKbn,
                    ExpItem = e.ExpItem,
                    FutaiCopyFlg = e.FutaiCopyFlg,
                    FutSF1Flg = e.FutSF1Flg,
                    FutSF2Flg = e.FutSF2Flg,
                    FutSF3Flg = e.FutSF3Flg,
                    FutSF4Flg = e.FutSF4Flg,
                    FutTumCdSeq = e.FutTumCdSeq,
                    GetSyoKbn = e.GetSyoKbn,
                    GoSyaAutoSet = e.GoSyaAutoSet,
                    GuiAutoSet = e.GuiAutoSet,
                    GuideFutTumCdSeq = e.GuideFutTumCdSeq,
                    HoukoKbn = e.HoukoKbn,
                    HouOutKbn = e.HouOutKbn,
                    HouZeiKbn = e.HouZeiKbn,
                    JisKinKyuNm01 = e.JisKinKyuNm01,
                    JisKinKyuNm02 = e.JisKinKyuNm02,
                    JisKinKyuNm03 = e.JisKinKyuNm03,
                    JisKinKyuNm04 = e.JisKinKyuNm04,
                    JisKinKyuNm05 = e.JisKinKyuNm05,
                    JisKinKyuNm06 = e.JisKinKyuNm06,
                    JisKinKyuNm07 = e.JisKinKyuNm07,
                    JisKinKyuNm08 = e.JisKinKyuNm08,
                    JisKinKyuNm09 = e.JisKinKyuNm09,
                    JisKinKyuNm10 = e.JisKinKyuNm10,
                    JKariKbn = e.JKariKbn,
                    JKBunPat = e.JKBunPat,
                    JoshaCopyFlg = e.JoshaCopyFlg,
                    JymAChkKbn = e.JymAChkKbn,
                    JyoSenInfoPtnCol1 = e.JyoSenInfoPtnCol1,
                    JyoSenInfoPtnCol10 = e.JyoSenInfoPtnCol10,
                    JyoSenInfoPtnCol11 = e.JyoSenInfoPtnCol11,
                    JyoSenInfoPtnCol12 = e.JyoSenInfoPtnCol12,
                    JyoSenInfoPtnCol13 = e.JyoSenInfoPtnCol13,
                    JyoSenInfoPtnCol14 = e.JyoSenInfoPtnCol14,
                    JyoSenInfoPtnCol15 = e.JyoSenInfoPtnCol15,
                    JyoSenInfoPtnCol2 = e.JyoSenInfoPtnCol2,
                    JyoSenInfoPtnCol3 = e.JyoSenInfoPtnCol3,
                    JyoSenInfoPtnCol4 = e.JyoSenInfoPtnCol4,
                    JyoSenInfoPtnCol5 = e.JyoSenInfoPtnCol5,
                    JyoSenInfoPtnCol6 = e.JyoSenInfoPtnCol6,
                    JyoSenInfoPtnCol7 = e.JyoSenInfoPtnCol7,
                    JyoSenInfoPtnCol8 = e.JyoSenInfoPtnCol8,
                    JyoSenInfoPtnCol9 = e.JyoSenInfoPtnCol9,
                    JyoSenInfoPtnKbn1 = e.JyoSenInfoPtnKbn1,
                    JyoSenInfoPtnKbn10 = e.JyoSenInfoPtnKbn10,
                    JyoSenInfoPtnKbn11 = e.JyoSenInfoPtnKbn11,
                    JyoSenInfoPtnKbn12 = e.JyoSenInfoPtnKbn12,
                    JyoSenInfoPtnKbn13 = e.JyoSenInfoPtnKbn13,
                    JyoSenInfoPtnKbn14 = e.JyoSenInfoPtnKbn14,
                    JyoSenInfoPtnKbn15 = e.JyoSenInfoPtnKbn15,
                    JyoSenInfoPtnKbn2 = e.JyoSenInfoPtnKbn2,
                    JyoSenInfoPtnKbn3 = e.JyoSenInfoPtnKbn3,
                    JyoSenInfoPtnKbn4 = e.JyoSenInfoPtnKbn4,
                    JyoSenInfoPtnKbn5 = e.JyoSenInfoPtnKbn5,
                    JyoSenInfoPtnKbn6 = e.JyoSenInfoPtnKbn6,
                    JyoSenInfoPtnKbn7 = e.JyoSenInfoPtnKbn7,
                    JyoSenInfoPtnKbn8 = e.JyoSenInfoPtnKbn8,
                    JyoSenInfoPtnKbn9 = e.JyoSenInfoPtnKbn9,
                    JyoSenMjPtnCol1 = e.JyoSenMjPtnCol1,
                    JyoSenMjPtnCol2 = e.JyoSenMjPtnCol2,
                    JyoSenMjPtnCol3 = e.JyoSenMjPtnCol3,
                    JyoSenMjPtnCol4 = e.JyoSenMjPtnCol4,
                    JyoSenMjPtnCol5 = e.JyoSenMjPtnCol5,
                    JyoSenMjPtnKbn1 = e.JyoSenMjPtnKbn1,
                    JyoSenMjPtnKbn2 = e.JyoSenMjPtnKbn2,
                    JyoSenMjPtnKbn3 = e.JyoSenMjPtnKbn3,
                    JyoSenMjPtnKbn4 = e.JyoSenMjPtnKbn4,
                    JyoSenMjPtnKbn5 = e.JyoSenMjPtnKbn5,
                    JyoSyaChkSiyoKbn = e.JyoSyaChkSiyoKbn,
                    KarSyuKiTimeSiyoKbn = e.KarSyuKiTimeSiyoKbn,
                    KoteiCopyFlg = e.KoteiCopyFlg,
                    KouYouSet = e.KouYouSet,
                    MeiShyKbn = e.MeiShyKbn,
                    QuotationTransfer = e.QuotationTransfer,
                    SeiCom1 = e.SeiCom1,
                    SeiCom2 = e.SeiCom2,
                    SeiCom3 = e.SeiCom3,
                    SeiCom4 = e.SeiCom4,
                    SeiCom5 = e.SeiCom5,
                    SeiCom6 = e.SeiCom6,
                    SeiGenFlg = e.SeiGenFlg,
                    SeiKrksKbn = e.SeiKrksKbn,
                    SeiMuki = e.SeiMuki,
                    SeisanCdSeq = e.SeisanCdSeq,
                    SenBackPtnCol = e.SenBackPtnCol,
                    SenBackPtnKbn = e.SenBackPtnKbn,
                    SenDayRenge = e.SenDayRenge,
                    SenDefWidth = e.SenDefWidth,
                    SenHyoHi = e.SenHyoHi,
                    SenMikDefFlg = e.SenMikDefFlg,
                    SenOBPtnCol = e.SenOBPtnCol,
                    SenOBPtnKbn = e.SenOBPtnKbn,
                    SenYouDefFlg = e.SenYouDefFlg,
                    SokoJunKbn = e.SokoJunKbn,
                    SryHyjHga = e.SryHyjHga,
                    SryHyjSyu = e.SryHyjSyu,
                    SryHyjTch = e.SryHyjTch,
                    SryHyjTde = e.SryHyjTde,
                    SryHyjTka = e.SryHyjTka,
                    SyaIrePat = e.SyaIrePat,
                    SyaSenInfoPtnCol1 = e.SyaSenInfoPtnCol1,
                    SyaSenInfoPtnCol10 = e.SyaSenInfoPtnCol10,
                    SyaSenInfoPtnCol11 = e.SyaSenInfoPtnCol11,
                    SyaSenInfoPtnCol12 = e.SyaSenInfoPtnCol12,
                    SyaSenInfoPtnCol13 = e.SyaSenInfoPtnCol13,
                    SyaSenInfoPtnCol14 = e.SyaSenInfoPtnCol14,
                    SyaSenInfoPtnCol15 = e.SyaSenInfoPtnCol15,
                    SyaSenInfoPtnCol2 = e.SyaSenInfoPtnCol2,
                    SyaSenInfoPtnCol3 = e.SyaSenInfoPtnCol3,
                    SyaSenInfoPtnCol4 = e.SyaSenInfoPtnCol4,
                    SyaSenInfoPtnCol5 = e.SyaSenInfoPtnCol5,
                    SyaSenInfoPtnCol6 = e.SyaSenInfoPtnCol6,
                    SyaSenInfoPtnCol7 = e.SyaSenInfoPtnCol7,
                    SyaSenInfoPtnCol8 = e.SyaSenInfoPtnCol8,
                    SyaSenInfoPtnCol9 = e.SyaSenInfoPtnCol9,
                    SyaSenInfoPtnKbn1 = e.SyaSenInfoPtnKbn1,
                    SyaSenInfoPtnKbn10 = e.SyaSenInfoPtnKbn10,
                    SyaSenInfoPtnKbn11 = e.SyaSenInfoPtnKbn11,
                    SyaSenInfoPtnKbn12 = e.SyaSenInfoPtnKbn12,
                    SyaSenInfoPtnKbn13 = e.SyaSenInfoPtnKbn13,
                    SyaSenInfoPtnKbn14 = e.SyaSenInfoPtnKbn14,
                    SyaSenInfoPtnKbn15 = e.SyaSenInfoPtnKbn15,
                    SyaSenInfoPtnKbn2 = e.SyaSenInfoPtnKbn2,
                    SyaSenInfoPtnKbn3 = e.SyaSenInfoPtnKbn3,
                    SyaSenInfoPtnKbn4 = e.SyaSenInfoPtnKbn4,
                    SyaSenInfoPtnKbn5 = e.SyaSenInfoPtnKbn5,
                    SyaSenInfoPtnKbn6 = e.SyaSenInfoPtnKbn6,
                    SyaSenInfoPtnKbn7 = e.SyaSenInfoPtnKbn7,
                    SyaSenInfoPtnKbn8 = e.SyaSenInfoPtnKbn8,
                    SyaSenInfoPtnKbn9 = e.SyaSenInfoPtnKbn9,
                    SyaSenMjPtnCol1 = e.SyaSenMjPtnCol1,
                    SyaSenMjPtnCol2 = e.SyaSenMjPtnCol2,
                    SyaSenMjPtnCol3 = e.SyaSenMjPtnCol3,
                    SyaSenMjPtnCol4 = e.SyaSenMjPtnCol4,
                    SyaSenMjPtnCol5 = e.SyaSenMjPtnCol5,
                    SyaSenMjPtnKbn1 = e.SyaSenMjPtnKbn1,
                    SyaSenMjPtnKbn2 = e.SyaSenMjPtnKbn2,
                    SyaSenMjPtnKbn3 = e.SyaSenMjPtnKbn3,
                    SyaSenMjPtnKbn4 = e.SyaSenMjPtnKbn4,
                    SyaSenMjPtnKbn5 = e.SyaSenMjPtnKbn5,
                    SyaUntKbn = e.SyaUntKbn,
                    SyohiHasu = e.SyohiHasu,
                    TehaiAutoSet = e.TehaiAutoSet,
                    TehaiCopyFlg = e.TehaiCopyFlg,
                    TesuHasu = e.TesuHasu,
                    TumiCopyFlg = e.TumiCopyFlg,
                    UkbCopyFlg = e.UkbCopyFlg,
                    UntZeiKbn = e.UntZeiKbn,
                    UpdPrgID = e.UpdPrgID,
                    UpdSyainCd = e.UpdSyainCd,
                    UpdTime = e.UpdTime,
                    UpdYmd = e.UpdYmd,
                    UriHenKbn = e.UriHenKbn,
                    UriHenKikan = e.UriHenKikan,
                    UriKbn = e.UriKbn,
                    UriMDKbn = e.UriMDKbn,
                    UriZeroChk = e.UriZeroChk,
                    YouSagaKbn = e.YouSagaKbn,
                    YouTesuKbn = e.YouTesuKbn,
                    YoySyuKiTimeSiyoKbn = e.YoySyuKiTimeSiyoKbn,
                    YykCopyFlg = e.YykCopyFlg,
                    YykHaiSTime = e.YykHaiSTime,
                    YykTouTime = e.YykTouTime,
                    ZasyuKbn = e.ZasyuKbn,
                    TumZeiKbn = e.TumZeiKbn,
                    GridNo = (i + 1).ToString(),
                    GridCompanyCode = e.CompanyCdSeqCompanyCd.ToString(),
                    GridCompanyName = e.CompanyCdSeqCompanyNm,
                    GridSaleClassification = e.UriKbnCodeKbnNm,
                    GridTaxFraction = e.SyohiHasuCodeKbnNm,
                    GridFeeFraction = e.TesuHasuCodeKbnNm,
                    GridReportClassification = e.HoukoKbnCodeKbnNm,
                    GridReportSummary = e.HouZeiKbnCodeKbnNm,
                    GridReportOutput = e.HouOutKbnCodeKbnNm,
                    GridAutoTemporaryBus = e.JKariKbnCodeKbnNm,
                    GridAutoPriorty = e.AutKarJyunCodeKbnNm,
                    GridAutoTemporaryBusDivision = e.JKBunPatCodeKbnNm,
                    GridVehicleReplacement = e.SyaIrePatCodeKbnNm,
                    GridCrewCompatibilityCheck = e.JymAChkKbnCodeKbnNm,
                    GridSaleChange = e.UriHenKbn == 1 ? e.UriHenKbnCodeKbnNm + e.UriHenKikan + e.UriMDKbnCodeKbnNm : e.UriHenKbnCodeKbnNm,
                    GridCheckZeroYen = e.UriZeroChkCodeKbnNm,
                    GridCancelClassification = e.CanKbn == 1 ? e.CanKbnCodeKbnNm + e.CanKikan + e.CanMDKbnCodeKbnNm : e.CanKbnCodeKbnNm,
                    GridHiredBusFee = e.YouTesuKbnCodeKbnNm,
                    GridHiredBusDifferentClassification = e.YouSagaKbnCodeKbnNm,
                    GridFareByVehicle = e.SyaUntKbnCodeKbnNm,
                    GridTransportationMiscellaneousIncome = e.ZasyuKbnCodeKbnNm,
                    GridIncidentalType1Addition = e.FutSF1FlgCodeKbnNm,
                    GridIncidentalType2Addition = e.FutSF2FlgCodeKbnNm,
                    GridIncidentalType3Addition = e.FutSF3FlgCodeKbnNm,
                    GridIncidentalType4Addition = e.FutSF4FlgCodeKbnNm,
                    GridTravelOrder = e.SokoJunKbnCodeKbnNm,
                    GridFareTaxDisplay = e.UntZeiKbnCodeKbnNm,
                    GridLoadingGoodsTaxDisplay = e.TumZeiKbnCodeKbnNm,
                    GridCancelRate1 = e.CanRit1.ToString(),
                    GridCancelRate1StartTime = e.CanSKan1.ToString(),
                    GridCancelRate1EndTime = e.CanEKan1.ToString(),
                    GridCancelRate2 = e.CanRit2.ToString(),
                    GridCancelRate2StartTime = e.CanSKan2.ToString(),
                    GridCancelRate2EndTime = e.CanEKan2.ToString(),
                    GridCancelRate3 = e.CanRit3.ToString(),
                    GridCancelRate3StartTime = e.CanSKan3.ToString(),
                    GridCancelRate3EndTime = e.CanEKan3.ToString(),
                    GridCancelRate4 = e.CanRit4.ToString(),
                    GridCancelRate4StartTime = e.CanSKan4.ToString(),
                    GridCancelRate4EndTime = e.CanEKan4.ToString(),
                    GridCancelRate5 = e.CanRit5.ToString(),
                    GridCancelRate5StartTime = e.CanSKan5.ToString(),
                    GridCancelRate5EndTime = e.CanEKan5.ToString(),
                    GridCancelRate6 = e.CanRit6.ToString(),
                    GridCancelRate6StartTime = e.CanSKan6.ToString(),
                    GridCancelRate6EndTime = e.CanEKan6.ToString(),
                    GridMonthlyProcess = e.GetSyoKbnCodeKbnNm,
                    GridBillForward = e.SeiKrksKbnCodeKbnNm,
                    GridDailyProcess = e.DaySyoKbnCodeKbnNm,
                    GridAutoKoban = e.AutKouKbnCodeKbnNm,
                    GridInitCopyProcessData = e.KoteiCopyFlgCodeKbnNm,
                    GridInitCopyIncidentalData = e.FutaiCopyFlgCodeKbnNm,
                    GridInitCopyLoadingGoodData = e.TumiCopyFlgCodeKbnNm,
                    GridInitCopyArrangeData = e.TehaiCopyFlgCodeKbnNm,
                    GridInitCopyBoardingPlaceData = e.JoshaCopyFlgCodeKbnNm,
                    GridInitCopyReservationRemarkData = e.YykCopyFlgCodeKbnNm,
                    GridInitCopyOperationDateRemarkData = e.UkbCopyFlgCodeKbnNm,
                    GridInitTransferEstimateData = e.QuotationTransferCodeKbnNm,
                    GridCurrentInvoice = e.SeiGenFlgCodeKbnNm,
                    GridDisplayDetailSelection = e.MeiShyKbnCodeKbnNm,
                    GridCharacter1DisplayByBusType = e.SyaSenMjPtnKbn1CodeKbnNm,
                    GridColor1DisplayByBusType = e.SyaSenMjPtnCol1,
                    GridCharacter2DisplayByBusType = e.SyaSenMjPtnKbn2CodeKbnNm,
                    GridColor2DisplayByBusType = e.SyaSenMjPtnCol2,
                    GridCharacter3DisplayByBusType = e.SyaSenMjPtnKbn3CodeKbnNm,
                    GridColor3DisplayByBusType = e.SyaSenMjPtnCol3,
                    GridCharacter4DisplayByBusType = e.SyaSenMjPtnKbn4CodeKbnNm,
                    GridColor4DisplayByBusType = e.SyaSenMjPtnCol4,
                    GridCharacter5DisplayByBusType = e.SyaSenMjPtnKbn5CodeKbnNm,
                    GridColor5DisplayByBusType = e.SyaSenMjPtnCol5,
                    GridCharacter1DisplayByCrew = e.JyoSenMjPtnKbn1CodeKbnNm,
                    GridColor1DisplayByCrew = e.JyoSenMjPtnCol1,
                    GridCharacter2DisplayByCrew = e.JyoSenMjPtnKbn2CodeKbnNm,
                    GridColor2DisplayByCrew = e.JyoSenMjPtnCol2,
                    GridCharacter3DisplayByCrew = e.JyoSenMjPtnKbn3CodeKbnNm,
                    GridColor3DisplayByCrew = e.JyoSenMjPtnCol3,
                    GridCharacter4DisplayByCrew = e.JyoSenMjPtnKbn4CodeKbnNm,
                    GridColor4DisplayByCrew = e.JyoSenMjPtnCol4,
                    GridCharacter5DisplayByCrew = e.JyoSenMjPtnKbn5CodeKbnNm,
                    GridColor5DisplayByCrew = e.JyoSenMjPtnCol5,
                    GridBillComent1 = e.SeiCom1,
                    GridBillComent2 = e.SeiCom2,
                    GridBillComent3 = e.SeiCom3,
                    GridBillComent4 = e.SeiCom4,
                    GridBillComent5 = e.SeiCom5,
                    GridBillComent6 = e.SeiCom6,
                    GridAchievementTotalWorkHolidayTypeName1 = e.JisKinKyuNm01,
                    GridAchievementTotalWorkHolidayTypeName2 = e.JisKinKyuNm02,
                    GridAchievementTotalWorkHolidayTypeName3 = e.JisKinKyuNm03,
                    GridAchievementTotalWorkHolidayTypeName4 = e.JisKinKyuNm04,
                    GridAchievementTotalWorkHolidayTypeName5 = e.JisKinKyuNm05,
                    GridAchievementTotalWorkHolidayTypeName6 = e.JisKinKyuNm06,
                    GridAchievementTotalWorkHolidayTypeName7 = e.JisKinKyuNm07,
                    GridAchievementTotalWorkHolidayTypeName8 = e.JisKinKyuNm08,
                    GridAchievementTotalWorkHolidayTypeName9 = e.JisKinKyuNm09,
                    GridAchievementTotalWorkHolidayTypeName10 = e.JisKinKyuNm10,
                    GridCharacterClassification1DisplayByBusType = e.SyaSenInfoPtnKbn1CodeKbnNm,
                    GridColorClassification1DisplayByBusType = e.SyaSenInfoPtnCol1,
                    GridCharacterClassification2DisplayByBusType = e.SyaSenInfoPtnKbn2CodeKbnNm,
                    GridColorClassification2DisplayByBusType = e.SyaSenInfoPtnCol2,
                    GridCharacterClassification3DisplayByBusType = e.SyaSenInfoPtnKbn3CodeKbnNm,
                    GridColorClassification3DisplayByBusType = e.SyaSenInfoPtnCol3,
                    GridCharacterClassification4DisplayByBusType = e.SyaSenInfoPtnKbn4CodeKbnNm,
                    GridColorClassification4DisplayByBusType = e.SyaSenInfoPtnCol4,
                    GridCharacterClassification5DisplayByBusType = e.SyaSenInfoPtnKbn5CodeKbnNm,
                    GridColorClassification5DisplayByBusType = e.SyaSenInfoPtnCol5,
                    GridCharacterClassification6DisplayByBusType = e.SyaSenInfoPtnKbn6CodeKbnNm,
                    GridColorClassification6DisplayByBusType = e.SyaSenInfoPtnCol6,
                    GridCharacterClassification7DisplayByBusType = e.SyaSenInfoPtnKbn7CodeKbnNm,
                    GridColorClassification7DisplayByBusType = e.SyaSenInfoPtnCol7,
                    GridCharacterClassification8DisplayByBusType = e.SyaSenInfoPtnKbn8CodeKbnNm,
                    GridColorClassification8DisplayByBusType = e.SyaSenInfoPtnCol8,
                    GridCharacterClassification9DisplayByBusType = e.SyaSenInfoPtnKbn9CodeKbnNm,
                    GridColorClassification9DisplayByBusType = e.SyaSenInfoPtnCol9,
                    GridCharacterClassification10DisplayByBusType = e.SyaSenInfoPtnKbn10CodeKbnNm,
                    GridColorClassification10DisplayByBusType = e.SyaSenInfoPtnCol10,
                    GridCharacterClassification11DisplayByBusType = e.SyaSenInfoPtnKbn11CodeKbnNm,
                    GridColorClassification11DisplayByBusType = e.SyaSenInfoPtnCol11,
                    GridCharacterClassification12DisplayByBusType = e.SyaSenInfoPtnKbn12CodeKbnNm,
                    GridColorClassification12DisplayByBusType = e.SyaSenInfoPtnCol12,
                    GridCharacterClassification13DisplayByBusType = e.SyaSenInfoPtnKbn13CodeKbnNm,
                    GridColorClassification13DisplayByBusType = e.SyaSenInfoPtnCol13,
                    GridCharacterClassification14DisplayByBusType = e.SyaSenInfoPtnKbn14CodeKbnNm,
                    GridColorClassification14DisplayByBusType = e.SyaSenInfoPtnCol14,
                    GridCharacterClassification15DisplayByBusType = e.SyaSenInfoPtnKbn15CodeKbnNm,
                    GridColorClassification15DisplayByBusType = e.SyaSenInfoPtnCol15,
                    GridCharacterClassification1DisplayByCrewType = e.JyoSenInfoPtnKbn1CodeKbnNm,
                    GridColorClassification1DisplayByCrewType = e.JyoSenInfoPtnCol1,
                    GridCharacterClassification2DisplayByCrewType = e.JyoSenInfoPtnKbn2CodeKbnNm,
                    GridColorClassification2DisplayByCrewType = e.JyoSenInfoPtnCol2,
                    GridCharacterClassification3DisplayByCrewType = e.JyoSenInfoPtnKbn3CodeKbnNm,
                    GridColorClassification3DisplayByCrewType = e.JyoSenInfoPtnCol3,
                    GridCharacterClassification4DisplayByCrewType = e.JyoSenInfoPtnKbn4CodeKbnNm,
                    GridColorClassification4DisplayByCrewType = e.JyoSenInfoPtnCol4,
                    GridCharacterClassification5DisplayByCrewType = e.JyoSenInfoPtnKbn5CodeKbnNm,
                    GridColorClassification5DisplayByCrewType = e.JyoSenInfoPtnCol5,
                    GridCharacterClassification6DisplayByCrewType = e.JyoSenInfoPtnKbn6CodeKbnNm,
                    GridColorClassification6DisplayByCrewType = e.JyoSenInfoPtnCol6,
                    GridCharacterClassification7DisplayByCrewType = e.JyoSenInfoPtnKbn7CodeKbnNm,
                    GridColorClassification7DisplayByCrewType =e.JyoSenInfoPtnCol7,
                    GridCharacterClassification8DisplayByCrewType = e.JyoSenInfoPtnKbn8CodeKbnNm,
                    GridColorClassification8DisplayByCrewType =e.JyoSenInfoPtnCol8,
                    GridCharacterClassification9DisplayByCrewType = e.JyoSenInfoPtnKbn9CodeKbnNm,
                    GridColorClassification9DisplayByCrewType = e.JyoSenInfoPtnCol9,
                    GridCharacterClassification10DisplayByCrewType = e.JyoSenInfoPtnKbn10CodeKbnNm,
                    GridColorClassification10DisplayByCrewType = e.JyoSenInfoPtnCol10,
                    GridCharacterClassification11DisplayByCrewType = e.JyoSenInfoPtnKbn11CodeKbnNm,
                    GridColorClassification11DisplayByCrewType = e.JyoSenInfoPtnCol11,
                    GridCharacterClassification12DisplayByCrewType = e.JyoSenInfoPtnKbn12CodeKbnNm,
                    GridColorClassification12DisplayByCrewType = e.JyoSenInfoPtnCol12,
                    GridCharacterClassification13DisplayByCrewType = e.JyoSenInfoPtnKbn13CodeKbnNm,
                    GridColorClassification13DisplayByCrewType = e.JyoSenInfoPtnCol13,
                    GridCharacterClassification14DisplayByCrewType = e.JyoSenInfoPtnKbn14CodeKbnNm,
                    GridColorClassification14DisplayByCrewType = e.JyoSenInfoPtnCol14,
                    GridCharacterClassification15DisplayByCrewType = e.JyoSenInfoPtnKbn15CodeKbnNm,
                    GridColorClassification15DisplayByCrewType = e.JyoSenInfoPtnCol15,
                    GridExtendItem = e.ExpItem,
                    GridLastUpdateDate = e.UpdYmd,
                    GridLastUpdateTime = e.UpdTime,
                    GridLastUpdateStaffCode = e.SyainCd,
                    GridLastUpdateStaffName = e.SyainNm,
                    GridLastUpdatePgId = e.UpdPrgID
                }).ToList();
                displayItems = items;
                InvokeAsync(StateHasChanged);
                await _loading.HideAsync();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
            
        }

        private string GetColor(string val) => string.Format(colorFormat, val);

        private void InitTable()
        {
            Header = new HeaderTemplate()
            {
                Rows = new List<RowHeaderTemplate>()
                {
                    new RowHeaderTemplate()
                    {
                        Columns = new List<ColumnHeaderTemplate>()
                        {
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridNo), ColName = _lang[nameof(RegulationSettingItem.GridNo)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCompanyCode), ColName = _lang[nameof(RegulationSettingItem.GridCompanyCode)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCompanyName), ColName = _lang[nameof(RegulationSettingItem.GridCompanyName)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridSaleClassification), ColName = _lang[nameof(RegulationSettingItem.GridSaleClassification)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridTaxFraction), ColName = _lang[nameof(RegulationSettingItem.GridTaxFraction)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridFeeFraction), ColName = _lang[nameof(RegulationSettingItem.GridFeeFraction)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridReportClassification), ColName = _lang[nameof(RegulationSettingItem.GridReportClassification)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridReportSummary), ColName = _lang[nameof(RegulationSettingItem.GridReportSummary)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridReportOutput), ColName = _lang[nameof(RegulationSettingItem.GridReportOutput)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoTemporaryBus), ColName = _lang[nameof(RegulationSettingItem.GridAutoTemporaryBus)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoPriorty), ColName = _lang[nameof(RegulationSettingItem.GridAutoPriorty)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoTemporaryBusDivision), ColName = _lang[nameof(RegulationSettingItem.GridAutoTemporaryBusDivision)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridVehicleReplacement), ColName = _lang[nameof(RegulationSettingItem.GridVehicleReplacement)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCrewCompatibilityCheck), ColName = _lang[nameof(RegulationSettingItem.GridCrewCompatibilityCheck)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridSaleChange), ColName = _lang[nameof(RegulationSettingItem.GridSaleChange)] },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCheckZeroYen), ColName = _lang[nameof(RegulationSettingItem.GridCheckZeroYen)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelClassification), ColName = _lang[nameof(RegulationSettingItem.GridCancelClassification)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridHiredBusFee), ColName = _lang[nameof(RegulationSettingItem.GridHiredBusFee)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridHiredBusDifferentClassification), ColName = _lang[nameof(RegulationSettingItem.GridHiredBusDifferentClassification)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridFareByVehicle), ColName = _lang[nameof(RegulationSettingItem.GridFareByVehicle)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridTransportationMiscellaneousIncome), ColName = _lang[nameof(RegulationSettingItem.GridTransportationMiscellaneousIncome)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType1Addition), ColName = _lang[nameof(RegulationSettingItem.GridIncidentalType1Addition)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType2Addition), ColName = _lang[nameof(RegulationSettingItem.GridIncidentalType2Addition)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType3Addition), ColName = _lang[nameof(RegulationSettingItem.GridIncidentalType3Addition)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType4Addition), ColName = _lang[nameof(RegulationSettingItem.GridIncidentalType4Addition)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridTravelOrder), ColName = _lang[nameof(RegulationSettingItem.GridTravelOrder)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridFareTaxDisplay), ColName = _lang[nameof(RegulationSettingItem.GridFareTaxDisplay)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLoadingGoodsTaxDisplay), ColName = _lang[nameof(RegulationSettingItem.GridLoadingGoodsTaxDisplay)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate1)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate1StartTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate1EndTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate2)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate2StartTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate2EndTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate3)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate3StartTime)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate3EndTime)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate4)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate4StartTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate4EndTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate5)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate5StartTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate5EndTime)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate6)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6StartTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate6StartTime)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6EndTime), ColName = _lang[nameof(RegulationSettingItem.GridCancelRate6EndTime)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridMonthlyProcess), ColName = _lang[nameof(RegulationSettingItem.GridMonthlyProcess)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillForward), ColName = _lang[nameof(RegulationSettingItem.GridBillForward)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridDailyProcess), ColName = _lang[nameof(RegulationSettingItem.GridDailyProcess)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoKoban), ColName = _lang[nameof(RegulationSettingItem.GridAutoKoban)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyProcessData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyProcessData)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyIncidentalData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyIncidentalData)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyLoadingGoodData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyLoadingGoodData)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyArrangeData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyArrangeData)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyBoardingPlaceData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyBoardingPlaceData)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyReservationRemarkData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyReservationRemarkData)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyOperationDateRemarkData), ColName = _lang[nameof(RegulationSettingItem.GridInitCopyOperationDateRemarkData)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridInitTransferEstimateData), ColName = _lang[nameof(RegulationSettingItem.GridInitTransferEstimateData)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCurrentInvoice), ColName = _lang[nameof(RegulationSettingItem.GridCurrentInvoice)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridDisplayDetailSelection), ColName = _lang[nameof(RegulationSettingItem.GridDisplayDetailSelection)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter1DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacter1DisplayByBusType)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor1DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColor1DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter2DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacter2DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor2DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColor2DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter3DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacter3DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor3DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColor3DisplayByBusType)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter4DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacter4DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor4DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColor4DisplayByBusType)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter5DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacter5DisplayByBusType)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor5DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColor5DisplayByBusType)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter1DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridCharacter1DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor1DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridColor1DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter2DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridCharacter2DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor2DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridColor2DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter3DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridCharacter3DisplayByCrew)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor3DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridColor3DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter4DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridCharacter4DisplayByCrew)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor4DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridColor4DisplayByCrew)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter5DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridCharacter5DisplayByCrew)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColor5DisplayByCrew), ColName = _lang[nameof(RegulationSettingItem.GridColor5DisplayByCrew)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent1), ColName = _lang[nameof(RegulationSettingItem.GridBillComent1)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent2), ColName = _lang[nameof(RegulationSettingItem.GridBillComent2)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent3), ColName = _lang[nameof(RegulationSettingItem.GridBillComent3)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent4), ColName = _lang[nameof(RegulationSettingItem.GridBillComent4)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent5), ColName = _lang[nameof(RegulationSettingItem.GridBillComent5)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent6), ColName = _lang[nameof(RegulationSettingItem.GridBillComent6)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName1), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName1)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName2), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName2)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName3), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName3)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName4), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName4)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName5), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName5)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName6), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName6)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName7), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName7)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName8), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName8)] , Width = 250},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName9), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName9)], Width = 250 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName10), ColName = _lang[nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName10)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification1DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification1DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification1DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification2DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification2DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification2DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification3DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification3DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification3DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification4DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification4DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification4DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification5DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification5DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification5DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification6DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification6DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification6DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification7DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification7DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification7DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification8DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification8DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification8DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification9DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification9DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification9DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification10DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification10DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification10DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification11DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification11DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification11DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification12DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification12DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification12DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification13DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification13DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification13DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification14DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification14DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification14DisplayByBusType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification15DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification15DisplayByBusType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification15DisplayByBusType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification1DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification1DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification1DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification2DisplayByCrewType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification2DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification2DisplayByCrewType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification3DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification3DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification3DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification4DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification4DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification4DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification5DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification5DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification5DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification6DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification6DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification6DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification7DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification7DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification7DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification8DisplayByCrewType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification8DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification8DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification9DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification9DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification9DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification10DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification10DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification10DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification11DisplayByCrewType)] , Width = 300},
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification11DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification11DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification12DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification12DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification12DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification13DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification13DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification13DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification14DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification14DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification14DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridCharacterClassification15DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification15DisplayByCrewType), ColName = _lang[nameof(RegulationSettingItem.GridColorClassification15DisplayByCrewType)], Width = 300 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridExtendItem), ColName = _lang[nameof(RegulationSettingItem.GridExtendItem)], Width = 200 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateDate), ColName = _lang[nameof(RegulationSettingItem.GridLastUpdateDate)], Width = 200 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateTime), ColName = _lang[nameof(RegulationSettingItem.GridLastUpdateTime)], Width = 200 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateStaffCode), ColName = _lang[nameof(RegulationSettingItem.GridLastUpdateStaffCode)], Width = 200 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateStaffName), ColName = _lang[nameof(RegulationSettingItem.GridLastUpdateStaffName)], Width = 200 },
                            new ColumnHeaderTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdatePgId), ColName = _lang[nameof(RegulationSettingItem.GridLastUpdatePgId)], Width = 200 }
                        }
                    }
                }
            };

            Body = new BodyTemplate()
            {
                Rows = new List<RowBodyTemplate>()
                {
                    new RowBodyTemplate()
                    {
                        Columns = new List<ColumnBodyTemplate>()
                        {
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridNo), DisplayFieldName = nameof(RegulationSettingItem.GridNo), AlignCol = AlignColEnum.Center },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCompanyCode), DisplayFieldName = nameof(RegulationSettingItem.GridCompanyCode), CustomTextFormatDelegate = KoboGridHelper.PadLeft5 , AlignCol = AlignColEnum.Center},
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCompanyName), DisplayFieldName = nameof(RegulationSettingItem.GridCompanyName) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridSaleClassification), DisplayFieldName = nameof(RegulationSettingItem.GridSaleClassification) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridTaxFraction), DisplayFieldName = nameof(RegulationSettingItem.GridTaxFraction) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridFeeFraction), DisplayFieldName = nameof(RegulationSettingItem.GridFeeFraction) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridReportClassification), DisplayFieldName = nameof(RegulationSettingItem.GridReportClassification) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridReportSummary), DisplayFieldName = nameof(RegulationSettingItem.GridReportSummary) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridReportOutput), DisplayFieldName = nameof(RegulationSettingItem.GridReportOutput) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoTemporaryBus), DisplayFieldName = nameof(RegulationSettingItem.GridAutoTemporaryBus) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoPriorty), DisplayFieldName = nameof(RegulationSettingItem.GridAutoPriorty) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoTemporaryBusDivision), DisplayFieldName = nameof(RegulationSettingItem.GridAutoTemporaryBusDivision) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridVehicleReplacement), DisplayFieldName = nameof(RegulationSettingItem.GridVehicleReplacement) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCrewCompatibilityCheck), DisplayFieldName = nameof(RegulationSettingItem.GridCrewCompatibilityCheck) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridSaleChange), DisplayFieldName = nameof(RegulationSettingItem.GridSaleChange) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCheckZeroYen), DisplayFieldName = nameof(RegulationSettingItem.GridCheckZeroYen) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelClassification), DisplayFieldName = nameof(RegulationSettingItem.GridCancelClassification) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridHiredBusFee), DisplayFieldName = nameof(RegulationSettingItem.GridHiredBusFee) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridHiredBusDifferentClassification), DisplayFieldName = nameof(RegulationSettingItem.GridHiredBusDifferentClassification) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridFareByVehicle), DisplayFieldName = nameof(RegulationSettingItem.GridFareByVehicle) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridTransportationMiscellaneousIncome), DisplayFieldName = nameof(RegulationSettingItem.GridTransportationMiscellaneousIncome) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType1Addition), DisplayFieldName = nameof(RegulationSettingItem.GridIncidentalType1Addition) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType2Addition), DisplayFieldName = nameof(RegulationSettingItem.GridIncidentalType2Addition) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType3Addition), DisplayFieldName = nameof(RegulationSettingItem.GridIncidentalType3Addition) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridIncidentalType4Addition), DisplayFieldName = nameof(RegulationSettingItem.GridIncidentalType4Addition) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridTravelOrder), DisplayFieldName = nameof(RegulationSettingItem.GridTravelOrder) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridFareTaxDisplay), DisplayFieldName = nameof(RegulationSettingItem.GridFareTaxDisplay) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLoadingGoodsTaxDisplay), DisplayFieldName = nameof(RegulationSettingItem.GridLoadingGoodsTaxDisplay) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate1) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate1StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate1EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate1EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate2) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate2StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate2EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate2EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate3) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate3StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate3EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate3EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate4) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate4StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate4EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate4EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate5) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate5StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate5EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate5EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate6) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6StartTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate6StartTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCancelRate6EndTime), DisplayFieldName = nameof(RegulationSettingItem.GridCancelRate6EndTime) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridMonthlyProcess), DisplayFieldName = nameof(RegulationSettingItem.GridMonthlyProcess) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillForward), DisplayFieldName = nameof(RegulationSettingItem.GridBillForward) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridDailyProcess), DisplayFieldName = nameof(RegulationSettingItem.GridDailyProcess) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAutoKoban), DisplayFieldName = nameof(RegulationSettingItem.GridAutoKoban) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyProcessData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyProcessData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyIncidentalData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyIncidentalData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyLoadingGoodData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyLoadingGoodData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyArrangeData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyArrangeData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyBoardingPlaceData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyBoardingPlaceData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyReservationRemarkData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyReservationRemarkData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitCopyOperationDateRemarkData), DisplayFieldName = nameof(RegulationSettingItem.GridInitCopyOperationDateRemarkData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridInitTransferEstimateData), DisplayFieldName = nameof(RegulationSettingItem.GridInitTransferEstimateData) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCurrentInvoice), DisplayFieldName = nameof(RegulationSettingItem.GridCurrentInvoice) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridDisplayDetailSelection), DisplayFieldName = nameof(RegulationSettingItem.GridDisplayDetailSelection) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter1DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter1DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor1DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor1DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter2DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter2DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor2DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor2DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter3DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter3DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor3DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor3DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter4DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter4DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor4DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor4DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter5DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter5DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor5DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor5DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter1DisplayByCrew), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter1DisplayByCrew) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor1DisplayByCrew), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor1DisplayByCrew) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter2DisplayByCrew), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter2DisplayByCrew) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor2DisplayByCrew), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor2DisplayByCrew) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter3DisplayByCrew), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter3DisplayByCrew) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor3DisplayByCrew), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor3DisplayByCrew) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter4DisplayByCrew), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter4DisplayByCrew) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor4DisplayByCrew), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor4DisplayByCrew) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacter5DisplayByCrew), DisplayFieldName = nameof(RegulationSettingItem.GridCharacter5DisplayByCrew) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColor5DisplayByCrew), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColor5DisplayByCrew) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent1), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent1) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent2), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent2) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent3), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent3) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent4), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent4) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent5), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent5) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridBillComent6), DisplayFieldName = nameof(RegulationSettingItem.GridBillComent6) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName1), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName1) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName2), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName2) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName3), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName3) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName4), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName4) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName5), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName5) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName6), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName6) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName7), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName7) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName8), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName8) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName9), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName9) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName10), DisplayFieldName = nameof(RegulationSettingItem.GridAchievementTotalWorkHolidayTypeName10) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification1DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification1DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification2DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification2DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification3DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification3DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification4DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification4DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification5DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification5DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification6DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification6DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification7DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification7DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification8DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification8DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification9DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification9DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification10DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification10DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification11DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification11DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification12DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification12DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification13DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification13DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification14DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification14DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByBusType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByBusType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification15DisplayByBusType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification15DisplayByBusType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification1DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification1DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification1DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification2DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification2DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification2DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification3DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification3DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification3DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification4DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification4DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification4DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification5DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification5DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification5DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification6DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification6DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification6DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification7DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification7DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification7DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification8DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification8DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification8DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification9DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification9DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification9DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification10DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification10DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification10DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification11DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification11DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification11DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification12DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification12DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification12DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification13DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification13DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification13DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification14DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification14DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification14DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByCrewType), DisplayFieldName = nameof(RegulationSettingItem.GridCharacterClassification15DisplayByCrewType) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridColorClassification15DisplayByCrewType), Control = new CellColorControl<RegulationSettingItem>() { ColorDelegate = item => GetColor(item.GridColorClassification15DisplayByCrewType) } },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridExtendItem), DisplayFieldName = nameof(RegulationSettingItem.GridExtendItem) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateDate), DisplayFieldName = nameof(RegulationSettingItem.GridLastUpdateDate), CustomTextFormatDelegate = KoboGridHelper.FormatYYYYMMDDDelegate },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateTime), DisplayFieldName = nameof(RegulationSettingItem.GridLastUpdateTime), CustomTextFormatDelegate = KoboGridHelper.FormatHHMMSSDelegate },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateStaffCode), DisplayFieldName = nameof(RegulationSettingItem.GridLastUpdateStaffCode), CustomTextFormatDelegate = KoboGridHelper.PadLeft10 },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdateStaffName), DisplayFieldName = nameof(RegulationSettingItem.GridLastUpdateStaffName) },
                            new ColumnBodyTemplate() { CodeName = nameof(RegulationSettingItem.GridLastUpdatePgId), DisplayFieldName = nameof(RegulationSettingItem.GridLastUpdatePgId) }
                        }
                    }
                }
            };
        }

        protected void TriggerTabChange()
        {
            StateHasChanged();
        }

        protected async Task CompanyChanged(CompanyListItem item, bool isFrom)
        {
            try
            {
                if (isFrom) model.CompanyFrom = item; else model.CompanyTo = item;
                if (formContext.Validate())
                {
                    LoadData();
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnChangePage(int page)
        {
            try
            {
                currentPage = page;
                displayItems = items.Skip(currentPage * itemPerPage).Take(itemPerPage).ToList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void OnChangeItemPerPage(byte _itemPerPage)
        {
            try
            {
                itemPerPage = _itemPerPage;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected async Task ClearBtnOnClick()
        {
            try
            {
                model = new RegulationSettingModel();
                await LoadData();
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _errService.HandleError(ex);
            }
        }

        protected void ShowPopup()
        {
            showPopup = true;
            isCreate = true;
            StateHasChanged();
        }

        protected void OnDbClick(RegulationSettingItem selected)
        {
            showPopup = true;
            isCreate = false;
            RegulationSettingForm.LoadData(selected);
            StateHasChanged();
        }

        public void Dispose()
        {
            source.Cancel();
        }
    }
}

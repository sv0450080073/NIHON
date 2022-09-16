using HassyaAllrightCloud.Commons.Constants;
using HassyaAllrightCloud.Commons.Extensions;
using HassyaAllrightCloud.Commons.Helpers;
using HassyaAllrightCloud.Domain.Dto;
using HassyaAllrightCloud.IService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Pages
{
    public class MonthlyTransportationResultInputBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jSRuntime { get; set; }
        [Inject]
        protected IStringLocalizer<MonthlyTransportationResultInput> _lang { get; set; }
        [Inject]
        protected IMonthlyTransportationService _service { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        protected IErrorHandlerService errorModalService { get; set; }
        [Inject]
        private ILoadingService _loading { get; set; }
        [Parameter]
        public string searchString { get; set; }

        protected MonthlyTransportationModel monthlyTransportationModel = new MonthlyTransportationModel();
        protected EditContext formContext { get; set; }
        protected int activeTabIndex = 0;
        protected int ActiveTabIndex { get => activeTabIndex; set { activeTabIndex = value; StateHasChanged(); } }
        protected bool isFirstRender { get; set; } = true;

        //To save state collapse or expand 
        protected string ValueTab1Big { get; set; } = "collapse show";
        protected string ValueTab1Medium { get; set; } = "collapse show";
        protected string ValueTab1Small { get; set; } = "collapse show";
        protected string ValueTab2Big { get; set; } = "collapse show";
        protected string ValueTab2Medium { get; set; } = "collapse show";
        protected string ValueTab2Small { get; set; } = "collapse show";
        protected string ValueAtributeTab1Big { get; set; } = "";
        protected string ValueAtributeTab1Medium { get; set; } = "";
        protected string ValueAtributeTab1Small { get; set; } = "";
        protected string ValueAtributeTab2Big { get; set; } = "";
        protected string ValueAtributeTab2Medium { get; set; } = "";
        protected string ValueAtributeTab2Small { get; set; } = "";
        protected bool isExpandTab1Big { get; set; } = true;
        protected bool isExpandTab1Medium { get; set; } = true;
        protected bool isExpandTab1Small { get; set; } = true;
        protected bool isExpandTab2Big { get; set; } = true;
        protected bool isExpandTab2Medium { get; set; } = true;
        protected bool isExpandTab2Small { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                monthlyTransportationModel = new MonthlyTransportationModel();
                formContext = new EditContext(monthlyTransportationModel);
                var searchParams = EncryptHelper.DecryptFromUrl<SearchParam>(searchString);
                var resultdata = await _service.GetJitHouDataResultInput(searchParams);

                var largeOil = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Big && x.NenRyoKbn == (int)NenRyoKbn.LightOil);
                var largeGasoline = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Big && x.NenRyoKbn == (int)NenRyoKbn.Gasoline);
                var largeLPG = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Big && x.NenRyoKbn == (int)NenRyoKbn.LPG);
                var largeGasTurbine = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Big && x.NenRyoKbn == (int)NenRyoKbn.GasTurbine);
                var largeOther = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Big && x.NenRyoKbn == (int)NenRyoKbn.Other);

                var mediumOil = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Medium && x.NenRyoKbn == (int)NenRyoKbn.LightOil);
                var mediumGasoline = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Medium && x.NenRyoKbn == (int)NenRyoKbn.Gasoline);
                var mediumLPG = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Medium && x.NenRyoKbn == (int)NenRyoKbn.LPG);
                var mediumGasTurbine = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Medium && x.NenRyoKbn == (int)NenRyoKbn.GasTurbine);
                var mediumOther = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Medium && x.NenRyoKbn == (int)NenRyoKbn.Other);

                var smallOil = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Small && x.NenRyoKbn == (int)NenRyoKbn.LightOil);
                var smallGasoline = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Small && x.NenRyoKbn == (int)NenRyoKbn.Gasoline);
                var smallLPG = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Small && x.NenRyoKbn == (int)NenRyoKbn.LPG);
                var smallGasTurbine = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Small && x.NenRyoKbn == (int)NenRyoKbn.GasTurbine);
                var smallOther = resultdata.FirstOrDefault(x => x.KataKbn == (int)KataKbn.Small && x.NenRyoKbn == (int)NenRyoKbn.Other);

                monthlyTransportationModel = new MonthlyTransportationModel
                {
                    ////////「輸送実績報告書1」タブ
                    ////大型
                    //軽油
                    LargeOil_NobeJyoCnt = largeOil?.NobeJyoCnt == 0 ? null : largeOil?.NobeJyoCnt,
                    LargeOil_NobeRinCnt = largeOil?.NobeRinCnt == 0 ? null : largeOil?.NobeRinCnt,
                    LargeOil_NobeSumCnt = largeOil?.NobeSumCnt == 0 ? null : largeOil?.NobeSumCnt,
                    LargeOil_NobeJitCnt = largeOil?.NobeJitCnt == 0 ? null : largeOil?.NobeJitCnt,
                    LargeOil_JitudoRitu = largeOil?.JitudoRitu == 0 ? null : largeOil?.JitudoRitu,
                    LargeOil_RinjiRitu = largeOil?.RinjiRitu == 0 ? null : largeOil?.RinjiRitu,
                    LargeOil_JitJisaKm = largeOil?.JitJisaKm == 0 ? null : largeOil?.JitJisaKm,
                    LargeOil_JitKisoKm = largeOil?.JitKisoKm == 0 ? null : largeOil?.JitKisoKm,
                    LargeOil_KmKei = (largeOil?.JitJisaKm + largeOil?.JitKisoKm) == 0 ? null :
                                     (largeOil?.JitJisaKm + largeOil?.JitKisoKm),
                    LargeOil_YusoJin = largeOil?.YusoJin == 0 ? null : largeOil?.YusoJin,

                    //ガソリン
                    LargeGasoline_NobeJyoCnt = largeGasoline?.NobeJyoCnt == 0 ? null : largeGasoline?.NobeJyoCnt,
                    LargeGasoline_NobeRinCnt = largeGasoline?.NobeRinCnt == 0 ? null : largeGasoline?.NobeRinCnt,
                    LargeGasoline_NobeSumCnt = largeGasoline?.NobeSumCnt == 0 ? null : largeGasoline?.NobeSumCnt,
                    LargeGasoline_NobeJitCnt = largeGasoline?.NobeJitCnt == 0 ? null : largeGasoline?.NobeJitCnt,
                    LargeGasoline_JitudoRitu = largeGasoline?.JitudoRitu == 0 ? null : largeGasoline?.JitudoRitu,
                    LargeGasoline_RinjiRitu = largeGasoline?.RinjiRitu == 0 ? null : largeGasoline?.RinjiRitu,
                    LargeGasoline_JitJisaKm = largeGasoline?.JitJisaKm == 0 ? null : largeGasoline?.JitJisaKm,
                    LargeGasoline_JitKisoKm = largeGasoline?.JitKisoKm == 0 ? null : largeGasoline?.JitKisoKm,
                    LargeGasoline_KmKei = (largeGasoline?.JitJisaKm + largeGasoline?.JitKisoKm) == 0 ? null :
                                          (largeGasoline?.JitJisaKm + largeGasoline?.JitKisoKm),
                    LargeGasoline_YusoJin = largeGasoline?.YusoJin == 0 ? null : largeGasoline?.YusoJin,

                    //LPG
                    LargeLPG_NobeJyoCnt = largeLPG?.NobeJyoCnt == 0 ? null : largeLPG?.NobeJyoCnt,
                    LargeLPG_NobeRinCnt = largeLPG?.NobeRinCnt == 0 ? null : largeLPG?.NobeRinCnt,
                    LargeLPG_NobeSumCnt = largeLPG?.NobeSumCnt == 0 ? null : largeLPG?.NobeSumCnt,
                    LargeLPG_NobeJitCnt = largeLPG?.NobeJitCnt == 0 ? null : largeLPG?.NobeJitCnt,
                    LargeLPG_JitudoRitu = largeLPG?.JitudoRitu == 0 ? null : largeLPG?.JitudoRitu,
                    LargeLPG_RinjiRitu = largeLPG?.RinjiRitu == 0 ? null : largeLPG?.RinjiRitu,
                    LargeLPG_JitJisaKm = largeLPG?.JitJisaKm == 0 ? null : largeLPG?.JitJisaKm,
                    LargeLPG_JitKisoKm = largeLPG?.JitKisoKm == 0 ? null : largeLPG?.JitKisoKm,
                    LargeLPG_KmKei = (largeLPG?.JitJisaKm + largeLPG?.JitKisoKm) == 0 ? null :
                                    (largeLPG?.JitJisaKm + largeLPG?.JitKisoKm),
                    LargeLPG_YusoJin = largeLPG?.YusoJin == 0 ? null : largeLPG?.YusoJin,

                    //ガソリン
                    LargeGasTurbine_NobeJyoCnt = largeGasTurbine?.NobeJyoCnt == 0 ? null : largeGasTurbine?.NobeJyoCnt,
                    LargeGasTurbine_NobeRinCnt = largeGasTurbine?.NobeRinCnt == 0 ? null : largeGasTurbine?.NobeRinCnt,
                    LargeGasTurbine_NobeSumCnt = largeGasTurbine?.NobeSumCnt == 0 ? null : largeGasTurbine?.NobeSumCnt,
                    LargeGasTurbine_NobeJitCnt = largeGasTurbine?.NobeJitCnt == 0 ? null : largeGasTurbine?.NobeJitCnt,
                    LargeGasTurbine_JitudoRitu = largeGasTurbine?.JitudoRitu == 0 ? null : largeGasTurbine?.JitudoRitu,
                    LargeGasTurbine_RinjiRitu = largeGasTurbine?.RinjiRitu == 0 ? null : largeGasTurbine?.RinjiRitu,
                    LargeGasTurbine_JitJisaKm = largeGasTurbine?.JitJisaKm == 0 ? null : largeGasTurbine?.JitJisaKm,
                    LargeGasTurbine_JitKisoKm = largeGasTurbine?.JitKisoKm == 0 ? null : largeGasTurbine?.JitKisoKm,
                    LargeGasTurbine_KmKei = (largeGasTurbine?.JitJisaKm + largeGasTurbine?.JitKisoKm) == 0 ? null :
                                            (largeGasTurbine?.JitJisaKm + largeGasTurbine?.JitKisoKm),
                    LargeGasTurbine_YusoJin = largeGasTurbine?.YusoJin == 0 ? null : largeGasTurbine?.YusoJin,

                    //その他
                    LargeOther_NobeJyoCnt = largeOther?.NobeJyoCnt == 0 ? null : largeOther?.NobeJyoCnt,
                    LargeOther_NobeRinCnt = largeOther?.NobeRinCnt == 0 ? null : largeOther?.NobeRinCnt,
                    LargeOther_NobeSumCnt = largeOther?.NobeSumCnt == 0 ? null : largeOther?.NobeSumCnt,
                    LargeOther_NobeJitCnt = largeOther?.NobeJitCnt == 0 ? null : largeOther?.NobeJitCnt,
                    LargeOther_JitudoRitu = largeOther?.JitudoRitu == 0 ? null : largeOther?.JitudoRitu,
                    LargeOther_RinjiRitu = largeOther?.RinjiRitu == 0 ? null : largeOther?.RinjiRitu,
                    LargeOther_JitJisaKm = largeOther?.JitJisaKm == 0 ? null : largeOther?.JitJisaKm,
                    LargeOther_JitKisoKm = largeOther?.JitKisoKm == 0 ? null : largeOther?.JitKisoKm,
                    LargeOther_KmKei = (largeOther?.JitJisaKm + largeOther?.JitKisoKm) == 0 ? null :
                                        (largeOther?.JitJisaKm + largeOther?.JitKisoKm),
                    LargeOther_YusoJin = largeOther?.YusoJin == 0 ? null : largeOther?.YusoJin,

                    ////中型
                    //軽油
                    MediumOil_NobeJyoCnt = mediumOil?.NobeJyoCnt == 0 ? null : mediumOil?.NobeJyoCnt,
                    MediumOil_NobeRinCnt = mediumOil?.NobeRinCnt == 0 ? null : mediumOil?.NobeRinCnt,
                    MediumOil_NobeSumCnt = mediumOil?.NobeSumCnt == 0 ? null : mediumOil?.NobeSumCnt,
                    MediumOil_NobeJitCnt = mediumOil?.NobeJitCnt == 0 ? null : mediumOil?.NobeJitCnt,
                    MediumOil_JitudoRitu = mediumOil?.JitudoRitu == 0 ? null : mediumOil?.JitudoRitu,
                    MediumOil_RinjiRitu = mediumOil?.RinjiRitu == 0 ? null : mediumOil?.RinjiRitu,
                    MediumOil_JitJisaKm = mediumOil?.JitJisaKm == 0 ? null : mediumOil?.JitJisaKm,
                    MediumOil_JitKisoKm = mediumOil?.JitKisoKm == 0 ? null : mediumOil?.JitKisoKm,
                    MediumOil_KmKei = (mediumOil?.JitJisaKm + mediumOil?.JitKisoKm) == 0 ? null :
                                        (mediumOil?.JitJisaKm + mediumOil?.JitKisoKm),
                    MediumOil_YusoJin = mediumOil?.YusoJin == 0 ? null : mediumOil?.YusoJin,

                    //ガソリン
                    MediumGasoline_NobeJyoCnt = mediumGasoline?.NobeJyoCnt == 0 ? null : mediumGasoline?.NobeJyoCnt,
                    MediumGasoline_NobeRinCnt = mediumGasoline?.NobeRinCnt == 0 ? null : mediumGasoline?.NobeRinCnt,
                    MediumGasoline_NobeSumCnt = mediumGasoline?.NobeSumCnt == 0 ? null : mediumGasoline?.NobeSumCnt,
                    MediumGasoline_NobeJitCnt = mediumGasoline?.NobeJitCnt == 0 ? null : mediumGasoline?.NobeJitCnt,
                    MediumGasoline_JitudoRitu = mediumGasoline?.JitudoRitu == 0 ? null : mediumGasoline?.JitudoRitu,
                    MediumGasoline_RinjiRitu = mediumGasoline?.RinjiRitu == 0 ? null : mediumGasoline?.RinjiRitu,
                    MediumGasoline_JitJisaKm = mediumGasoline?.JitJisaKm == 0 ? null : mediumGasoline?.JitJisaKm,
                    MediumGasoline_JitKisoKm = mediumGasoline?.JitKisoKm == 0 ? null : mediumGasoline?.JitKisoKm,
                    MediumGasoline_KmKei = (mediumGasoline?.JitJisaKm + mediumGasoline?.JitKisoKm) == 0 ? null :
                                        (mediumGasoline?.JitJisaKm + mediumGasoline?.JitKisoKm),
                    MediumGasoline_YusoJin = mediumGasoline?.YusoJin == 0 ? null : mediumGasoline?.YusoJin,

                    //LPG
                    MediumLPG_NobeJyoCnt = mediumLPG?.NobeJyoCnt == 0 ? null : mediumLPG?.NobeJyoCnt,
                    MediumLPG_NobeRinCnt = mediumLPG?.NobeRinCnt == 0 ? null : mediumLPG?.NobeRinCnt,
                    MediumLPG_NobeSumCnt = mediumLPG?.NobeSumCnt == 0 ? null : mediumLPG?.NobeSumCnt,
                    MediumLPG_NobeJitCnt = mediumLPG?.NobeJitCnt == 0 ? null : mediumLPG?.NobeJitCnt,
                    MediumLPG_JitudoRitu = mediumLPG?.JitudoRitu == 0 ? null : mediumLPG?.JitudoRitu,
                    MediumLPG_RinjiRitu = mediumLPG?.RinjiRitu == 0 ? null : mediumLPG?.RinjiRitu,
                    MediumLPG_JitJisaKm = mediumLPG?.JitJisaKm == 0 ? null : mediumLPG?.JitJisaKm,
                    MediumLPG_JitKisoKm = mediumLPG?.JitKisoKm == 0 ? null : mediumLPG?.JitKisoKm,
                    MediumLPG_KmKei = (mediumLPG?.JitJisaKm + mediumLPG?.JitKisoKm) == 0 ? null :
                                        (mediumLPG?.JitJisaKm + mediumLPG?.JitKisoKm),
                    MediumLPG_YusoJin = mediumLPG?.YusoJin == 0 ? null : mediumLPG?.YusoJin,

                    //ガソリン
                    MediumGasTurbine_NobeJyoCnt = mediumGasTurbine?.NobeJyoCnt == 0 ? null : mediumGasTurbine?.NobeJyoCnt,
                    MediumGasTurbine_NobeRinCnt = mediumGasTurbine?.NobeRinCnt == 0 ? null : mediumGasTurbine?.NobeRinCnt,
                    MediumGasTurbine_NobeSumCnt = mediumGasTurbine?.NobeSumCnt == 0 ? null : mediumGasTurbine?.NobeSumCnt,
                    MediumGasTurbine_NobeJitCnt = mediumGasTurbine?.NobeJitCnt == 0 ? null : mediumGasTurbine?.NobeJitCnt,
                    MediumGasTurbine_JitudoRitu = mediumGasTurbine?.JitudoRitu == 0 ? null : mediumGasTurbine?.JitudoRitu,
                    MediumGasTurbine_RinjiRitu = mediumGasTurbine?.RinjiRitu == 0 ? null : mediumGasTurbine?.RinjiRitu,
                    MediumGasTurbine_JitJisaKm = mediumGasTurbine?.JitJisaKm == 0 ? null : mediumGasTurbine?.JitJisaKm,
                    MediumGasTurbine_JitKisoKm = mediumGasTurbine?.JitKisoKm == 0 ? null : mediumGasTurbine?.JitKisoKm,
                    MediumGasTurbine_KmKei = (mediumGasTurbine?.JitJisaKm + mediumGasTurbine?.JitKisoKm) == 0 ? null :
                                        (mediumGasTurbine?.JitJisaKm + mediumGasTurbine?.JitKisoKm),
                    MediumGasTurbine_YusoJin = mediumGasTurbine?.YusoJin == 0 ? null : mediumGasTurbine?.YusoJin,

                    //その他
                    MediumOther_NobeJyoCnt = mediumOther?.NobeJyoCnt == 0 ? null : mediumOther?.NobeJyoCnt,
                    MediumOther_NobeRinCnt = mediumOther?.NobeRinCnt == 0 ? null : mediumOther?.NobeRinCnt,
                    MediumOther_NobeSumCnt = mediumOther?.NobeSumCnt == 0 ? null : mediumOther?.NobeSumCnt,
                    MediumOther_NobeJitCnt = mediumOther?.NobeJitCnt == 0 ? null : mediumOther?.NobeJitCnt,
                    MediumOther_JitudoRitu = mediumOther?.JitudoRitu == 0 ? null : mediumOther?.JitudoRitu,
                    MediumOther_RinjiRitu = mediumOther?.RinjiRitu == 0 ? null : mediumOther?.RinjiRitu,
                    MediumOther_JitJisaKm = mediumOther?.JitJisaKm == 0 ? null : mediumOther?.JitJisaKm,
                    MediumOther_JitKisoKm = mediumOther?.JitKisoKm == 0 ? null : mediumOther?.JitKisoKm,
                    MediumOther_KmKei = (mediumOther?.JitJisaKm + mediumOther?.JitKisoKm) == 0 ? null :
                                        (mediumOther?.JitJisaKm + mediumOther?.JitKisoKm),
                    MediumOther_YusoJin = mediumOther?.YusoJin == 0 ? null : mediumOther?.YusoJin,

                    ////小型
                    //軽油
                    SmallOil_NobeJyoCnt = smallOil?.NobeJyoCnt == 0 ? null : smallOil?.NobeJyoCnt,
                    SmallOil_NobeRinCnt = smallOil?.NobeRinCnt == 0 ? null : smallOil?.NobeRinCnt,
                    SmallOil_NobeSumCnt = smallOil?.NobeSumCnt == 0 ? null : smallOil?.NobeSumCnt,
                    SmallOil_NobeJitCnt = smallOil?.NobeJitCnt == 0 ? null : smallOil?.NobeJitCnt,
                    SmallOil_JitudoRitu = smallOil?.JitudoRitu == 0 ? null : smallOil?.JitudoRitu,
                    SmallOil_RinjiRitu = smallOil?.RinjiRitu == 0 ? null : smallOil?.RinjiRitu,
                    SmallOil_JitJisaKm = smallOil?.JitJisaKm == 0 ? null : smallOil?.JitJisaKm,
                    SmallOil_JitKisoKm = smallOil?.JitKisoKm == 0 ? null : smallOil?.JitKisoKm,
                    SmallOil_KmKei = (smallOil?.JitJisaKm + smallOil?.JitKisoKm) == 0 ? null :
                                        (smallOil?.JitJisaKm + smallOil?.JitKisoKm),
                    SmallOil_YusoJin = smallOil?.YusoJin == 0 ? null : smallOil?.YusoJin,

                    //ガソリン
                    SmallGasoline_NobeJyoCnt = smallGasoline?.NobeJyoCnt == 0 ? null : smallGasoline?.NobeJyoCnt,
                    SmallGasoline_NobeRinCnt = smallGasoline?.NobeRinCnt == 0 ? null : smallGasoline?.NobeRinCnt,
                    SmallGasoline_NobeSumCnt = smallGasoline?.NobeSumCnt == 0 ? null : smallGasoline?.NobeSumCnt,
                    SmallGasoline_NobeJitCnt = smallGasoline?.NobeJitCnt == 0 ? null : smallGasoline?.NobeJitCnt,
                    SmallGasoline_JitudoRitu = smallGasoline?.JitudoRitu == 0 ? null : smallGasoline?.JitudoRitu,
                    SmallGasoline_RinjiRitu = smallGasoline?.RinjiRitu == 0 ? null : smallGasoline?.RinjiRitu,
                    SmallGasoline_JitJisaKm = smallGasoline?.JitJisaKm == 0 ? null : smallGasoline?.JitJisaKm,
                    SmallGasoline_JitKisoKm = smallGasoline?.JitKisoKm == 0 ? null : smallGasoline?.JitKisoKm,
                    SmallGasoline_KmKei = (smallGasoline?.JitJisaKm + smallGasoline?.JitKisoKm) == 0 ? null :
                                        (smallGasoline?.JitJisaKm + smallGasoline?.JitKisoKm),
                    SmallGasoline_YusoJin = smallGasoline?.YusoJin == 0 ? null : smallGasoline?.YusoJin,

                    //LPG
                    SmallLPG_NobeJyoCnt = smallLPG?.NobeJyoCnt == 0 ? null : smallLPG?.NobeJyoCnt,
                    SmallLPG_NobeRinCnt = smallLPG?.NobeRinCnt == 0 ? null : smallLPG?.NobeRinCnt,
                    SmallLPG_NobeSumCnt = smallLPG?.NobeSumCnt == 0 ? null : smallLPG?.NobeSumCnt,
                    SmallLPG_NobeJitCnt = smallLPG?.NobeJitCnt == 0 ? null : smallLPG?.NobeJitCnt,
                    SmallLPG_JitudoRitu = smallLPG?.JitudoRitu == 0 ? null : smallLPG?.JitudoRitu,
                    SmallLPG_RinjiRitu = smallLPG?.RinjiRitu == 0 ? null : smallLPG?.RinjiRitu,
                    SmallLPG_JitJisaKm = smallLPG?.JitJisaKm == 0 ? null : smallLPG?.JitJisaKm,
                    SmallLPG_JitKisoKm = smallLPG?.JitKisoKm == 0 ? null : smallLPG?.JitKisoKm,
                    SmallLPG_KmKei = (smallLPG?.JitJisaKm + smallLPG?.JitKisoKm) == 0 ? null :
                                        (smallLPG?.JitJisaKm + smallLPG?.JitKisoKm),
                    SmallLPG_YusoJin = smallLPG?.YusoJin == 0 ? null : smallLPG?.YusoJin,

                    //ガソリン
                    SmallGasTurbine_NobeJyoCnt = smallGasTurbine?.NobeJyoCnt == 0 ? null : smallGasTurbine?.NobeJyoCnt,
                    SmallGasTurbine_NobeRinCnt = smallGasTurbine?.NobeRinCnt == 0 ? null : smallGasTurbine?.NobeRinCnt,
                    SmallGasTurbine_NobeSumCnt = smallGasTurbine?.NobeSumCnt == 0 ? null : smallGasTurbine?.NobeSumCnt,
                    SmallGasTurbine_NobeJitCnt = smallGasTurbine?.NobeJitCnt == 0 ? null : smallGasTurbine?.NobeJitCnt,
                    SmallGasTurbine_JitudoRitu = smallGasTurbine?.JitudoRitu == 0 ? null : smallGasTurbine?.JitudoRitu,
                    SmallGasTurbine_RinjiRitu = smallGasTurbine?.RinjiRitu == 0 ? null : smallGasTurbine?.RinjiRitu,
                    SmallGasTurbine_JitJisaKm = smallGasTurbine?.JitJisaKm == 0 ? null : smallGasTurbine?.JitJisaKm,
                    SmallGasTurbine_JitKisoKm = smallGasTurbine?.JitKisoKm == 0 ? null : smallGasTurbine?.JitKisoKm,
                    SmallGasTurbine_KmKei = (smallGasTurbine?.JitJisaKm + smallGasTurbine?.JitKisoKm) == 0 ? null :
                                        (smallGasTurbine?.JitJisaKm + smallGasTurbine?.JitKisoKm),
                    SmallGasTurbine_YusoJin = smallGasTurbine?.YusoJin == 0 ? null : smallGasTurbine?.YusoJin,

                    //その他
                    SmallOther_NobeJyoCnt = smallOther?.NobeJyoCnt == 0 ? null : smallOther?.NobeJyoCnt,
                    SmallOther_NobeRinCnt = smallOther?.NobeRinCnt == 0 ? null : smallOther?.NobeRinCnt,
                    SmallOther_NobeSumCnt = smallOther?.NobeSumCnt == 0 ? null : smallOther?.NobeSumCnt,
                    SmallOther_NobeJitCnt = smallOther?.NobeJitCnt == 0 ? null : smallOther?.NobeJitCnt,
                    SmallOther_JitudoRitu = smallOther?.JitudoRitu == 0 ? null : smallOther?.JitudoRitu,
                    SmallOther_RinjiRitu = smallOther?.RinjiRitu == 0 ? null : smallOther?.RinjiRitu,
                    SmallOther_JitJisaKm = smallOther?.JitJisaKm == 0 ? null : smallOther?.JitJisaKm,
                    SmallOther_JitKisoKm = smallOther?.JitKisoKm == 0 ? null : smallOther?.JitKisoKm,
                    SmallOther_KmKei = (smallOther?.JitJisaKm + smallOther?.JitKisoKm) == 0 ? null :
                                        (smallOther?.JitJisaKm + smallOther?.JitKisoKm),
                    SmallOther_YusoJin = smallOther?.YusoJin == 0 ? null : smallOther?.YusoJin,

                    //////「輸送実績報告書2」タブ
                    /////大型
                    //軽油
                    LargeOil_UnkoCnt = largeOil?.UnkoCnt == 0 ? null : largeOil?.UnkoCnt,
                    LargeOil_UnkoKikak1Cnt = largeOil?.UnkoKikak1Cnt == 0 ? null : largeOil?.UnkoKikak1Cnt,
                    LargeOil_UnkoKikak2Cnt = largeOil?.UnkoKikak2Cnt == 0 ? null : largeOil?.UnkoKikak2Cnt,
                    LargeOil_UnkoOthCnt = largeOil?.UnkoOthCnt == 0 ? null : largeOil?.UnkoOthCnt,
                    LargeOil_UnsoSyu = Convert.ToInt32(largeOil?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(largeOil?.UnsoSyu),
                    LargeOil_DayTotalKm = largeOil?.DayTotalKm == 0 ? null : largeOil?.DayTotalKm,
                    LargeOil_DayYusoJin = largeOil?.DayYusoJin == 0 ? null : largeOil?.DayYusoJin,
                    LargeOil_DayUnsoSyu = largeOil?.DayUnsoSyu == 0 ? null : largeOil?.DayUnsoSyu,
                    LargeOil_DayJisaKm = largeOil?.DayJisaKm == 0 ? null : largeOil?.DayJisaKm,
                    LargeOil_UnsoZaSyu = Convert.ToInt32(largeOil?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(largeOil?.UnsoZaSyu),

                    //ガソリン
                    LargeGasoline_UnkoCnt = largeGasoline?.UnkoCnt == 0 ? null : largeGasoline?.UnkoCnt,
                    LargeGasoline_UnkoKikak1Cnt = largeGasoline?.UnkoKikak1Cnt == 0 ? null : largeGasoline?.UnkoKikak1Cnt,
                    LargeGasoline_UnkoKikak2Cnt = largeGasoline?.UnkoKikak2Cnt == 0 ? null : largeGasoline?.UnkoKikak2Cnt,
                    LargeGasoline_UnkoOthCnt = largeGasoline?.UnkoOthCnt == 0 ? null : largeGasoline?.UnkoOthCnt,
                    LargeGasoline_UnsoSyu = Convert.ToInt32(largeGasoline?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(largeGasoline?.UnsoSyu),
                    LargeGasoline_DayTotalKm = largeGasoline?.DayTotalKm == 0 ? null : largeGasoline?.DayTotalKm,
                    LargeGasoline_DayYusoJin = largeGasoline?.DayYusoJin == 0 ? null : largeGasoline?.DayYusoJin,
                    LargeGasoline_DayUnsoSyu = largeGasoline?.DayUnsoSyu == 0 ? null : largeGasoline?.DayUnsoSyu,
                    LargeGasoline_DayJisaKm = largeGasoline?.DayJisaKm == 0 ? null : largeGasoline?.DayJisaKm,
                    LargeGasoline_UnsoZaSyu = Convert.ToInt32(largeGasoline?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(largeGasoline?.UnsoZaSyu),

                    //LPG
                    LargeLPG_UnkoCnt = largeLPG?.UnkoCnt == 0 ? null : largeLPG?.UnkoCnt,
                    LargeLPG_UnkoKikak1Cnt = largeLPG?.UnkoKikak1Cnt == 0 ? null : largeLPG?.UnkoKikak1Cnt,
                    LargeLPG_UnkoKikak2Cnt = largeLPG?.UnkoKikak2Cnt == 0 ? null : largeLPG?.UnkoKikak2Cnt,
                    LargeLPG_UnkoOthCnt = largeLPG?.UnkoOthCnt == 0 ? null : largeLPG?.UnkoOthCnt,
                    LargeLPG_UnsoSyu = Convert.ToInt32(largeLPG?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(largeLPG?.UnsoSyu),
                    LargeLPG_DayTotalKm = largeLPG?.DayTotalKm == 0 ? null : largeLPG?.DayTotalKm,
                    LargeLPG_DayYusoJin = largeLPG?.DayYusoJin == 0 ? null : largeLPG?.DayYusoJin,
                    LargeLPG_DayUnsoSyu = largeLPG?.DayUnsoSyu == 0 ? null : largeLPG?.DayUnsoSyu,
                    LargeLPG_DayJisaKm = largeLPG?.DayJisaKm == 0 ? null : largeLPG?.DayJisaKm,
                    LargeLPG_UnsoZaSyu = Convert.ToInt32(largeLPG?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(largeLPG?.UnsoZaSyu),

                    //ガスタービン
                    LargeGasTurbine_UnkoCnt = largeGasTurbine?.UnkoCnt == 0 ? null : largeGasTurbine?.UnkoCnt,
                    LargeGasTurbine_UnkoKikak1Cnt = largeGasTurbine?.UnkoKikak1Cnt == 0 ? null : largeGasTurbine?.UnkoKikak1Cnt,
                    LargeGasTurbine_UnkoKikak2Cnt = largeGasTurbine?.UnkoKikak2Cnt == 0 ? null : largeGasTurbine?.UnkoKikak2Cnt,
                    LargeGasTurbine_UnkoOthCnt = largeGasTurbine?.UnkoOthCnt == 0 ? null : largeGasTurbine?.UnkoOthCnt,
                    LargeGasTurbine_UnsoSyu = Convert.ToInt32(largeGasTurbine?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(largeGasTurbine?.UnsoSyu),
                    LargeGasTurbine_DayTotalKm = largeGasTurbine?.DayTotalKm == 0 ? null : largeGasTurbine?.DayTotalKm,
                    LargeGasTurbine_DayYusoJin = largeGasTurbine?.DayYusoJin == 0 ? null : largeGasTurbine?.DayYusoJin,
                    LargeGasTurbine_DayUnsoSyu = largeGasTurbine?.DayUnsoSyu == 0 ? null : largeGasTurbine?.DayUnsoSyu,
                    LargeGasTurbine_DayJisaKm = largeGasTurbine?.DayJisaKm == 0 ? null : largeGasTurbine?.DayJisaKm,
                    LargeGasTurbine_UnsoZaSyu = Convert.ToInt32(largeGasTurbine?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(largeGasTurbine?.UnsoZaSyu),

                    //その他
                    LargeOther_UnkoCnt = largeOther?.UnkoCnt == 0 ? null : largeOther?.UnkoCnt,
                    LargeOther_UnkoKikak1Cnt = largeOther?.UnkoKikak1Cnt == 0 ? null : largeOther?.UnkoKikak1Cnt,
                    LargeOther_UnkoKikak2Cnt = largeOther?.UnkoKikak2Cnt == 0 ? null : largeOther?.UnkoKikak2Cnt,
                    LargeOther_UnkoOthCnt = largeOther?.UnkoOthCnt == 0 ? null : largeOther?.UnkoOthCnt,
                    LargeOther_UnsoSyu = Convert.ToInt32(largeOther?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(largeOther?.UnsoSyu),
                    LargeOther_DayTotalKm = largeOther?.DayTotalKm == 0 ? null : largeOther?.DayTotalKm,
                    LargeOther_DayYusoJin = largeOther?.DayYusoJin == 0 ? null : largeOther?.DayYusoJin,
                    LargeOther_DayUnsoSyu = largeOther?.DayUnsoSyu == 0 ? null : largeOther?.DayUnsoSyu,
                    LargeOther_DayJisaKm = largeOther?.DayJisaKm == 0 ? null : largeOther?.DayJisaKm,
                    LargeOther_UnsoZaSyu = Convert.ToInt32(largeOther?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(largeOther?.UnsoZaSyu),

                    ////中型
                    //軽油
                    MediumOil_UnkoCnt = mediumOil?.UnkoCnt == 0 ? null : mediumOil?.UnkoCnt,
                    MediumOil_UnkoKikak1Cnt = mediumOil?.UnkoKikak1Cnt == 0 ? null : mediumOil?.UnkoKikak1Cnt,
                    MediumOil_UnkoKikak2Cnt = mediumOil?.UnkoKikak2Cnt == 0 ? null : mediumOil?.UnkoKikak2Cnt,
                    MediumOil_UnkoOthCnt = mediumOil?.UnkoOthCnt == 0 ? null : mediumOil?.UnkoOthCnt,
                    MediumOil_UnsoSyu = Convert.ToInt32(mediumOil?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(mediumOil?.UnsoSyu),
                    MediumOil_DayTotalKm = mediumOil?.DayTotalKm == 0 ? null : mediumOil?.DayTotalKm,
                    MediumOil_DayYusoJin = mediumOil?.DayYusoJin == 0 ? null : mediumOil?.DayYusoJin,
                    MediumOil_DayUnsoSyu = mediumOil?.DayUnsoSyu == 0 ? null : mediumOil?.DayUnsoSyu,
                    MediumOil_DayJisaKm = mediumOil?.DayJisaKm == 0 ? null : mediumOil?.DayJisaKm,
                    MediumOil_UnsoZaSyu = Convert.ToInt32(mediumOil?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(mediumOil?.UnsoZaSyu),

                    //ガソリン
                    MediumGasoline_UnkoCnt = mediumGasoline?.UnkoCnt == 0 ? null : mediumGasoline?.UnkoCnt,
                    MediumGasoline_UnkoKikak1Cnt = mediumGasoline?.UnkoKikak1Cnt == 0 ? null : mediumGasoline?.UnkoKikak1Cnt,
                    MediumGasoline_UnkoKikak2Cnt = mediumGasoline?.UnkoKikak2Cnt == 0 ? null : mediumGasoline?.UnkoKikak2Cnt,
                    MediumGasoline_UnkoOthCnt = mediumGasoline?.UnkoOthCnt == 0 ? null : mediumGasoline?.UnkoOthCnt,
                    MediumGasoline_UnsoSyu = Convert.ToInt32(mediumGasoline?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(mediumGasoline?.UnsoSyu),
                    MediumGasoline_DayTotalKm = mediumGasoline?.DayTotalKm == 0 ? null : mediumGasoline?.DayTotalKm,
                    MediumGasoline_DayYusoJin = mediumGasoline?.DayYusoJin == 0 ? null : mediumGasoline?.DayYusoJin,
                    MediumGasoline_DayUnsoSyu = mediumGasoline?.DayUnsoSyu == 0 ? null : mediumGasoline?.DayUnsoSyu,
                    MediumGasoline_DayJisaKm = mediumGasoline?.DayJisaKm == 0 ? null : mediumGasoline?.DayJisaKm,
                    MediumGasoline_UnsoZaSyu = Convert.ToInt32(mediumGasoline?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(mediumGasoline?.UnsoZaSyu),

                    //LPG
                    MediumLPG_UnkoCnt = mediumLPG?.UnkoCnt == 0 ? null : mediumLPG?.UnkoCnt,
                    MediumLPG_UnkoKikak1Cnt = mediumLPG?.UnkoKikak1Cnt == 0 ? null : mediumLPG?.UnkoKikak1Cnt,
                    MediumLPG_UnkoKikak2Cnt = mediumLPG?.UnkoKikak2Cnt == 0 ? null : mediumLPG?.UnkoKikak2Cnt,
                    MediumLPG_UnkoOthCnt = mediumLPG?.UnkoOthCnt == 0 ? null : mediumLPG?.UnkoOthCnt,
                    MediumLPG_UnsoSyu = Convert.ToInt32(mediumLPG?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(mediumLPG?.UnsoSyu),
                    MediumLPG_DayTotalKm = mediumLPG?.DayTotalKm == 0 ? null : mediumLPG?.DayTotalKm,
                    MediumLPG_DayYusoJin = mediumLPG?.DayYusoJin == 0 ? null : mediumLPG?.DayYusoJin,
                    MediumLPG_DayUnsoSyu = mediumLPG?.DayUnsoSyu == 0 ? null : mediumLPG?.DayUnsoSyu,
                    MediumLPG_DayJisaKm = mediumLPG?.DayJisaKm == 0 ? null : mediumLPG?.DayJisaKm,
                    MediumLPG_UnsoZaSyu = Convert.ToInt32(mediumLPG?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(mediumLPG?.UnsoZaSyu),

                    //ガスタービン
                    MediumGasTurbine_UnkoCnt = mediumGasTurbine?.UnkoCnt == 0 ? null : mediumGasTurbine?.UnkoCnt,
                    MediumGasTurbine_UnkoKikak1Cnt = mediumGasTurbine?.UnkoKikak1Cnt == 0 ? null : mediumGasTurbine?.UnkoKikak1Cnt,
                    MediumGasTurbine_UnkoKikak2Cnt = mediumGasTurbine?.UnkoKikak2Cnt == 0 ? null : mediumGasTurbine?.UnkoKikak2Cnt,
                    MediumGasTurbine_UnkoOthCnt = mediumGasTurbine?.UnkoOthCnt == 0 ? null : mediumGasTurbine?.UnkoOthCnt,
                    MediumGasTurbine_UnsoSyu = Convert.ToInt32(mediumGasTurbine?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(mediumGasTurbine?.UnsoSyu),
                    MediumGasTurbine_DayTotalKm = mediumGasTurbine?.DayTotalKm == 0 ? null : mediumGasTurbine?.DayTotalKm,
                    MediumGasTurbine_DayYusoJin = mediumGasTurbine?.DayYusoJin == 0 ? null : mediumGasTurbine?.DayYusoJin,
                    MediumGasTurbine_DayUnsoSyu = mediumGasTurbine?.DayUnsoSyu == 0 ? null : mediumGasTurbine?.DayUnsoSyu,
                    MediumGasTurbine_DayJisaKm = mediumGasTurbine?.DayJisaKm == 0 ? null : mediumGasTurbine?.DayJisaKm,
                    MediumGasTurbine_UnsoZaSyu = Convert.ToInt32(mediumGasTurbine?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(mediumGasTurbine?.UnsoZaSyu),

                    //その他
                    MediumOther_UnkoCnt = mediumOther?.UnkoCnt == 0 ? null : mediumOther?.UnkoCnt,
                    MediumOther_UnkoKikak1Cnt = mediumOther?.UnkoKikak1Cnt == 0 ? null : mediumOther?.UnkoKikak1Cnt,
                    MediumOther_UnkoKikak2Cnt = mediumOther?.UnkoKikak2Cnt == 0 ? null : mediumOther?.UnkoKikak2Cnt,
                    MediumOther_UnkoOthCnt = mediumOther?.UnkoOthCnt == 0 ? null : mediumOther?.UnkoOthCnt,
                    MediumOther_UnsoSyu = Convert.ToInt32(mediumOther?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(mediumOther?.UnsoSyu),
                    MediumOther_DayTotalKm = mediumOther?.DayTotalKm == 0 ? null : mediumOther?.DayTotalKm,
                    MediumOther_DayYusoJin = mediumOther?.DayYusoJin == 0 ? null : mediumOther?.DayYusoJin,
                    MediumOther_DayUnsoSyu = mediumOther?.DayUnsoSyu == 0 ? null : mediumOther?.DayUnsoSyu,
                    MediumOther_DayJisaKm = mediumOther?.DayJisaKm == 0 ? null : mediumOther?.DayJisaKm,
                    MediumOther_UnsoZaSyu = Convert.ToInt32(mediumOther?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(mediumOther?.UnsoZaSyu),

                    ////小型
                    //軽油
                    SmallOil_UnkoCnt = smallOil?.UnkoCnt == 0 ? null : smallOil?.UnkoCnt,
                    SmallOil_UnkoKikak1Cnt = smallOil?.UnkoKikak1Cnt == 0 ? null : smallOil?.UnkoKikak1Cnt,
                    SmallOil_UnkoKikak2Cnt = smallOil?.UnkoKikak2Cnt == 0 ? null : smallOil?.UnkoKikak2Cnt,
                    SmallOil_UnkoOthCnt = smallOil?.UnkoOthCnt == 0 ? null : smallOil?.UnkoOthCnt,
                    SmallOil_UnsoSyu = Convert.ToInt32(smallOil?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(smallOil?.UnsoSyu),
                    SmallOil_DayTotalKm = smallOil?.DayTotalKm == 0 ? null : smallOil?.DayTotalKm,
                    SmallOil_DayYusoJin = smallOil?.DayYusoJin == 0 ? null : smallOil?.DayYusoJin,
                    SmallOil_DayUnsoSyu = smallOil?.DayUnsoSyu == 0 ? null : smallOil?.DayUnsoSyu,
                    SmallOil_DayJisaKm = smallOil?.DayJisaKm == 0 ? null : smallOil?.DayJisaKm,
                    SmallOil_UnsoZaSyu = Convert.ToInt32(smallOil?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(smallOil?.UnsoZaSyu),

                    //ガソリン
                    SmallGasoline_UnkoCnt = smallGasoline?.UnkoCnt == 0 ? null : smallGasoline?.UnkoCnt,
                    SmallGasoline_UnkoKikak1Cnt = smallGasoline?.UnkoKikak1Cnt == 0 ? null : smallGasoline?.UnkoKikak1Cnt,
                    SmallGasoline_UnkoKikak2Cnt = smallGasoline?.UnkoKikak2Cnt == 0 ? null : smallGasoline?.UnkoKikak2Cnt,
                    SmallGasoline_UnkoOthCnt = smallGasoline?.UnkoOthCnt == 0 ? null : smallGasoline?.UnkoOthCnt,
                    SmallGasoline_UnsoSyu = Convert.ToInt32(smallGasoline?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(smallGasoline?.UnsoSyu),
                    SmallGasoline_DayTotalKm = smallGasoline?.DayTotalKm == 0 ? null : smallGasoline?.DayTotalKm,
                    SmallGasoline_DayYusoJin = smallGasoline?.DayYusoJin == 0 ? null : smallGasoline?.DayYusoJin,
                    SmallGasoline_DayUnsoSyu = smallGasoline?.DayUnsoSyu == 0 ? null : smallGasoline?.DayUnsoSyu,
                    SmallGasoline_DayJisaKm = smallGasoline?.DayJisaKm == 0 ? null : smallGasoline?.DayJisaKm,
                    SmallGasoline_UnsoZaSyu = Convert.ToInt32(smallGasoline?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(smallGasoline?.UnsoZaSyu),

                    //LPG
                    SmallLPG_UnkoCnt = smallLPG?.UnkoCnt == 0 ? null : smallLPG?.UnkoCnt,
                    SmallLPG_UnkoKikak1Cnt = smallLPG?.UnkoKikak1Cnt == 0 ? null : smallLPG?.UnkoKikak1Cnt,
                    SmallLPG_UnkoKikak2Cnt = smallLPG?.UnkoKikak2Cnt == 0 ? null : smallLPG?.UnkoKikak2Cnt,
                    SmallLPG_UnkoOthCnt = smallLPG?.UnkoOthCnt == 0 ? null : smallLPG?.UnkoOthCnt,
                    SmallLPG_UnsoSyu = Convert.ToInt32(smallLPG?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(smallLPG?.UnsoSyu),
                    SmallLPG_DayTotalKm = smallLPG?.DayTotalKm == 0 ? null : smallLPG?.DayTotalKm,
                    SmallLPG_DayYusoJin = smallLPG?.DayYusoJin == 0 ? null : smallLPG?.DayYusoJin,
                    SmallLPG_DayUnsoSyu = smallLPG?.DayUnsoSyu == 0 ? null : smallLPG?.DayUnsoSyu,
                    SmallLPG_DayJisaKm = smallLPG?.DayJisaKm == 0 ? null : smallLPG?.DayJisaKm,
                    SmallLPG_UnsoZaSyu = Convert.ToInt32(smallLPG?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(smallLPG?.UnsoZaSyu),

                    //ガスタービン
                    SmallGasTurbine_UnkoCnt = smallGasTurbine?.UnkoCnt == 0 ? null : smallGasTurbine?.UnkoCnt,
                    SmallGasTurbine_UnkoKikak1Cnt = smallGasTurbine?.UnkoKikak1Cnt == 0 ? null : smallGasTurbine?.UnkoKikak1Cnt,
                    SmallGasTurbine_UnkoKikak2Cnt = smallGasTurbine?.UnkoKikak2Cnt == 0 ? null : smallGasTurbine?.UnkoKikak2Cnt,
                    SmallGasTurbine_UnkoOthCnt = smallGasTurbine?.UnkoOthCnt == 0 ? null : smallGasTurbine?.UnkoOthCnt,
                    SmallGasTurbine_UnsoSyu = Convert.ToInt32(smallGasTurbine?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(smallGasTurbine?.UnsoSyu),
                    SmallGasTurbine_DayTotalKm = smallGasTurbine?.DayTotalKm == 0 ? null : smallGasTurbine?.DayTotalKm,
                    SmallGasTurbine_DayYusoJin = smallGasTurbine?.DayYusoJin == 0 ? null : smallGasTurbine?.DayYusoJin,
                    SmallGasTurbine_DayUnsoSyu = smallGasTurbine?.DayUnsoSyu == 0 ? null : smallGasTurbine?.DayUnsoSyu,
                    SmallGasTurbine_DayJisaKm = smallGasTurbine?.DayJisaKm == 0 ? null : smallGasTurbine?.DayJisaKm,
                    SmallGasTurbine_UnsoZaSyu = Convert.ToInt32(smallGasTurbine?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(smallGasTurbine?.UnsoZaSyu),

                    //その他
                    SmallOther_UnkoCnt = smallOther?.UnkoCnt == 0 ? null : smallOther?.UnkoCnt,
                    SmallOther_UnkoKikak1Cnt = smallOther?.UnkoKikak1Cnt == 0 ? null : smallOther?.UnkoKikak1Cnt,
                    SmallOther_UnkoKikak2Cnt = smallOther?.UnkoKikak2Cnt == 0 ? null : smallOther?.UnkoKikak2Cnt,
                    SmallOther_UnkoOthCnt = smallOther?.UnkoOthCnt == 0 ? null : smallOther?.UnkoOthCnt,
                    SmallOther_UnsoSyu = Convert.ToInt32(smallOther?.UnsoSyu) == 0 ? (int?)null : Convert.ToInt32(smallOther?.UnsoSyu),
                    SmallOther_DayTotalKm = smallOther?.DayTotalKm == 0 ? null : smallOther?.DayTotalKm,
                    SmallOther_DayYusoJin = smallOther?.DayYusoJin == 0 ? null : smallOther?.DayYusoJin,
                    SmallOther_DayUnsoSyu = smallOther?.DayUnsoSyu == 0 ? null : smallOther?.DayUnsoSyu,
                    SmallOther_DayJisaKm = smallOther?.DayJisaKm == 0 ? null : smallOther?.DayJisaKm,
                    SmallOther_UnsoZaSyu = Convert.ToInt32(smallOther?.UnsoZaSyu) == 0 ? (int?)null : Convert.ToInt32(smallOther?.UnsoZaSyu),
                };
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        public async void UpdateFormValue(string propertyName1, dynamic value1)
        {
            try
            {
                //tab1
                string propertyName2 = string.Empty;
                dynamic value2 = null;
                string propertyName3 = string.Empty;
                dynamic value3 = null;
                string propertyName4 = string.Empty;
                dynamic value4 = null;

                //tab2
                //DaySoukoKm --> JitSumKm ÷ NobeJitCnt
                string propertyName5 = string.Empty;
                dynamic value5 = null;
                //DayUnsoCnt --> YusoJin ÷ NobeJitCnt
                string propertyName6 = string.Empty;
                dynamic value6 = null;
                //DayUnsoSyu --> UnsoSyu ÷ NobeJitCnt
                string propertyName7 = string.Empty;
                dynamic value7 = null;
                //UnkoJisyaKm --> JitJisaKm ÷ UnkoCnt
                string propertyName8 = string.Empty;
                dynamic value8 = null;

                if (value1 != null)
                {
                    //validate maxlength server side
                    if (propertyName1.Contains("NobeJyoCnt") || propertyName1.Contains("NobeRinCnt") || propertyName1.Contains("NobeJitCnt") || propertyName1.Contains("YusoJi")
                        || propertyName1.Contains("UnkoCnt") || propertyName1.Contains("UnkoKikak1Cnt") || propertyName1.Contains("UnkoKikak2Cnt") || propertyName1.Contains("UnkoOthCn"))
                        value1 = Convert.ToInt32(StringExtensions.TruncateWithMaxLength(value1?.ToString(), 8));
                    else if (propertyName1.Contains("UnsoSyu") || propertyName1.Contains("UnsoZaSyu"))
                        value1 = Convert.ToInt32(StringExtensions.TruncateWithMaxLength(value1?.ToString(), 9));
                    else if ((propertyName1.Contains("JitJisaKm") || propertyName1.Contains("JitKisoKm")) && value1?.ToString()?.Contains("."))
                        value1 = Convert.ToDecimal(StringExtensions.TruncateWithMaxLength(value1?.ToString(), 10));
                    else if ((propertyName1.Contains("JitJisaKm") || propertyName1.Contains("JitKisoKm")) && !value1?.ToString()?.Contains("."))
                        value1 = Convert.ToDecimal(StringExtensions.TruncateWithMaxLength(value1?.ToString(), 7));
                }

                ////Large
                //軽油
                if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOil_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOil_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOil_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOil_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.LargeOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.LargeOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeOil_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.LargeOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOil_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOil_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.LargeOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOil_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.LargeOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOil_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOil_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.LargeOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOil_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOil_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.LargeOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.LargeOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOil_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOil_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.LargeOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOil_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガソリン
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasoline_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasoline_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasoline_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasoline_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.LargeGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeGasoline_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.LargeGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasoline_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasoline_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.LargeGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasoline_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.LargeGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasoline_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasoline_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.LargeGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasoline_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasoline_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.LargeGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.LargeGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasoline_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.LargeGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasoline_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //LPG
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeLPG_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeLPG_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeLPG_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeLPG_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.LargeLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.LargeLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeLPG_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.LargeLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeLPG_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.LargeLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeLPG_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.LargeLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeLPG_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.LargeLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeLPG_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeLPG_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.LargeLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeLPG_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.LargeLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeLPG_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.LargeLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.LargeLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.LargeLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeLPG_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.LargeLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeLPG_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガスタービン
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasTurbine_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasTurbine_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasTurbine_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasTurbine_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.LargeGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.LargeGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeGasTurbine_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.LargeGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeGasTurbine_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasTurbine_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.LargeGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasTurbine_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.LargeGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasTurbine_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasTurbine_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.LargeGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasTurbine_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeGasTurbine_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.LargeGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.LargeGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.LargeGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeGasTurbine_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.LargeGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeGasTurbine_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //その他
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOther_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOther_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOther_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.LargeOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOther_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.LargeOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.LargeOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.LargeOther_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.LargeOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.LargeOther_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOther_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.LargeOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOther_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.LargeOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOther_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOther_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.LargeOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOther_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.LargeOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.LargeOther_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.LargeOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.LargeOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.LargeOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.LargeOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.LargeOther_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.LargeOther_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.LargeOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.LargeOther_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }


                ////Medium
                //軽油
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOil_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOil_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOil_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOil_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.MediumOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.MediumOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumOil_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.MediumOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOil_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOil_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.MediumOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOil_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.MediumOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOil_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOil_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.MediumOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOil_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOil_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.MediumOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.MediumOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOil_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOil_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.MediumOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOil_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガソリン
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasoline_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasoline_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasoline_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasoline_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.MediumGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumGasoline_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.MediumGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasoline_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasoline_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.MediumGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasoline_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.MediumGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasoline_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasoline_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.MediumGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasoline_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasoline_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.MediumGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.MediumGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasoline_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.MediumGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasoline_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //LPG
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumLPG_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumLPG_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumLPG_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumLPG_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.MediumLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.MediumLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumLPG_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.MediumLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumLPG_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.MediumLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumLPG_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.MediumLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumLPG_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.MediumLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumLPG_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumLPG_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.MediumLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumLPG_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.MediumLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumLPG_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.MediumLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.MediumLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.MediumLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumLPG_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.MediumLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumLPG_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガスタービン
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasTurbine_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasTurbine_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasTurbine_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasTurbine_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.MediumGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.MediumGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumGasTurbine_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.MediumGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumGasTurbine_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasTurbine_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.MediumGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasTurbine_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.MediumGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasTurbine_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasTurbine_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.MediumGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasTurbine_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumGasTurbine_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.MediumGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.MediumGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.MediumGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumGasTurbine_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.MediumGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumGasTurbine_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //その他
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOther_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOther_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOther_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.MediumOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOther_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.MediumOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.MediumOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.MediumOther_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.MediumOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.MediumOther_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOther_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.MediumOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOther_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.MediumOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOther_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOther_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.MediumOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOther_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.MediumOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.MediumOther_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.MediumOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.MediumOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.MediumOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.MediumOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.MediumOther_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.MediumOther_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.MediumOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.MediumOther_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                ////Small
                //軽油
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOil_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOil_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOil_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOil_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOil_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.SmallOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.SmallOil_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallOil_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.SmallOil_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOil_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOil_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.SmallOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOil_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.SmallOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOil_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOil_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.SmallOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOil_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOil_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOil_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOil_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.SmallOil_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOil_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.SmallOil_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOil_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOil_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.SmallOil_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOil_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガソリン
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasoline_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasoline_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasoline_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasoline_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasoline_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.SmallGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasoline_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallGasoline_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.SmallGasoline_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasoline_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasoline_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.SmallGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasoline_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.SmallGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasoline_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasoline_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.SmallGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasoline_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasoline_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasoline_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasoline_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.SmallGasoline_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.SmallGasoline_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasoline_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasoline_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.SmallGasoline_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasoline_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //LPG
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallLPG_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallLPG_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallLPG_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallLPG_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallLPG_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.SmallLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.SmallLPG_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallLPG_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.SmallLPG_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallLPG_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.SmallLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallLPG_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.SmallLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallLPG_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.SmallLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallLPG_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallLPG_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.SmallLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallLPG_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.SmallLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallLPG_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallLPG_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.SmallLPG_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.SmallLPG_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.SmallLPG_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallLPG_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallLPG_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.SmallLPG_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallLPG_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //ガスタービン
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasTurbine_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasTurbine_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasTurbine_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasTurbine_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasTurbine_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.SmallGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.SmallGasTurbine_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallGasTurbine_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.SmallGasTurbine_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallGasTurbine_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasTurbine_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.SmallGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasTurbine_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.SmallGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasTurbine_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasTurbine_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.SmallGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasTurbine_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallGasTurbine_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallGasTurbine_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.SmallGasTurbine_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.SmallGasTurbine_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.SmallGasTurbine_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallGasTurbine_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallGasTurbine_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.SmallGasTurbine_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallGasTurbine_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }

                //その他
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_NobeJyoCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOther_NobeRinCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOther_NobeJitCnt, value2);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, "", "", "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_NobeRinCnt))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOther_NobeSumCnt);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOther_NobeJyoCnt, null);
                    propertyName3 = nameof(monthlyTransportationModel.SmallOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOther_NobeJitCnt, value2);
                    propertyName4 = nameof(monthlyTransportationModel.SmallOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, propertyName3, value3, propertyName4, value4, "", "", "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_NobeJitCnt))
                {
                    propertyName3 = nameof(monthlyTransportationModel.SmallOther_JitudoRitu);
                    value3 = CommonUtil.CheckNullAndPercentage(value1, monthlyTransportationModel.SmallOther_NobeSumCnt);
                    propertyName4 = nameof(monthlyTransportationModel.SmallOther_RinjiRitu);
                    value4 = CommonUtil.CheckNullAndPercentage(monthlyTransportationModel.SmallOther_NobeRinCnt, value1);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOther_KmKei, value1);
                    propertyName6 = nameof(monthlyTransportationModel.SmallOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOther_YusoJin, value1);
                    propertyName7 = nameof(monthlyTransportationModel.SmallOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOther_YusoJin, value1);
                    SetValue(propertyName1, value1, "", "", propertyName3, value3, propertyName4, value4, propertyName5, value5, propertyName6, value6, propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_JitJisaKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOther_JitKisoKm, null);
                    propertyName8 = nameof(monthlyTransportationModel.SmallOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOther_UnkoCnt);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", propertyName8, value8);
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_JitKisoKm))
                {
                    propertyName2 = nameof(monthlyTransportationModel.SmallOther_KmKei);
                    value2 = CommonUtil.CheckNullAndSum(value1, monthlyTransportationModel.SmallOther_JitJisaKm, null);
                    propertyName5 = nameof(monthlyTransportationModel.SmallOther_DayTotalKm);
                    value5 = CommonUtil.CheckNullAndCaculate(value2, monthlyTransportationModel.SmallOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", propertyName5, value5, "", "", "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_YusoJin))
                {
                    propertyName6 = nameof(monthlyTransportationModel.SmallOther_DayYusoJin);
                    value6 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOther_NobeJitCnt);
                    SetValue(propertyName1, value1, propertyName2, value2, "", "", "", "", "", "", propertyName6, value6, "", "", "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_UnsoSyu))
                {
                    propertyName7 = nameof(monthlyTransportationModel.SmallOther_DayUnsoSyu);
                    value7 = CommonUtil.CheckNullAndCaculate(value1, monthlyTransportationModel.SmallOther_NobeJitCnt);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", propertyName7, value7, "", "");
                }
                else if (propertyName1 == nameof(monthlyTransportationModel.SmallOther_UnkoCnt))
                {
                    propertyName8 = nameof(monthlyTransportationModel.SmallOther_DayJisaKm);
                    value8 = CommonUtil.CheckNullAndCaculate(monthlyTransportationModel.SmallOther_JitJisaKm, value1);
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", propertyName8, value8);
                }
                else
                    SetValue(propertyName1, value1, "", "", "", "", "", "", "", "", "", "", "", "", "", "");

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        private async void SetValue(string propertyName1, dynamic value1, string propertyName2, dynamic value2,
                                    string propertyName3, dynamic value3, string propertyName4, dynamic value4,
                                    string propertyName5, dynamic value5, string propertyName6, dynamic value6,
                                    string propertyName7, dynamic value7, string propertyName8, dynamic value8)
        {
            //tab1
            var propertyInfoTab1 = monthlyTransportationModel.GetType().GetProperty(propertyName1);
            var propertyInfoNobeSumCnt = monthlyTransportationModel.GetType().GetProperty(propertyName2);
            var propertyInfoJitudoRitu = monthlyTransportationModel.GetType().GetProperty(propertyName3);
            var propertyInfoRinjiRitu = monthlyTransportationModel.GetType().GetProperty(propertyName4);
            if (propertyInfoTab1 != null)
                propertyInfoTab1.SetValue(monthlyTransportationModel, value1, null);
            if (propertyInfoNobeSumCnt != null)
                propertyInfoNobeSumCnt.SetValue(monthlyTransportationModel, value2, null);
            if (propertyInfoJitudoRitu != null)
                propertyInfoJitudoRitu.SetValue(monthlyTransportationModel, value3, null);
            if (propertyInfoRinjiRitu != null)
                propertyInfoRinjiRitu.SetValue(monthlyTransportationModel, value4, null);

            //tab2
            var propertyInfoTab2 = monthlyTransportationModel.GetType().GetProperty(propertyName1);
            var propertyInfoTab2UnsoSyu = monthlyTransportationModel.GetType().GetProperty(propertyName5);
            var propertyInfoTab2DayTotalKm = monthlyTransportationModel.GetType().GetProperty(propertyName6);
            var propertyInfoTab2DayYusoJin = monthlyTransportationModel.GetType().GetProperty(propertyName7);
            var propertyInfoTab2DayUnsoSyu = monthlyTransportationModel.GetType().GetProperty(propertyName8);
            if (propertyInfoTab2 != null)
                propertyInfoTab2.SetValue(monthlyTransportationModel, value1, null);
            if (propertyInfoTab2UnsoSyu != null)
                propertyInfoTab2UnsoSyu.SetValue(monthlyTransportationModel, value5, null);
            if (propertyInfoTab2DayTotalKm != null)
                propertyInfoTab2DayTotalKm.SetValue(monthlyTransportationModel, value6, null);
            if (propertyInfoTab2DayYusoJin != null)
                propertyInfoTab2DayYusoJin.SetValue(monthlyTransportationModel, value7, null);
            if (propertyInfoTab2DayUnsoSyu != null)
                propertyInfoTab2DayUnsoSyu.SetValue(monthlyTransportationModel, value8, null);
            await InvokeAsync(StateHasChanged);
        }

        protected void SaveStage(int value)
        {
            try
            {
                if (value == (int)ModeTab.Tab1Big)
                {
                    isExpandTab1Big = !isExpandTab1Big;
                    if (isExpandTab1Big)
                    {
                        ValueTab1Big = "collapse show";
                        ValueAtributeTab1Big = "";
                    }
                    else
                    {
                        ValueTab1Big = "collapse";
                        ValueAtributeTab1Big = "collapsed";
                    }
                }
                else if (value == (int)ModeTab.Tab1Medium)
                {
                    isExpandTab1Medium = !isExpandTab1Medium;
                    if (isExpandTab1Medium)
                    {
                        ValueTab1Medium = "collapse show";
                        ValueAtributeTab1Medium = "";
                    }
                    else
                    {
                        ValueTab1Medium = "collapse";
                        ValueAtributeTab1Medium = "collapsed";
                    }
                }
                else if (value == (int)ModeTab.Tab1Small)
                {
                    isExpandTab1Small = !isExpandTab1Small;
                    if (isExpandTab1Small)
                    {
                        ValueTab1Small = "collapse show";
                        ValueAtributeTab1Small = "";
                    }
                    else
                    {
                        ValueTab1Small = "collapse";
                        ValueAtributeTab1Small = "collapsed";
                    }
                }
                else if (value == (int)ModeTab.Tab2Big)
                {
                    isExpandTab2Big = !isExpandTab2Big;
                    if (isExpandTab2Big)
                    {
                        ValueTab2Big = "collapse show";
                        ValueAtributeTab2Big = "";
                    }
                    else
                    {
                        ValueTab2Big = "collapse";
                        ValueAtributeTab2Big = "collapsed";
                    }
                }
                else if (value == (int)ModeTab.Tab2Medium)
                {
                    isExpandTab2Medium = !isExpandTab2Medium;
                    if (isExpandTab2Medium)
                    {
                        ValueTab2Medium = "collapse show";
                        ValueAtributeTab2Medium = "";
                    }
                    else
                    {
                        ValueTab2Medium = "collapse";
                        ValueAtributeTab2Medium = "collapsed";
                    }
                }
                else if (value == (int)ModeTab.Tab2Small)
                {
                    isExpandTab2Small = !isExpandTab2Small;
                    if (isExpandTab2Small)
                    {
                        ValueTab2Small = "collapse show";
                        ValueAtributeTab2Small = "";
                    }
                    else
                    {
                        ValueTab2Small = "collapse";
                        ValueAtributeTab2Small = "collapsed";
                    }
                }
                //InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected async Task SaveJitHou()
        {
            try
            {
                await _loading.ShowAsync();
                var searchParams = EncryptHelper.DecryptFromUrl<SearchParam>(searchString);
                await _service.SaveJitHou(searchParams, monthlyTransportationModel);
                _navigationManager.NavigateTo("monthlytransportationresult", true);
                await _loading.HideAsync();
            }
            catch (Exception ex)
            {
                errorModalService.HandleError(ex);
            }
        }

        protected void Back()
        {
            _navigationManager.NavigateTo("monthlytransportationresult", true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jSRuntime.InvokeVoidAsync("setMaxlenghtforNumberField", ".code-number-length");
        }
    }
}

using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;


namespace HassyaAllrightCloud.Application.Validation
{
    public class HyperFormValidator : AbstractValidator<HyperFormData>
    {
        public HyperFormValidator()
        {
            // 配車日
            When(x => x.HaishaBiFrom != null && x.HaishaBiTo != null && x.HaishaBiFrom > x.HaishaBiTo, () => {
                RuleFor(m => m.HaishaBiFrom).Empty().WithMessage("BI_T001");
                RuleFor(m => m.HaishaBiTo).Empty().WithMessage("BI_T001");
            });

            // 到着日
            When(x => x.TochakuBiFrom != null && x.TochakuBiTo != null && x.TochakuBiFrom > x.TochakuBiTo, () => {
                RuleFor(m => m.TochakuBiFrom).Empty().WithMessage("BI_T002");
                RuleFor(m => m.TochakuBiTo).Empty().WithMessage("BI_T002");
            });

            // 予約日
            When(x => x.YoyakuBiFrom != null && x.YoyakuBiTo != null && x.YoyakuBiFrom > x.YoyakuBiTo, () =>
            {
                RuleFor(m => m.YoyakuBiFrom).Empty().WithMessage("BI_T003");
                RuleFor(m => m.YoyakuBiTo).Empty().WithMessage("BI_T003");
            });

            // 受付番号
            When(x => !String.IsNullOrEmpty(x.UketsukeBangoFrom) && !String.IsNullOrEmpty(x.UketsukeBangoTo) && long.Parse(x.UketsukeBangoFrom) > long.Parse(x.UketsukeBangoTo), () => {
                RuleFor(m => m.UketsukeBangoFrom).Empty().WithMessage("BI_T004");
                RuleFor(m => m.UketsukeBangoTo).Empty().WithMessage("BI_T004");
            });

            // 予約区分
            When(x => x.YoyakuFrom != null && x.YoyakuTo != null && x.YoyakuFrom.YoyaKbn > x.YoyakuTo.YoyaKbn, () =>
            {
                RuleFor(m => m.YoyakuFrom).Empty().WithMessage("BI_T005");
                RuleFor(m => m.YoyakuTo).Empty().WithMessage("BI_T005");
            });

            // 営業担当
            When(x => x.EigyoTantoShaFrom != null && x.EigyoTantoShaTo != null &&
                (string.Compare(x.EigyoTantoShaFrom.SyainCd, x.EigyoTantoShaTo.SyainCd) > 0 || (string.Compare(x.EigyoTantoShaFrom.SyainCd, x.EigyoTantoShaTo.SyainCd) == 0 && x.EigyoTantoShaFrom.SyainCdSeq > x.EigyoTantoShaTo.SyainCdSeq)), () =>
            {
                RuleFor(m => m.EigyoTantoShaFrom).Empty().WithMessage("BI_T006");
                RuleFor(m => m.EigyoTantoShaTo).Empty().WithMessage("BI_T006");
            });

            // 受付営業所
            When(x => x.UketsukeEigyoJoFrom != null && x.UketsukeEigyoJoTo != null &&
                (x.UketsukeEigyoJoFrom.EigyoCd > x.UketsukeEigyoJoTo.EigyoCd || (x.UketsukeEigyoJoFrom.EigyoCd == x.UketsukeEigyoJoTo.EigyoCd && x.UketsukeEigyoJoFrom.EigyoCdSeq > x.UketsukeEigyoJoTo.EigyoCdSeq)), () =>
            {
                RuleFor(m => m.UketsukeEigyoJoFrom).Empty().WithMessage("BI_T007");
                RuleFor(m => m.UketsukeEigyoJoTo).Empty().WithMessage("BI_T007");
            });

            // 入力担当
            When(x => x.NyuryokuTantoShaFrom != null && x.NyuryokuTantoShaTo != null &&
                (string.Compare(x.NyuryokuTantoShaFrom.SyainCd, x.NyuryokuTantoShaTo.SyainCd) > 0 || (string.Compare(x.NyuryokuTantoShaFrom.SyainCd, x.NyuryokuTantoShaTo.SyainCd) == 0 && x.NyuryokuTantoShaFrom.SyainCdSeq > x.NyuryokuTantoShaTo.SyainCdSeq)), () =>
            {
                RuleFor(m => m.NyuryokuTantoShaFrom).Empty().WithMessage("BI_T008");
                RuleFor(m => m.NyuryokuTantoShaTo).Empty().WithMessage("BI_T008");
            });

            // 得意先
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd > x.GyosyaTokuiSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaTokuiSakiFrom).Empty().WithMessage("BI_T009");
                RuleFor(m => m.GyosyaTokuiSakiTo).Empty().WithMessage("BI_T009");
            });
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd > x.TokiskTokuiSakiTo.TokuiCd))
                , () =>
                    {
                        RuleFor(m => m.TokiskTokuiSakiFrom).Empty().WithMessage("BI_T009");
                        RuleFor(m => m.TokiskTokuiSakiTo).Empty().WithMessage("BI_T009");
                    });
            When(x => ((x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && x.TokiStTokuiSakiFrom != null && x.TokiStTokuiSakiTo != null)
                    && ((x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd > x.TokiStTokuiSakiTo.SitenCd)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq > x.GyosyaTokuiSakiTo.GyosyaCdSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq > x.TokiskTokuiSakiTo.TokuiSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq == x.TokiskTokuiSakiTo.TokuiSeq && x.TokiStTokuiSakiFrom.SitenCdSeq > x.TokiStTokuiSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStTokuiSakiFrom).Empty().WithMessage("BI_T009");
                        RuleFor(m => m.TokiStTokuiSakiTo).Empty().WithMessage("BI_T009");
                    });

            // 仕入先
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd > x.GyosyaShiireSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaShiireSakiFrom).Empty().WithMessage("BI_T010");
                RuleFor(m => m.GyosyaShiireSakiTo).Empty().WithMessage("BI_T010");
            });
            When(x => (x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd > x.TokiskShiireSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskShiireSakiFrom).Empty().WithMessage("BI_T010");
                    RuleFor(m => m.TokiskShiireSakiTo).Empty().WithMessage("BI_T010");
                });
            When(x =>((x.GyosyaShiireSakiFrom != null && x.GyosyaShiireSakiTo != null && x.TokiskShiireSakiFrom != null && x.TokiskShiireSakiTo != null && x.TokiStShiireSakiFrom != null && x.TokiStShiireSakiTo != null)
                    && ((x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd > x.TokiStShiireSakiTo.SitenCd)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq > x.GyosyaShiireSakiTo.GyosyaCdSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq > x.TokiskShiireSakiTo.TokuiSeq)
                    || (x.GyosyaShiireSakiFrom.GyosyaCd == x.GyosyaShiireSakiTo.GyosyaCd && x.TokiskShiireSakiFrom.TokuiCd == x.TokiskShiireSakiTo.TokuiCd && x.TokiStShiireSakiFrom.SitenCd == x.TokiStShiireSakiTo.SitenCd && x.GyosyaShiireSakiFrom.GyosyaCdSeq == x.GyosyaShiireSakiTo.GyosyaCdSeq && x.TokiskShiireSakiFrom.TokuiSeq == x.TokiskShiireSakiTo.TokuiSeq && x.TokiStShiireSakiFrom.SitenCdSeq > x.TokiStShiireSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStShiireSakiFrom).Empty().WithMessage("BI_T010");
                        RuleFor(m => m.TokiStShiireSakiTo).Empty().WithMessage("BI_T010");
                    });

            // 団体区分
            When(x => x.DantaiKbnFrom != null && x.DantaiKbnTo != null &&
                (string.Compare(x.DantaiKbnFrom.CodeKbn, x.DantaiKbnTo.CodeKbn) > 0 || (string.Compare(x.DantaiKbnFrom.CodeKbn, x.DantaiKbnTo.CodeKbn) == 0 && x.DantaiKbnFrom.CodeKbnSeq > x.DantaiKbnTo.CodeKbnSeq)), () =>
            {
                RuleFor(m => m.DantaiKbnFrom).Empty().WithMessage("BI_T011");
                RuleFor(m => m.DantaiKbnTo).Empty().WithMessage("BI_T011");
            });

            // 客種区分
            When(x => x.KyakuDaneKbnFrom != null && x.KyakuDaneKbnTo != null &&
                (x.KyakuDaneKbnFrom.JyoKyakuCd > x.KyakuDaneKbnTo.JyoKyakuCd || (x.KyakuDaneKbnFrom.JyoKyakuCd == x.KyakuDaneKbnTo.JyoKyakuCd && x.KyakuDaneKbnFrom.JyoKyakuCdSeq > x.KyakuDaneKbnTo.JyoKyakuCdSeq)), () =>
            {
                RuleFor(m => m.KyakuDaneKbnFrom).Empty().WithMessage("BI_T012");
                RuleFor(m => m.KyakuDaneKbnTo).Empty().WithMessage("BI_T012");
            });

            // 行先
            When(x => x.YukiSakiFrom != null && x.YukiSakiTo != null &&
                ((string.Compare(x.YukiSakiFrom.CodeKbn, x.YukiSakiTo.CodeKbn) > 0) || (string.Compare(x.YukiSakiFrom.CodeKbn, x.YukiSakiTo.CodeKbn) == 0 && string.Compare(x.YukiSakiFrom.BasyoMapCd, x.YukiSakiTo.BasyoMapCd) > 0)
                    || (string.Compare(x.YukiSakiFrom.CodeKbn, x.YukiSakiTo.CodeKbn) == 0 && string.Compare(x.YukiSakiFrom.BasyoMapCd, x.YukiSakiTo.BasyoMapCd) == 0 && x.YukiSakiFrom.CodeKbnSeq > x.YukiSakiTo.CodeKbnSeq)
                    || (string.Compare(x.YukiSakiFrom.CodeKbn, x.YukiSakiTo.CodeKbn) == 0 && string.Compare(x.YukiSakiFrom.BasyoMapCd, x.YukiSakiTo.BasyoMapCd) == 0 && x.YukiSakiFrom.CodeKbnSeq == x.YukiSakiTo.CodeKbnSeq && x.YukiSakiFrom.BasyoMapCdSeq > x.YukiSakiTo.BasyoMapCdSeq)), () =>
            {
                RuleFor(m => m.YukiSakiFrom).Empty().WithMessage("BI_T013");
                RuleFor(m => m.YukiSakiTo).Empty().WithMessage("BI_T013");
            });

            // 配車地
            When(x => x.HaishaChiFrom != null && x.HaishaChiTo != null &&
                ((string.Compare(x.HaishaChiFrom.CodeKbn, x.HaishaChiTo.CodeKbn) > 0) || (string.Compare(x.HaishaChiFrom.CodeKbn, x.HaishaChiTo.CodeKbn) == 0 && string.Compare(x.HaishaChiFrom.HaiSCd, x.HaishaChiTo.HaiSCd) > 0)
                    || (string.Compare(x.HaishaChiFrom.CodeKbn, x.HaishaChiTo.CodeKbn) == 0 && string.Compare(x.HaishaChiFrom.HaiSCd, x.HaishaChiTo.HaiSCd) == 0 && x.HaishaChiFrom.CodeKbnSeq > x.HaishaChiTo.CodeKbnSeq)
                    || (string.Compare(x.HaishaChiFrom.CodeKbn, x.HaishaChiTo.CodeKbn) == 0 && string.Compare(x.HaishaChiFrom.HaiSCd, x.HaishaChiTo.HaiSCd) == 0 && x.HaishaChiFrom.CodeKbnSeq == x.HaishaChiTo.CodeKbnSeq && x.HaishaChiFrom.HaiScdSeq > x.HaishaChiTo.HaiScdSeq)), () =>
            {
                RuleFor(m => m.HaishaChiFrom).Empty().WithMessage("BI_T014");
                RuleFor(m => m.HaishaChiTo).Empty().WithMessage("BI_T014");
            });

            // 発生地
            When(x => x.HasseiChiFrom != null && x.HasseiChiTo != null &&
                ((string.Compare(x.HasseiChiFrom.CodeKbn, x.HasseiChiTo.CodeKbn) > 0) || (string.Compare(x.HasseiChiFrom.CodeKbn, x.HasseiChiTo.CodeKbn) == 0 && string.Compare(x.HasseiChiFrom.BasyoMapCd, x.HasseiChiTo.BasyoMapCd) > 0)
                    || (string.Compare(x.HasseiChiFrom.CodeKbn, x.HasseiChiTo.CodeKbn) == 0 && string.Compare(x.HasseiChiFrom.BasyoMapCd, x.HasseiChiTo.BasyoMapCd) == 0 && x.HasseiChiFrom.CodeKbnSeq > x.HasseiChiTo.CodeKbnSeq)
                    || (string.Compare(x.HasseiChiFrom.CodeKbn, x.HasseiChiTo.CodeKbn) == 0 && string.Compare(x.HasseiChiFrom.BasyoMapCd, x.HasseiChiTo.BasyoMapCd) == 0 && x.HasseiChiFrom.CodeKbnSeq == x.HasseiChiTo.CodeKbnSeq && x.HasseiChiFrom.BasyoMapCdSeq > x.HasseiChiTo.BasyoMapCdSeq)), () =>
            {
                RuleFor(m => m.HasseiChiFrom).Empty().WithMessage("BI_T015");
                RuleFor(m => m.HasseiChiTo).Empty().WithMessage("BI_T015");
            });

            // エリア
            When(x => x.AreaFrom != null && x.AreaTo != null &&
                ((string.Compare(x.AreaFrom.CodeKbn, x.AreaTo.CodeKbn) > 0) || (string.Compare(x.AreaFrom.CodeKbn, x.AreaTo.CodeKbn) == 0 && string.Compare(x.AreaFrom.BasyoMapCd, x.AreaTo.BasyoMapCd) > 0)
                    || (string.Compare(x.AreaFrom.CodeKbn, x.AreaTo.CodeKbn) == 0 && string.Compare(x.AreaFrom.BasyoMapCd, x.AreaTo.BasyoMapCd) == 0 && x.AreaFrom.CodeKbnSeq > x.AreaTo.CodeKbnSeq)
                    || (string.Compare(x.AreaFrom.CodeKbn, x.AreaTo.CodeKbn) == 0 && string.Compare(x.AreaFrom.BasyoMapCd, x.AreaTo.BasyoMapCd) == 0 && x.AreaFrom.CodeKbnSeq == x.AreaTo.CodeKbnSeq && x.AreaFrom.BasyoMapCdSeq > x.AreaTo.BasyoMapCdSeq)), () =>
            {
                RuleFor(m => m.AreaFrom).Empty().WithMessage("BI_T016");
                RuleFor(m => m.AreaTo).Empty().WithMessage("BI_T016");
            });

            // 車種
            When(x => x.ShashuFrom != null && x.ShashuTo != null &&
                (x.ShashuFrom.SyaSyuCd > x.ShashuTo.SyaSyuCd || (x.ShashuFrom.SyaSyuCd == x.ShashuTo.SyaSyuCd && x.ShashuFrom.SyaSyuCdSeq > x.ShashuTo.SyaSyuCdSeq)), () =>
            {
                RuleFor(m => m.ShashuFrom).Empty().WithMessage("BI_T017");
                RuleFor(m => m.ShashuTo).Empty().WithMessage("BI_T017");
            });

            // 車種単価
            When(x => !String.IsNullOrEmpty(x.ShashuTankaFrom) && !String.IsNullOrEmpty(x.ShashuTankaTo) && Decimal.Parse(x.ShashuTankaFrom) > Decimal.Parse(x.ShashuTankaTo), () => {
                RuleFor(m => m.ShashuTankaFrom).Empty().WithMessage("BI_T018");
                RuleFor(m => m.ShashuTankaTo).Empty().WithMessage("BI_T018");
            });

            // 受付条件
            When(x => x.UketsukeJokenFrom != null && x.UketsukeJokenTo != null &&
                (string.Compare(x.UketsukeJokenFrom.CodeKbn, x.UketsukeJokenTo.CodeKbn) > 0 || (string.Compare(x.UketsukeJokenFrom.CodeKbn, x.UketsukeJokenTo.CodeKbn) == 0 && x.UketsukeJokenFrom.CodeKbnSeq > x.UketsukeJokenTo.CodeKbnSeq)), () =>
                {
                    RuleFor(m => m.UketsukeJokenFrom).Empty().WithMessage("BI_T019");
                    RuleFor(m => m.UketsukeJokenTo).Empty().WithMessage("BI_T019");
                });
        }
    }
}

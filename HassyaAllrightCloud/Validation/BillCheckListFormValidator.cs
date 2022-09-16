using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HassyaAllrightCloud.Application.Validation
{
    public class BillCheckListFormValidator : AbstractValidator<BillsCheckListFormData>
    {
        public BillCheckListFormValidator()
        {
            // ​請求対象期間
            RuleFor(e => e.BillPeriodFrom).Must((obj, v) => obj.BillPeriodTo == null || v == null || v <= obj.BillPeriodTo)
                                            .WithMessage("BI_T001");

            RuleFor(e => e.BillPeriodTo).Must((obj, v) => obj.BillPeriodFrom == null || v == null || v >= obj.BillPeriodFrom)
                                            .WithMessage("BI_T001");
            // 請求先
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd > x.GyosyaTokuiSakiTo.GyosyaCd)), () =>
            {
                RuleFor(m => m.GyosyaTokuiSakiFrom).Empty().WithMessage("BI_T002");
                RuleFor(m => m.GyosyaTokuiSakiTo).Empty().WithMessage("BI_T002");
            });
            When(x => (x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd > x.TokiskTokuiSakiTo.TokuiCd))
                , () =>
                {
                    RuleFor(m => m.TokiskTokuiSakiFrom).Empty().WithMessage("BI_T002");
                    RuleFor(m => m.TokiskTokuiSakiTo).Empty().WithMessage("BI_T002");
                });
            When(x => ((x.GyosyaTokuiSakiFrom != null && x.GyosyaTokuiSakiTo != null && x.TokiskTokuiSakiFrom != null && x.TokiskTokuiSakiTo != null && x.TokiStTokuiSakiFrom != null && x.TokiStTokuiSakiTo != null)
                    && ((x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd > x.TokiStTokuiSakiTo.SitenCd)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq > x.GyosyaTokuiSakiTo.GyosyaCdSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq > x.TokiskTokuiSakiTo.TokuiSeq)
                    || (x.GyosyaTokuiSakiFrom.GyosyaCd == x.GyosyaTokuiSakiTo.GyosyaCd && x.TokiskTokuiSakiFrom.TokuiCd == x.TokiskTokuiSakiTo.TokuiCd && x.TokiStTokuiSakiFrom.SitenCd == x.TokiStTokuiSakiTo.SitenCd && x.GyosyaTokuiSakiFrom.GyosyaCdSeq == x.GyosyaTokuiSakiTo.GyosyaCdSeq && x.TokiskTokuiSakiFrom.TokuiSeq == x.TokiskTokuiSakiTo.TokuiSeq && x.TokiStTokuiSakiFrom.SitenCdSeq > x.TokiStTokuiSakiTo.SitenCdSeq))), () =>
                    {
                        RuleFor(m => m.TokiStTokuiSakiFrom).Empty().WithMessage("BI_T002");
                        RuleFor(m => m.TokiStTokuiSakiTo).Empty().WithMessage("BI_T002");
                    });

            // 受付番号
            RuleFor(e => e.StartReceiptNumber).Must((obj, v) => obj.EndReceiptNumber == null || v == null || long.Parse(v) <= long.Parse(obj.EndReceiptNumber))
                                .WithMessage("BI_T003");

            RuleFor(e => e.EndReceiptNumber).Must((obj, v) => obj.StartReceiptNumber == null || v == null || long.Parse(v) >= long.Parse(obj.StartReceiptNumber))
                                .WithMessage("BI_T003");

            // ​予約区分
            When(x => x.YoyakuFrom != null && x.YoyakuTo != null && x.YoyakuFrom.YoyaKbn > x.YoyakuTo.YoyaKbn, () =>
            {
                RuleFor(m => m.YoyakuFrom).Empty().WithMessage("BI_T004");
                RuleFor(m => m.YoyakuTo).Empty().WithMessage("BI_T004");
            });

            // ​請求区分
            RuleFor(e => e.StartBillClassification).Must((obj, v) => obj.EndBillClassification == null || v == null || v.CodeKbnSeq <= obj.EndBillClassification.CodeKbnSeq)
                                .WithMessage("BI_T005");

            RuleFor(e => e.EndBillClassification).Must((obj, v) => obj.StartBillClassification == null || v == null || v.CodeKbnSeq >= obj.StartBillClassification.CodeKbnSeq)
                                .WithMessage("BI_T005");
        }
    }
}

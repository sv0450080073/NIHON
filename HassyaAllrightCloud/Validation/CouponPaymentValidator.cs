using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Validation
{
    public class CouponPaymentValidator : AbstractValidator<CouponPaymentFormModel>
    {
        public CouponPaymentValidator()
        {
            When(_ => _.EndIssuePeriod != null, () =>
            {
                RuleFor(e => e.StartIssuePeriod).Must((obj, value) => obj.StartIssuePeriod == null || (value != null && value <= obj.EndIssuePeriod)).WithMessage("BI_T001");
            });
            When(_ => _.StartIssuePeriod != null, () =>
            {
                RuleFor(e => e.EndIssuePeriod).Must((obj, value) => obj.EndIssuePeriod == null || (value != null && value >= obj.StartIssuePeriod)).WithMessage("BI_T001");
            });

            When(x => x.SelectedGyosyaFrom != null && x.SelectedGyosyaTo != null, () =>
            {
                RuleFor(e => e.SelectedGyosyaTo).Must((obj, value) => value == null || value.GyosyaCd >= obj.SelectedGyosyaFrom.GyosyaCd).WithMessage("BI_T002");
                RuleFor(e => e.SelectedGyosyaFrom).Must((obj, value) => value == null || value.GyosyaCd <= obj.SelectedGyosyaTo.GyosyaCd).WithMessage("BI_T002");
            });

            When(x => x.SelectedTokiskFrom != null && x.SelectedTokiskTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999), () =>
            {
                RuleFor(e => e.SelectedTokiskTo).Must((obj, value) => value == null || value.TokuiCd >= obj.SelectedTokiskFrom.TokuiCd).WithMessage("BI_T002");
                RuleFor(e => e.SelectedTokiskFrom).Must((obj, value) => value == null || value.TokuiCd <= obj.SelectedTokiskTo.TokuiCd).WithMessage("BI_T002");
            });

            When(x => x.SelectedTokiStFrom != null && x.SelectedTokiStTo != null &&
                    (x.SelectedGyosyaFrom?.GyosyaCd ?? 0) == (x.SelectedGyosyaTo?.GyosyaCd ?? 999) &&
                    (x.SelectedTokiskFrom?.TokuiCd ?? 0) == (x.SelectedTokiskTo?.TokuiCd ?? 9999), () =>
            {
                RuleFor(e => e.SelectedTokiStTo).Must((obj, value) => value == null || value.SitenCd >= obj.SelectedTokiStFrom.SitenCd).WithMessage("BI_T002");
                RuleFor(e => e.SelectedTokiStFrom).Must((obj, value) => value == null || value.SitenCd <= obj.SelectedTokiStTo.SitenCd).WithMessage("BI_T002");
            });


            When(x => x.StartReservationClassificationSort != null && x.EndReservationClassificationSort != null && x.StartReservationClassificationSort.YoyaKbn > x.EndReservationClassificationSort.YoyaKbn, () =>
            {
                RuleFor(m => m.EndReservationClassificationSort).Empty().WithMessage("BI_T003");
                RuleFor(m => m.StartReservationClassificationSort).Empty().WithMessage("BI_T003");
            });
        }
    }
}

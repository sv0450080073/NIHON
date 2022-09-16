using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class RevenueSummaryValidator : AbstractValidator<FormSearchModel>
    {
        public RevenueSummaryValidator()
        {
            When(_ => _.EigyoTo != null, () =>
            {
                RuleFor(e => e.EigyoFrom).Must((obj, value) => obj.EigyoFrom == null || (value != null && value.EigyoCd <= obj.EigyoTo.EigyoCd)).WithMessage("BI_T003");
            });
            When(_ => _.EigyoFrom != null, () =>
            {
                RuleFor(e => e.EigyoTo).Must((obj, value) => obj.EigyoTo == null || (value != null && value.EigyoCd >= obj.EigyoFrom.EigyoCd)).WithMessage("BI_T003");
            });
            RuleFor(e => e.UriYmdFrom).NotEmpty().WithMessage("BI_T001_1");
            RuleFor(e => e.UriYmdTo).NotEmpty().WithMessage("BI_T001_2");
            When(_ => _.UriYmdTo != null, () =>
            {
                RuleFor(e => e.UriYmdFrom).Must((obj, value) => obj.UriYmdFrom == null || (value != null && value <= obj.UriYmdTo && value.Month == obj.UriYmdTo.Month && value.Year == obj.UriYmdTo.Year)).WithMessage("BI_T002");
            });
            When(_ => _.UriYmdFrom != null, () =>
            {
                RuleFor(e => e.UriYmdTo).Must((obj, value) => obj.UriYmdTo == null || (value != null && value >= obj.UriYmdFrom && value.Month == obj.UriYmdFrom.Month && value.Year == obj.UriYmdFrom.Year)).WithMessage("BI_T002");
            });
            When(_ => !string.IsNullOrEmpty(_.UkeNoTo), () =>
            {
                RuleFor(e => e.UkeNoFrom).Must((obj, value) => string.IsNullOrEmpty(obj.UkeNoFrom) || (long.TryParse(value, out long m) && long.TryParse(obj.UkeNoTo, out long n) && m <= n)).WithMessage("BI_T004");
            });
            When(_ => !string.IsNullOrEmpty(_.UkeNoFrom), () =>
            {
                RuleFor(e => e.UkeNoTo).Must((obj, value) => string.IsNullOrEmpty(obj.UkeNoTo) || (long.TryParse(value, out long m) && long.TryParse(obj.UkeNoFrom, out long n) && m >= n)).WithMessage("BI_T004");
            });
            When(_ => _.YoyaKbnTo != null, () =>
            {
                RuleFor(e => e.YoyaKbnFrom).Must((obj, value) => obj.YoyaKbnFrom == null || value.YoyaKbn <= obj.YoyaKbnTo.YoyaKbn).WithMessage("BI_T005");
            });
            When(_ => _.YoyaKbnFrom != null, () =>
            {
                RuleFor(e => e.YoyaKbnTo).Must((obj, value) => obj.YoyaKbnTo == null || value.YoyaKbn >= obj.YoyaKbnFrom.YoyaKbn).WithMessage("BI_T005");
            });
        }
    }
}

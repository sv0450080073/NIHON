using FluentValidation;
using HassyaAllrightCloud.Domain.Dto;

namespace HassyaAllrightCloud.Application.Validation
{
    public class TransportationSummaryValidator : AbstractValidator<TransportationSummarySearchModel>
    {
        public TransportationSummaryValidator()
        {
            When(_ => _.EigyoTo != null, () =>
            {
                RuleFor(e => e.EigyoFrom).Must((obj, value) => obj.EigyoFrom == null || (value != null && value.EigyoCd <= obj.EigyoTo.EigyoCd)).WithMessage("InvalidRange");
            });
            When(_ => _.EigyoFrom != null, () =>
            {
                RuleFor(e => e.EigyoTo).Must((obj, value) => obj.EigyoTo == null || (value != null && value.EigyoCd >= obj.EigyoFrom.EigyoCd)).WithMessage("InvalidRange");
            });
            RuleFor(e => e.ProcessingDate).NotEmpty().WithMessage("DateRequired");
        }
    }
}
